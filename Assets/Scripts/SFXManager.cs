using UnityEngine;
using UnityEngine.Rendering;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Inst;
    private AudioSource SFXSource;
    public float SFXVolume=0.25f;
    //PlayerSounds
    public AudioClip PlayerHit;
    public AudioClip PlayerBlock;
    public AudioClip PlayerPickup;
    //BossSounds
    public AudioClip BossAttack;
    public AudioClip BossSpawn;
    public AudioClip DemonSpawn;
    public AudioClip SpearThrowSound;

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
    public void PlaySFX(AudioClip clip,float ClipVolume)
    {
        if (clip != null)
            SFXSource.PlayOneShot(clip, Mathf.Clamp01(ClipVolume) * SFXVolume);
    }
}
