using System.Collections;
using UnityEngine;

public class ghostmovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public float segmentLength = 10f; // Length of each segment
    public float followSpeed = 7f; // Speed of movement when following the player
    private string audioClipPath = "Audios/ghostaudio"; // Path relative to the Resources folder
    public float maxVolumeDistance = 5f; // Distance at which the sound will be loudest
    public float minVolumeDistance = 20f; // Distance at which the sound will be quietest

    private Rigidbody rb;
    private Vector3 targetPosition;
    private Transform playerTransform;
    private bool isFollowingPlayer = false;
    private float initialHeight;
    private Vector3 startPosition;
    private Renderer ghostRenderer;
    private bool isStopped = false; // To track if the ghost is stopped
    private AudioSource ghostSound; // Dynamically created AudioSource

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position; // Store the initial position as the bottom-left corner
        initialHeight = startPosition.y; // Store the initial height

        // Set the first target position within the square bounds
        SetRandomTargetPosition();
        ghostRenderer = GetComponent<Renderer>();
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
        SetupGhostAudio();
    }

    void Update()
    {
        AdjustSoundVolume(); // Ensure this is continuously updating the volume
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

    private void SetupGhostAudio()
    {
        // Load the audio clip from the Resources folder
        AudioClip clip = Resources.Load<AudioClip>(audioClipPath);
        if (clip == null)
        {
            Debug.LogError("AudioClip not found at path: " + audioClipPath);
            return;
        }

        // Add AudioSource component to the ghost and configure it
        ghostSound = gameObject.AddComponent<AudioSource>();
        ghostSound.clip = clip;
        ghostSound.loop = true; // Set to loop
        ghostSound.playOnAwake = false; // Do not play on awake
        ghostSound.spatialBlend = 1.0f; // Set to 3D sound
        ghostSound.maxDistance = minVolumeDistance; // Set the max distance for sound attenuation
        ghostSound.rolloffMode = AudioRolloffMode.Linear; // Linear falloff of sound
    }

    // Adjust the volume of the ghost sound based on the distance to the player
    private void AdjustSoundVolume()
    {
        if (playerTransform != null && ghostSound != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // Calculate the volume based on the distance
            float volume = Mathf.Clamp(1 - (distanceToPlayer - maxVolumeDistance) / (minVolumeDistance - maxVolumeDistance), 0, 1);

            ghostSound.volume = volume;

            // Play the sound if not already playing
            if (!ghostSound.isPlaying)
            {
                ghostSound.Play();
            }
        }
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

        // Start flickering effect
        StartCoroutine(FlickerEffect(10f)); // Flicker for the duration of the stop


        StartCoroutine(ResumeMovementAfterDelay(10f)); // Resume after 10 seconds
    }

    // Coroutine to handle flickering effect
    private IEnumerator FlickerEffect(float duration)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            ghostRenderer.enabled = !ghostRenderer.enabled; // Toggle visibility
            yield return new WaitForSeconds(0.2f); // Flicker every 0.2 seconds
        }
        ghostRenderer.enabled = true; // Ensure visibility is restored after flickering
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
