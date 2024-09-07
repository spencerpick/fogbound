using System.Collections;
using UnityEngine;
using UnityEngine.AI; // Required for NavMeshAgent

public class ZombieFollowNav : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float stunDuration = 3f; // Duration of the stun
    public float attackRange = 1.5f;  // Range within which the zombie can damage the player
    public float damageCooldown = 2f; // Time between attacks
    public PlayerLives playerLives;   // Reference to player's lives script (for damaging player)

    private Animator animator;
    private NavMeshAgent agent; // Reference to the NavMeshAgent
    private bool isStunned = false;
    private bool canAttack = true; // To control the attack cooldown

    void Start()
    {
        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();

        // Get the Animator component
        animator = GetComponent<Animator>();

        // Start the crawling animation
        animator.SetBool("isCrawling", true);

        // Find the player GameObject and get its PlayerLives component
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerLives = playerObject.GetComponent<PlayerLives>(); // Assign PlayerLives component here

            // Debugging information
            Debug.Log("Player object found: " + playerObject.name);
            if (playerLives != null)
            {
                Debug.Log("PlayerLives script found and assigned.");
            }
            else
            {
                Debug.LogError("PlayerLives script is not attached to the Player object.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject with 'Player' tag not found.");
        }
    }

    void Update()
    {
        if (player != null && !isStunned)
        {
            // Set the NavMeshAgent destination to the player's position
            agent.SetDestination(player.position);

            // Check if the zombie is within attack range
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange && canAttack)
            {
                TryAttackPlayer();
            }

            // Ensure the crawling animation keeps playing
            animator.SetBool("isCrawling", true);
        }
    }

    // Attempt to attack the player
    private void TryAttackPlayer()
    {
        if (playerLives != null && canAttack)
        {
            Debug.Log("PlayerLives script found, attacking player!");
            playerLives.LoseLife();
            StartCoroutine(AttackCooldown());
        }
    }

    // Coroutine to handle cooldown between attacks
    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(damageCooldown); // Wait for the cooldown time
        canAttack = true; // Allow the zombie to attack again
    }

    // Handle collisions with the flashlight
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Flashlight"))
        {
            FlashlightController flashlightController = other.GetComponent<FlashlightController>();

            if (flashlightController != null && flashlightController.IsUVModeActive)
            {
                Debug.Log("UV Mode is active, zombie should be stunned.");
                StartCoroutine(StunZombie());
            }
        }
    }

    // Coroutine to handle stunning the zombie and pausing the animation
    private IEnumerator StunZombie()
    {
        if (!isStunned)
        {
            isStunned = true; // Set the zombie to a stunned state

            // Stop the NavMeshAgent movement
            agent.isStopped = true;

            // Pause the animation by setting the animator's speed to 0
            animator.speed = 0f;
            Debug.Log("Zombie stunned, animation paused.");

            yield return new WaitForSeconds(stunDuration); // Wait for the stun duration

            // Resume the NavMeshAgent movement
            agent.isStopped = false;

            // Resume the animation by setting the animator's speed back to 1
            animator.speed = 1f;
            Debug.Log("Zombie no longer stunned, animation resumed.");

            isStunned = false; // Resume normal behavior after the stun
        }
    }
}
