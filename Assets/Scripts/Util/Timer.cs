using System.Collections;
using UnityEngine;

public delegate void Callback();
public delegate void Callback<T>(T t);

/* Singleton which provides "delay" functionality to classes which
do not themselves extend MonoBehaviour.
This script is attached to the EventSystem GameObject in Unity
and should not be attached to any other objects. */
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

    public static void DelayThenInvoke<T>(float delaySeconds, Callback<T> callback, T t)
    {
        instance.StartCoroutine(instance.DelInv(delaySeconds, callback, t));
    }

    private IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private IEnumerator DelInv(float seconds, Callback callback)
    {
        yield return Delay(seconds);
        callback();
    }

    private IEnumerator DelInv<T>(float seconds, Callback<T> callback, T t)
    {
        yield return Delay(seconds);
        callback(t);
    }
}
