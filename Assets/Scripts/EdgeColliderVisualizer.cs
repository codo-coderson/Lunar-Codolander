using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class EdgeColliderVisualizer : MonoBehaviour
{
    private EdgeCollider2D edgeCollider;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        UpdateLineRendererPositions();
    }

private void UpdateLineRendererPositions()
{
    Vector2[] colliderPoints = edgeCollider.points;
    lineRenderer.positionCount = colliderPoints.Length;

    for (int i = 0; i < colliderPoints.Length; i++)
    {
        Vector3 position = new Vector3(colliderPoints[i].x, colliderPoints[i].y - 2.21f, colliderPoints[i].y);
        lineRenderer.SetPosition(i, position);
    }
}
}
