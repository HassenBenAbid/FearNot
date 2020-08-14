using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEnemy : Enemy
{

    [SerializeField] private Transform rotatingArm;
    [SerializeField] private Weapon gun;

    protected override void notDead()
    {
        base.notDead();
        if (playerDetected)
        {
            rotateArm();
            attack();
        }
    }

    protected override void attack()
    {
        if (attackTimer <= 0)
        {
            gun.fire((int)Mathf.Sign(transform.localScale.x), false);
            attackTimer = attackCooldown;
        }else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void rotateArm()
    {
            Vector2 distance = (Vector2)(rotatingArm.position - targetTransform.position);
            float angle = Mathf.Atan2(distance.x, distance.y) * Mathf.Rad2Deg + (25.0f * Mathf.Sign(transform.localScale.x));
            rotatingArm.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
    }
}
