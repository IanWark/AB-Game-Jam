using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text scoreUI;
    public int score;
    
    public int maxHealth = 1000;
    public Slider healthSlider;
    private int currentHealth;
    
    public int maxDash = 1000;
    public Slider dashSlider;
    private int currentDash;
    
    public bool dashing = false;
    private float dashMaxTime = 1.0f;
    private float dashTimer = 0;
    
    public const float speed = 2;
    // Current speed changes whenever the number of melee enemies touching us changes
    private float currentSpeed = speed;
    public float getCurrentSpeed() { return currentSpeed; }

    // The number of melee enemies touching us, slowing us down and dealing us damage
    private int numMeleeEnemiesTouching = 0;
    private float meleeDamageTimer = 0;
    private float meleeSlowPlayerPercentage = DwarfMelee.slowPlayerPercentage;
    private float meleeAttackSpeed = DwarfMelee.attackSpeed;
    private int meleeAttackDamage = DwarfMelee.attackDamage;

    public bool controlEnabled = true;
    private float controlMaxTime = 2;
    private float controlTimer = 0;

    private Rigidbody2D rb2d;
    private Animator animator;
    public BoxCollider2D stompCollider;
    public int stompDamage;
    public BoxCollider2D punchCollider;
    public int punchDamage;
    public BoxCollider2D dashCollider;
    public int dashDamage;

    public AudioSource audioMain;
    public AudioSource audioWalk1;
    public AudioSource audioWalk2;
    public AudioClip stompSound;
    public AudioClip punchSound;
    public AudioClip dashSound;
    public AudioClip walkSound;
    private int walkTurn = 1;
    private const float mainVolume = 1;

    public ScoreTracker tracker;

    void Awake()
    {
        Globals.player = this;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        tracker = FindObjectOfType<ScoreTracker>();
        currentHealth = maxHealth;
        currentDash = maxDash;
        score = 0;
        
        stompDamage = 20;
        punchDamage = 30;
        dashDamage = 3;
    }

    // Update is called once per frame
    void Update()
    {
        // Replenish dash
        if (currentDash < maxDash)
        {
            currentDash += 2;
            dashSlider.value = currentDash;
        }
        
        // If player is not attacking/is able to move
        if (controlEnabled)
        {
            // Move left/right
            // Collider will stop from moving offscreen to the left
            float movementInput = Input.GetAxisRaw("Horizontal");
            if (movementInput > 0) {
                animator.Play("monster_walk");
            }
            else if (movementInput < 0) {
                animator.Play("monster_walk_backwards");
            } else
            {
                animator.Play("monster_idle");
            }
            
            rb2d.MovePosition(rb2d.position + new Vector2(movementInput * currentSpeed * Time.deltaTime, 0));

            // Stomp
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J))
            {
                animator.Play("monster_stomp");
                controlEnabled = false;
            }
            // Punch
            else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.K))
            {
                animator.Play("monster_punch");
                controlEnabled = false;
            }
            // Dash
            else if (Input.GetKeyDown(KeyCode.Space) && currentDash == maxDash)
            {
                dashing = true;
                controlEnabled = false;
                animator.speed = 2.0f;
                animator.Play("monster_walk");
                PlayMainSound(dashSound, 1, 1);
            }
        }
        
        scoreUI.text = score.ToString("D9");
    }

    private void FixedUpdate()
    {
        if (numMeleeEnemiesTouching > 0)
        {
            // If enemies are touching us, take damage every so often
            meleeDamageTimer += Time.fixedDeltaTime;
            if (meleeDamageTimer > meleeAttackSpeed)
            {
                OnHit(numMeleeEnemiesTouching * meleeAttackDamage);
                meleeDamageTimer = 0;
            }
        }
        else
        {
            meleeDamageTimer = 0;
        }

        // We take control away from player when attacking
        // There's a rare bug where it doesn't come back, so also we have a timer to reset after 2 seconds
        if (!controlEnabled)
        {
            controlTimer += Time.fixedDeltaTime;

            if (controlTimer > controlMaxTime)
            {
                controlEnabled = true;
                controlTimer = 0;
            }
        } else
        {
            controlTimer = 0;
        }

        if (dashing)
        {
            dashTimer += Time.fixedDeltaTime;

            // Dash and do damage while we're dashing
            Dash();
            Attack(dashCollider, dashDamage);

            // fade out sound if halfway through
            if (dashTimer / dashMaxTime > 0.5f)
            {
                audioMain.volume = Mathf.Lerp(mainVolume, 0, dashTimer / dashMaxTime);
            }

            if (dashTimer > dashMaxTime)
            {
                // Stop dashing, reset animation system
                dashing = false;
                UpdateSpeed();
                dashTimer = 0;
                animator.enabled = false;
                animator.speed = 1.0f;
                animator.enabled = true;
                controlEnabled = true;

                audioMain.Stop();
                audioMain.volume = mainVolume;
            }
        }
        else
        {
            dashTimer = 0;
        }
    }

    public void OnStomp()
    {
        Attack(stompCollider, stompDamage);
        PlayMainSound(stompSound, 0.5f, 0.8f);
    }

    public void OnPunch()
    {
        Attack(punchCollider, punchDamage);
        PlayMainSound(punchSound, 0.8f, 1.0f);
    }

    public void ResumeControl()
    {
        controlEnabled = true;
    }

    public void OnHit(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        
        if (currentHealth <= 0)
        {
            // Die, show score on game over screen
            tracker.GameOver(score);
        }
    }

    public void Attack(BoxCollider2D collider, int damage)
    {
        Collider2D[] hits = GetAllCollidersHit(collider);

        foreach (var target in hits)
        {
            Enemy enemy = target.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                ImpactPoint impactPoint = collider.gameObject.GetComponentInChildren<ImpactPoint>();
                if (impactPoint != null)
                {
                    enemy.OnHit(damage, new Vector2(impactPoint.transform.position.x, impactPoint.transform.position.y));
                } else
                {
                    enemy.OnHit(damage, enemy.transform.up);
                }
            }
        }
    }
    
    public void Dash()
    {
        // The camera and background move based on current speed
        // We want them to move faster because we are moving caster, so set current speed * 3
        UpdateSpeed();
        currentSpeed *= 3;

        rb2d.MovePosition(rb2d.position + new Vector2(currentSpeed * Time.deltaTime, 0));
        currentDash = 0;
        dashSlider.value = currentDash;
    }

    public void AddMeleeEnemy()
    {
        numMeleeEnemiesTouching += 1;
        UpdateSpeed();
    }

    public void RemoveMeleeEnemy()
    {
        numMeleeEnemiesTouching -= 1;
        UpdateSpeed();
    }

    public void UpdateSpeed()
    {
        currentSpeed = speed * (Mathf.Pow(meleeSlowPlayerPercentage, numMeleeEnemiesTouching));
    }

    public Collider2D[] GetAllCollidersHit(BoxCollider2D ourCollider)
    {
        return Physics2D.OverlapBoxAll(ourCollider.transform.position, ourCollider.size, 0);
    }

    public void PlayMainSound(AudioClip sound, float pitchMin, float pitchMax)
    {
        audioMain.volume = mainVolume;
        audioMain.pitch = Random.Range(pitchMin, pitchMax);
        audioMain.PlayOneShot(sound);
    }

    public void WalkSound()
    {
        float pitch = Random.Range(0.5f, 1.0f);

        if (walkTurn == 1)
        {
            audioWalk1.pitch = pitch;
            audioWalk1.Play();

            walkTurn = 2;
        } else
        {
            audioWalk2.pitch = pitch;
            audioWalk2.Play();

            walkTurn = 1;
        }
    }
}
