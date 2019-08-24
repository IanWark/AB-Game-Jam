using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;

    public bool controlEnabled = true;

    private Rigidbody2D rb2d;

    void Awake()
    {
        Globals.player = this;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controlEnabled)
        {
            rb2d.MovePosition(rb2d.position + new Vector2(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0));
        }
    }
}
