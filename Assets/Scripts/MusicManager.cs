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
        PlayZoneMusic(Zone.Heaven);
    }

    private void OnEnable()
    {
        StartCoroutine(SubscribeWhenReady());
    }

    private IEnumerator SubscribeWhenReady()
    {
        if (isSubscribed) yield break;
        while (EventManager.Inst == null)
        {
            yield return null;
        }
        if (!isSubscribed)
        {
            EventManager.Inst.OnZoneChanged += HandleZoneChange;
            isSubscribed = true;
        }
    }
    private IEnumerator Unsubscribe()
    {
        if (!isSubscribed) yield break;
        if (EventManager.Inst != null)
        {
            EventManager.Inst.OnZoneChanged -= HandleZoneChange;
        }
        isSubscribed = false;

        CancelInvoke(nameof(SubscribeWhenReady));
        yield return null;
    }
    private void OnDisable()
    {
        StartCoroutine(Unsubscribe());
    }

    public void PlayZoneMusic(Zone zone)
    {
        MusicSource.clip = (zone == Zone.Heaven) ? HeavenMusic : HellMusic;
        MusicSource.Play();
    }

    private void HandleZoneChange(Zone newZone, Zone previousZone)
    {
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
