using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI

public class FlashlightController : MonoBehaviour
{
    public KeyCode flashlightKey = KeyCode.F; // The key to toggle the flashlight
    private Light flashlight;
    private bool isUVMode = false; // Keep track of whether we are in UV mode
    private bool uvModeExhausted = false; // Track if UV mode is exhausted

    // Default values for Natural Light and UV Light
    public float naturalLightIntensity = 5f;
    public Color naturalLightColor = Color.white;
    public float naturalLightRange = 20f; // Default longer range for the normal spotlight

    public float uvLightIntensity = 2f;
    public Color uvLightColor = new Color(0.5f, 0f, 1f); // Purple for UV
    public float uvLightRange = 5f; // Shorter range for the UV light
    public float maxUVTime = 30f; // Max UV time in seconds

    private float currentUVTime = 0f; // Track how much UV time has been used

    // Reference to the UI slider
    public Slider UVslider;
    void Start()
    {
        // Get the Light component
        flashlight = GetComponent<Light>();

        // Set the light to always be on with the natural light settings
        flashlight.enabled = true;
        SetNaturalLightMode();

        // Initialize the UV slider
        if (UVslider != null)
        {
            UVslider.maxValue = maxUVTime; // Set the max value of the slider to maxUVTime
            UVslider.value = maxUVTime;    // Set the initial value to the max
        }
    }

    void Update()
    {
        // Only allow switching if UV mode is not exhausted
        if (Input.GetKeyDown(flashlightKey) && !uvModeExhausted)
        {
            // Toggle between UV and Natural light modes
            isUVMode = !isUVMode;

            if (isUVMode && currentUVTime < maxUVTime)
            {
                SetUVLightMode();
            }
            else
            {
                SetNaturalLightMode();
            }
        }

        // Decrement UV time if UV mode is active
        if (isUVMode)
        {
            currentUVTime += Time.deltaTime;

            // Update the slider to reflect the remaining UV time
            if (UVslider != null)
            {
                UVslider.value = maxUVTime - currentUVTime;
            }

            // Check if we've exhausted UV time
            if (currentUVTime >= maxUVTime)
            {
                uvModeExhausted = true; // Set the flag to disable UV mode
                SetNaturalLightMode(); // Automatically switch back to natural light
                Debug.Log("UV mode exhausted!");

                // Set the slider to 0
                if (UVslider != null)
                {
                    UVslider.value = 0;
                }
            }
        }
    }

    // Set the flashlight to Natural Light mode
    void SetNaturalLightMode()
    {
        flashlight.intensity = naturalLightIntensity;
        flashlight.color = naturalLightColor;
        flashlight.range = naturalLightRange; // Set the range for normal light
        isUVMode = false; // Ensure that UV mode is turned off
        Debug.Log("Flashlight set to Natural Light Mode");
    }

    // Set the flashlight to UV Light mode
    void SetUVLightMode()
    {
        flashlight.intensity = uvLightIntensity;
        flashlight.color = uvLightColor;
        flashlight.range = uvLightRange; // Set the shorter range for UV light
        Debug.Log("Flashlight set to UV Light Mode");
    }
}
