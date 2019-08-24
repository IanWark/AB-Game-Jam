using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float lifetime = 10;
    public int damage = 10;
    public float speed = 10;

    private Vector2 direction; // Direction arrow is going

    public Vector2 Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;
            GetComponent<Rigidbody2D>().velocity = direction * speed;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.OnHit(damage);
            Destroy(gameObject);
        }
    }
}
