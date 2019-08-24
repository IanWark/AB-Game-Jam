using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 1000;
    public Slider healthSlider;
    private int currentHealth;
    
    public int maxDash = 1000;
    public Slider dashSlider;
    private int currentDash;
    
    public const float speed = 3;
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

    private Rigidbody2D rb2d;
    public BoxCollider2D stompCollider;
    public int stompDamage = 2;
    public BoxCollider2D punchCollider;
    public int punchDamage = 3;
    public BoxCollider2D dashCollider;
    public int dashDamage = 5;

    void Awake()
    {
        Globals.player = this;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        currentDash = maxDash;
    }

    // Update is called once per frame
    void Update()
    {
        // Replenish dash
        if (currentDash < maxDash)
        {
            currentDash += 5;
            dashSlider.value = currentDash;
        }
        
        // If a button is being pressed
        if (controlEnabled)
        {
            // Move left/right
            // Collider will stop from moving offscreen to the left
            rb2d.MovePosition(rb2d.position + new Vector2(Input.GetAxisRaw("Horizontal") * currentSpeed * Time.deltaTime, 0));

            // Stomp
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J))
            {
                Attack(stompCollider, stompDamage);
            }
            // Punch
            else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.K))
            {
                Attack(punchCollider, punchDamage);
            }
            // Dash
            else if (Input.GetKeyDown(KeyCode.Space) && currentDash == maxDash)
            {
                Attack(dashCollider, dashDamage);
                Dash();
            }
        }
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
    }

    public void OnHit(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        
        if (currentHealth <= 0)
        {
            // Die, show score on game over screen
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
                enemy.OnHit(damage);
            }
        }
    }
    
    public void Dash()
    {
        rb2d.MovePosition(rb2d.position + new Vector2(10 * currentSpeed * Time.deltaTime, 0));
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
}
