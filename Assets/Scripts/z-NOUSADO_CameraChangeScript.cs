using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChangeScript : MonoBehaviour
{
    // Declare public variables that can be adjusted in the inspector
    public float smoothSpeed = 0.125f; // Speed of camera movement
    public Vector3 offset; // Offset of camera position from target
    public float zoom = 5f; // Zoom level of camera

    // Method called when the trigger collider is entered
    void OnTriggerEnter2D(Collider2D other)
    {
        Camera.main.orthographicSize = 1.5F;
        GameObject ship = GameObject.Find("ship");
        if (ship != null)
        {
            Debug.Log("Ship position: " + ship.transform.position);
            Vector3 targetPosition = ship.transform.position + offset;
            Quaternion targetRotation = Quaternion.LookRotation(ship.transform.position - Camera.main.transform.position);

            StartCoroutine(SmoothCameraMovement(targetPosition, targetRotation, smoothSpeed));
        }
    }

    IEnumerator SmoothCameraMovement(Vector3 targetPosition, Quaternion targetRotation, float smoothTime)
    {
        Vector3 velocity = Vector3.zero;
        targetPosition.z = Camera.main.transform.position.z; // Keep the camera's Z coordinate fixed
        while (Vector2.Distance(Camera.main.transform.position, targetPosition) > 0.01f)
        {
            Vector3 smoothDampedPosition = Vector3.SmoothDamp(Camera.main.transform.position, targetPosition, ref velocity, smoothTime);
            Camera.main.transform.position = new Vector3(smoothDampedPosition.x, smoothDampedPosition.y, Camera.main.transform.position.z); // Maintain the camera's Z coordinate
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, targetRotation, smoothTime * Time.deltaTime);
            yield return null;
        }
    }

}


