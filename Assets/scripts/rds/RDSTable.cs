using System.Linq;
using System.Collections.Generic;
using rds;

namespace rds
{
    public class RDSTable : IRDSTABLE
    {

        public RDSTable() : this(null, 1, false, false, true, 1)
        {

        }
    
        public RDSTable(IEnumerable<IRDSOBJECT> content, double probability, bool unique, bool Always, bool Enabled, int count)
        {
            if (content != null)
            {
                currentContent = content.ToList();
            }else
            {
                clearContent();
            }
    
            rdsProbability = probability;
            rdsUnique = unique; 
            rdsAlways = Always;
            rdsEnabled = Enabled;
            rdsCount = count;
        }
    
    
        //fields :
    
        public double rdsProbability { get; set; }
    
        public bool rdsUnique { get; set; }
    
        public bool rdsAlways { get; set; }
    
        public bool rdsEnabled { get; set; }
    
        public int rdsCount { get; set; }
    
        public RDSTable rdsTable { get; set; }
    
        protected List<IRDSOBJECT> currentContent = null;
    
        //methods :
    
        public virtual void clearContent()
        {
            currentContent = new List<IRDSOBJECT>() ;
        }
    
        public virtual void addEntry(IRDSOBJECT entry)
        {
            currentContent.Add(entry);
            //entry.rdsTable = this;
        }
    
        public virtual void removeEntry(IRDSOBJECT entry)
        {
            currentContent.Remove(entry);
            //entry.rdsTable = null;
        }
    
        public virtual void removeEntry(int index)
        {
            IRDSOBJECT entry = currentContent[index];
            //entry.rdsTable = null;
            currentContent.RemoveAt(index);
        }
    
        public IEnumerable<IRDSOBJECT> rdsContent
        {
            get { return currentContent; }
        }
    
        //result:
    
        protected List<IRDSOBJECT> uniqueDrops = new List<IRDSOBJECT>();
    
        protected void addToResult(List<IRDSOBJECT> currentResult, IRDSOBJECT currentObject)
        {
            if (!currentObject.rdsUnique || !uniqueDrops.Contains(currentObject))
            {
                if (currentObject.rdsUnique)
                {
                    uniqueDrops.Add(currentObject);
                }
    
                if(!(currentObject is RDSNull))
                {
                    if (currentObject is IRDSTABLE)
                    {
                        currentResult.AddRange(((IRDSTABLE)currentObject).rdsResult);
                    }else
                    {
                        currentResult.Add(currentObject);
                    }
                }
            }
        }
    
        public virtual IEnumerable<IRDSOBJECT> rdsResult
        {
            get
            {
                List<IRDSOBJECT> returnList = new List<IRDSOBJECT>();
                uniqueDrops = new List<IRDSOBJECT>();

                foreach (IRDSOBJECT currentObject in currentContent.Where(e => e.rdsAlways && e.rdsEnabled))
                {
                    addToResult(returnList, currentObject);
                }

                int alwaysOn = currentContent.Count(e => e.rdsAlways && e.rdsEnabled);
                int lootRemain = rdsCount - alwaysOn;

                if (lootRemain > 0)
                {
                    for (int i=0; i<lootRemain; i++)
                    {
                        IEnumerable<IRDSOBJECT> dropable = currentContent.Where(e => e.rdsEnabled && !e.rdsAlways);

                        double hitValue = RDSRandom.getDouble(dropable.Sum(e => e.rdsProbability));

                        double runningValue = 0;
                        foreach(IRDSOBJECT thisObject in dropable)
                        {
                            runningValue += thisObject.rdsProbability;
                            if (hitValue < runningValue)
                            {
                                addToResult(returnList, thisObject);
                                break;
                            }
                        }
                    }
                }

                return returnList;
    
            }
        }
    
    }
}
