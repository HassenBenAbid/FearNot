using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleSkull : Enemy
{

    protected override void move()
    {
        rg.velocity = new Vector2(speed * Vector2.left.x * Time.deltaTime, 0);
        Destroy(this.gameObject, attackCooldown);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            target.hit();
            Destroy(this.gameObject);
        }
    }


}
