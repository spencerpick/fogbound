using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePillar : MonoBehaviour
{
    public RotationSegment[] pillarSegments;

    [SerializeField] private List<bool> pillarState;

    // Start is called before the first frame update
    void Start()
    {

        pillarState = new List<bool> { false, false, false };

        pillarSegments = GetComponentsInChildren<RotationSegment>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<bool> GetPillarState()
    {
        return pillarState;
    }

    public void FlipSegment(int segmentNumber)
    {
        // Ensure the segment number is valid (within bounds)
        if (segmentNumber >= 0 && segmentNumber < pillarSegments.Length)
        {
            // Rotate the segment based on its current state
            pillarSegments[segmentNumber].transform.Rotate(0, 0, 90 * (pillarState[segmentNumber] ? 1 : -1));

            // Toggle the state of the segment
            pillarState[segmentNumber] = !pillarState[segmentNumber];
        }
    }
}
