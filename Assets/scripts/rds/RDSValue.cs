using System.Collections;
using System.Collections.Generic;
using rds;

public class RDSValue<T> : IRDSVALUE<T>
{
    public RDSValue(T value, double probability) : this(value, probability, false, false, true)
    {

    }

    public RDSValue(T value, double probability, bool unique, bool always, bool enabled)
    {
        rdsValue = value;
        rdsProbability = probability;
        rdsUnique = unique;
        rdsAlways = always;
        rdsEnabled = enabled;
        rdsTable = null;
    }

    private T mvalue;

    //fields:

    public double rdsProbability { get; set; }

    public bool rdsUnique { get; set; }

    public bool rdsAlways { get; set; }

    public bool rdsEnabled { get; set; }

    public T rdsValue {
        get {
            return mvalue;    
        }
        set{
            mvalue = value;
        }
    }

    public RDSTable rdsTable { get; set; }
}
