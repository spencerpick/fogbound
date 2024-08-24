using System.Collections;
using UnityEngine;

public class ghostmovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public float segmentLength = 10f; // Length of each segment
    public float followSpeed = 7f; // Speed of movement when following the player

    private Rigidbody rb;
    private Vector3 targetPosition;
    private Transform playerTransform;
    private bool isFollowingPlayer = false;
    private float initialHeight;
    private Vector3 startPosition;

    private bool isStopped = false; // To track if the ghost is stopped

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position; // Store the initial position as the bottom-left corner
        initialHeight = startPosition.y; // Store the initial height

        // Set the first target position within the square bounds
        SetRandomTargetPosition();

        // Find the player by tag
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found in the scene.");
        }
    }

    void Update()
    {
        if (isStopped)
        {
            return; // Do nothing if the ghost is stopped
        }

        if (isFollowingPlayer && playerTransform != null)
        {
            FollowPlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        // Rotate towards the target position
        RotateTowardsTarget();

        // Move towards the target position
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;
        newPosition.y = initialHeight; // Maintain the initial height
        transform.position = newPosition;

        // Check if the ghost has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            // Select a new random target position
            SetRandomTargetPosition();
        }
    }

    private void RotateTowardsTarget()
    {
        // Calculate the direction to the target
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        directionToTarget.y = 0; // Only rotate on the Y axis

        // Rotate smoothly towards the target direction
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);
        }
    }

    private void SetRandomTargetPosition()
    {
        // Define the square bounds using the startPosition as the bottom-left corner
        float minX = startPosition.x;
        float minZ = startPosition.z;
        float maxX = minX + segmentLength;
        float maxZ = minZ + segmentLength;

        Vector3 randomPosition;
        float distance;

        do
        {
            // Randomly select a point within the square
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);

            randomPosition = new Vector3(randomX, initialHeight, randomZ);
            distance = Vector3.Distance(transform.position, randomPosition);
        }
        // Ensure the new target position is at least segmentLength/2 away from the current position
        while (distance < segmentLength / 3);

        // Set the new target position
        targetPosition = randomPosition;
    }

    private void FollowPlayer()
    {
        // Move towards the player
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Maintain the initial height
        Vector3 newPosition = transform.position + directionToPlayer * followSpeed * Time.deltaTime;
        newPosition.y = initialHeight; // Keep the Y position constant

        // Move the ghost to the new position
        transform.position = newPosition;

        // Ensure the ghost is looking at the player, maintaining the same height
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Reset the target position upon collision
        SetRandomTargetPosition();
    }

    // Call this method from the Detection script
    public void SetFollowingPlayer(bool follow)
    {
        isFollowingPlayer = follow;
    }

    // Stop all movement for 10 seconds
    public void StopMovement()
    {
        if (isStopped) return; // Prevent multiple calls

        if (playerTransform != null)
        {
            // Turn to face the player
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            directionToPlayer.y = 0; // Keep the rotation only on the Y axis
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 1f);
        }

        // Stop movement
        isStopped = true;
        moveSpeed = 0f;
        followSpeed = 0f;

        // Ensure the Detection script is also updated
        Detection detectionScript = GetComponent<Detection>();
        if (detectionScript != null)
        {
            detectionScript.DisableDetection(); // Hide detection visuals
        }

        StartCoroutine(ResumeMovementAfterDelay(10f)); // Resume after 10 seconds
    }

    // Coroutine to resume movement and enable the arc and circle drawing
    private IEnumerator ResumeMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        isStopped = false; // Resume movement
        moveSpeed = 5f; // Reset move speed to the original value
        followSpeed = 7f; // Reset follow speed to the original value

        // Re-enable the drawing of arcs and circles
        Detection detectionScript = GetComponent<Detection>();
        if (detectionScript != null)
        {
            detectionScript.playerTouched = false;  // Reset playerTouched flag
            detectionScript.EnableDetection(); // Reactivate detection visuals
        }
    }
}
