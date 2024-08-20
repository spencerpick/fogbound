using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public KeyCode flashlightKey = KeyCode.F; // The key to toggle the flashlight
    private Light flashlight;

    void Start()
    {
        flashlight = GetComponent<Light>();
    }

    void Update()
    {
        if (Input.GetKeyDown(flashlightKey))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}