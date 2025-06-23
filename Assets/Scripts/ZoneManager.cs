using UnityEngine;
using System.Collections;
using System;

public enum Zone { Heaven, Hell }

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Inst { get; private set; }

    public float ZoneDuration = 30f;
    public GameObject HeavenShader;
    public GameObject HellShader;
    
    public GameObject Angel;
    public GameObject Demon;
    public float SpawnDelay = 15f;
    public float BossDuration = 15f;
    public float BossSpawnDistance = 200f;
    public int BossBasePoints = 100;
    public int BossPointsPerLevel = 10;

    // Events
    public delegate void ZoneChangedHandler(Zone newZone, Zone previousZone);
    public event ZoneChangedHandler OnZoneChanged;
    public delegate void BossEventHandler(GameObject boss, int pointValue);

    private float ZoneTimer; 
    private int ZoneCount;
    public Zone CurrentZone;
    public Zone PreviousZone;
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
        CurrentZone = Zone.Hell;
        SwitchZones();
    }

    private void Update()
    {
        ZoneTimer += Time.deltaTime;
        if (ZoneTimer >= ZoneDuration)
        {
            SwitchZones();
            ZoneTimer = 0f; 
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            SwitchZones();
        }
    }

    public void SwitchZones()
    {
        PreviousZone = CurrentZone;
        CurrentZone = CurrentZone == Zone.Heaven ? Zone.Hell : Zone.Heaven;
        ZoneCount++; 

        InitializeZone(CurrentZone);
        OnZoneChanged?.Invoke(CurrentZone, PreviousZone);
        Debug.Log($"Switched to {CurrentZone} (Count: {ZoneCount})");
    }

    private void InitializeZone(Zone zone)
    {
        if (HeavenShader == null || HellShader == null)
        {
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