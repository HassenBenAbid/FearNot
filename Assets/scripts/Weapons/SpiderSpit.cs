using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSpit : MonoBehaviour
{
    const float TIME_BEFORE_DESTROY = 10.0f;

    [SerializeField] private float speed = 250.0f, followTime = 5.0f;
    [SerializeField] private GameObject blood;

    private Transform target;
    private Rigidbody2D rg;
    private float timer, destroyTimer;
    private Vector2 distance;

    private void OnEnable()
    {
        timer = 0;
        destroyTimer = 0;
    }

    private void Start()
    {
        target = GameObject.Find("player").transform;
        rg = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        move();

        if (destroyTimer < TIME_BEFORE_DESTROY)
        {
            destroyTimer += Time.deltaTime;
        }else
        {
            gameObject.SetActive(false);
        }
    }

    private void move()
    {
        if (timer < followTime)
        {
            distance = (target.position - transform.position).normalized;
            timer += Time.deltaTime;
        }
        rg.velocity = (Vector2)distance * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.SendMessage("hit");
            Instantiate(blood, collision.transform.position, Quaternion.identity);
        }

        if (collision.tag != "enemy")
        {
            gameObject.SetActive(false);
        }
    }
}
