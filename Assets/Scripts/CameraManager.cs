using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform transfPlayer, accBorder;
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
        if(transfPlayer.position.y > accBorder.position.y)
        {
            CameraSpeed();
        }
    }

    void MoveCamera()
    {
        //Start lerp on beginning only once
        if(currentSpeed <= speed)
        {
            LerpStartAccerleration();
        }
        
        //moving up
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
        float maxAccerlerationSpeed = transfPlayer.position.y - accBorder.position.y;

        currentSpeed = maxAccerlerationSpeed * speedMultiplikator;

        //avoid camera speed lower then std
        if(currentSpeed < speed)
        {
            currentSpeed = speed;
        }
    }
}
