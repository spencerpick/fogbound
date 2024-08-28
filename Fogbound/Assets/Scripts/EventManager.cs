using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{

    // ******  EVENTS ****** //

    public static event Action OnPuzzle_1_DoorOpen;


    // ****** METHODS ****** //

    public static void TriggerPuzzle_1_DoorOpen()
    {
        OnPuzzle_1_DoorOpen?.Invoke();
    }
}
