﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_1 : MonoBehaviour
{

    [SerializeField] bool CompletePuzzle = false; // Testing purposes

    private Dictionary<string, int> toyAssignments = new Dictionary<string, int>();

    void Start()
    {
        GiveToysRandomNumbers();
    }

    void OnEnable()
    {
        EventManager.OnToyPlacedOnPillar += HandleToyPlacedOnPillar;
    }

    void OnDisable()
    {
        EventManager.OnToyPlacedOnPillar -= HandleToyPlacedOnPillar;
    }


    void Update()
    {
        if (CompletePuzzle == true)
        {

        }
    }

    private void GiveToysRandomNumbers() // Asign a unique pillar number to each toy
    {
        string[] toyNames = { "Bear", "Duck", "Rabbit", "Monkey", "Penguin", "Pig" }; // All the names of the toys

        List<int> availableNumbers = new List<int> { 1, 2, 3, 4, 5, 6 };

        foreach (string toyName in toyNames)
        {
            int randomIndex = Random.Range(0, availableNumbers.Count); // Pick a random number from the available numbers list
            int assignedNumber = availableNumbers[randomIndex]; // Set that number to be the assigned number that will be given to the current toy
            availableNumbers.RemoveAt(randomIndex); // Remove that number from the available numbers list so it doesn't get chosen again

            toyAssignments[toyName] = assignedNumber; // Set the number to the current toy in the dictionary

            Debug.Log($"{toyName} assigned to pillar {assignedNumber}");

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

    private void SnapToyToPillar(GameObject toy, int pillarNumber) // Snap a toy into position so it is placed on the pillar nicely a
    {
        GameObject pillar = GameObject.Find("Pillar_" + pillarNumber); // Find the pillar object based on its associated number

        if (pillar != null) // Ensure the pillar exists to avoid errors 
        {
            // Snap the toy to the top of the pillar
            toy.transform.position = pillar.transform.position + new Vector3(0, 1.5f, 0); // Set y position
            toy.transform.rotation = Quaternion.Euler(0, 90, 0);

        }
    }

    private void HandleToyPlacedOnPillar(GameObject toy, int pillarNumber)
    {
        string toyName = toy.name; // Get the name of the toy object (e.g., "Bear")
        int assignedNumber = GetAssignedNumber(toyName); // Use toyname to retrieve which pillar it is associated with and thus needs to be placed on

       // if(assignedNumber == pillarNumber)
      //  {
            SnapToyToPillar(toy, pillarNumber);
      //  }
    }

    public bool IsToyOnCorrectPillar(GameObject toy, int pillarNumber)
    {
        string toyName = toy.name; // Get the toy's name
        if (toyAssignments.TryGetValue(toyName, out int correctPillar))
        {
            return correctPillar == pillarNumber; // Returns true if on correct pillar
        }
        return false;
    }

    //public void CheckPuzzleCompletition()
    //{
    //    bool isComplete = true;

    //    // Iterate through all toys and check if they're on the correct pillars
    //    foreach (var toyAssignment in toyAssignments)
    //    {
    //        GameObject toy = GameObject.Find(toyAssignment.Key);
    //        int correctPillar = toyAssignment.Value;

    //        // Find the trigger/pillar the toy is on (you might need more specific checking logic here)
    //       // PillarTrigger pillarTrigger = FindPillarTriggerForToy(toy); // Implement this method based on your setup

    //        if (pillarTrigger == null || !IsToyOnCorrectPillar(toy, pillarTrigger.pillarNumber))
    //        {
    //            isComplete = false;
    //            break;
    //        }
    //    }

    //    if (isComplete)
    //    {
    //        Debug.Log("Puzzle complete!");
    //        EventManager.TriggerPuzzle_1_DoorOpen(); // Trigger the door open event
    //        EventManager.TriggerThoughtUpdate("Hello this is a testttt");
    //    }
    //    else
    //    {
    //        Debug.Log("Puzzle not yet complete.");
    //    }
    }

