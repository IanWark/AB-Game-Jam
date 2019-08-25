using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfCivilian : Dwarf
{
    public Vector3 localScale;
    public Vector3 facingLeft;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        detectionRange = Random.Range(3.0f,4.0f);
        localScale = transform.localScale;
        facingLeft = transform.localScale;
        animator = GetComponent<Animator>();
        animator.Play("dwarf_civilian_idle");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active)
        {
            if (GetDetectedPlayer())
            {
                // RUN AWAY!
                
                if (localScale == facingLeft)
                {
                    localScale.x *= -1;
                    transform.localScale = localScale;
                }
                
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
