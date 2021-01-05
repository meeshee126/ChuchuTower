using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemy : EnemyManager
{
    [SerializeField] GameObject bodyPrefab;
    [SerializeField] List<Transform> bodyParts = new List<Transform>();
    [SerializeField] int addSize;
    [SerializeField] float minDistance = 0.25f;
    [Space]
    [SerializeField] float frequency; // Speed of sine movement
    [SerializeField] float magnitude; //  Size of sine movement
    [SerializeField] bool chase;

    float distance;
    Transform curBodyPart;
    Transform prevBodyPart;

    Vector3 pos;

    private void Start()
    {
        player = GameObject.Find("Player");

        for (int i = 0; i < addSize - 1; i++)
        {
            AddBodyPart();
        }

        pos = transform.position;
        RotateToPlayer();
    }

    void Update()
    {
        if(chase)
        RotateToPlayer();

        Move();         
    }

    void AddBodyPart()
    {
        Transform newPart = (Instantiate(bodyPrefab, bodyParts[bodyParts.Count - 1].position, bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;

        newPart.SetParent(transform.parent);

        bodyParts.Add(newPart);
    }

    void Move()
    {
        float curSpeed = speed;

        pos += transform.right * speed * Time.deltaTime;
        transform.position = pos + (transform.up * Mathf.Sin(frequency * Time.time) * magnitude);

        for (int i = 1; i < bodyParts.Count; i++)
        {
            curBodyPart = bodyParts[i];
            prevBodyPart = bodyParts[i - 1];

            distance = Vector2.Distance(prevBodyPart.position, curBodyPart.position);

            Vector2 newPos = prevBodyPart.position;

           // newPos.y = bodyParts[0].position.y;

            float time = Time.deltaTime * distance / minDistance * curSpeed;

            if (time > 0.5f)
                time = 0.5f;

            curBodyPart.position = Vector3.Slerp(curBodyPart.position, newPos, time);
            curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, prevBodyPart.rotation, time);
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
