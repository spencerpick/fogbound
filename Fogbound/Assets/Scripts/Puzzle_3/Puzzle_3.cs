using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_3 : MonoBehaviour
{
    public GameObject PuzzleReward;
    public GameObject RewardSpawnLocation;
    public GameObject pressurePlatesEmpty;
    private PressurePlate[] pressurePlates;

    private bool rewardGiven = false;

    public bool puzzleCompleted;

    // Start is called before the first frame update
    void Start()
    {
        pressurePlates = pressurePlatesEmpty.GetComponentsInChildren<PressurePlate>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isFinished = true;
        foreach(var pp in pressurePlates)
        {

            if (!pp.isSufficientlyPressed)
            {
                isFinished = false;
            }
        }
        if (isFinished)
        {
            puzzleCompleted = true;
            if (!rewardGiven)
            {
                GameObject.FindWithTag("GameManager").GetComponent<InteractiveObjectManager>().AddIntereactive(Instantiate(PuzzleReward, RewardSpawnLocation.transform));
                rewardGiven = true;
            }
        }
    }
}
