using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("game");
    }
}
