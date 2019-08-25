using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Enemy
{
    public float spawnHeight = -0.65f;
    public float arrowSpawnExtraY = 0.4f;
    public float arrowSpawnExtraX = -0.1f;

    public Arrow arrow;
    public float attackSpeed = 3;
    private float attackTimer = 0;
    
    public int maxHealth;
    private int currentHealth;
    
    private int scoreValue = 1500;
    
    protected Rigidbody2D rb2d;
    protected Collider2D col;

    public bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        detectionRange = 10;
        maxHealth = 6;
        currentHealth = maxHealth;
        
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if (GetDetectedPlayer() && active)
        {
            attackTimer += Time.fixedDeltaTime;
            
            if (attackTimer > attackSpeed)
            {
                Vector2 ourPos = new Vector2(transform.position.x + arrowSpawnExtraX, transform.position.y + arrowSpawnExtraY);
                Vector2 theirPos = new Vector2(Globals.player.transform.position.x, Globals.player.transform.position.y);

                Vector2 arrowDirection = theirPos - ourPos;
                arrowDirection.Normalize();

                Arrow newArrow = Instantiate(arrow, ourPos, Quaternion.identity);
                newArrow.Direction = arrowDirection;

                attackTimer = 0;
            } 
        }
    }

    public override void OnHit(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die(new Vector2(0, 10));
        }
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
