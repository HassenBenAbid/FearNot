using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rds;

public class RDSUnityObject : MonoBehaviour, IRDSOBJECT
{
    [SerializeField] private double Probability;
    [SerializeField] private bool Unique;
    [SerializeField] private bool Always;
    [SerializeField] private bool Enabled;

    public double rdsProbability
    {
        get
        {
            return Probability;
        }
        set
        {
            Probability = value;
        }
    }

    public bool rdsUnique
    {
        get
        {
            return Unique;
        }
        set
        {
            Unique = value;
        }
    }

    public bool rdsAlways
    {
        get
        {
            return Always;
        }
        set
        {
            Always = value;
        }
    }

    public bool rdsEnabled
    {
        get
        {
            return enabled;
        }
        set
        {
            enabled = value;
        }
    }

    public RDSTable rdsTable { get; set; }
}
