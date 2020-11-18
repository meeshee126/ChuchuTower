using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : EnemyManager
{
    void Start()
    {
    
    }

    void Update()
    {
        if(player != null)
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
