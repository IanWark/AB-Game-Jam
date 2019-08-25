using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dwarf : Enemy
{
    public float spawnHeight = -1.05f;
    public float moveSpeed = 1;

    protected Rigidbody2D rb2d;
    protected Collider2D col;

    public bool active = true;
    
    private int scoreValue = 150;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnHit(int damage, Vector2 impactPosition)
    {
        // Send enemies flying away from the impact
        Vector2 direction = new Vector2(transform.position.x, transform.position.y) - impactPosition;
        direction.Normalize();
        direction.y += 0.5f;

        Die(direction * 100);
    }

    public abstract void DieAnimation();

    void Die(Vector2 force)
    {
        // Stop AI
        active = false;
        col.enabled = false;
        // Play death animation
        DieAnimation();
        // Start gravity
        rb2d.constraints = 0;
        // Launch dwarf
        rb2d.AddForce(force);
        
        // Increase score
        GameObject Player = GameObject.Find("Player");
        PlayerController playerController = Player.GetComponent<PlayerController>();
        playerController.score += scoreValue;

        // Start a timer to destroy object
        Destroy(gameObject, 5);
    }
}
