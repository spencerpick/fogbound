using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Required for NavMeshAgent

public class GhostFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float stopDistance = 1.5f;
    public float stunDuration = 3f; // Duration of the stun

    private NavMeshAgent agent;
    private bool isStunned = false;

    void Start()
    {
        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed; // Set the speed of the NavMeshAgent
        agent.stoppingDistance = stopDistance; // Set the stopping distance
    }

    void Update()
    {
        if (!isStunned)
        {
            if (player != null)
            {
                agent.SetDestination(player.position);
                Debug.Log("Setting destination to player at position: " + player.position);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Flashlight"))
        {
            Light flashlight = other.GetComponent<Light>();
            if (flashlight != null && flashlight.enabled)
            {
                StartCoroutine(StunEnemy());
            }
        }
    }

    private IEnumerator StunEnemy()
    {
        isStunned = true;
        agent.isStopped = true; // Stop the NavMeshAgent from moving
        Debug.Log("Enemy stunned, stopping NavMeshAgent.");
        yield return new WaitForSeconds(stunDuration);
        agent.isStopped = false; // Resume movement after the stun duration
        isStunned = false;
        Debug.Log("Enemy no longer stunned, resuming NavMeshAgent.");
    }

}
