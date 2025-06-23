using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public enum Zone { Heaven, Hell }

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Inst { get; private set; }

    public float ZoneDuration = 30f;
    public GameObject HeavenShader;
    public GameObject HellShader;

    public Renderer Background;
    public Material HeavenBG;
    public Material HellBG;
    
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
            Destroy(this);
            return;
        }

        Inst = this;
       // DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ZoneCount = 0;
        //Debug.Log($"Scene {scene.name} loaded with mode {mode}");
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
    }

    public void SwitchZones()
    {
        PreviousZone = CurrentZone;
        if (ZoneCount < 2)
            CurrentZone = CurrentZone == Zone.Heaven ? Zone.Hell : Zone.Heaven;
        else
            CurrentZone = (Zone)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Zone)).Length);
        ZoneCount++;
        InitializeZone(CurrentZone);
        OnZoneChanged?.Invoke(CurrentZone, PreviousZone);
        //Debug.Log($"Switched to {CurrentZone} (Count: {ZoneCount})");
    }

    private void InitializeZone(Zone zone)
    {
        if (HeavenShader == null || HellShader == null)
        {
            Debug.Log("Null Shader");
            return;
        }

        switch (zone)
        {
            case Zone.Heaven:
                HeavenShader.SetActive(true);
                HellShader.SetActive(false);
                Background.material = HeavenBG;

                break;
            case Zone.Hell:
                HeavenShader.SetActive(false);
                HellShader.SetActive(true);
                Background.material = HellBG;
                break;
        }
    }

}