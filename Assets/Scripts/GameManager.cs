using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private Camera mainCamera;
    public Vector2 widthThresold;
    public Vector2 heightThresold;

    void Awake()
    {
        //Get your mainCamera here. If you are pretty sure that the camera is always the Camera.main you don't need to implement here. Just call for Camera.main later.
    }

    void Update()
    {
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(transform.position);
        if (screenPosition.x < widthThresold.x || screenPosition.x > widthThresold.y || screenPosition.y < heightThresold.x || screenPosition.y > heightThresold.y)
            Destroy(GameObject.Find("Player"));
    }

}
