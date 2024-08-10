using UnityEngine;

public class Detection : MonoBehaviour
{
    public float detectionRadius = 5f; // Radius within which to detect the player
    public Color defaultColor = Color.white;
    public Color proximityColor = Color.red;

    private GameObject cylinder; // Reference to the cylinder GameObject
    private Transform playerTransform; // Reference to the player’s Transform
    private Renderer cylinderRenderer;
    private Transform cylinderTransform;

    void Start()
    {
        // Find the cylinder by name
        cylinder = transform.Find("Area_ghost").gameObject;
        if (cylinder != null)
        {
            cylinderRenderer = cylinder.GetComponent<Renderer>();
            cylinderTransform = cylinder.transform;
            cylinderRenderer.material.color = defaultColor;
            cylinderTransform.localScale = new Vector3(detectionRadius, 0.005f, detectionRadius);
        }
        else
        {
            Debug.LogError("Cylinder GameObject not found in the scene.");
        }

        // Find the player by tag
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found in the scene.");
        }
    }


    void Update()
    {
        // Check if the player is within the detection radius
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= detectionRadius)
        {
            Debug.Log("Player is within detection radius.");
            ChangeColor(proximityColor);
        }
        else
        {
            ChangeColor(defaultColor);
        }

    }
    void ChangeColor(Color newColor)
    {
        if (cylinderRenderer != null)
        {
            cylinderRenderer.material.color = newColor;
        }
    }
}
