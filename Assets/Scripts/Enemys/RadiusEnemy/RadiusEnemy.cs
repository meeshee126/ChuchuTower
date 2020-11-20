using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusEnemy : EnemyManager
{
    [SerializeField] float timeDoDestroy;

    float count = 0;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;

        count += Time.deltaTime;

        if (count > timeDoDestroy)
            Destroy(this.gameObject);
    }
}
