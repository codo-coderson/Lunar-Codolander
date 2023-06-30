using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
 
    public CinemachineVirtualCamera overallCamera;
    public CinemachineVirtualCamera zoomedCamera;

    // Start is called before the first frame update
    void Start()
    {
        zoomedCamera.Priority = 5;
        overallCamera.Priority = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        zoomedCamera.Priority = 10;
        overallCamera.Priority = 5;    
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        zoomedCamera.Priority = 5;
        overallCamera.Priority = 10;
    }
       
}
