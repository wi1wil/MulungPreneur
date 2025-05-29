using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControllerScript : MonoBehaviour
{
    public static bool isGamePaused { get; private set; } = false;

    public static void setPaused(bool pause)
    {
        isGamePaused = pause;
    }
}
