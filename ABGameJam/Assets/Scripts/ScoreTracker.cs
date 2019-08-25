using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{

    public int score;
    public static ScoreTracker instance;

    //If there is a score tracker, replace it with this one that is not deleted on scene change
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void GameOver(int endScore)
    {
        score = endScore;
        SceneManager.LoadScene("GameOver");
    }
}
