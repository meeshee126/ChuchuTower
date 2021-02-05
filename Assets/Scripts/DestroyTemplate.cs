using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTemplate : MonoBehaviour
{
    [SerializeField] Transform cameraDestroyPoint, templateDestroyPoint;

    private void Start()
    {
        cameraDestroyPoint = GameObject.Find("CameraDestroyPoint").transform;
    }

    void Update()
    {
        if(cameraDestroyPoint.position.y > templateDestroyPoint.position.y)
        {
            Destroy(this.gameObject);
        }
    }
}
