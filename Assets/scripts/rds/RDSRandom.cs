using System.Collections;
using System.Collections.Generic;
using System;
using rds;

namespace rds
{
    public class RDSRandom 
    {
        private static Random rnd = null;
    
        static RDSRandom()
        {
            setRandomizer(null); 
        }
    
        public static void setRandomizer(Random randomizer)
        {
            if (randomizer == null)
            {
                rnd = new Random();
            }else
            {
                rnd = randomizer;
            }
        }
    
        public static double getDouble(double max)
        {
            return rnd.NextDouble() * max;
        }
    
        public static double getDouble(double min, double max)
        {
            return min + rnd.NextDouble() * (max - min);
        }
    
        public static int getInteger(int max)
        {
            return rnd.Next(max);
        }
    
        public static int getInteger(int min, int max)
        {
            return rnd.Next(min, max);
        }
    
    }
}
