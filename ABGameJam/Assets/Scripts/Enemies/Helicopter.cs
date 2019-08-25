using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : Enemy
{
    public float spawnHeight = 0;
    private int scoreValue = 1000;

    public float accelerationPerSecond = 2.5f;
    public float maxSpeed = 2.5f;
    private float currentSpeed = 0;

    public float targetDistanceMin = 2.5f;
    public float targetDistanceMax = 3.5f;
    private float targetDistance;

    protected Rigidbody2D rb2d;
    protected Collider2D col;
    public bool active = true;

    public Arrow missile;
    public float attackSpeed = 3;
    private float attackTimer = 1;
    public float missileSpawnExtraY = -0.2f;
    public float missileSpawnExtraX = -0.1f;
    public float missileTargetExtraYMin = -0.5f;
    public float missileTargetExtraYMax = 0.5f;
    public AudioClip shootSound;
    public AudioClip deathSound;

    public float rotorPitchMin = 0.5f;
    public float rotorPitchMax = 0.8f;
    private AudioSource rotorSource;

    // Start is called before the first frame update
    void Start()
    {
        base.Awake();

        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rotorSource = GetComponentInChildren<AudioSource>();
        rotorSource.pitch = Random.Range(rotorPitchMin, rotorPitchMax);

        targetDistance = Random.Range(targetDistanceMin, targetDistanceMax);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetDetectedPlayer() && active)
        {
            Vector2 playerPos = new Vector2(Globals.player.transform.position.x, Globals.player.transform.position.y);
            Vector2 ourPos = new Vector2(transform.position.x, transform.position.y);

            // Move back if too close to player
            if (ourPos.x - playerPos.x < targetDistance)
            {
                // accelerate if not at full speed
                if (currentSpeed < maxSpeed) { currentSpeed = Mathf.Min(maxSpeed, currentSpeed + (accelerationPerSecond * Time.fixedDeltaTime)); }
                // Move
                rb2d.MovePosition(rb2d.position + new Vector2(currentSpeed * Time.deltaTime, 0));
            } else
            {
                // Decellerate
                currentSpeed = Mathf.Max(0, currentSpeed - (accelerationPerSecond * Time.fixedDeltaTime));
                // Move
                rb2d.MovePosition(rb2d.position + new Vector2(currentSpeed * Time.deltaTime, 0));
            }

            // Attack
            attackTimer += Time.fixedDeltaTime;

            if (attackTimer > attackSpeed)
            {
                // Shoot an arrow
                Vector2 missileSource = ourPos;
                missileSource.x += missileSpawnExtraX;
                missileSource.y += missileSpawnExtraY;
                Vector2 missileTarget = playerPos;
                missileTarget.y += Random.Range(missileTargetExtraYMin, missileTargetExtraYMax);

                Vector2 arrowDirection = missileTarget - missileSource;
                arrowDirection.Normalize();

                Arrow newArrow = Instantiate(missile, missileSource, Quaternion.identity);
                newArrow.Direction = arrowDirection;

                // Make a sound
                audioSource.pitch = Random.Range(0.6f, 0.8f);
                audioSource.PlayOneShot(shootSound);

                attackTimer = 0;
            }
        }
    }

    public override void OnHit(int damage, Vector2 impactPosition)
    {
        Die(new Vector2(0.90f, 0.10f) * 500);
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
        // Make sound
        DieSound();

        // Increase score
        Globals.player.score += scoreValue;

        // Start a timer to destroy object
        Destroy(gameObject, 5);
    }

    void DieSound()
    {
        rotorSource.Stop();
        audioSource.PlayOneShot(deathSound);
    }
}
