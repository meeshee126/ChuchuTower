using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeManager : EnemyManager
{
    [SerializeField] int maxMoveRange = 5;

    Vector3 nextPos;

    bool moving;

    void Start()
    {
        RotateToNextPosition();
    }

    void Update()
    {
        if(!moving)
            RotateToNextPosition();

        Move();
    }

    void Move()
    {
        if (Vector3.Distance(nextPos, this.transform.position) < 0.3f)
            moving = false;

        this.transform.position += transform.right * Time.deltaTime * speed;    
    }

    Vector3 SetPosition()
    {
        //set compass directions
        Vector3[] directions = new Vector3[] { this.transform.position + new Vector3(0, maxMoveRange, 0),    // north point
                                               this.transform.position + new Vector3(maxMoveRange, 0, 0),    // east point
                                               this.transform.position + new Vector3(0, -maxMoveRange, 0),   // south point
                                               this.transform.position + new Vector3(-maxMoveRange, 0, 0)};  // west point

        //set first closest position
        Vector3 closestPos = directions[0];

        //iterate trough all compass directions and find the closest position to player 
        for (int i = 0; i < directions.Length; i++)
        {
            if(Vector3.Distance(player.transform.position, closestPos) > Vector3.Distance(player.transform.position, directions[i]))
            {
                closestPos = directions[i];
            }
        }

        nextPos = closestPos;

        return closestPos;
    }

    void RotateToNextPosition()
    {
        Vector3 difference = SetPosition() - this.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        moving = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position + new Vector3(0, maxMoveRange, 0), 0.1f);
        Gizmos.DrawWireSphere(this.transform.position + new Vector3(maxMoveRange, 0, 0), 0.1f);
        Gizmos.DrawWireSphere(this.transform.position + new Vector3(0, -maxMoveRange, 0), 0.1f);
        Gizmos.DrawWireSphere(this.transform.position + new Vector3(-maxMoveRange, 0, 0), 0.1f);
    }
}
