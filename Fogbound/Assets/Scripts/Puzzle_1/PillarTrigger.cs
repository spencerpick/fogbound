using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarTrigger : MonoBehaviour
{
    public int pillarNumber; // The number of the pillar this is attached to
    private bool isToyOnPillar = false; // Track whether toy is on pillar or not
    private Puzzle_1 puzzleManager;

    private void Start()
    {
        
        puzzleManager = FindObjectOfType<Puzzle_1>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Toy") && isToyOnPillar == false)
        {
            Rigidbody toyRb = other.GetComponent<Rigidbody>(); // Get the RB of the toy

            if (toyRb != null && !toyRb.isKinematic && !isToyOnPillar) // Check if the toy is being held currently (kinematic is set to false)
            {
                Debug.Log("SNAPPING TO PILLAR");
                EventManager.TriggerToyPlacedOnPillar(other.gameObject, pillarNumber); // Snap the toy to the pillar
                isToyOnPillar = true;

                // Check if the toy is on the correct pillar
                bool isCorrectPillar = puzzleManager.IsToyOnCorrectPillar(other.gameObject, pillarNumber);
                if (isCorrectPillar)
                {
                    Debug.Log(other.gameObject.name + " is on the correct pillar: " + pillarNumber);

                    // Here you could call a method to check puzzle completion
                    //puzzleManager.CheckPuzzleCompletition();
                }
                else
                {
                    Debug.Log(other.gameObject.name + " is on the wrong pillar.");
                }
            }
       
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isToyOnPillar && other.gameObject.layer == LayerMask.NameToLayer("Toy"))
        {
            isToyOnPillar = false;
        }
    }
}
