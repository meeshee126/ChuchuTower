using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Button buttonRestart;

    public List<GameObject> templates;

    void Start()
    {

    }

    void Update()
    {
       
        if (player == null)
            GameOver();
    }

    void GameOver()
    {
        buttonRestart.gameObject.SetActive(true);
    }

    public void ButtonRestart()
    {
        SceneManager.LoadScene(0);
    }
}
