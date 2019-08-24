using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float detectionRange = 4;
    private bool detectedPlayer = false;

    protected bool GetDetectedPlayer()
    {
        if (!detectedPlayer)
        {
            // Check if we can detect
            float playerDistance = Globals.player.transform.position.x - transform.position.x;
            if (Mathf.Abs(playerDistance) <= detectionRange)
            {
                detectedPlayer = true;
            }
        }

        return detectedPlayer;       
    }

    public abstract void OnHit(int damage);
}
