using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 1000;
    
    public float speed = 5;

    public bool controlEnabled = true;
    
    private int currentHealth;

    private Rigidbody2D rb2d;

    void Awake()
    {
        Globals.player = this;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlEnabled)
        {
            rb2d.MovePosition(rb2d.position + new Vector2(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0));
        }
    }
    
    public void OnHit(int damage)
    {
        currentHealth -= damage;

        //healthSlider.value = currentHealth;
    }
}
