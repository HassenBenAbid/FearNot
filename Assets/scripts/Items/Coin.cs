using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Items
{
    private const int MONEY_AMOUNT = 1;

    public override void startEffect()
    {
        LevelManager.Instance.addMoney(MONEY_AMOUNT);

        base.startEffect();
    }
}
