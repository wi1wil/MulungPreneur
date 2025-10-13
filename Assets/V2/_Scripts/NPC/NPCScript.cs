using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class NPCScript : MonoBehaviour, IInteractable
{
    public bool RequiresHold => false;
    public void Interact()
    {
        Debug.Log("Interacting with NPC");
    }
}
