using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfMelee : Dwarf
{
    public float closeEnoughDistance = 0.5f;
    static public float slowPlayerPercentage = 0.5f;
    static public int attackDamage = 5;
    static public float attackSpeed = 1;

    private bool attacking = false;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        animator.Play("dwarf_melee_idle");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active)
        {
            float playerDistance = Globals.player.transform.position.x - transform.position.x;

            if (GetDetectedPlayer() && Mathf.Abs(playerDistance) >= closeEnoughDistance)
            {
                float xSpeed = (playerDistance > 0 ? 1 : -1) * moveSpeed;
                rb2d.MovePosition(rb2d.position + new Vector2(xSpeed * Time.deltaTime, 0));

                if (!attacking) { animator.Play("dwarf_melee_run"); }
            }
        }
    }

    public override void DieAnimation()
    {
        animator.Play("dwarf_melee_die");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>(); 
        if (player != null)
        {
            // Attach to player to slow them and damage them periodically (handled in PlayerController)
            player.AddMeleeEnemy();
            attacking = true;
            animator.Play("dwarf_melee_attack");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            // Detach from player
            player.RemoveMeleeEnemy();
            attacking = false;
            animator.Play("dwarf_melee_idle");
        }
    }
}
