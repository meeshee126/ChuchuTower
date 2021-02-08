using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusEnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject  player , entity;
    [SerializeField] int numberOfEntitys;
    [SerializeField] float spawnInterval, spaceInRotation;

    Transform enemyStack;
    float rotationZ, increase;
    
    private void Awake()
    {
        player = GameObject.Find("Player");
        enemyStack = GameObject.Find("EnemyStack").transform;
    }

    void Start()
    {
        increase = spaceInRotation;

        if(player != null)
            RotateToPlayer();

        SetRadius();
        StartCoroutine(spawnEntity(spawnInterval, numberOfEntitys));
    }

    // look at player position
    void RotateToPlayer()
    {
        Vector3 difference = player.transform.position - this.transform.position;
        rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    // set spacing between spawning entitys
    void SetRadius()
    {
        float average = numberOfEntitys / 2;
        spaceInRotation += (average * -spaceInRotation);
    }

    IEnumerator spawnEntity ( float interval, float invokeCount)
    {
        for (int i = 0; i < invokeCount; i++)
        {
            GameObject ob = Instantiate(entity, this.transform.position, Quaternion.Euler(0, 0, rotationZ + spaceInRotation), enemyStack);

            spaceInRotation += increase;

            yield return new WaitForSeconds(interval);
        }

        Destroy(this.gameObject);
    }
}
