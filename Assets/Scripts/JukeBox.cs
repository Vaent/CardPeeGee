using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Singleton which manages background music */
public class JukeBox : MonoBehaviour
{
    private static JukeBox instance;

    // component references
    public AudioListener listener;
    public AudioSource speaker;
    // music clips
    public AudioClip[] ambient;
    public AudioClip ambientIntro;
    public AudioClip[] battle;
    public AudioClip battleIntro;
    public AudioClip healer;
    public AudioClip trap;
    public AudioClip treasure;

    // state variables
    private AudioClip[] availableClips;
    private int nextClipIndex;
    private bool playRandomOnCompletion;
    private System.Random random = new System.Random();

    void Start()
    {
        instance = this;
        Play(Track.Ambient);
    }

    void Update()
    {
        if (playRandomOnCompletion && !speaker.isPlaying)
        {
            speaker.clip = availableClips[nextClipIndex];
            speaker.Play();
            CalculateNextClipIndex(nextClipIndex);
        }
    }

    // Use this method to avoid randomly selecting the same clip repeatedly
    private void CalculateNextClipIndex(int currentIndex)
    {
        if ((availableClips is null) || (availableClips.Length == 0))
        {
            Debug.LogError("JukeBox cannot select a random clip as no clips are available");
        }
        else if (availableClips.Length == 1)
        {
            if (currentIndex == 0)
            {
                Debug.Log("JukeBox switching to loop mode as only one clip is available for selection");
                speaker.loop = true;
                playRandomOnCompletion = false;
            }
            else
            {
                nextClipIndex = 0;
            }
        }
        else
        {
            do {
                nextClipIndex = random.Next(availableClips.Length);
            } while (nextClipIndex == currentIndex);
        }
    }

    private IEnumerator FadeOut()
    {
        if (speaker.isPlaying)
        {
            while (speaker.volume > 0)
            {
                speaker.volume -= 0.02f;
                yield return new WaitForFixedUpdate();
            }
            KillTheMusic();
            speaker.volume = 1;
        }
    }

    private IEnumerator FadeOutThenPlay(AudioClip clip)
    {
        yield return FadeOut();
        Play(clip);
    }

    private IEnumerator FadeOutThenPlay(AudioClip intro, AudioClip[] mainClips)
    {
        yield return FadeOut();
        Play(intro, mainClips);
    }

    public static void KillTheMusic()
    {
        instance.speaker.Stop();
    }

    // This overload of Play is used for playing a single clip on loop
    private void Play(AudioClip clip)
    {
        playRandomOnCompletion = false;
        speaker.loop = true;
        speaker.clip = clip;
        speaker.Play();
        availableClips = null;
    }

    // This overload of Play is used for randomised multi-clip tracks
    private void Play(AudioClip intro, AudioClip[] mainClips)
    {
        speaker.loop = false;
        availableClips = mainClips;
        playRandomOnCompletion = true;
        speaker.clip = intro;
        speaker.Play();
        CalculateNextClipIndex(-1);
    }

    public static void Play(Track track)
    {
        switch (track)
        {
            case Track.Ambient:
                instance.StartCoroutine(instance.FadeOutThenPlay(instance.ambientIntro, instance.ambient));
                break;
            case Track.Battle:
                instance.StartCoroutine(instance.FadeOutThenPlay(instance.battleIntro, instance.battle));
                break;
            case Track.Healer:
                instance.StartCoroutine(instance.FadeOutThenPlay(instance.healer));
                break;
            case Track.Trap:
                instance.StartCoroutine(instance.FadeOutThenPlay(instance.trap));
                break;
            case Track.Treasure:
                instance.StartCoroutine(instance.FadeOutThenPlay(instance.treasure));
                break;
        }
    }

    public enum Track
    {
        Ambient,
        Battle,
        Healer,
        Trap,
        Treasure
    }
}
