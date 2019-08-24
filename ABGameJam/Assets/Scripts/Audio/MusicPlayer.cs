using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip gameplayMusic;

    public AudioSource player1;

    // Start is called before the first frame update
    void Start()
    {
        player1.PlayOneShot(gameplayMusic);
    }

    // Update is called once per frame
    void Update()
    {
        if (!player1.isPlaying)
        {
            PlaySong(gameplayMusic);
        }
    }

    void PlaySong(AudioClip song)
    {
        player1.PlayOneShot(gameplayMusic);
    }
}
