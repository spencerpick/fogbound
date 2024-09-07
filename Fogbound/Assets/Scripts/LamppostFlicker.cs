using System.Collections;
using UnityEngine;

public class LamppostFlicker : MonoBehaviour
{
    [SerializeField] private Light lampLight; // Reference to point light
    [SerializeField] private float minFlickerDelay = 0.1f; // Min time between flickers
    [SerializeField] private float maxFlickerDelay = 1f;   // Max time between flickers
    [SerializeField] private float minIntensity = 0f;      // Min light intensity (turned off)
    [SerializeField] private float maxIntensity = 2f;      // Max light intensity (full brightness)

    private bool isFlickering = false;

    void Start()
    {
        // Start the flickering coroutine (using coroutine so we can have delays in flickers)
        StartCoroutine(FlickerLamp());
    }

    IEnumerator FlickerLamp()
    {
        while (true) // Infinite loop to keep flickering ongoiing
        {
            if (!isFlickering)
            {
                isFlickering = true;

                // Randomize the light intensity for the flicker
                float randomIntensity = Random.Range(minIntensity, maxIntensity);
                lampLight.intensity = randomIntensity;

                // Wait for random amount of time
                float randomDelay = Random.Range(minFlickerDelay, maxFlickerDelay);
                yield return new WaitForSeconds(randomDelay);

                isFlickering = false;
            }

            yield return null; 
        }
    }
}
