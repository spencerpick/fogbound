using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{

    // ******  EVENTS ****** //

    public static event Action<string> OnThoughtUpdate;

    public static event Action<GameObject, float, float, bool, float> OnHighlightObject;

    public static event Action<GameObject, int> OnToyPlacedOnPillar;

    public static event Action OnPuzzle_1_DoorOpen;



    // ****** METHODS ****** //

    public static void TriggerThoughtUpdate(string newThought)
    {
        OnThoughtUpdate?.Invoke(newThought);
    }

    public static void TriggerHighlightObject(GameObject obj, float intensity, float range, bool highlight, float yPosOffset = 0f)
    {
        OnHighlightObject?.Invoke(obj, intensity, range, highlight, yPosOffset);
    }

    public static void TriggerToyPlacedOnPillar(GameObject toy, int pillarNumber)
    {
        OnToyPlacedOnPillar?.Invoke(toy, pillarNumber);
    }

    public static void TriggerPuzzle_1_DoorOpen()
    {
        OnPuzzle_1_DoorOpen?.Invoke();
    }




}
