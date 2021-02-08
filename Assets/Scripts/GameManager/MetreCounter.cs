using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetreCounter : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Text uiMetres;

    int lastMetre;
    int metres;

    void Start()
    {
        //set starting metre
        metres = (int)player.transform.position.y;
        lastMetre = metres;
    }

    void Update()
    {
        if (player != null)
            metres = (int)player.transform.position.y;

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
