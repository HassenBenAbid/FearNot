using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserUnlockable : ScriptableObject
{
    private const int WEAPONS_NUMBER = 3;
    private const int TEMP_ITEMS_NUMBER = 2;

    private static int money;

    private static bool[] weapons = new bool[WEAPONS_NUMBER];

    private static bool[] tempUnlockable = new bool[TEMP_ITEMS_NUMBER];

    private static int activeWeapon = 0;

    public static int getMoney()
    {
        return money;
    }

    public static void addMoney(int value)
    {
        money += value;
    }

    public static void saveWeapon(int weaponIndex, bool unlocked)
    {
        weapons[weaponIndex] = unlocked;
    }

    public static void saveTempUnlockable(bool[] unlockableArray)
    {
        tempUnlockable = unlockableArray;
    }

    public static void changeActiveWeapon(int weaponIndex)
    {
        activeWeapon = weaponIndex;
    }

    public static int getActiveWeapon()
    {
        return activeWeapon;
    }

    public static bool isBought(int weaponIndex)
    {
        return weapons[weaponIndex];
    }


    public static void changeActiveItem(int itemIndex, bool unlocked)
    {
        tempUnlockable[itemIndex] = unlocked;
    }

    public static bool isItemActive(int itemIndex)
    {
        return tempUnlockable[itemIndex];
    }

    public static void disableAllItems()
    {
        for(int i = 0; i<tempUnlockable.Length; i++)
        {
            tempUnlockable[i] = false;
        }
    }

    //for the savesystem

    public static bool[] getAllWeapons()
    {
        return weapons;
    }
    public static bool[] getAllItems()
    {
        return tempUnlockable;
    }

    public static void setAllWeapons(bool[] allWeapons)
    {
        weapons = allWeapons;
    }

    public static void setAllItems(bool[] allItems)
    {
        tempUnlockable = allItems;
    }




}
