using System;
using System.Collections.Generic;
using Research.Core;

namespace Research.DeterminingAd
{
    public class PossibleNeighbours
    {
        //Given a point, returns all (valid) neighbours
        public static List<Point> FindAllNeighbours(Point startingPoint, List<Point> generators)
        {
            List<Point> neighbours = new List<Point>();
            var dimension = startingPoint.GetDimension();
            var validFlag = true;

            foreach(Point generator in generators)
            {
                validFlag = true;

                Point newPoint = Point.AddPoints(startingPoint, generator);

                if (newPoint == null)
                {
                    Console.WriteLine("ERROR: startingPoint and generator are not the same dimension.");
                    continue;
                }

                if (!Heuristics.TestDoubleDescription(newPoint, generators))
                {
                    validFlag = false;
                    Console.WriteLine("Point: " + newPoint.ToString() + " eliminated because of DD.");
                }

                if (validFlag && !Heuristics.TestEachCoordinate(newPoint))
                {
                    validFlag = false;
                    Console.WriteLine("Point: " + newPoint.ToString() + " eliminated because of Coordinate Test.");
                }

                if (validFlag)
                    neighbours.Add(newPoint);
            }
            //add each vector to starting point
            //use heuristics to determine if point is valid
            //if valid, add to list (neighbours)
            //if not valid, log reason why it's not valid.


            return neighbours;
        }
    
        //Creates all generator vectors for a given dimension
        public static List<Point> CreateGeneratorVectors(int dimension)
        {
            List<Point> GeneratorVectors = new List<Point>();

            int[] NullCoordinates = new int[dimension];
            for (int i = 0; i < dimension; i++)
                NullCoordinates[i] = 0;
            Point emptyPoint = new Point(NullCoordinates);

            GeneratorVectors.AddRange(CreateGeneratorVectorsHelper(emptyPoint, 0));

            return GeneratorVectors;
        }

        //Recursive helper for CreateGeneratorVectors
        private static List<Point> CreateGeneratorVectorsHelper(Point originalPoint, int num)
        {
            List<Point> result = new List<Point>();
            Point modifiedPoint = Point.IncrementCoordinateByDimension(originalPoint, num);
            result.Add(modifiedPoint);

            if(num < originalPoint.Coordinates.Length - 1)
            {
                result.AddRange(CreateGeneratorVectorsHelper(originalPoint, num + 1));
                result.AddRange(CreateGeneratorVectorsHelper(modifiedPoint, num + 1));
            }

            return result;
        }
    }
}
