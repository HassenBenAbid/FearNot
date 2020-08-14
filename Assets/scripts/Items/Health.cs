using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Items
{

    public override void startEffect()
    {
        Player thePlayer = GameObject.Find("player").GetComponent<Player>();

        thePlayer.addHealth();

        base.startEffect();
    }
}
