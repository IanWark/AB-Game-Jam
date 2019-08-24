using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwarf : MonoBehaviour
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
        // TODO need to detect it is the player hitting us with a valid attack
        // TODO need to send a better force reacting to attack
        // TODO need to detect when to despawn
        Die(new Vector2(0, 200));
    }


}
