using UnityEngine;

public class ghostmovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public float segmentLength = 10f; // Length of each segment
    public float turnSpeed = 2f; // Speed of the turn interpolation

    private Rigidbody rb;
    private Vector3 movementDirection;
    private float distanceTravelled;
    private Quaternion targetRotation;
    private bool isTurning = false;

    // Store the ghost's path for drawing
    private Vector3 lastPosition;
    private Vector3 nextPosition;

    // Store initial position and direction
    private Vector3 initialPosition;
    private Vector3 initialDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementDirection = transform.forward; // Start moving forward
        distanceTravelled = 0f;
        targetRotation = transform.rotation;

        // Initialize positions for path drawing
        lastPosition = transform.position;
        nextPosition = transform.position + transform.forward * segmentLength;

        // Store the initial position and direction for consistent path drawing
        initialPosition = transform.position;
        initialDirection = transform.forward;
    }

    void Update()
    {
        // Calculate movement
        float moveAmount = moveSpeed * Time.deltaTime;
        transform.Translate(new Vector3(0, 0, 1) * moveAmount);

        distanceTravelled += moveAmount;

        if (distanceTravelled >= segmentLength && !isTurning)
        {
            // Reset distance and start turning
            distanceTravelled = 0f;
            Turn();
        }

        // Smoothly rotate towards the target rotation
        if (isTurning)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            // Check if the rotation is close enough to the target rotation
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isTurning = false; // Stop turning

                // Update positions for path drawing
                lastPosition = nextPosition;
                nextPosition = transform.position + transform.forward * segmentLength;
            }
        }
    }

    private void Turn()
    {
        // Rotate the direction by 90 degrees around the Y-axis
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 90, 0));
        isTurning = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Reset distance and start turning if a collision occurs
        distanceTravelled = 0f;
        Turn();
    }

}
