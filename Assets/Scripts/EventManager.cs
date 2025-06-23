using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Inst { get; private set; }
    //Points Events
    public event Action<int> OnPointsGained;

    //Pick-up Events
    public event Action<int> OnDoubleJumpPickup;
    public event Action<int> OnShieldPickup;
    public event Action<int,float> OnPointsMultiplierPickup;
    
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

    //Zones
    public void TriggerZoneChange(Zone newZone, Zone previousZone)
    { 
        OnZoneChanged?.Invoke(newZone, previousZone);
    }
    //Bosses
    public void TriggerBossSpawned(GameObject boss, int points)
    {
        OnBossSpawned?.Invoke(boss, points);
    }
        
    public void TriggerBossDefeated(GameObject boss, int points)
    {
        OnBossDefeated?.Invoke(boss, points);
    }
    //Points
    public void TriggerPointsGained(int amount)
    {
        OnPointsGained?.Invoke(amount);
    }
    //Pick-ups
    public void TriggerDoubleJumpPickup(int duration)
    {
        OnDoubleJumpPickup?.Invoke(duration);
        Debug.Log($"Double jump pickup triggered for {duration}sec");
    }

    public void TriggerShieldPickup(int duration)
    {
        OnShieldPickup?.Invoke(duration);
        Debug.Log($"Shield pickup triggered for {duration}sec");
    }

    public void TriggerPointsMultiplierPickup(int duration,float multiplier)
    {
        OnPointsMultiplierPickup?.Invoke(duration,multiplier);
        //Debug.Log($"Points Multiplier pickup triggered for {duration}sec");
    }

}
