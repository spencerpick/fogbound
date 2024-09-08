using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Puzzle_2 : MonoBehaviour
{
    public GameObject PuzzleReward;
    public GameObject RewardSpawnLocation;
    public bool rewardGiven = false;
    [SerializeField] PuzzlePillar[] puzzleScripts;
    [SerializeField] List<bool> puzzleSolution;

    public GameObject Puzzle2Clues;
    public bool puzzleCompleted = false;

    void Start()
    {
        // Get all of the puzzle pillars
        puzzleScripts = GetComponentsInChildren<PuzzlePillar>();

        // Generate the solution to the puzzle
        puzzleSolution = GenerateSolution(puzzleScripts.Length * 3);

        // Debug.Log("Solution generated:" + puzzleSolution.ToString());

        //List<TextMeshPro> clueText;
        TextMeshPro[] clueText = Puzzle2Clues.GetComponentsInChildren<TextMeshPro>();

        for (var i = 0; i < clueText.Length; i++)
        {
            string clue_text = "";

            // Ensure you don't exceed the length of the puzzleSolution
            for (int j = 0; j < 3 && j + (i * 3) < puzzleSolution.Count; j++)
            {
                // Access the correct index in puzzleSolution for each character
                int solutionIndex = j + (i * 3);
                clue_text += (puzzleSolution[solutionIndex] ? "D" : "U");

                if(j < 2)
                {
                    clue_text += "\n";
                }
            }

            // Update the text for the corresponding TextMeshPro element
            clueText[i].text = clue_text;
        }


        StartCoroutine(TestPuzzle());
    }

    IEnumerator TestPuzzle()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Puzzle State: " + PuzzleCheck().ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool PuzzleCheck()
    {
        bool isSolved = true;

        for (var i = 0; i < puzzleScripts.Length; i++)
        {
            // Get the current pillar's state
            List<bool> currentPillarState = puzzleScripts[i].GetPillarState();

            // Compare the pillar's state with the corresponding part of the solution
            if (currentPillarState[0] != puzzleSolution[i * 3] ||
                currentPillarState[1] != puzzleSolution[i * 3 + 1] ||
                currentPillarState[2] != puzzleSolution[i * 3 + 2])
            {

                // Stop checking if one is incorrect
                isSolved = false;
                break;
            }
        }

        // Return the result
        return isSolved;
    }

    private List<bool> GenerateSolution(int solutionLength)
    {
        List<bool> solution = new List<bool>(solutionLength);
        for(var i = 0; i < solutionLength; i++)
        {
            solution.Add(Random.value > 0.5f);
        }

        return solution;
    }

    public void giveReward()
    {
        if (!rewardGiven)
        {
            GameObject.FindWithTag("GameManager").GetComponent<InteractiveObjectManager>().AddIntereactive(Instantiate(PuzzleReward, RewardSpawnLocation.transform));
        }
    }
}
