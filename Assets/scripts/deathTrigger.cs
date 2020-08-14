using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().oneShotKill();
            gameObject.SetActive(false);
        }else if (collision.tag == "enemy" || collision.tag == "enemyCollider")
        {
            Destroy(collision.gameObject);
        }
    }
}
