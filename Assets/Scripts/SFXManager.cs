using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Inst;
    private AudioSource audioSource;
    public float Volume=0.1f;
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
            audioSource = GetComponent<AudioSource>();
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
            audioSource.PlayOneShot(clip, Volume);
    }
}
