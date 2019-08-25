using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public Text scoretext;
    public ScoreTracker tracker;
    // Start is called before the first frame update
    void Awake()
    {
        scoretext = FindObjectOfType<Text>();
        tracker = FindObjectOfType<ScoreTracker>();
        scoretext.text = tracker.score.ToString("D9");
    }

    //exit the game completely
    public void ExitGame()
    {
        print("quit");
        Application.Quit();
    }

    public void RestartGame()
    {
        //score = 0;
        print("restart");
        SceneManager.LoadScene("GameScene");
    }
}
