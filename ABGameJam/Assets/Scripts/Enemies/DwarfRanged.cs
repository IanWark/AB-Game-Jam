using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfRanged : Dwarf
{
    //public int attackSpeed;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        // Spawn from destroyed buildings that appeared to have ranged dwarves in them
        dieNow();
    }

    // A combination of OnHit() and Die()
    void dieNow()
    {
        // Send enemies flying away from the destroyed building
        // Aim
        Vector2 randomMod = new Vector2(transform.position.x + Random.Range(-10.0f, 10.0f), transform.position.y + Random.Range(-10.0f, 0.0f));
        Vector2 direction = new Vector2(transform.position.x, transform.position.y) - randomMod;
        direction.Normalize();
        direction.y += 0.5f;

        // Launch
        rb2d.AddForce(direction * 150);
        
        // Start a timer to destroy object
        Destroy(gameObject, 5);
    }
    
    public override void DieAnimation()
    {
        animator.Play("dwarf_ranged_die");
    }
}
