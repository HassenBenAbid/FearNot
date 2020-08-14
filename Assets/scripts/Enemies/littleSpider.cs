using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class littleSpider : Enemy
{
    [SerializeField] private GameObject blood;

    protected override void Start()
    {
        base.Start();
        moving = true;
        anim.SetBool("walk", moving);
    }

    private void OnEnable()
    {
        attackTimer = attackCooldown;
    }

    protected override void notDead()
    {
        if (attackTimer <= 0)
        {
            base.notDead();
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    protected override void move()
    {
        if (moving && !target.isPlayerDead())
        {
            Vector2 newDestination = (targetTransform.position - transform.position).normalized;

            if (Mathf.Sign(transform.localScale.x) != Mathf.Sign(newDestination.x))
            {
                transform.localScale = new Vector2(transform.localScale.x *-1, transform.localScale.y);
            }
            rg.velocity = new Vector2(newDestination.x * speed * Time.deltaTime, rg.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !dead)
        {

            Vector2 newDestination = (targetTransform.position - transform.position).normalized;
            //rg.velocity = new Vector2(Mathf.Sign(newDestination.x) * 500.0f, rg.velocity.y);
            rg.velocity = Vector2.zero;
            rg.AddForce(new Vector2(-Mathf.Sign(newDestination.x) * 500.0f, 150.0f));
            target.hit();
            Instantiate(blood, collision.transform.position, Quaternion.identity);
            StartCoroutine(waitWalk());
        }
    }

    private IEnumerator waitWalk()
    {
        moving = false;
        anim.SetBool("walk", moving);
        yield return new WaitForSeconds(1.0f);
        moving = true;
        anim.SetBool("walk", moving);
    }
}
