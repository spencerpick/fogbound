using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f; 
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpingHeight = 2f;
    [SerializeField] private float gravity = -9.81f;  // This number represents earths gravity
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private float moveX;
    private float moveZ;
    private Vector3 velocity;
    private float currentSpeed;
    private bool onGround;


    private AudioSource audioSource;
    private float footstepTimer; // Timer so we can play footsteps at given intervals
    [SerializeField] private float walkStepInterval = 0.5f; // Interval for walking footsteps
    [SerializeField] private float runStepInterval = 0.3f;  // Interval for running footsteps

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>(); // Get character controller component

        audioSource = GetComponent<AudioSource>(); // Get audio source component
    }

    // Update is called once per frame
    void Update()
    {

        HandleMovement();

        HandleJumping();

        PlayFootStepSounds();


    }

    private void HandleMovement()
    {
        // HANDLE MOVEMENT //
        onGround = controller.isGrounded; //  Store if player is currently on the ground or not
        if (onGround && velocity.y < 0) // If the player is on the ground and they are not falling
        {
            velocity.y = -2f; // Ensures player stays grounded when they are supposed to be
        }

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ; // Vector to store the instructed movement

        // If the player is on the ground then allow them to change movement speed by holding shift to run (stops speed change mid air)
        if (onGround)
        {
            currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed; // If shift held then run speed else walk speed
        }

        controller.Move(move * currentSpeed * Time.deltaTime);

    }


    private void HandleJumping()
    {
        // HANDLE JUMPING //
        if (onGround && Input.GetKeyDown(KeyCode.Space)) // Space bar is held down and the player is on the floor
        {
            velocity.y = Mathf.Sqrt(jumpingHeight * -2f * gravity); // Use kinematic equation to allow player to jump 
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity to player
        controller.Move(velocity * Time.deltaTime); // Moves the character controller vertically based on velocity after being adjusted for gravity


    }


    private void PlayFootStepSounds()
    {
        // FOOTSTEP SOUND EFFECTS //
        if (onGround && (moveX != 0 || moveZ != 0)) // Ensure player is on ground and is moving
        {
            footstepTimer -= Time.deltaTime; // Countdown timer

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftShift)) // If the player changes between running / walking stop the current sound effect immediately and reset interval timer
            {
                audioSource.Stop();
                footstepTimer = 0;
            }

            if (footstepTimer <= 0 && !audioSource.isPlaying) // If timer is 0 and there is no footstep sound already playing then play the sound
            {
                audioSource.pitch = currentSpeed == runSpeed ? 2f : 1f;
                audioSource.Play();

                footstepTimer = currentSpeed == runSpeed ? runStepInterval : walkStepInterval;
            }
        }
        else // The player is not moving or not on the ground so stop playing the footsteps sound effect immediately and reset interval timer
        {
            audioSource.Stop();
            footstepTimer = 0; // Reset the timer when player is no longer moving
        }
    }

}


