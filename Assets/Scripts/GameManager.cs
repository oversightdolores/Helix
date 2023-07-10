

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public int bestScore;
    public int currentScore;
    public int currentLevel = 0;
    public AudioSource winAudio;
    
    

    public static GameManager singleton;


    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }

        bestScore = PlayerPrefs.GetInt("BestScore");
    }

   

    public void NextLevel()
    {
        winAudio.Play();
        currentLevel++;
        Object.FindAnyObjectByType<BallController>().ResetBall();
        Object.FindAnyObjectByType<HellixController>().LoadStage(currentLevel);
        //SceneManager.LoadScene(currentLevel)
    }

    public void RestartLevel()
    {

        Debug.Log("Reiniciar Nivel");
        singleton.currentScore = 0;
        Object.FindAnyObjectByType<BallController>().ResetBall();
        Object.FindAnyObjectByType<PassScorePoint>().ResetEllix();
        Object.FindAnyObjectByType<HellixController>().LoadStage(currentLevel);

        
    
        
    }

    public void AddScore(int scoreToAdd)
    {
        
        currentScore += scoreToAdd;

        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", currentScore);
        
        }
    }
}
