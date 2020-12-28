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
    [SerializeField] float dashDistance;
    [SerializeField] float dashtime;
    [SerializeField] float dashCD;
    [SerializeField] LayerMask playerLayer;

    Rigidbody2D rb;

    float time;
    float expanse;
    float dashCount;
    float dashCDCount;
    bool dashing;
    bool canDash;

    Vector3 moveToPlayer;
    Vector3 snakeMovement;


    Transform curBodyPart;
    Transform frontBodyPart;

    private void Start()
    {

        for (int i = 0; i < addSize - 1; i++)
        {
            AddBodyPart();
        }

        expanse = magnitude;

        rb = GetComponent<Rigidbody2D>();

        player = GameObject.Find("Player");

        //avoid starting from 0,0,0, vector
        moveToPlayer = transform.position;

        RotateToPlayer();
    }

    void AddBodyPart()
    {
        Transform newPart = (Instantiate(bodyPrefab, 
                             bodyParts[bodyParts.Count - 1].position, 
                             bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;

        newPart.SetParent(transform.parent);
        bodyParts.Add(newPart);
    }

    void Update()
    {  
        MoveHeadPart();
        MoveBodyPart();
    }

    void MoveHeadPart()
    {
        if (chase && !dashing)
        {
            RotateToPlayer();
        }

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.right, dashDistance, playerLayer);

        if (hit && !dashing && canDash)    
        {          
            dashing = true;
            RotateToPlayer();
            Debug.Log("hit");
        }
        //dash at a certain distance
        //if (Vector2.Distance(this.transform.position, player.transform.position) < dashDistance)

        if(dashing)
        {
            canDash = false;

            dashCount += Time.deltaTime;
           
            time = time >= 1 ? time = 1 : time += 2f * Time.deltaTime;
            expanse = Mathf.Lerp(magnitude, 0.1f, time);

            //Invoke("DashCD", dashtime);

            if(dashCount > dashtime)
            {
                dashing = false;
            }

            Debug.Log("dashing");
        }
        //turn back to snake movement
        else
        {
            dashCount = 0;
            
            canDash = true;
           
            time = time <= 0.1f ? time = 0.1f : time -= 1f * Time.deltaTime;
            expanse = Mathf.Lerp(magnitude, 0.1f, time);

            Debug.Log("not dahsing");
        }
       
        moveToPlayer += transform.right * speed * Time.deltaTime;
        snakeMovement = transform.up * Mathf.Sin(frequency * Time.time) * expanse;

        //move head part
        transform.position = moveToPlayer + snakeMovement;
    }

    void DashCD()
    {
        dashing = false;
    }

    void MoveBodyPart()
    {
        float distance;

        //follow front body/Head part
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
