using System.Collections;
using System.Collections.Generic;
using rds;

public class RDSNull : RDSValue<object>
{
    public RDSNull(double probability) : base(null, probability, false, false, true)
    {

    }
}
