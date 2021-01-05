using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemy : EnemyManager
{
    [SerializeField] GameObject bodyPrefab;
    [SerializeField] List<Transform> bodyParts = new List<Transform>();
    [SerializeField] int addSize;
    [SerializeField] float minDistance = 0.25f;
    [SerializeField] float frequency; // Speed of sine movement
    [SerializeField] float magnitude; //  Size of sine movement
    [SerializeField] bool chase;
    [SerializeField] float straightChaseDistance;
    [SerializeField] Vector3 snakeMovement;

    Rigidbody2D rb;

    float distance;
    Transform curBodyPart;
    Transform frontBodyPart;

    Vector3 moveToPlayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.Find("Player");

        for (int i = 0; i < addSize - 1; i++)
        {
            AddBodyPart();
        }

        moveToPlayer = transform.position;
        RotateToPlayer();
    }

    void AddBodyPart()
    {
        Transform newPart = (Instantiate(bodyPrefab, bodyParts[bodyParts.Count - 1].position, bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;

        newPart.SetParent(transform.parent);

        bodyParts.Add(newPart);
    }

    void Update()
<<<<<<< HEAD
    {
        if(chase)
        RotateToPlayer();

        Move();         
    }

    void Move()
    {
        float expanse = magnitude; ;

        moveToPlayer = transform.right * speed * Time.deltaTime;

        //straight chase at a certain distance
        if (Vector2.Distance(this.transform.position, player.transform.position) < straightChaseDistance)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }

        snakeMovement = (transform.up * Mathf.Sin(frequency * Time.time) * magnitude);
=======
    {
        if(chase)
        RotateToPlayer();

        Move();         
    }

    void Move()
    {
        float expanse = magnitude; ;

        moveToPlayer = transform.right * speed * Time.deltaTime;

        //straight chase at a certain distance
        if (Vector2.Distance(this.transform.position, player.transform.position) < straightChaseDistance)
        {
            transform.position += transform.right * speed * Time.deltaTime;

        }

        snakeMovement = (transform.up * Mathf.Sin(frequency * Time.time) * magnitude);


>>>>>>> parent of 3dcc2d4... ALMOST DONE Snake movement

        //follow front body part
        for (int i = 1; i < bodyParts.Count; i++)
        {
            curBodyPart = bodyParts[i];
            frontBodyPart = bodyParts[i - 1];

            distance = Vector2.Distance(frontBodyPart.position, curBodyPart.position);

            //newPos.y = bodyParts[0].position.y;

            float bodySpeed = Time.deltaTime * distance / minDistance * speed;

            if (bodySpeed > 0.5f)
                bodySpeed = 0.5f;

            curBodyPart.position = Vector3.Slerp(curBodyPart.position, frontBodyPart.position, bodySpeed);
            curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, frontBodyPart.rotation, bodySpeed);
        }
    }

    //face player
    void RotateToPlayer()
    {
        Vector3 difference = player.transform.position - this.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }
}
