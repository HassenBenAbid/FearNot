using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponsUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> gunsUI;
    [SerializeField] private TextMeshProUGUI ammoUI;
    [SerializeField] private Transform gunsUIPosition;

    private int activeWeaponIndex = -1;

    private void Awake()
    {
        for(int i=0; i<gunsUI.Count; i++)
        {
            gunsUI[i].transform.position = gunsUIPosition.position;
            gunsUI[i].SetActive(false);
        }

        ammoUI.gameObject.SetActive(false);
    }

    public void setWeapon(int weaponIndex, int maxAmmo = 0)
    {

        if (weaponIndex != activeWeaponIndex)
        {
            if (weaponIndex == -1)
            {
                ammoUI.gameObject.SetActive(false);
                gunsUI[activeWeaponIndex].SetActive(false);
            }
            else
            {
                gunsUI[weaponIndex].SetActive(true);
                ammoUI.gameObject.SetActive(true);
                if (weaponIndex == 0)
                {
                    ammoUI.text = "/ INFINITY";
                }
                else
                {
                    ammoUI.text = "/ " + maxAmmo;
                }

            }

            activeWeaponIndex = weaponIndex;
        }
    }

    public void refreshAmmo(int currentAmmo)
    {
        if (activeWeaponIndex != 0)
        {
            ammoUI.text = "/ " + currentAmmo;
        }
    }


}
