using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Text uiMetres;

    public List<GameObject> templates;

    int lastMetre;
    int metres;

    void Start()
    {
        //set starting metre
        metres = (int)player.position.y;
        lastMetre = metres;
    }

    void Update()
    {
        CountMetres();
    }

    void CountMetres()
    {
        metres = (int)player.position.y;

        //avoid metre subtraction
        if (metres < lastMetre)
        {
            uiMetres.text = lastMetre.ToString();
        }
        //count metres
        else
        {
            lastMetre = metres;

            uiMetres.text = metres.ToString();
        }
    }
}
