using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEnemy : Enemy
{
    [SerializeField] private GameObject meleeAttack;

    protected override void notDead()
    {
        base.notDead();
        if (playerDetected)
        {
            attack();
        }
    }

    protected override void attack()
    {
        base.attack();
        if (attackTimer <= 0)
        {
            float distance = transform.position.x - targetTransform.position.x;
            if (Mathf.Abs(distance) <= movingDistance)
            {
                anim.SetTrigger("attack");
                meleeAttack.SetActive(true);
                attackTimer = attackCooldown;
            }
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

}
