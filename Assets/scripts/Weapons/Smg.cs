using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smg : Weapon
{

    public override void fire(int playerDirection, bool isPlayer = true)
    {
        if (fireTimer >= fireCooldown && currentClipAmmo > 0)
        {
            fireSound.Play();
            GameObject b = useObject();
            b.GetComponent<Bullet>().playerBullet(isPlayer);
            b.GetComponent<Bullet>().setBulletLifetime(bulletLifeTime);
            b.transform.position = firePos.position;
            b.transform.rotation = transform.rotation;
            b.transform.localScale = new Vector2(playerDirection * Mathf.Abs(b.transform.localScale.x), b.transform.localScale.y);
            b.SetActive(true);
            fireTimer = 0;
            base.fire(playerDirection);
            currentClipAmmo--;
        }else if (currentClipAmmo <= 0 && !isReloading)
        {
            isReloading = true;
            reloadingTimer = reloadTime;
        }

    }



}
