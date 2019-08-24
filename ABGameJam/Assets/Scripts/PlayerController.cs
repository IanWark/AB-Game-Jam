﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 1000;
    public Slider healthSlider;
    
    public float speed = 5;

    public bool controlEnabled = true;
    
    private int currentHealth;

    private Rigidbody2D rb2d;
    public BoxCollider2D stompCollider;
    public int stompDamage = 2;
    public BoxCollider2D punchCollider;
    public int punchDamage = 3;

    void Awake()
    {
        Globals.player = this;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlEnabled)
        {
            // Move left/right
            // Collider will stop from moving offscreen to the left
            rb2d.MovePosition(rb2d.position + new Vector2(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0));

            // Stomp
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J))
            {
                Attack(stompCollider, stompDamage);
            }
            else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.K))
            {
                Attack(punchCollider, punchDamage);
            }
        }
    }
    
    public void OnHit(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        
        if (currentHealth <= 0)
        {
            // Die
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
                enemy.TakeDamage(damage);
            }
        }
    }

    public Collider2D[] GetAllCollidersHit(BoxCollider2D ourCollider)
    {
        return Physics2D.OverlapBoxAll(ourCollider.transform.position, ourCollider.size, 0);
    }
}
