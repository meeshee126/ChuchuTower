using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] float speed, startAcceleration, maxSpeed, speedUpAccerleration, speedDownDeceleration;

    //value for lerping camera
    float tStart, tSpeedUp, tSpeedDown;

    float currentSpeed;

    bool isSpeedingUp = false;

    void Start()
    {
        
    }

    void Update()
    {
        MoveCamera();

        //lerp camera speed wwhen enter trigger
        if(isSpeedingUp)
        {
            LerpSpeedUp();
        }
        else
        {
            LerpSpeedDown();
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

    void LerpSpeedUp()
    {
        tSpeedUp += speedUpAccerleration * Time.deltaTime;
        currentSpeed = Mathf.Lerp(speed, maxSpeed, tSpeedUp);
    }
    
    void LerpSpeedDown()
    {
        tSpeedDown += speedDownDeceleration * Time.deltaTime;
        currentSpeed = Mathf.Lerp(currentSpeed, speed, tSpeedDown);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isSpeedingUp = true;

            //reset lerp timer
            tSpeedUp = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isSpeedingUp = false;

            //reset lerp timer
            tSpeedDown = 0;
        }
    }
}
