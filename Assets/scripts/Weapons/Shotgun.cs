using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private int bulletNumber;

    public override void fire(int playerDirection, bool isPlayer = true)
    {
        if (fireTimer >= fireCooldown && currentClipAmmo > 0)
        {
            fireSound.Play();
            for(float i=-2.5f * bulletNumber; i<bulletNumber * 2 * 2.5f; i += 2.5f * bulletNumber)
            {
                GameObject currentBullet = useObject();
                currentBullet.GetComponent<Bullet>().playerBullet(isPlayer);
                currentBullet.GetComponent<Bullet>().setBulletLifetime(bulletLifeTime);
                currentBullet.transform.position = firePos.position;
                currentBullet.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + i);
                currentBullet.transform.localScale = new Vector2(playerDirection * Mathf.Abs(currentBullet.transform.localScale.x), currentBullet.transform.localScale.y);
                currentBullet.SetActive(true);
            }
            fireTimer = 0;
            base.fire(playerDirection);
            currentClipAmmo -= 3;
        }
        else if (currentClipAmmo <= 0 && !isReloading)
        {
            isReloading = true;
            reloadingTimer = reloadTime;
        }
    }

}
