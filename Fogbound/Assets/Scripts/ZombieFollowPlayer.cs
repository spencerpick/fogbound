using System.Collections;
using UnityEngine;

public class ZombieFollowPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float crawlSpeed = 1f; // Speed of the zombie crawl
    public float rotationSpeed = 2f; // How fast the zombie rotates to face the player
    public float stunDuration = 3f; // Duration of the stun

    private Animator animator;
    private bool isStunned = false;

    void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();

        // Start the crawling animation
        animator.SetBool("isCrawling", true);
    }

    void Update()
    {
        if (player != null && !isStunned)
        {
            // Rotate to face the player
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // Move the zombie forward continuously
            transform.Translate(Vector3.forward * crawlSpeed * Time.deltaTime);

            // Ensure the crawling animation keeps playing
            animator.SetBool("isCrawling", true);
        }
    }

    // Handle collisions with the flashlight
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Flashlight"))
        {
            Debug.Log("Flashlight hit the zombie!");
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

            // Pause the animation by setting the animator's speed to 0
            animator.speed = 0f;
            Debug.Log("Zombie stunned, animation paused.");

            yield return new WaitForSeconds(stunDuration); // Wait for the stun duration

            // Resume the animation by setting the animator's speed back to 1
            animator.speed = 1f;
            Debug.Log("Zombie no longer stunned, animation resumed.");

            isStunned = false; // Resume normal behavior after the stun
        }
    }
}
