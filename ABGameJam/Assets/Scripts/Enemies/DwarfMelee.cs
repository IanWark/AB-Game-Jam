using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfMelee : Dwarf
{
    public float spawnHeight = -1.05f;
    static public float slowPlayerPercentage = 0.5f;
    static public int attackDamage = 1;
    static public float attackSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
