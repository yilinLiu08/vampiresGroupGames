using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //reference to player
    public Transform playerTransform;
    //how smoothly the camera transitions to the target position
    //higher value will follow player quickly while lower is more gradual movement
    public float smoothSpeed;
    public Vector3 offsetPos;

  

    // Start is called before the first frame update
    void Start()
    {

    }

   
       
      

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerTransform != null)
        {
            //calculate the target pos the camera should move to
            //creates a new position that considers where the cam is relative to the player
            Vector3 desiredPos = playerTransform.position + offsetPos;
            //lerp smoothly interpolate btwn current pos and desired pos
            Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
            smoothPos.z = offsetPos.z;
            //this line sets the cam pos to the new smoothly interpolated position
            transform.position = smoothPos;

        }
    }
    
}
