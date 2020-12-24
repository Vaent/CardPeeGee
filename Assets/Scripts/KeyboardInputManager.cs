using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown("d")) GameState.Next();
    }
}
