using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dwarf : Enemy
{
    public float spawnHeight = -1.05f;
    public float moveSpeed = 1;

    protected Rigidbody2D rb2d;
    protected Collider2D col;
   
    public AudioClip dyingSound_f;
    public AudioClip dyingSound_m;
    public AudioClip dyingSound_w;
    private AudioClip dyingSound;

    public float chanceToWilhelm = 0.05f;
    public float voicePitchMin = 0.8f;
    public float voicePitchMax = 1.2f;
    protected int voice = 0;

    public bool active = true;
    
    private int scoreValue = 150;

    protected override void Awake()
    {
        base.Awake();

        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // Randomly generate which voice to use
        // 0 is female, 1 is male
        voice = Random.Range(0, 2);
        if (voice == 0)
        { dyingSound = dyingSound_f; }
        else
        { dyingSound = dyingSound_m; }
        SetVoice();

        audioSource.pitch = Random.Range(voicePitchMin, voicePitchMax);
    }

    // Set anything that depends on voice
    abstract protected void SetVoice();

    public override void OnHit(int damage, Vector2 impactPosition)
    {
        // Send enemies flying away from the impact
        Vector2 direction = new Vector2(transform.position.x, transform.position.y) - impactPosition;
        direction.Normalize();
        direction.y += 0.5f;

        Die(direction * 100);
    }

    public abstract void DieAnimation();

    protected void DieSound()
    {
        // Random chance to wilhelm scream
        if (Random.Range(0.0f, 1.0f) < chanceToWilhelm)
        {
            PlaySoundWithRandomDelay(dyingSound_w);
        }
        // 50% chance to play
        else if (0.5f >= Random.Range(0.0f, 1.0f))
        {
            PlaySoundWithRandomDelay(dyingSound);
        }
    }

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
        // Make sound
        DieSound();

        // Increase score
        Globals.player.score += scoreValue;

        // Start a timer to destroy object
        Destroy(gameObject, 5);
    }
}
