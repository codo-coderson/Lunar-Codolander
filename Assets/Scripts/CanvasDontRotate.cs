using UnityEngine;

public class CanvasDontRotate : MonoBehaviour
{
    private Quaternion initialRotation;

    void Start()
    {
        // Guarda la rotación inicial del Canvas
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // Bloquea la rotación del Canvas a su rotación inicial
        transform.rotation = initialRotation;
    }
}
