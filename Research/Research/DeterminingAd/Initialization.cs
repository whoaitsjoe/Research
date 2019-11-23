using System;
using System.Collections.Generic;
using Research.Core;

namespace Research.DeterminingAd
{
    public class Initialization
    {
		public static List<Point2> InitializeVertices(int dimension)
		{
			List<Point2> result = new List<Point2>();

			if (Globals.d == 3)
			{
				List<Point2> A2 = new List<Point2>();
				A2.Add(new Point2("0,0"));
				A2.Add(new Point2("0,1"));

				foreach (Point2 p in A2)
				{
					result.Add(CoreOperations.Lift0(p));
					result.Add(CoreOperations.LiftSigmaD(p));
				}
			}
			else if (Globals.d == 4)
			{
				List<Point2> A3 = new List<Point2>();
				A3.Add(new Point2("0,0,0"));
				A3.Add(new Point2("0,0,1"));
				A3.Add(new Point2("0,1,2"));
				A3.Add(new Point2("0,2,2"));
				A3.Add(new Point2("1,1,3"));

				foreach (Point2 p in A3)
				{
					result.Add(CoreOperations.Lift0(p));
					result.Add(CoreOperations.LiftSigmaD(p));
				}
			}
			else if (Globals.d == 5)
			{
				List<Point2> A3 = new List<Point2>();
				//red vertices
				A3.Add(new Point2("0,0,0,0"));
				A3.Add(new Point2("0,0,0,1"));
				A3.Add(new Point2("0,0,1,2"));
				A3.Add(new Point2("0,0,2,2"));
				A3.Add(new Point2("0,1,1,3"));
				A3.Add(new Point2("0,4,4,4"));
				A3.Add(new Point2("0,3,4,4"));
				A3.Add(new Point2("0,2,3,4"));
				A3.Add(new Point2("0,2,2,4"));
				A3.Add(new Point2("0,1,3,3"));

				A3.Add(new Point2("1,3,5,5"));
				A3.Add(new Point2("1,2,4,5"));
				A3.Add(new Point2("1,2,2,5"));
				A3.Add(new Point2("1,1,1,4"));
				A3.Add(new Point2("1,1,4,4"));
				A3.Add(new Point2("2,2,4,6"));
				A3.Add(new Point2("2,2,3,6"));
				A3.Add(new Point2("3,3,3,7"));

				foreach (Point2 p in A3)
				{
					result.Add(CoreOperations.Lift0(p));
					result.Add(CoreOperations.LiftSigmaD(p));
				}
			}

			//Adding point 11...1d
			int[] intArray = new int[dimension];

			for (int i = 0; i < dimension - 1; i++)
				intArray[i] = 1;

			intArray[dimension - 1] = dimension;
			result.Add(new Point2(intArray));

			//Adding point 2^(d-2) - 1, ..., 2^(d-1) - 1
			for (int i = 0; i < dimension - 1; i++)
				intArray[i] = (int)Math.Pow(2, (dimension - 2)) - 1;

			intArray[dimension - 1] = dimension;
			result.Add(new Point2(intArray));

			//sort each point lexicographically
			foreach (Point2 p in result)
				p.SortLexicographically();

			return result;
		}

		public static List<Point2> InitializeRedVertices(int dimension)
		{
			List<Point2> result = new List<Point2>();
			List<Tuple<int, List<Point2>>> allVertices = CoreOperations.ReadRedVerticesFromFile();
			List<Point2> previousVertices = new List<Point2>();

			foreach (var thisTuple in allVertices)
			{
				if (thisTuple.Item1 == dimension - 1)
				{
					previousVertices = thisTuple.Item2;
					break;
				}
			}

			foreach (Point2 p in previousVertices)
			{
				result.Add(CoreOperations.Lift0(p));
				result.Add(CoreOperations.LiftSigmaD(p));
			}

			//sort each point lexicographically
			foreach (Point2 p in result)
				p.SortLexicographically();

			return result;
		}

		public static List<Point2> initializeEdges()
		{
			int[] numArray = new int[Globals.d];

			Point2 startEdge = new Point2(numArray);

			List<Point2> result = initializeEdgesHelper(startEdge, 0);

			if (result[0].SumOfCoordinates() == 0)
				result.RemoveAt(0);

			return result;
		}

		private static List<Point2> initializeEdgesHelper(Point2 startPoint, int index)
		{
			if (index >= Globals.d)
			{
				List<Point2> returnValue = new List<Point2>();
				returnValue.Add(startPoint);
				return returnValue;
			}
			List<Point2> result = new List<Point2>();
			Point2 modifiedPoint = startPoint.Clone();
			modifiedPoint.IncrementCoordinateByDimension(index);

			result.AddRange(initializeEdgesHelper(startPoint, index + 1));
			result.AddRange(initializeEdgesHelper(modifiedPoint, index + 1));

			return result;
		}
    }
}
