using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloadTitle : MonoBehaviour
{
    public AudioSource audioSource;
    public Canvas canvas;
    public AudioClip explode;
    public AudioClip fanfare;

    public void WakeUp()
    {
        canvas.enabled = true;
        audioSource.PlayOneShot(explode);
        StartCoroutine(PauseThenFanfare());
    }

    IEnumerator PauseThenFanfare()
    {
        yield return new WaitForSecondsRealtime(0.5F);
        audioSource.PlayOneShot(fanfare);
    }
}
