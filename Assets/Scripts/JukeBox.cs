using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBox : MonoBehaviour
{
    // component references
    public AudioListener listener;
    public AudioSource speaker;
    // music clips
    public AudioClip[] ambient;
    public AudioClip ambientIntro;
    public AudioClip[] combat;
    public AudioClip combatIntro;
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
        PlayAmbient();
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
        if (availableClips.Length <= 1)
        {
            nextClipIndex = 0;
        }
        else
        {
            do {
                nextClipIndex = random.Next(availableClips.Length);
            } while (nextClipIndex == currentIndex);
        }
    }

    // This overload of Play is used for playing a single clip on loop
    private void Play(AudioClip clip)
    {
        playRandomOnCompletion = false;
        speaker.loop = true;
        speaker.clip = clip;
        speaker.Play();
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

    public void PlayAmbient()
    {
        Play(ambientIntro, ambient);
    }

    public void PlayCombat()
    {
        Play(combatIntro, combat);
    }

    public void PlayHealer()
    {
        Play(healer);
    }

    public void PlayTrap()
    {
        Play(trap);
    }

    public void PlayTreasure()
    {
        Play(treasure);
    }
}
