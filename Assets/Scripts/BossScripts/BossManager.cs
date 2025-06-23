using System.Collections;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager Inst { get; private set; }
    private bool isSubscribed=false;
    public GameObject Angel;
    public GameObject Demon;
    public float SpawnDelay = 15f;
    public float BossDuration = 15f;
    public Vector3 SpawnOffset = new(-10, 2, 5);
    public int BasePoints = 100;
    public int PointsPerLevel = 10;

    private GameObject CurrentBoss;
    private float ZoneLevel=0;
    private Transform Player;
    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start() 
    {
        Player = GameObject.FindFirstObjectByType<PlayerControls>().transform;
    }
    private void OnEnable()
    {
        StartCoroutine(SubscribeWhenReady());
    }

    private IEnumerator SubscribeWhenReady()
    {
        while (ZoneManager.Inst == null)
        {
            yield return null;
        }
        ZoneManager.Inst.OnZoneChanged += HandleZoneChange;
        HandleZoneChange(ZoneManager.Inst.CurrentZone, Zone.Heaven);
    }

    private void OnDisable()
    {
        if (isSubscribed && ZoneManager.Inst != null)
        {
            ZoneManager.Inst.OnZoneChanged -= HandleZoneChange;
        }
        StopAllCoroutines();
    }


    private void HandleZoneChange(Zone newZone, Zone previousZone)
    {
        Debug.Log("BossManager Zone Changed");
        StopAllCoroutines();
        if (CurrentBoss != null)
        {
            Destroy(CurrentBoss);
            CancelInvoke(nameof(DefeatBoss)); 
        }
        GameObject prefab = newZone == Zone.Heaven ? Angel : Demon;
        StartCoroutine(SpawnBoss(prefab, SpawnDelay));

        ZoneLevel++;
    }

    private int CalculateBossPoints()
    {
        return BasePoints + (int)(PointsPerLevel*ZoneLevel);
    }

    private IEnumerator SpawnBoss(GameObject prefab, float delay)
    {
        Player = GameObject.FindFirstObjectByType<PlayerControls>().transform;
        yield return new WaitForSeconds(delay);
        if (CurrentBoss != null)
            Destroy(CurrentBoss);
        CurrentBoss = Instantiate(prefab,Player.position + SpawnOffset,Quaternion.Euler(50f, -14f, 0f));
        EventManager.Inst.TriggerBossSpawned(CurrentBoss, CalculateBossPoints());
        Invoke(nameof(DefeatBoss), BossDuration);
    }

    private void DefeatBoss()
    {
        if (!CurrentBoss) return;
        int points = CalculateBossPoints();
        EventManager.Inst.TriggerBossDefeated(CurrentBoss, points);
        EventManager.Inst.TriggerPointsGained(points);

        Destroy(CurrentBoss);
    }
}