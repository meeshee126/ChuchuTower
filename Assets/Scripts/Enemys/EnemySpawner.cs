using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> enemy;
    Transform[] spawner;
    Transform enemyStack;

    public float spawnTimer;
    float count = 0;

    void Start()
    {
        enemyStack = GameObject.Find("EnemyStack").transform;

        // store all spawner in an array
        spawner = new Transform[transform.childCount];

        for (int i = 0; i < spawner.Length; i++)
        {
            spawner[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        Spawning();
    }

    void Spawning()
    {
        count += Time.deltaTime;

        if(count > spawnTimer)
        {
            Instantiate(enemy[Random.Range(0, enemy.Count)], spawner[Random.Range(0, spawner.Length)].position, Quaternion.identity, enemyStack);
            count = 0;
        }
    }
}
