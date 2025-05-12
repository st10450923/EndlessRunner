using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameEngine : MonoBehaviour
{
    public float Points;
    public static GameEngine Inst;
    public Text points;
    public Text FinalScore;
    public GameObject GameOverScreen;
    public GameObject Scoreboard;
    public float PointsMultiplier=1;
    public int PickupSpawnRate=1;

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
    public void AddPoints(int AddedPoints)
    {
        Points+=AddedPoints*PointsMultiplier;
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
