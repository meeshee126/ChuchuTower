using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTemplate : MonoBehaviour
{
    GameManager gameManager;
    List<GameObject> templates;
    Transform environment;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        templates = gameManager.templates;

        environment = GameObject.Find("Environment").GetComponent<Transform>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Instantiate(templates[Random.Range(0, templates.Count)], this.transform.parent.position + new Vector3(0, 10, 0), Quaternion.identity, environment);
        }
        Destroy(this.gameObject);
    }
}
