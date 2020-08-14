using System.Collections;
using System.Collections.Generic;

namespace rds
{
    public interface IRDSOBJECT
    {
        //fields:

        double rdsProbability { get; set; }

        bool rdsUnique { get; set; }

        bool rdsAlways { get; set; }

        bool rdsEnabled { get; set; }

        RDSTable rdsTable { get; set; }
    }

    public interface IRDSTABLE : IRDSOBJECT
    {
        int rdsCount { get; set; }

        IEnumerable<IRDSOBJECT> rdsContent { get; }

        IEnumerable<IRDSOBJECT> rdsResult { get; }
    }

    public interface IRDSVALUE<T> : IRDSOBJECT
    {
        T rdsValue { get; set; }
    }
}
