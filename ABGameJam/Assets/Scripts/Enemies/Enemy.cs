using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float detectionRange = 4;
    private bool detectedPlayer = false;

    protected AudioSource audioSource;

    // Can be set in inherited class to play on seeing player
    protected AudioClip detectPlayerSound;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected bool GetDetectedPlayer()
    {
        if (!detectedPlayer)
        {
            // Check if we can detect
            float playerDistance = Globals.player.transform.position.x - transform.position.x;
            if (Mathf.Abs(playerDistance) <= detectionRange)
            {
                detectedPlayer = true;

                if (detectPlayerSound != null)
                {
                    // 30% chance to play
                    if (0.3f >= Random.Range(0.0f, 1.0f))
                    PlaySoundWithRandomDelay(detectPlayerSound);
                }
            }
        }

        return detectedPlayer;       
    }

    // So that everyone is not screaming all at once
    public void PlaySoundWithRandomDelay(AudioClip clip)
    {
        audioSource.Stop();
        float delay = Random.Range(0.0f, 0.2f);
        StartCoroutine(PlaySoundAfterDelay(clip, delay));
    }

    public IEnumerator PlaySoundAfterDelay(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(clip);
    }

    public abstract void OnHit(int damage, Vector2 impactPoint);
}
