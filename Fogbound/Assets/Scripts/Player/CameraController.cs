using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] Transform playerBody;

    private float rotationX = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks curser in middle of screen and makes it invisible

        rotationX = transform.localRotation.eulerAngles.x; // Initalizes rotationX to be the camera current rotation to avoid starting looking at floor
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse input //
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // Get mouse horizontal x input
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; // Get mouse vertical y input

        // Apply vertical rotation //
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Prevents camera flipping

        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f); // Applies the vertical rotation to camera (x Axis)
        playerBody.Rotate(Vector3.up * mouseX); // Rotates the players body around the y-axis
    }
}
