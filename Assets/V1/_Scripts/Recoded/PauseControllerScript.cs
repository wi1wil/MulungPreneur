using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControllerScript : MonoBehaviour
{
    public static bool isGamePaused { get; private set; } = false;
    private static PauseControllerScript instance; 

    private void Awake()
    {
        instance = this;
    }

    public static void setPaused(bool pause)
    {
        isGamePaused = pause;
    }
}
