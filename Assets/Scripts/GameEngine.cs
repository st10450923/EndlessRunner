using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using System.Threading.Tasks;
using static UnityEditor.Experimental.GraphView.GraphView;


public class GameEngine : MonoBehaviour
{
    private static string LeaderboardID = "Leaderboard";

    private Transform Player;
    public float Points;
    public static GameEngine Inst;
    public bool Paused;
    //variables for UI
    public Text points;
    public Text FinalScore;
    public GameObject GameOverScreen;
    public GameObject Leaderboard;
    public GameObject Scoreboard;
    public GameObject PauseMenu;
    //Variables for pickups
    public float PointsMultiplier=1;
    public int PickupSpawnRate=1;


    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject); 
        }
        else
        {
            Inst = this;
            DontDestroyOnLoad(Inst); 
        }
    }
    private void Start()
    {
        Player = GameObject.FindFirstObjectByType<PlayerControls>().transform;
        Leaderboard.SetActive(false);
        GameOverScreen.SetActive(false);
        PauseMenu.SetActive(false);
    }

    private async void UpdateLeaderboard()
    {
        await UnityServices.InitializeAsync();
        if(!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardID, Points);
        await LeaderboardsService.Instance.GetScoresAsync(LeaderboardID);
    }

    public void AddPoints(int AddedPoints)
    {
        Points+=AddedPoints*PointsMultiplier;
        points.text = "Points: " + Points;
    }
    public void GameOver()
    {
        UpdateLeaderboard();
        Scoreboard.SetActive(false);
        Leaderboard.SetActive(true);
        GameOverScreen.SetActive(true);
    }
    public void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }
    public void Unpause()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }
    public void TogglePause()
    {
        if (Paused)
            Unpause();
        else
            Pause();
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Unpause();
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
