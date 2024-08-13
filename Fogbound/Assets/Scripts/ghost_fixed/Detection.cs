using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Detection : MonoBehaviour
{
    public float detectionRadius = 3f;        // Radius within which to detect the player
    public Color defaultColor = Color.white;  // Default color of the arc
    public Color proximityColor = Color.red;  // Color of the arc when the player is detected
    public int arcResolution = 50;            // Number of segments in the arc
    public float detectionAngle = 120f;       // Angle of the detection arc

    private float innerRadius = 0.75f;        // Inner radius for the arc

    private Mesh mesh;
    private Transform playerTransform;
    private MeshRenderer meshRenderer;

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Create a new mesh and assign it to the MeshFilter
        mesh = new Mesh { name = "DetectionArc" };
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
            enabled = false; // Disable script if player not found
            return;
        }

        // Draw the detection arc initially with the default color
        DrawArc(defaultColor);
    }

    void Update()
    {
        if (playerTransform == null) return;

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0; // Ignore vertical differences

        float distance = directionToPlayer.magnitude;
        if (distance <= detectionRadius * 2)
        {
            // Calculate the angle between the forward direction of the NPC and the direction to the player
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            // Check if the player is within the defined detection angle
            if (angleToPlayer <= detectionAngle / 2f) // Use half of the detection angle
            {
                DrawArc(proximityColor);
            }
            else
            {
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
        int vertexCount = (arcResolution + 1) * 2; // Two vertices for each arc point (inner and outer)
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[arcResolution * 6]; // Two triangles per segment

        float halfAngle = detectionAngle / 2f;
        float angleStep = detectionAngle / arcResolution;

        for (int i = 0; i <= arcResolution; i++)
        {
            float currentAngle = -halfAngle + i * angleStep;
            float rad = Mathf.Deg2Rad * currentAngle;

            float outerX = Mathf.Sin(rad) * detectionRadius;
            float outerZ = Mathf.Cos(rad) * detectionRadius;
            float innerX = Mathf.Sin(rad) * innerRadius;
            float innerZ = Mathf.Cos(rad) * innerRadius;

            vertices[i * 2] = new Vector3(innerX, 0f, innerZ); // Inner arc vertex
            vertices[i * 2 + 1] = new Vector3(outerX, 0f, outerZ); // Outer arc vertex
        }

        for (int i = 0; i < arcResolution; i++)
        {
            int startIndex = i * 2;
            triangles[i * 6] = startIndex;
            triangles[i * 6 + 1] = startIndex + 1;
            triangles[i * 6 + 2] = startIndex + 2;

            triangles[i * 6 + 3] = startIndex + 1;
            triangles[i * 6 + 4] = startIndex + 3;
            triangles[i * 6 + 5] = startIndex + 2;
        }

        // Create the mesh
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
