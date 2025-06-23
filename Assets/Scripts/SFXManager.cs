using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Inst;
    private AudioSource SFXSource;
    public float SFXVolume=0.1f;
    //PlayerSounds
    public AudioClip PlayerHit;
    public AudioClip PlayerBlock;
    public AudioClip PlayerPickup;
    //BossSounds
    public AudioClip BossAttack;
    public AudioClip BossSpawn;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            SFXSource = gameObject.AddComponent<AudioSource>();
            SFXSource.volume = SFXVolume;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            SFXSource.PlayOneShot(clip, SFXVolume);
    }
}
