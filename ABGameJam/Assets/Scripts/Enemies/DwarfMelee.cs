using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfMelee : Dwarf
{
    public float closeEnoughDistance = 0.5f;
    static public float slowPlayerPercentage = 0.5f;
    static public int attackDamage = 5;
    static public float attackSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            float playerDistance = Globals.player.transform.position.x - transform.position.x;

            if (GetDetectedPlayer() && Mathf.Abs(playerDistance) >= closeEnoughDistance)
            {
                float xSpeed = (playerDistance > 0 ? 1 : -1) * moveSpeed;
                rb2d.MovePosition(rb2d.position + new Vector2(xSpeed * Time.deltaTime, 0));
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>(); 
        if (player != null)
        {
            // Attach to player to slow them and damage them periodically (handled in PlayerController)
            player.AddMeleeEnemy();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            // Detach from player
            player.RemoveMeleeEnemy();
        }
    }
}
