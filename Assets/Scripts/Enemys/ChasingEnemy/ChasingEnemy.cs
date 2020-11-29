using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : EnemyManager
{
    [SerializeField] float frequency = 10.0f; // Speed of sine movement
    [SerializeField] float magnitude = 0.5f; //  Size of sine movement
    [SerializeField] bool chase;
    Vector3 pos;

    private void Start()
    {
        pos = transform.position;
        RotateToPlayer();
    }

    void Update()
    {
        if(chase)
        RotateToPlayer();

        Move();         
    }

    void Move()
    {
        pos += transform.right * speed * Time.deltaTime;
        transform.position = pos + (transform.up * Mathf.Sin(frequency * Time.time) * magnitude);
    }

    void RotateToPlayer()
    {
        Vector3 difference = player.transform.position - this.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }
}
