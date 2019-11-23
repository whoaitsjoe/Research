using System;
using System.Collections.Generic;
using System.IO;
using Research.Core;

namespace Research.DeterminingAd
{
    public class CoreOperations
    {
		public static Point2 Lift0(Point2 startPoint)
		{
            Point2 temp = startPoint.Clone();
            temp.IncreaseDimensionality(0, 0);
            return temp;
		}

		public static Point2 LiftSigmaD(Point2 startPoint)
		{
            int[] intArray = new int[startPoint.GetDimension()];

            int sigma_dMinus1 = (int)Math.Pow(2, startPoint.GetDimension() - 1);

			for (int i = 0; i < intArray.Length; i++)
                intArray[i] = sigma_dMinus1 - startPoint.Coordinates[i];

            Point2 temp = new Point2(intArray);
            temp.IncreaseDimensionality(0,0);
            return temp;
		}


		//************************************************************************
		//*************************     FILE IO     ******************************
		//************************************************************************

        //reads red vertices from a file (default infile name of RedVertices
        //returns List of Tuples. Each tuple contains an integer denoting the dimension, 
        //and a list of points corresponding to red vertices for that dimension.
		public static List<Tuple<int, List<Point2>>> ReadRedVerticesFromFile(string fileName = "RedVertices")
        {
            List<Tuple<int, List<Point2>>> result = new List<Tuple<int, List<Point2>>>();
            var path = Path.Combine(Environment.CurrentDirectory, @"Data/", fileName);

            string[] lines = File.ReadAllLines(path);

            var startOfFile = false;
            var beginRead = false;
            var readDimension = false;
            var currentDimension = 0;
            List<Point2> currentListOfPoints = new List<Point2>();

            foreach(string line in lines)
            {
                if (line.Equals("BEGIN"))
                {
                    startOfFile = true;
                    continue;
                }

                if (!startOfFile)
                    continue;

                if (line.Equals("START"))
                {
                    beginRead = true;
                    readDimension = true;
                    currentListOfPoints = new List<Point2>();
                    continue;
                }

                if(line.Equals("STOP"))
                {
                    beginRead = false;
                    result.Add(new Tuple<int, List<Point2>>(currentDimension, currentListOfPoints));
                    continue;
                }

                if (line.Equals("END"))
                    break;

                if (readDimension)
                {
                    currentDimension = Convert.ToInt32(line);
                    readDimension = false;
                    continue;
                }

                if (beginRead)
                {
                    currentListOfPoints.Add(new Point2(line));
                    continue;
				}
			}

            return result;
        }







        //Original Methods written for Point class

        public static Point Lift0(Point startPoint)
        {
			string temp = "0";

            temp += startPoint.Coordinates.ToString();

            return new Point(temp);
        }

        public static Point LiftSigmaD(Point startPoint)
		{
			string temp = "0";

			int[] intArray = startPoint.ToIntArray();

            int sigma_dMinus1 = (int)Math.Pow(2, intArray.Length - 1);

            for (int i = 0; i < intArray.Length; i++)
                intArray[i] = sigma_dMinus1 - intArray[i];

            string newCoordinates = temp + string.Join("", intArray);

            return new Point(newCoordinates);
        }
    }
}
