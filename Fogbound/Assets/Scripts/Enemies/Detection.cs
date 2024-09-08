using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Detection : MonoBehaviour
{
    public float detectionRadius = 5f;       // Radius within which to detect the player
    public float circleRadius = 1f;          // Radius of the small circle around the ghost
    public Color defaultColor = Color.white; // Default color of the arc
    public Color proximityColor = Color.red; // Color of the arc when the player is detected
    public int arcResolution = 50;           // Number of segments in the arc
    public int circleResolution = 30;        // Number of segments in the small circle
    public float detectionAngle = 60f;       // Angle of the detection arc
    public bool playerTouched = false;       // Whether the player is within the small circle
    private PlayerLives playerLives;

    private Mesh mesh;                       // Mesh for the arc and circle
    private Transform playerTransform;       // Reference to the player's Transform
    private MeshRenderer meshRenderer;       // Renderer to control the color of the arc
    private ghostmovement ghostMovement;     // Reference to the ghostmovement script

    void Start()
    {
        // Get the MeshFilter and MeshRenderer components
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Create a new mesh and assign it to the MeshFilter
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        // Find the player by tag
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerLives = player.GetComponent<PlayerLives>(); // Add this line
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found in the scene.");
        }

        // Get reference to the ghostmovement script
        ghostMovement = GetComponent<ghostmovement>();

        // Draw the detection arc initially with the default color
        DrawArc(defaultColor);
    }

    void Update()
    {
        if (playerTouched) return; // If player is touched, don't continue

        if (playerTransform == null) return; // Safety check if playerTransform is not assigned

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0; // Ignore vertical differences

        float distance = directionToPlayer.magnitude;

        // Check if the player is within the small circle
        bool isInCircle = distance <= circleRadius;

        // Check if the player is within the detection arc
        bool isInArc = false;
        if (distance <= detectionRadius * 2)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer <= detectionAngle / 2f) // Use half of the detection angle
            {
                isInArc = true;
            }
        }

        if (isInCircle)
        {
            if (playerLives != null)
            {
                playerLives.LoseLife();
            }
            // Player is within the circle
            playerTouched = true;
            ghostMovement.SetFollowingPlayer(false); // Stop the ghost
            ghostMovement.StopMovement(); // Stop all movement
            meshRenderer.enabled = false; // Hide the arc and circle
        }
        else if (isInArc)
        {
            DrawArc(proximityColor);
            ghostMovement.SetFollowingPlayer(true); // Start following the player
        }
        else
        {
            DrawArc(defaultColor);
            ghostMovement.SetFollowingPlayer(false); // Stop following the player
        }
    }


    void DrawArc(Color arcColor)
    {
        if (!meshRenderer.enabled) return; // Don't draw if the mesh is disabled

        meshRenderer.material.color = arcColor;

        // Vertices count: arcResolution + 1 for the arc + circleResolution + 1 for the circle
        Vector3[] vertices = new Vector3[arcResolution + 2 + circleResolution + 1];
        int vertexIndex = 0;

        // Vertices for the detection arc
        vertices[vertexIndex++] = Vector3.zero; // Center of the arc
        float halfAngle = detectionAngle / 2f;
        float angleStep = detectionAngle / arcResolution;

        for (int i = 0; i <= arcResolution; i++)
        {
            float currentAngle = -halfAngle + i * angleStep;
            float rad = Mathf.Deg2Rad * currentAngle;

            float x = Mathf.Sin(rad) * detectionRadius;
            float z = Mathf.Cos(rad) * detectionRadius;

            vertices[vertexIndex++] = new Vector3(x, 0f, z);
        }

        // Vertices for the small circle
        int circleStartIndex = vertexIndex;
        vertices[vertexIndex++] = Vector3.zero; // Center of the circle
        for (int i = 0; i < circleResolution; i++)
        {
            float angle = (i / (float)circleResolution) * Mathf.PI * 2f;
            float x = Mathf.Sin(angle) * circleRadius;
            float z = Mathf.Cos(angle) * circleRadius;
            vertices[vertexIndex++] = new Vector3(x, 0f, z);
        }

        // Create triangles for the arc
        int[] triangles = new int[arcResolution * 3 + circleResolution * 3];
        int triangleIndex = 0;

        for (int i = 0; i < arcResolution; i++)
        {
            triangles[triangleIndex++] = 0;            // Center vertex of the arc
            triangles[triangleIndex++] = i + 1;        // Current vertex
            triangles[triangleIndex++] = i + 2;        // Next vertex
        }

        // Create triangles for the small circle
        int circleCenterIndex = circleStartIndex;       // Center of the circle
        for (int i = 0; i < circleResolution; i++)
        {
            triangles[triangleIndex++] = circleCenterIndex;                   // Center vertex of the circle
            triangles[triangleIndex++] = circleStartIndex + i + 1;            // Current vertex
            triangles[triangleIndex++] = circleStartIndex + ((i + 1) % circleResolution) + 1; // Next vertex
        }

        // Create the mesh
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    // Method to enable detection visuals (arc and circle)
    public void EnableDetection()
    {
        meshRenderer.enabled = true;
        DrawArc(defaultColor); // Draw the arc with the default color
    }

    // Method to disable detection visuals (arc and circle)
    public void DisableDetection()
    {
        meshRenderer.enabled = false;
    }
}


