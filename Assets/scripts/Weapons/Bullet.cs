using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 100.0f;
    [SerializeField] private GameObject blood;

    private float lifetime = 2.0f;
    private Rigidbody2D rg;
    private string hitTag = "enemy", ignoreTag = "Player";
    private float lifeTimer = 0;
    private int direction = 1;


    private void Start()
    {
        rg = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        lifeTimer = lifetime;
        direction = (int)Mathf.Sign(transform.localScale.x);
    }

    private void Update()
    {
        if (lifeTimer <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            lifeTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        move();
    }

    private void move()
    {
        rg.velocity = (Vector2)transform.right * speed * Time.deltaTime * direction;
        //rg.velocity = new Vector2(transform.right.x * speed * Time.deltaTime * Mathf.Sign(transform.localScale.x), transform.right.y * speed * Time.deltaTime) ;
    }

    public void playerBullet(bool answer)
    {
        if (answer)
        {
            hitTag = "enemy";
            ignoreTag = "Player";
        }else
        {
            hitTag = "Player";
            ignoreTag = "enemy";
        }
    }

    public void setBulletLifetime(float life)
    {
        lifetime = life;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.gameObject.tag == hitTag)
         {
            Instantiate(blood, collision.transform.position, Quaternion.identity);
            if (collision.GetComponent<Enemy>() || collision.GetComponent<Player>())
            {
                collision.SendMessage("hit");
            }
         }

         if (collision.gameObject.tag != ignoreTag)
         {
              gameObject.SetActive(false);   
         }  

         
    }
}
