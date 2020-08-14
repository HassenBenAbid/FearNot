using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rds;

public class Items : RDSUnityObject
{  
    public virtual void startEffect()
    {
        Destroy(this.gameObject);
    }
}
