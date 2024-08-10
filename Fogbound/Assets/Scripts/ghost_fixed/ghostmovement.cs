using UnityEngine;

public class ghostmovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public float segmentLength = 10f; // Length of each segment

    private Rigidbody rb;
    private Vector3 movementDirection;
    private float distanceTravelled;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementDirection = transform.forward; // Start moving forward
        distanceTravelled = 0f;
    }

    void Update()
    {
        // Calculate movement
        float moveAmount = moveSpeed * Time.deltaTime;
        transform.Translate(new Vector3(0, 0, 1)* moveAmount);

        distanceTravelled += moveAmount;

        if (distanceTravelled >= segmentLength)
        {
            // Reset distance and turn 90 degrees
            distanceTravelled = 0f;
            Turn();
        }
    }

    private void Turn()
    {
        // Rotate the direction by 90 degrees around the Y-axis
        transform.Rotate(new Vector3(0, 90, 0));
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Reset distance and turn 90 degrees if a collision occurs
        distanceTravelled = 0f;
        Turn();
    }
}
