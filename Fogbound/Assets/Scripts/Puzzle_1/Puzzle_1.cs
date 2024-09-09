﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_1 : MonoBehaviour
{

    [SerializeField] bool CompletePuzzle = false; // Whether puzzle is complete or not
    [SerializeField] private Transform[] spawnPoints; // Stores all references to different spawn point objects
    [SerializeField] private GameObject[] toyPrefabs; // Stores the toy game objects so they can be spawned in

    private Dictionary<string, int> toyAssignments = new Dictionary<string, int>();  // Stores which toy should go on which pillar
    public Dictionary<int, string> currentToyPlacement = new Dictionary<int, string>(); // Stores which toys are currently on which pillars
    private Dictionary<string, bool> toyPlacementStatus = new Dictionary<string, bool>();  // Stores whether toy is on its correct pillar or not

    void Start()
    {
        GiveToysRandomNumbers(); // Assign each toy a random number between 1 - 6
        SpawnToys(); // Spawn the toys on randomised spawn points between 1 - 6

        // Initalize all toy placement statuses to be false as they are not correctly placed on pillars
        foreach (var toy in toyAssignments.Keys)
        {
            toyPlacementStatus[toy] = false;
        }
    }

    void OnEnable()
    {
        EventManager.OnToyPlacedOnPillar += HandleToyPlacedOnPillar;
    }

    void OnDisable()
    {
        EventManager.OnToyPlacedOnPillar -= HandleToyPlacedOnPillar;
    }

    private void GiveToysRandomNumbers() // Asign a unique pillar number to each toy
    {
        string[] toyNames = { "Bear", "Duck", "Rabbit", "Monkey", "Penguin", "Pig" }; // All the names of the toys

        List<int> availableNumbers = new List<int> { 1, 2, 3, 4, 5, 6 }; // Available numbers for toys to be assigned to

        foreach (string toyName in toyNames)
        {
            int randomIndex = Random.Range(0, availableNumbers.Count); // Pick a random number from the available numbers list
            int assignedNumber = availableNumbers[randomIndex]; // Set that number to be the assigned number that will be given to the current toy
            availableNumbers.RemoveAt(randomIndex); // Remove that number from the available numbers list so it doesn't get chosen again

            toyAssignments[toyName] = assignedNumber; // Set the number to the current toy in the dictionary

            //Debug.Log($"{toyName} assigned to pillar {assignedNumber}");

        }
        
    }

    public int GetAssignedNumber(string toyName) // Retrieve which pillar number is associated with a given toy
    { 
        if (toyAssignments.TryGetValue(toyName, out int assignedNumber)) // Try get pillar number associated with toyname
        {
            return assignedNumber; // If the toyname exists then return its pillar value
        }

        return -1; // Return -1 to indicate an error if the toy wasn't found
    }

    private void SpawnToys() // Spawn toys at their corresponding spawn points
    {
        foreach (var toyAssignment in toyAssignments)
        {
            //Debug.Log($"{toyAssignment.Key} should be placed on pillar {toyAssignment.Value}");
            string toyName = toyAssignment.Key;
            int assignedPillar = toyAssignment.Value;

            // Find the toy prefab that corresponds to the toyName
            GameObject toyPrefab = GetToyPrefabByName(toyName);

            if (toyPrefab != null && assignedPillar >= 1 && assignedPillar <= 6)
            {
                // Spawn the toy at the corresponding spawn point based on the assigned pillar
                Transform spawnPoint = spawnPoints[assignedPillar - 1];
                GameObject spawnedToy = Instantiate(toyPrefab, spawnPoint.position, spawnPoint.rotation);

                spawnedToy.name = toyName; // Remove clone bit from name

                //Debug.Log($"Spawned {toyName} at spawn point for pillar {assignedPillar}");
            }
            else
            {
               // Debug.LogError($"Failed to spawn {toyName} at spawn point {assignedPillar}");
            }
        }
    }

    private GameObject GetToyPrefabByName(string toyName) // Used to get the toyprefab so they can be used to spawn in the toys
    {
        foreach (var toyPrefab in toyPrefabs)
        {
            if (toyPrefab.name == toyName)
            {
                return toyPrefab;
            }
        }
        return null;
    }


private void SnapToyToPillar(GameObject toy, int pillarNumber) // Snap a toy into position so it is rotated nicely when placed on pillar
    {
        GameObject pillar = GameObject.Find("Pillar_" + pillarNumber); // Find the pillar object based on its associated number

        if (pillar != null) // Ensure the pillar exists to avoid errors 
        {
            // Snap the toy to the top of the pillar
            toy.transform.position = pillar.transform.position + new Vector3(0, 1.5f, 0); // Set y position
            toy.transform.rotation = Quaternion.Euler(0, 90, 0);

        }
    }

    private void HandleToyPlacedOnPillar(GameObject toy, int pillarNumber) // Handles logic for when a toy enters the trigger on a pillar
    {
        string toyName = toy.name;
        int assignedNumber = GetAssignedNumber(toyName);

        // Remove toy from its previous pillar if already placed elsewhere
        if (currentToyPlacement.ContainsValue(toyName))
        {
            int previousPillar = -1;
            foreach (var kvp in currentToyPlacement)
            {
                if (kvp.Value == toyName && kvp.Key != pillarNumber)
                {
                    previousPillar = kvp.Key;
                    break;
                }
            }
            if (previousPillar != -1)
            {
                currentToyPlacement.Remove(previousPillar);
            }
        }

        // Remove any toy already on the current pillar
        if (currentToyPlacement.ContainsKey(pillarNumber))
        {
            string existingToy = currentToyPlacement[pillarNumber];
            Debug.Log($"Pillar {pillarNumber} already has {existingToy}. Removing it.");
            toyPlacementStatus[existingToy] = false;  // Mark the previous toy as not placed correctly
            currentToyPlacement.Remove(pillarNumber);
        }
        
        SnapToyToPillar(toy, pillarNumber); // Snap the toy to the pillar

        currentToyPlacement[pillarNumber] = toyName; // Update currentToyPlacement

        // Update toyPlacementStatus
        if (assignedNumber == pillarNumber)
        {
            toyPlacementStatus[toyName] = true;
            Debug.Log($"{toyName} placed correctly on pillar {pillarNumber}.");
        }
        else
        {
            toyPlacementStatus[toyName] = false;
            Debug.Log($"{toyName} placed on the wrong pillar {pillarNumber}.");
        }
        
        CheckPuzzleCompletition(); // Check puzzle completion
    }




    public bool IsToyOnCorrectPillar(GameObject toy, int pillarNumber)
    {
        string toyName = toy.name; // Get the toy's name

        if (toyAssignments.TryGetValue(toyName, out int correctPillar))
        {
            bool isCorrectPlacement = correctPillar == pillarNumber; // Is toy on correct pillar?
            toyPlacementStatus[toyName] = isCorrectPlacement; // Update status of whether it is on correct pillar or not
            return isCorrectPlacement; // Return true if on correct pillar

        }
        return false;
    }

    public void RemoveToyFromPillar(int pillarNumber, string toyName) // Removes a toy from a pillar if it is taken out of the trigger
    {
        if (currentToyPlacement.ContainsKey(pillarNumber))
        {
            currentToyPlacement.Remove(pillarNumber);

            // Update toyPlacementStatus
            toyPlacementStatus[toyName] = false;

            // Check puzzle completion again
            CheckPuzzleCompletition();
        }
    }

    private string GetCurrentPlacementsString() // Gets current placement of toys
    {
        List<string> placements = new List<string>();
        foreach (var kvp in currentToyPlacement)
        {
            placements.Add($"Pillar {kvp.Key}: {kvp.Value}");
        }
        return string.Join(", ", placements);
    }

    public void CheckPuzzleCompletition() // Checks whether puzzle has been completed
    {
        bool isComplete = true;

        // Ensure all toys are placed correctly on their respective pillars
        foreach (var toyStatus in toyPlacementStatus.Values)
        {
            if (!toyStatus)
            {
                isComplete = false;
                Debug.Log("Not all toys are placed correctly.");
                break;
            }
        }

        // Ensure all pillars have toys placed on them
        if (currentToyPlacement.Count != toyAssignments.Count)
        {
            isComplete = false;
           // Debug.Log($"Not all pillars have toys placed. {currentToyPlacement.Count}/{toyAssignments.Count} pillars filled.");
        }

        // If everything is correct, mark the puzzle as complete
        if (isComplete)
        {
          //  Debug.Log("Puzzle complete!");
            EventManager.TriggerPuzzle_1_DoorOpen();
        }
        else
        {
         //   Debug.Log("Puzzle not yet complete.");
        }

    }






}

