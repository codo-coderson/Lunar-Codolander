using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class TerrainVisualizer : MonoBehaviour
{
    private EdgeCollider2D edgeCollider; // Reference to the EdgeCollider2D component
    private LineRenderer lineRenderer; // Reference to the LineRenderer component

    private void Awake()
    {
        // Get the EdgeCollider2D component attached to the GameObject
        edgeCollider = GetComponent<EdgeCollider2D>();
        // Get the LineRenderer component attached to the GameObject
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        // Update the positions of the LineRenderer based on the EdgeCollider2D points
        UpdateLineRendererPositions();
    }

    private void UpdateLineRendererPositions()
    {
        // Get the points from the EdgeCollider2D
        Vector2[] colliderPoints = edgeCollider.points;
        // Set the number of positions in the LineRenderer
        lineRenderer.positionCount = colliderPoints.Length;

        // Loop through each point in the EdgeCollider2D
        for (int i = 0; i < colliderPoints.Length; i++)
        {
            // Create a new Vector3 position for the LineRenderer
            Vector3 position = new Vector3(colliderPoints[i].x, colliderPoints[i].y - 2.21f, colliderPoints[i].y);
            // Set the position in the LineRenderer
            lineRenderer.SetPosition(i, position);
        }
    }

    // Let the terrain be drawn in the Scene window
    private void OnDrawGizmos()
    {
        // Check if the edgeCollider is null and get the EdgeCollider2D component if necessary
        if (edgeCollider == null)
        {
            edgeCollider = GetComponent<EdgeCollider2D>();
        }

        // Get the points from the EdgeCollider2D
        Vector2[] colliderPoints = edgeCollider.points;
        // Set the Gizmos color to white
        Gizmos.color = Color.white;

        // Loop through each point in the EdgeCollider2D except the last one
        for (int i = 0; i < colliderPoints.Length - 1; i++)
        {
            // Create a new Vector3 position for the start of the line
            Vector3 start = new Vector3(colliderPoints[i].x, colliderPoints[i].y - 2.21f, colliderPoints[i].y);
            // Create a new Vector3 position for the end of the line
            Vector3 end = new Vector3(colliderPoints[i + 1].x, colliderPoints[i + 1].y - 2.21f, colliderPoints[i + 1].y);
            // Draw a line between the start and end positions
            Gizmos.DrawLine(start, end);
        }
    }
}
