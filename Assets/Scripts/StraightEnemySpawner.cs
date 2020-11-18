using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightEnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject  player , entity;
    [SerializeField] int numberOfEntitys;
    [SerializeField] float spawnInterval, spaceInRotation;

    float rotationZ, increase;
    
    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    void Start()
    {
        increase = spaceInRotation;

        RotateToPlayer();
        SetRadius();
        StartCoroutine(spawnEntity(spawnInterval, numberOfEntitys));
    }

    void RotateToPlayer()
    {
        Vector3 difference = player.transform.position - this.transform.position;
        rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    void SetRadius()
    {
        float average = numberOfEntitys / 2;
        spaceInRotation += (average * -spaceInRotation);
    }

    IEnumerator spawnEntity ( float interval, float invokeCount)
    {
        for (int i = 0; i < invokeCount; i++)
        {
            GameObject ob = Instantiate(entity, this.transform.position, Quaternion.Euler(0, 0, rotationZ + spaceInRotation), this.transform);

            spaceInRotation += increase;

            yield return new WaitForSeconds(interval);
        }
    }
}
