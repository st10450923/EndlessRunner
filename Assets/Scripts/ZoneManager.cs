using UnityEngine;
using System.Collections;

public enum Zone { Heaven, Hell }

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Inst { get; private set; }

    public float ZoneDuration = 120f;
    public GameObject HeavenShader;
    public GameObject HellShader;
    
    public GameObject Angel;
    public GameObject Demon;
    public float SpawnDelay = 60f;
    public float BossDuration = 60f;
    public float BossSpawnDistance = 200f;
    public int BossBasePoints = 100;
    public int BossPointsPerLevel = 10;
    private Vector3 BossOffset = new(-10, 2, 5);

    // Events
    public delegate void ZoneChangedHandler(Zone newZone, Zone previousZone);
    public event ZoneChangedHandler OnZoneChanged;
    public delegate void BossEventHandler(GameObject boss, int pointValue);

    private Zone CurrentZone;
    private Zone PreviousZone;
    private float ZoneAmount;
    private GameObject BossInst;
    private Transform Player;

    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }
        Inst = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Player = GameObject.FindFirstObjectByType<PlayerControls>().transform;
        CurrentZone = Zone.Heaven;
        InitializeZone(CurrentZone);
    }

    private void Update()
    {
        ZoneAmount += Time.deltaTime;
        if (ZoneAmount >= ZoneDuration)
        {
            SwitchZones();
            ZoneAmount = 0f;
        }
    }

    public void SwitchZones()
    {
        PreviousZone = CurrentZone;
        CurrentZone = CurrentZone == Zone.Heaven ? Zone.Hell : Zone.Heaven;
        ZoneAmount++;
        InitializeZone(CurrentZone);
        OnZoneChanged(CurrentZone, PreviousZone);
    }

    private void InitializeZone(Zone zone)
    {
        if (HeavenShader == null || HellShader == null)
        {
            Debug.LogError("Shader references not set in ZoneManager!");
            return;
        }

        switch (zone)
        {
            case Zone.Heaven:
                HeavenShader.SetActive(true);
                HellShader.SetActive(false);
                break;
            case Zone.Hell:
                HeavenShader.SetActive(false);
                HellShader.SetActive(true);
                break;
        }
    }

}