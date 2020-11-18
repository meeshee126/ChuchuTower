using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightEnemy : EnemyManager
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
