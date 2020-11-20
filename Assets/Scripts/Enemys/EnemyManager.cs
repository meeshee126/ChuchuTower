using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] protected GameObject player;
    [SerializeField] protected float speed;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
          //  Destroy(collision.gameObject);
        }
    }
}
