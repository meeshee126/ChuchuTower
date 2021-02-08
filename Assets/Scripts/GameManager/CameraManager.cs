using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform player, accelerationBorder, cameraDestroyPoint;
    [SerializeField] float speed, startAcceleration, speedMultiplikator =1;

    //value for lerping camera
    float tStart;

    float currentSpeed;
   
    private void Start()
    {
       
    }

    void Update()
    {       
        MoveCamera();

        //camera speed up when player get closer to camera top border
        if(player != null && player.position.y > accelerationBorder.position.y)
        {
            CameraSpeed();
        }

        if(player != null)
            DestroyPlayer();
    }

    void MoveCamera()
    {
        //Start lerp on beginning only once
        if(currentSpeed <= speed)
        {
            LerpStartAccerleration();
        }
        
        //moving up
        if(player != null)
           transform.position += Vector3.up * currentSpeed * Time.deltaTime;
    }

    void LerpStartAccerleration()
    {
        tStart += startAcceleration * Time.deltaTime;
        currentSpeed = Mathf.Lerp(0, speed, tStart);
    }

    void CameraSpeed()
    {
        //the higher the player, the higher the camera speed
        float maxAccerlerationSpeed = player.position.y - accelerationBorder.position.y;

        currentSpeed = maxAccerlerationSpeed * speedMultiplikator;

        //avoid camera speed lower then std
        if(currentSpeed < speed)
        {
            currentSpeed = speed;
        }
    }

    void DestroyPlayer()
    {
        //get top collider boundary position
        float playerTopBoundaryPosition = player.position.y + (player.gameObject.GetComponent<BoxCollider2D>().bounds.extents.y * 2);

        if (cameraDestroyPoint.position.y > playerTopBoundaryPosition)
            Destroy(player.gameObject);
    }
}
