using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{

    public GameObject homeFrog;

    private void OnEnable()
    {
        homeFrog.SetActive(true);
    }

    private void OnDisable()
    {
        homeFrog.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enabled = true;
            FindObjectOfType<GameManager>().HomeOccupied();
        }   
    }
}
