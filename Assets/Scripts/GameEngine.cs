using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using System.Threading.Tasks;
using static UnityEditor.Experimental.GraphView.GraphView;
using System.Collections;


public class GameEngine : MonoBehaviour
{
    private static string LeaderboardID = "Leaderboard";

    private Transform Player;
    public float Points;
    public static GameEngine Inst;
    public bool Paused;
    private bool isSubscribed=false;
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
    private bool isPointBoostActive=false;

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
    private void OnEnable()
    {
        StartCoroutine(SubscribeWhenReady());
    }
    private void OnDisable()
    {
        StartCoroutine(Unsubscribe());
    }
    private IEnumerator SubscribeWhenReady()
    {
        while (EventManager.Inst == null)
        {
            yield return null;
        }

        if (!isSubscribed)
        {
            EventManager.Inst.OnPointsGained += AddPoints;
            EventManager.Inst.OnPointsMultiplierPickup += ActivatePointBoost;
            isSubscribed = true;
        }
    }
    private IEnumerator Unsubscribe()
    {
        if (EventManager.Inst != null)
        {
            EventManager.Inst.OnPointsGained -= AddPoints;
            EventManager.Inst.OnPointsMultiplierPickup -= ActivatePointBoost;
        }
        yield break; 
    }

    private void ActivatePointBoost(int duration, float multiplier)
    {
        if (isPointBoostActive)
        {
            CancelInvoke(nameof(EndPointBoost));
        }
        else
        {
            isPointBoostActive = true;
        }

        PointsMultiplier = multiplier;
        Invoke(nameof(EndPointBoost), duration);

    }
    private void EndPointBoost()
    {
        PointsMultiplier = 1f;
        isPointBoostActive = false;
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
    public void DeactivateDeathScreen()
    {
        GameOverScreen.SetActive(false);
        Leaderboard.SetActive(false);
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
        Unpause();
    }

}
