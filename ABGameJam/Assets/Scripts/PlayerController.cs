﻿using System.Collections;
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
    public int stompDamage = 2;
    public BoxCollider2D punchCollider;
    public int punchDamage = 3;
    public BoxCollider2D dashCollider;
    public int dashDamage = 10;

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
                Attack(dashCollider, dashDamage);
                Dash();
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
    }

    public void OnStomp()
    {
        Attack(stompCollider, stompDamage);
    }

    public void OnPunch()
    {
        Attack(punchCollider, punchDamage);
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
