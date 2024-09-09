using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarTrigger : MonoBehaviour
{
    public int pillarNumber; // Unique number for each pillar
    private Puzzle_1 puzzleManager;  // Reference to puzzle_1 script
    private HashSet<GameObject> toysOnPillar = new HashSet<GameObject>(); // Track toys on the pillar
    private Dictionary<GameObject, float> toyStabilizationTimers = new Dictionary<GameObject, float>(); // Timer for each toy

    private float stabilizationTime = 2.0f; // Toy must stay on the pillar for 2 seconds to be considered placed
    private float resetTime = 2.0f; // Time it takes for a toy to be removed from placement after exiting

    private void Start()
    {
        puzzleManager = FindObjectOfType<Puzzle_1>(); // Initialise reference to puzzle_1 script
    }

    private void Update()
    {
        List<GameObject> stabilizedToys = new List<GameObject>(); // List of which toys are considered to be on the pillars 

        // Decrease stabilization timers for each toy
        foreach (var entry in new Dictionary<GameObject, float>(toyStabilizationTimers)) 
        {
            toyStabilizationTimers[entry.Key] -= Time.deltaTime;

            if (toyStabilizationTimers[entry.Key] <= 0)
            {
                // Stablized meaning it has been on pillar for longer than 2 seconds and is now considered placed
                stabilizedToys.Add(entry.Key);
            }
        }

        // Once stabilized, mark toys as placed on the pillar
        foreach (var toy in stabilizedToys)
        {
            if (!toysOnPillar.Contains(toy))
            {
                toysOnPillar.Add(toy);
                Debug.Log($"Toy {toy.name} stabilized and placed on Pillar {pillarNumber}.");

                EventManager.TriggerToyPlacedOnPillar(toy, pillarNumber); // Trigger placement event

                toyStabilizationTimers.Remove(toy); // Remove from stabilization timer
            }
        }
    }

    private void OnTriggerEnter(Collider other) // What happens when a toy enters a pillars trigger
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Toy"))  // If the object is a toy
        {
            if (!toyStabilizationTimers.ContainsKey(other.gameObject)) 
            {
                toyStabilizationTimers[other.gameObject] = stabilizationTime; // Start stabilization timer
               // Debug.Log($"Toy {other.gameObject.name} entered Pillar {pillarNumber}. Stabilization timer started.");
            }
        }
    }

    private void OnTriggerExit(Collider other) // What happens when a toy leaves a pillars trigger
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Toy"))
        {
            if (toysOnPillar.Contains(other.gameObject))
            {
                StartCoroutine(DelayedToyRemoval(other.gameObject)); // Delay the removal slightly, as to make sure that its not just the player trying to place the toy on the pillar
            }

            // Remove from stabilization timer if it exits early
            if (toyStabilizationTimers.ContainsKey(other.gameObject))
            {
                toyStabilizationTimers.Remove(other.gameObject);
                //Debug.Log($"Toy {other.gameObject.name} left Pillar {pillarNumber} before stabilizing. Timer reset.");
            }
        }
    }

    private void OnTriggerStay(Collider other) // Called every frame that the given toy stays in the trigger
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Toy"))
        {
            if (!toyStabilizationTimers.ContainsKey(other.gameObject))
            {
                // If a toy re-enters the trigger after leaving, restart the timer
                toyStabilizationTimers[other.gameObject] = stabilizationTime;
                //Debug.Log($"Toy {other.gameObject.name} is staying in Pillar {pillarNumber}'s trigger.");
            }
        }
    }

   
    private IEnumerator DelayedToyRemoval(GameObject toy)  // Delayed removal to ensure  it isnt just leaving because the player is trying to place it on the pillar 
    {
        yield return new WaitForSeconds(resetTime);

        // After the delay, check if the toy is still in the pillars trigger
        if (!toyStabilizationTimers.ContainsKey(toy)) // If its no longer being stabilized
        {
            if (toysOnPillar.Contains(toy))
            {
                toysOnPillar.Remove(toy);
                Debug.Log($"Toy {toy.name} exited Pillar {pillarNumber} after delay. Removed from placement.");

                puzzleManager.RemoveToyFromPillar(pillarNumber, toy.name); // Toy has left pillar
            }
        }
    }
}
