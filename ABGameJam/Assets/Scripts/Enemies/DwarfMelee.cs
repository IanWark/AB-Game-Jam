﻿using System.Collections;
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

    public AudioClip battlecrySound_f;
    public AudioClip battlecrySound_m;
    public AudioClip attackSound;

    void Start()
    {
        detectionRange = Random.Range(3.5f,4.0f);
        animator = GetComponent<Animator>();
        animator.Play("dwarf_melee_idle");
    }

    // Set anything that depends on voice
    protected override void SetVoice()
    {
        if (voice == 0)
        { detectPlayerSound = battlecrySound_f; }
        else
        { detectPlayerSound = battlecrySound_m; }
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

    public void AttackHit()
    {
        // 85% chance to play
        if(0.85f > Random.Range(0.0f, 1.0f) )
        {
            audioSource.PlayOneShot(attackSound);
        }
    }
}
