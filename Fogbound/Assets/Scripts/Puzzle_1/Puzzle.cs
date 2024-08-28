using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{

    [SerializeField] bool CompletePuzzle = false;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
