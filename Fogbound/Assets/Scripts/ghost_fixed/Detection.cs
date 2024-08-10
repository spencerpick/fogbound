using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Detection : MonoBehaviour
{
    public float detectionRadius = 5f;       // Radius within which to detect the player
    public Color defaultColor = Color.white; // Default color of the arc
    public Color proximityColor = Color.red; // Color of the arc when the player is detected
    public int arcResolution = 50;           // Number of segments in the arc
    public float detectionAngle = 60f;       // Angle of the detection arc

    private Mesh mesh;                       // Mesh for the arc
    private Transform playerTransform;       // Reference to the player's Transform
    private MeshRenderer meshRenderer;       // Renderer to control the color of the arc

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
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found in the scene.");
        }

        // Draw the detection arc initially with the default color
        DrawArc(defaultColor);
    }

    void Update()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0; // Ignore vertical differences

        float distance = directionToPlayer.magnitude;
        if (distance <= detectionRadius*2)
        {
            // Calculate the angle between the forward direction of the enemy and the direction to the player
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            Debug.Log("Angle to player: " + angleToPlayer);

            // Check if the player is within the defined detection angle
            if (angleToPlayer <= detectionAngle / 2f) // Use half of the detection angle
            {
                Debug.Log("Player is within detection radius and within the detection arc.");
                DrawArc(proximityColor);
            }
            else
            {
                Debug.Log("Player is within detection radius but not within the detection arc.");
                Debug.Log("Angle to player: " + angleToPlayer);
                DrawArc(defaultColor);
            }
        }
        else
        {
            DrawArc(defaultColor);
        }
    }

    void DrawArc(Color arcColor)
    {
        meshRenderer.material.color = arcColor;

        // Create the vertices for the arc
        Vector3[] vertices = new Vector3[arcResolution + 2];
        vertices[0] = Vector3.zero; // Center of the arc

        float halfAngle = detectionAngle / 2f;
        float angleStep = detectionAngle / arcResolution;

        for (int i = 0; i <= arcResolution; i++)
        {
            float currentAngle = -halfAngle + i * angleStep;
            float rad = Mathf.Deg2Rad * currentAngle;

            float x = Mathf.Sin(rad) * detectionRadius;
            float z = Mathf.Cos(rad) * detectionRadius;

            vertices[i + 1] = new Vector3(x, 0f, z);
        }

        // Create the triangles for the arc
        int[] triangles = new int[arcResolution * 3];
        for (int i = 0; i < arcResolution; i++)
        {
            triangles[i * 3] = 0;            // Center vertex
            triangles[i * 3 + 1] = i + 1;    // Current vertex
            triangles[i * 3 + 2] = i + 2;    // Next vertex
        }

        // Create the mesh
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
