using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarTrigger : MonoBehaviour
{
    public int pillarNumber; // Unique number for each pillar
    private Puzzle_1 puzzleManager;
    private HashSet<GameObject> toysOnPillar = new HashSet<GameObject>(); // Track toys on the pillar
    private Dictionary<GameObject, float> toyStabilizationTimers = new Dictionary<GameObject, float>(); // Timer for each toy

    private float stabilizationTime = 2.0f; // Toy must stay on the pillar for 2 seconds to be considered placed
    private float resetTime = 2.0f; // Time it takes for a toy to be removed from placement after exiting

    private void Start()
    {
        puzzleManager = FindObjectOfType<Puzzle_1>();
        if (puzzleManager == null)
        {
            Debug.LogError("Puzzle_1 not found in the scene.");
        }
    }

    private void Update()
    {
        List<GameObject> stabilizedToys = new List<GameObject>();

        // Decrease stabilization timers for each toy
        foreach (var entry in new Dictionary<GameObject, float>(toyStabilizationTimers)) // Safe iteration
        {
            toyStabilizationTimers[entry.Key] -= Time.deltaTime;

            if (toyStabilizationTimers[entry.Key] <= 0)
            {
                // Timer expired, consider the toy stabilized on the pillar
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

                // Trigger placement event
                EventManager.TriggerToyPlacedOnPillar(toy, pillarNumber);

                // Remove from stabilization timer
                toyStabilizationTimers.Remove(toy);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Toy"))
        {
            if (!toyStabilizationTimers.ContainsKey(other.gameObject)) // If not already tracking this toy
            {
                toyStabilizationTimers[other.gameObject] = stabilizationTime;
                Debug.Log($"Toy {other.gameObject.name} entered Pillar {pillarNumber}. Stabilization timer started.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Toy"))
        {
            if (toysOnPillar.Contains(other.gameObject))
            {
                // Delay the removal slightly, ensuring we check if the toy is still "staying" in the trigger
                StartCoroutine(DelayedToyRemoval(other.gameObject));
            }

            // Remove from stabilization timer if it exits early
            if (toyStabilizationTimers.ContainsKey(other.gameObject))
            {
                toyStabilizationTimers.Remove(other.gameObject);
                Debug.Log($"Toy {other.gameObject.name} left Pillar {pillarNumber} before stabilizing. Timer reset.");
            }
        }
    }

    // This method is called every frame for every object that stays within the trigger
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Toy"))
        {
            if (!toyStabilizationTimers.ContainsKey(other.gameObject))
            {
                // If a toy re-enters the trigger after leaving, restart the timer
                toyStabilizationTimers[other.gameObject] = stabilizationTime;
                Debug.Log($"Toy {other.gameObject.name} is staying in Pillar {pillarNumber}'s trigger.");
            }
        }
    }

    // Delayed removal to ensure the toy isn't just leaving for a very short time
    private IEnumerator DelayedToyRemoval(GameObject toy)
    {
        yield return new WaitForSeconds(resetTime);

        // After the delay, check if the toy is still in the pillar's trigger
        if (!toyStabilizationTimers.ContainsKey(toy)) // If it's no longer being stabilized
        {
            if (toysOnPillar.Contains(toy))
            {
                toysOnPillar.Remove(toy);
                Debug.Log($"Toy {toy.name} exited Pillar {pillarNumber} after delay. Removed from placement.");

                // Notify puzzle manager that toy has left the pillar
                puzzleManager.RemoveToyFromPillar(pillarNumber, toy.name);
            }
        }
    }
}
