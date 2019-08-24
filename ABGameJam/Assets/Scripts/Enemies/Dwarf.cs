using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dwarf : Enemy
{
    Rigidbody2D rb2D;

    public bool active = true;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(int damage)
    {
        // TODO need to send a better force reacting to attack
        Die(new Vector2(0, 200));
    }

    void Die(Vector2 force)
    {
        // Stop AI
        active = false;
        // Start gravity
        rb2D.constraints = 0;
        // Launch dwarf
        rb2D.AddForce(force);

        // Start a timer to destroy object
        Destroy(gameObject, 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
