using System;
using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private bool isSubscribed=false;
    public AudioClip HeavenMusic;
    public AudioClip HellMusic;
    public float MusicVolume = 0.5f;
    private AudioSource MusicSource;
    public float MusicFadeDuration=5;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }

        MusicSource = gameObject.AddComponent<AudioSource>();
        MusicSource.loop = true;
        MusicSource.volume = MusicVolume;
    }
    private void Start()
    {
        PlayZoneMusic(Zone.Hell);
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

    public void PlayZoneMusic(Zone zone)
    {
        MusicSource.clip = (zone == Zone.Heaven) ? HeavenMusic : HellMusic;
        MusicSource.Play();
    }

    private void HandleZoneChange(Zone newZone, Zone previousZone)
    {
        Debug.Log("MusicManager Zone Changed");
        switch (newZone)
        {
            case Zone.Heaven:
                FadeToMusic(HeavenMusic,MusicFadeDuration);
                break;
            case Zone.Hell:
                FadeToMusic(HellMusic, MusicFadeDuration);
                break;
        }
    }

    public IEnumerator FadeToMusic(AudioClip newClip, float fadeTime = 3f)
    {
        float startVolume = MusicSource.volume;
        float elapsed = 0;

        while (elapsed < fadeTime)
        {
            MusicSource.volume = Mathf.Lerp(startVolume, 0, elapsed / fadeTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        MusicSource.clip = newClip;
        MusicSource.Play();
        elapsed = 0;
        while (elapsed < fadeTime)
        {
            MusicSource.volume = Mathf.Lerp(0, startVolume, elapsed / fadeTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
