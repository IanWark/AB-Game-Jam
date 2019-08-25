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
    
    private int scoreValue = 100;

    // TODO clean up Dwarves who end up too far ahead or behind the camera (using colliders?)

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnHit(int damage)
    {
        // TODO need to send a better force reacting to attack
        Die(new Vector2(0, 200));
    }

    void Die(Vector2 force)
    {
        // Stop AI
        active = false;
        col.enabled = false;
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
