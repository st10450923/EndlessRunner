using System.Collections;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    private bool isSubscribed=false;
    public GameObject Angel;
    public GameObject Demon;
    public float SpawnDelay = 0f;
    public float BossDuration = 30f;
    public Vector3 SpawnOffset = new(-10, 2, 5);
    public int BasePoints = 100;
    public int PointsPerLevel = 10;

    private GameObject CurrentBoss;
    private float ZoneLevel=0;
    private Transform Player;

    private void Start() 
    {
        Player = GameObject.FindFirstObjectByType<PlayerControls>().transform;
    }
    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        if (isSubscribed) return;
        if (EventManager.Inst == null)
        {
            Invoke(nameof(SubscribeToEvents), 0.1f);
            return;
        }

        EventManager.Inst.OnZoneChanged += HandleZoneChanged;
        isSubscribed = true;
    }

    private void UnsubscribeFromEvents()
    {
        if (!isSubscribed) return;
        if (EventManager.Inst != null)
        {
            EventManager.Inst.OnZoneChanged -= HandleZoneChanged;
        }
        isSubscribed = false;
        CancelInvoke(nameof(SubscribeToEvents));
    }

    private void HandleZoneChanged(Zone newZone, Zone previousZone)
    {
        StopAllCoroutines();
        GameObject prefab = newZone == Zone.Heaven ? Angel : Demon;
        StartCoroutine(SpawnBoss(prefab, SpawnDelay));
    }
    private int CalculateBossPoints()
    {
        return BasePoints + (int)(PointsPerLevel*ZoneLevel);
    }

    private IEnumerator SpawnBoss(GameObject prefab, float delay)
    {
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
        ZoneLevel++;
        int points = CalculateBossPoints();
        EventManager.Inst.TriggerBossDefeated(CurrentBoss, points);
        EventManager.Inst.TriggerPointsGained(points);

        Destroy(CurrentBoss);
    }
}