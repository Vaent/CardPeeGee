using System.Collections;
using UnityEngine;

public delegate void Callback();
public delegate void CallbackCard(Card card);
public delegate void CallbackInt(int i);

/* Singleton which provides "delay" functionality to classes which
do not themselves extend MonoBehaviour.
This script is attached to the EventSystem GameObject in Unity
and should not be attached to any other objects.
Additional delegate overloads may be added as required. */
public class Timer : MonoBehaviour
{
    private static Timer instance;

    void Start()
    {
        Timer.instance = this;
    }

    public static void DelayThenInvoke(float delaySeconds, Callback callback)
    {
        instance.StartCoroutine(instance.DelInv(delaySeconds, callback));
    }

    public static void DelayThenInvoke(float delaySeconds, CallbackCard callback, Card card)
    {
        instance.StartCoroutine(instance.DelInv(delaySeconds, callback, card));
    }

    public static void DelayThenInvoke(float delaySeconds, CallbackInt callback, int i)
    {
        instance.StartCoroutine(instance.DelInv(delaySeconds, callback, i));
    }

    private IEnumerator Delay(float seconds)
    {
        Debug.Log("Begin delay of " + seconds + " seconds");
        yield return new WaitForSeconds(seconds);
        Debug.Log("Delay complete");
    }

    private IEnumerator DelInv(float seconds, Callback callback)
    {
        yield return Delay(seconds);
        callback();
    }

    private IEnumerator DelInv(float seconds, CallbackCard callback, Card card)
    {
        yield return Delay(seconds);
        callback(card);
    }

    private IEnumerator DelInv(float seconds, CallbackInt callback, int i)
    {
        yield return Delay(seconds);
        callback(i);
    }
}
