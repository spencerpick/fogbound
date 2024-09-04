using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] float openSpeed = 2f;
    [SerializeField] float openDistance = 5f;
    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpening = false;

    // Start is called before the first frame update
    void Start()
    {
        closedPos = transform.localPosition; // Set closed position to original location of the door placement
        openPos = closedPos - new Vector3(0, openDistance, 0); // Set open position using the open distance
    }

    void OnEnable()
    {
        EventManager.OnPuzzle_1_DoorOpen += OpenDoor; // Subscribe to the puzzle 1 open door event in event manager (so we can do other stuff when door opens too, without coupling scripts)
    }

    void OnDisable()
    {
        EventManager.OnPuzzle_1_DoorOpen -= OpenDoor; // Unsubscribe to be on safe side (stop issues if decide to disable or remove the door)
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpening)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, openPos, openSpeed * Time.deltaTime); // Open the door by moving its position in transform

            if (transform.localPosition == openPos) // If the door is in the open position then set it to no longer be opening
            {
                isOpening = false;
            }
        }
    }

    private void OpenDoor()
    {
        isOpening = true;
    }
}
