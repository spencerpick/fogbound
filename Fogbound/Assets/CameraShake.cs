using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the camera's Transform
    public float shakeDuration = 0.2f; // Duration of the camera shake
    public float shakeAmount = 0.3f;   // Amount of shake
    public float decreaseFactor = 1.0f; // Speed at which the shake decreases

    private Vector3 originalPos;
    private float currentShakeDuration = 0f;

    void Start()
    {
        // Store the original position of the camera
        if (cameraTransform == null)
        {
            cameraTransform = GetComponent<Transform>();
        }
        originalPos = cameraTransform.localPosition;
    }

    void Update()
    {
        if (currentShakeDuration > 0)
        {
            // Apply random shake to the camera's position
            cameraTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            // Decrease the shake duration over time
            currentShakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            // Once the shake is done, reset the camera to its original position
            currentShakeDuration = 0f;
            cameraTransform.localPosition = originalPos;
        }
    }

    // Method to trigger the camera shake
    public void TriggerShake()
    {
        currentShakeDuration = shakeDuration;
    }
}
