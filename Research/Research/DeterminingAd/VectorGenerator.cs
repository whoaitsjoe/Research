using System;
using System.Collections.Generic;
using Research.Core;

namespace Research.DeterminingAd
{
    public class VectorGenerator
    {
        //recursive method to generate ALL vectors including symmetrical ones.
        public static List<Point> GenerateAllVectors(int dimension)
        {
            if(dimension == 1)
			{
				List<Point> allVectors = new List<Point>();

                for (int i = 0; i < Globals.d; i++)
                    allVectors.Add(new Point(i.ToString()));

                return allVectors;
            }
            else
            {
                List<Point> allVectorsInLowerDimension = GenerateAllVectors(dimension - 1);
                List<Point> allVectors = new List<Point>();

                foreach(Point p in allVectorsInLowerDimension)
                {
                    for (int i = 0; i < Globals.d; i++)
                    {
                        Point newPoint = p.Clone();
                        newPoint.IncreaseDimensionality(dimension - 1, i);
                        allVectors.Add(newPoint);
                    }
                }

                return allVectors;
            }
        }

        //Removes duplicate vectors based symmetry
        public static List<Point> RemoveDuplicateVectors(List<Point> allVectors)
        {
            Dictionary<string, int> uniqueVectorDictionary = new Dictionary<string, int>();

            foreach(Point thisVector in allVectors)
            {
                var sortedVector = thisVector.ToLexicographicalString();

                //check 1: unique when sorted in lexicographical order
                if (!uniqueVectorDictionary.ContainsKey(sortedVector))
                {
                    int gcd = Convert.ToInt32(sortedVector.Substring(sortedVector.Length - 1));

                    //check 2: gcd of all coords = 1
                    for (int i = 0; i < Globals.d - 1; i++)
					{
                        if (Convert.ToInt32(sortedVector.Substring(i, 1)) == 0)
                            continue;

                        gcd = MathOperations.GCD(Convert.ToInt32(sortedVector.Substring(i, 1)), gcd);

                        if (gcd == 1)
                            break;
					}

                    if(gcd == 1)
                        uniqueVectorDictionary.Add(sortedVector, 0);
                }
            }

            var uniqueVectors = new List<Point>();

            foreach (string key in uniqueVectorDictionary.Keys)
                uniqueVectors.Add(new Point(key));

            return uniqueVectors;
        }

        //todo-read from file if file exists, otherwise start from d = 1

    }
}
