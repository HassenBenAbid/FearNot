using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drug : Items
{
    private const float SECONDS_ADDED = 10.0f;   

    public override void startEffect()
    {
        LevelManager.Instance.addToTimer(SECONDS_ADDED);

        base.startEffect();
    }
}
