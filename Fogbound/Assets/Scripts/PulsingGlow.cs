using UnityEngine;

public class PulsingGlow : MonoBehaviour
{
    private Material glowMaterial; // Assign your material in the Inspector
    public Color glowColor = Color.blue; // The color of the glow
    public float pulseSpeed = 2f; // Speed of the pulsing effect
    public float minGlowIntensity = 0.5f; // Minimum intensity of the glow
    public float maxGlowIntensity = 2f; // Maximum intensity of the glow

    private void Start()
    {
        // Get the material from the Renderer
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            glowMaterial = renderer.material; // This gets the material instance applied to the object
        }
    }

    void Update()
    {
        // Calculate the intensity based on a sine wave
        float emission = minGlowIntensity + Mathf.PingPong(Time.time * pulseSpeed, maxGlowIntensity - minGlowIntensity);

        // Apply the emission to the material
        glowMaterial.SetColor("_EmissionColor", glowColor * emission);

        // Make sure the material emits light
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.EnableKeyword("_EMISSION");
        }
    }
}
