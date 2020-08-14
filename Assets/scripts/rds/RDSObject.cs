using System.Collections;
using System.Collections.Generic;
using rds;

public class RDSObject : IRDSOBJECT
{
    public RDSObject() : this(0)
    {

    }

    public RDSObject(double probability) : this(probability, false, false, true)
    {

    }

    public RDSObject(double probability, bool unique, bool always, bool enabled)
    {
        rdsProbability = probability;
        rdsUnique = unique;
        rdsAlways = always;
        rdsEnabled = enabled;
        rdsTable = null;
    }

    //fields:

    public double rdsProbability { get; set; }

    public bool rdsUnique { get; set; }

    public bool rdsAlways { get; set; }

    public bool rdsEnabled { get; set; }

    public RDSTable rdsTable { get; set; }


}
