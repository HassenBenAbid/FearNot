using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    private bool playerDetected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerDetected = true;
            //LevelManager.Instance.triggerSomething();
        }
    }

    public bool playerInside()
    {
        return playerDetected;
    }
}
