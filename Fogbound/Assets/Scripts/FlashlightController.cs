using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UI

public class FlashlightController : MonoBehaviour
{
    public KeyCode flashlightKey = KeyCode.F; // The key to toggle the flashlight
    private Light flashlight;
    private enum FlashlightMode { Off, On, UV }
    private FlashlightMode currentMode = FlashlightMode.On; // Start in normal light mode

    private bool uvModeExhausted = false; // Track if UV mode is exhausted

    // Default values for Normal Light and UV Light
    public float normalLightIntensity = 5f;
    public Color normalLightColor = Color.white;
    public float normalLightRange = 20f; // Longer range for the normal spotlight

    public float uvLightIntensity = 2f;
    public Color uvLightColor = new Color(0.5f, 0f, 1f); // Purple for UV
    public float uvLightRange = 5f; // Shorter range for the UV light
    public float maxUVTime = 30f; // Max UV time in seconds

    private float currentUVTime = 0f; // Track how much UV time has been used

    // Reference to the UI slider
    public Slider UVslider;

    // Audio components for click and UV mode sounds
    public AudioClip clickSound;       // Sound for toggling the flashlight
    public AudioClip uvModeSound;      // Sound for switching to UV mode
    private AudioSource audioSource;   // The AudioSource to play the sounds

    // Public property to check if UV mode is active
    public bool IsUVModeActive
    {
        get { return currentMode == FlashlightMode.UV && !uvModeExhausted; }
    }

    void Start()
    {
        // Get the Light component
        flashlight = GetComponent<Light>();

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Set the light to start in the On mode
        SetNormalLightMode();

        // Initialize the UV slider if used
        if (UVslider != null)
        {
            UVslider.maxValue = maxUVTime;
            UVslider.value = maxUVTime; // Set initial value to max
        }
    }

    void Update()
    {
        // Toggle between modes when the key is pressed
        if (Input.GetKeyDown(flashlightKey))
        {
            ToggleFlashlightMode();
        }

        // Handle UV mode depletion
        if (currentMode == FlashlightMode.UV)
        {
            currentUVTime += Time.deltaTime;

            if (UVslider != null)
            {
                UVslider.value = maxUVTime - currentUVTime;
            }

            if (currentUVTime >= maxUVTime)
            {
                uvModeExhausted = true;
                SetNormalLightMode();
                if (UVslider != null)
                {
                    UVslider.value = 0;
                }
            }
        }
    }

    // Method to toggle between Off, On, and UV modes
    void ToggleFlashlightMode()
    {
        // Play click sound for all mode changes
        PlayClickSound();

        switch (currentMode)
        {
            case FlashlightMode.Off:
                SetNormalLightMode(); // Switch to Normal Light
                break;

            case FlashlightMode.On:
                if (!uvModeExhausted)
                {
                    SetUVLightMode(); // Switch to UV Light
                }
                else
                {
                    SetOffMode(); // UV is exhausted, turn flashlight off
                }
                break;

            case FlashlightMode.UV:
                SetOffMode(); // Switch to Off
                break;
        }
    }

    // Set the flashlight to Normal Light mode
    void SetNormalLightMode()
    {
        currentMode = FlashlightMode.On;
        flashlight.enabled = true;
        flashlight.intensity = normalLightIntensity;
        flashlight.color = normalLightColor;
        flashlight.range = normalLightRange;
        Debug.Log("Flashlight set to Normal Light Mode");
    }

    // Set the flashlight to UV Light mode
    void SetUVLightMode()
    {
        currentMode = FlashlightMode.UV;
        flashlight.enabled = true;
        flashlight.intensity = uvLightIntensity;
        flashlight.color = uvLightColor;
        flashlight.range = uvLightRange;

        // Play UV mode sound
        PlayUVSound();
        Debug.Log("Flashlight set to UV Light Mode");
    }

    // Turn the flashlight Off
    void SetOffMode()
    {
        currentMode = FlashlightMode.Off;
        flashlight.enabled = false;
        Debug.Log("Flashlight is Off");
    }

    // Play the click sound when switching modes
    void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    // Play the UV mode sound when switching to UV
    void PlayUVSound()
    {
        if (uvModeSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(uvModeSound); // Play the UV sound once
        }
    }
}
