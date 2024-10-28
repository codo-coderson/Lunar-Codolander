using UnityEngine;

public class CanvasDontRotate : MonoBehaviour
{
    private Quaternion initialRotation;

    void Start()
    {
        // Guarda la rotaci�n inicial del Canvas
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // Bloquea la rotaci�n del Canvas a su rotaci�n inicial
        transform.rotation = initialRotation;
    }
}
