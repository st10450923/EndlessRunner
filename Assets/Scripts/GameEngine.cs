using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameEngine : MonoBehaviour
{
    public int Points;
    public static GameEngine Inst;
    public Text points;
    public Text FinalScore;
    public GameObject GameOverScreen;
    public GameObject Scoreboard;

    private void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        GameOverScreen.SetActive(false);
    }

    void Update()
    {
        
    }
    public void IncrementScore()
    {
        Points++;
        points.text = "Points: " + Points;
    }
    public void GameOver()
    {
        FinalScore.text = "Final Score: " + Points;
        Scoreboard.SetActive(false);
        GameOverScreen.SetActive(true);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
