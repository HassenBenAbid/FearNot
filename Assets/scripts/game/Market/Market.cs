using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Market : Singleton<Market>
{
    [SerializeField] private List<MarketWeapons> WeaponsToSell;
    [SerializeField] private TextMeshProUGUI currentMoney;

    private void OnEnable()
    {
        int money = UserUnlockable.getMoney();
        currentMoney.text = money.ToString();
    }


    public void unequip()
    {
        for(int i=0; i<WeaponsToSell.Count; i++)
        {
            if (WeaponsToSell[i].isEquiped())
            {
                WeaponsToSell[i].unequip();
                break;
            }
        }
    }

    public void exitMarket()
    {
        this.gameObject.SetActive(false);
    }
}
