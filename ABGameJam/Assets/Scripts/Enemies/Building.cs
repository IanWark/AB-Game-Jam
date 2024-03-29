﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Enemy
{
    public float spawnHeight = -0.65f;
    public float arrowSpawnExtraY = 0.4f;
    public float arrowSpawnExtraX = -0.1f;

    public Arrow arrow;
    public float attackSpeed = 3;
    private float attackTimer = 1;
    
    public int maxHealth;
    private int currentHealth;
    
    private int scoreValue = 1500;
    
    protected Rigidbody2D rb2d;
    protected Collider2D col;
    protected SpriteRenderer spriteRenderer;

    public bool active = true;

    // For spawning dead Ranged Dwarves
    public int numDwarves;
    public DwarfRanged dwarfRanged;
    public Sprite emptySprite;
    public AudioClip takeDamageSound;
    public AudioClip dieSound;
    public AudioClip shootSound;

    // Start is called before the first frame update
    void Start()
    {
        detectionRange = Random.Range(9.0f,10.0f);
        maxHealth = 60;
        currentHealth = maxHealth;
        
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (GetDetectedPlayer() && active)
        {
            attackTimer += Time.fixedDeltaTime;
            
            if (attackTimer > attackSpeed)
            {
                // Shoot an arrow
                Vector2 ourPos = new Vector2(transform.position.x + arrowSpawnExtraX, transform.position.y + arrowSpawnExtraY);
                Vector2 theirPos = new Vector2(Globals.player.transform.position.x, Globals.player.transform.position.y);

                Vector2 arrowDirection = theirPos - ourPos;
                arrowDirection.Normalize();

                Arrow newArrow = Instantiate(arrow, ourPos, Quaternion.identity);
                newArrow.Direction = arrowDirection;

                // Make a sound
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(shootSound);

                attackTimer = 0;
            } 
        }
    }

    public override void OnHit(int damage, Vector2 impactPoint)
    {
        currentHealth -= damage;
        Globals.mainCamera.Shake(true);
        // Damage sound
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(takeDamageSound);
        }

        if (currentHealth <= 0)
        {
            //Globals.mainCamera.Shake(true);
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
        // Launch building
        rb2d.AddForce(force);
        // Make building look empty
        spriteRenderer.sprite = emptySprite;
        // Make sound
        audioSource.PlayOneShot(dieSound);
        // Spawn dwarf(s)
        // Adding 0.5f makes them spawn at the top of the building
        Spawn(dwarfRanged, transform.position.x, transform.position.y + 0.5f, 1);

        // Increase score
        Globals.player.score += scoreValue;

        // Start a timer to destroy object
        Destroy(gameObject, 5);
    }
    
    void Spawn(Enemy prefab, float spawnX, float spawnY, int numberToSpawn)
    {
        for (int i = 0; i < numberToSpawn; ++i)
        {
            Instantiate(prefab, new Vector2(spawnX, spawnY), gameObject.transform.rotation);
        }
    }
}
