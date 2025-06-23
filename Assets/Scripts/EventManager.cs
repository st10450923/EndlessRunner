using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Inst { get; private set; }
    //Points Events
    public event Action<int> OnPointsGained;
    //Boss Events
    public delegate void BossEventHandler(GameObject boss, int pointValue);
    public event BossEventHandler OnBossSpawned;
    public event BossEventHandler OnBossDefeated;
    //Zone Events
    public delegate void ZoneChangedHandler(Zone newZone, Zone previousZone);
    public event ZoneChangedHandler OnZoneChanged;

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

    public void TriggerZoneChange(Zone newZone, Zone previousZone)
    { 
        OnZoneChanged?.Invoke(newZone, previousZone);
    }
    public void TriggerBossSpawned(GameObject boss, int points)
    {
        OnBossSpawned?.Invoke(boss, points);
    }
        
    public void TriggerBossDefeated(GameObject boss, int points)
    {
        OnBossDefeated?.Invoke(boss, points);
    }
        
    public void TriggerPointsGained(int amount)
    {
        OnPointsGained?.Invoke(amount);
    }
}
