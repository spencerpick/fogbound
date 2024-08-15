using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GhostFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float stopDistance = 1.5f;
    public float stunDuration = 3f; // Duration of the stun

    private Rigidbody rb;
    private bool isStunned = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!isStunned)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > stopDistance)
            {
                Vector3 moveVector = direction * speed * Time.fixedDeltaTime;
                rb.MovePosition(rb.position + moveVector);
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
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }
}

