using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationBullet : MonoBehaviour
{

    [SerializeField] private float speed = 200.0f;

    private Vector2 targetPosition;
    private Rigidbody2D rg;
    private bool playerFollow;

    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
        playerFollow = true;
    }

    public void goToPlayer(bool answer, Vector2 direction  = default)
    {
        playerFollow = answer;
        if (!answer)
        {
            targetPosition = direction.normalized;
            speed = 350.0f;
        }
    }

    private void OnEnable()
    {
        if (playerFollow)
        {
            targetPosition = (GameObject.FindWithTag("Player").transform.position - transform.position).normalized;
            if (targetPosition != Vector2.zero)
            {
                float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
            }
        }
    }

    private void Update()
    {
        move();
    }

    private void move()
    {
        rg.velocity = targetPosition * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.SendMessage("hit");
        }

        if (collision.tag != "enemy")
        {
            gameObject.SetActive(false);
        }
    }
}
