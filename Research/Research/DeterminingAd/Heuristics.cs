using System;
using System.Collections.Generic;
using Research.Core;

namespace Research.DeterminingAd
{
    public class Heuristics
    {
        //returns true if inputPoint cannot be described by more than 1 combination of generators..
        public static bool TestDoubleDescription(Point inputPoint, List<Point> generators)
        {

            return true;
        }

        //returns false if at least 1 coordinate is 0 or 2^d-1 and at least 1 other coordinate is > 2^(d-2)
        public static bool TestEachCoordinate(Point inputPoint)
        {
            int[] intRepresentation = inputPoint.ToIntArray();
            var valid = false;

            for (int i = 0; i < intRepresentation.Length; i++)
            {
                if(intRepresentation[i] == 0 || intRepresentation[i] == ((int)Math.Pow(2, intRepresentation.Length) - 1))
                {
                    valid = true;
                    break;
                }    
            }

            if(valid)
            {
                
            }

            return true;
        }
    }
}
