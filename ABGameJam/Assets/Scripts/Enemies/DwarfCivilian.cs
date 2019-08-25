using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfCivilian : Dwarf
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        animator.Play("dwarf_civilian_idle");
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (GetDetectedPlayer())
            {
                // RUN AWAY!
                animator.Play("dwarf_civilian_run");
                rb2d.MovePosition(rb2d.position + new Vector2(moveSpeed * Time.deltaTime, 0));
            }
        }
    }

    public override void DieAnimation()
    {
        animator.Play("dwarf_civilian_die");
    }
}
