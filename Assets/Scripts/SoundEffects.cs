using UnityEngine;

/* Singleton for playing sound effects */
public class SoundEffects : MonoBehaviour
{
    private static SoundEffects instance;

    // component references
    public AudioListener listener;
    public AudioSource speaker;
    // sound effects
    public AudioClip damageLevel0;
    public AudioClip damageLevel1;
    public AudioClip damageLevel2;
    public AudioClip damageLevel3;
    public AudioClip damageLevel4;
    public AudioClip damageLevel5;
    public AudioClip healingChant;
    public AudioClip[] trapAssists;

    // audio clip accessors
    public static AudioClip HealingChant { get => instance.healingChant; }
    public static AudioClip DamageLevel0 { get => instance.damageLevel0; }
    public static AudioClip DamageLevel1 { get => instance.damageLevel1; }
    public static AudioClip DamageLevel2 { get => instance.damageLevel2; }
    public static AudioClip DamageLevel3 { get => instance.damageLevel3; }
    public static AudioClip DamageLevel4 { get => instance.damageLevel4; }
    public static AudioClip DamageLevel5 { get => instance.damageLevel5; }

    void Start()
    {
        instance = this;
        speaker.loop = false;
    }

    public static void Play(AudioClip clip)
    {
        instance.speaker.PlayOneShot(clip);
    }

    public static void PlayRandomTrapAssistClip()
    {
        Play(instance.trapAssists[Random.Range(0, instance.trapAssists.Length)]);
    }
}
