using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{

    [SerializeField] bool CompletePuzzle = false;

    [SerializeField] private GameObject objectToHighlight;

    // Start is called before the first frame update
    void Start()
    {
        HighlightObject(3f, 5f, true, 1.3f);
    }

    void HighlightObject(float intensity, float range, bool highlight, float yPosOffset = 0)
    {
        EventManager.TriggerHighlightObject(objectToHighlight, intensity, range, highlight, yPosOffset);
    }

    // Update is called once per frame
    void Update()
    {
        if (CompletePuzzle == true)
        {
            PuzzleSolved();
        }
    }
    void PuzzleSolved()
    {
        EventManager.TriggerPuzzle_1_DoorOpen(); // Trigger the door open event
        EventManager.TriggerThoughtUpdate("Hello this is a testttt");
        EventManager.TriggerHighlightObject(objectToHighlight, 3f, 5f, false, 1.3f);
    }
}
