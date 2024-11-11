using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera mainCamera; // referencia a la cámara principal
    //private Vector3 offset = new Vector3(0.207F, -0.08F, 0);

    void OnTriggerEnter2D(Collider2D other)
    {
        // cambia el padre de la cámara al gameObject "ship"
        mainCamera.transform.parent = transform;
        mainCamera.orthographicSize = 1.5F;
        //mainCamera.transform.position = mainCamera.transform.position + offset;
    }

    void LateUpdate()
    {
        // update the X and Y position of the camera
        Vector3 newPosition = transform.position; // + offset;
        mainCamera.transform.position = new Vector3(newPosition.x, newPosition.y, mainCamera.transform.position.z);

        // reset the rotation of the camera
        mainCamera.transform.rotation = Quaternion.identity;
    }
}

