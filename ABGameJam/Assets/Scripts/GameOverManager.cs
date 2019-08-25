using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public Text scoretext;
    public ScoreTracker tracker;
    
    public AudioClip deathMusic;
    public AudioSource player2;
    
    // Start is called before the first frame update
    void Awake()
    {
        scoretext = FindObjectOfType<Text>();
        tracker = FindObjectOfType<ScoreTracker>();
        scoretext.text = tracker.score.ToString("D9");
        
        player2.volume = 0.7f;
        player2.PlayOneShot(deathMusic);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!player2.isPlaying)
        {
            PlaySong(deathMusic);
        }
    }

    void PlaySong(AudioClip deathMusic)
    {
        player2.PlayOneShot(deathMusic);
    }

    //exit the game completely
    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
