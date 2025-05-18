using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
public class GameEngine : MonoBehaviour
{
    private Transform Player;
    public float Points;
    public static GameEngine Inst;
    //variables for scoreboard
    public Text points;
    public Text FinalScore;
    public GameObject GameOverScreen;
    public GameObject Scoreboard;
    //Variables for pickups
    public float PointsMultiplier=1;
    public int PickupSpawnRate=1;
    //Variables for boss
    public GameObject Boss;
    private GameObject BossInstance;
    public float InitialSpawnDelay = 30f;
    public float BossSpawnDistance = 200f;
    private Vector3 BossOffset = new Vector3(-10, 2, 5);
    bool BossActive = false;
    private float DistanceSinceBoss=0;
    public float BossDuration=20f;


    private void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        Player = GameObject.FindFirstObjectByType<PlayerControls>().transform;
        GameOverScreen.SetActive(false);
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

    void SpawnBoss()
    {
        Vector3 BossPosition = Player.position + BossOffset;
        BossInstance = Instantiate(Boss, BossPosition,new Quaternion(0,0,0,0));
        Invoke("DestroyBoss",BossDuration);
        BossActive = true;
        DistanceSinceBoss = 0f;
    }
    void DestroyBoss()
    {
        Debug.Log("Destroy Boss!");
        Destroy(BossInstance);
        BossActive = false;
        AddPoints(250);
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
