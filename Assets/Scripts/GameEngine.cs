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
    //Variables for boss
    public GameObject Boss;
    private GameObject BossInstance;
    public int BossPointValue = 100;
    public float InitialSpawnDelay = 30f;
    public float BossSpawnDistance = 200f;
    private Vector3 BossOffset = new(-10, 2, 5);
    bool BossActive = false;
    private float DistanceSinceBoss=0;
    public float BossDuration=20f;
    private static string LeaderboardID = "Leaderboard";

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        Player = GameObject.FindFirstObjectByType<PlayerControls>().transform;
        Leaderboard.SetActive(false);
        GameOverScreen.SetActive(false);
        PauseMenu.SetActive(false);
        Invoke(nameof(SpawnBoss), InitialSpawnDelay);
    }

    void FixedUpdate()
    {

        if (!BossActive)
        {
            DistanceSinceBoss += Player.GetComponent<PlayerControls>().ForwardSpeed * Time.deltaTime;
            if (DistanceSinceBoss >= BossSpawnDistance)
            {
                SpawnBoss();
            }
        }
    }

    private async void UpdateLeaderboard()
    {
        await UnityServices.InitializeAsync();
        if(!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardID, Points);
        await LeaderboardsService.Instance.GetScoresAsync(LeaderboardID);
    }

    void SpawnBoss()
    {
        Vector3 BossPosition = Player.position + BossOffset;
        Quaternion BossRotaiton = Quaternion.Euler(50f,-14f,0f);
        BossInstance = Instantiate(Boss, BossPosition, BossRotaiton);
        Invoke("DestroyBoss",BossDuration);
        BossActive = true;
        DistanceSinceBoss = 0f;
    }
    void DestroyBoss()
    {
        Debug.Log("Destroyed Boss!");
        Destroy(BossInstance);
        BossActive = false;
        AddPoints(BossPointValue);
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
