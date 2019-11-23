using System;
using System.Collections.Generic;

//Used to store a point in d dimension and associated state
namespace Research.Core
{
	public class Point : IEquatable<Point>
	{
        //Variables
		public string Coordinates
		{
			get;
			set;
		}

		//Constructors
		public Point()
		{
			Coordinates = "";
		}

		public Point(string coordinates)
		{
			Coordinates = coordinates;
		}

		public Point(int[] input)
		{
			Coordinates = string.Join("", input);
		}

        //Methods
		public int GetDimension()
		{
			return Coordinates.Length;
		}

		public override string ToString()
		{
			return Coordinates;
		}

        public int[] ToIntArray()
        {
            return new List<char>(Coordinates.ToCharArray()).ConvertAll(c => Convert.ToInt32(c.ToString())).ToArray();
        }

		public string ToLexicographicalString()
		{
			char[] charArray = Coordinates.ToCharArray();

			Array.Sort(charArray);

			return new string(charArray);
		}

        //todo -- change to double/float to account for larger d values.
		public int ToInt()
		{
			return Convert.ToInt32(Coordinates);
		}

		public bool Equals(Point otherPoint)
		{
			return Coordinates.Equals(otherPoint.Coordinates, StringComparison.Ordinal);
		}

		public bool Equals(string comparisonString)
		{
			return Coordinates.Equals(comparisonString, StringComparison.Ordinal);
		}

		public Point Clone()
		{
			return new Point(Coordinates);
		}

		//returns true if this point shares at least one outer facet with otherPoint 
        //(e.g. same dimension has the same value of either 0 or k)
		public bool ShareFacet(Point otherPoint)
		{
			if (otherPoint.Coordinates.Length != Coordinates.Length)
				return false;

			for (int i = 0; i < Coordinates.Length; i++)
			{
                if ((otherPoint.Coordinates[i] == 0 && Coordinates[i] == 0) || 
                    (otherPoint.Coordinates[i] == Globals.k && Coordinates[i] == Globals.k))
					return true;
			}

			return false;
		}

		//increase current point by 1 dimension, and kEqualsZero 
        //specifies whether a 0 or k value is added.
		public void IncreaseDimensionality(int dimension, bool kEqualsZero)
		{
			string temp;

			temp = Coordinates.Substring(0, dimension);
            temp += (kEqualsZero) ? ("0") : Globals.k.ToString();
			temp += Coordinates.Substring(dimension);

			Coordinates = temp;
		}
        //increase current point by 1 dimension and nextFacet specifies the
        //dimension where nextFacet=0
		public void IncreaseDimensionality(int nextFacet)
		{
			string temp;

			temp = Coordinates.Substring(0, nextFacet / 2);
			temp += (nextFacet % 2 == 0) ? ("0") : Globals.k.ToString();
			temp += Coordinates.Substring(nextFacet / 2);

			Coordinates = temp;
		}

        //increase point by 1 dimension where new dimension have a value of newDimensionValue
        public void IncreaseDimensionality(int dimension, int newDimensionValue)
        {
            string temp;

            temp = Coordinates.Substring(0, dimension);
            temp += newDimensionValue.ToString();
            temp += Coordinates.Substring(dimension);

            Coordinates = temp;
        }


		//returns projection of point onto plane in 1 lower dimension where the 
        //dimension being reduced is defined by input parameter
		public Point DecreaseDimensionality(int dimension)
		{
			string temp;

			temp = Coordinates.Substring(0, dimension);
			temp += Coordinates.Substring(dimension + 1);

			return new Point(temp);
		}

		public static string ConvertIntArrayToString(int[] array)
		{
			return string.Join("", array);
		}

		//increase coordinates by 1 dimension where kEqualsZero specifies
        //whether a 0 or k is added.
		public static string IncreaseDimensionality(string coordinates, int dimension, bool kEqualsZero)
		{
			string temp;

			temp = coordinates.Substring(0, dimension);
			temp += (kEqualsZero) ? ("0") : Globals.k.ToString();
			temp += coordinates.Substring(dimension);

			return temp;
		}

        //increases the coordinate in the given dimension by 1. 0 based indexing
        public static Point IncrementCoordinateByDimension(Point point, int dimension)
        {
            int[] integerArray = point.ToIntArray();
			integerArray[dimension]++;
            return new Point(string.Join("", integerArray));
        }

        //returns sum of coordinates of both points as a new point
        public static Point AddPoints(Point point1, Point point2)
        {
            Point newPoint;

            int[] intArray1 = point1.ToIntArray();
            int[] intArray2 = point2.ToIntArray();

            if (intArray1.Length != intArray2.Length)
                return null;

            for (int i = 0; i < intArray1.Length; i++)
                intArray1[i] += intArray2[i];

            newPoint = new Point(intArray1);

            return newPoint;
        }

		//returns Point1 - Point2, if one of the coordinates is negative, returns null
		public static Point SubtractPoints(Point point1, Point point2)
		{
			Point newPoint;

			int[] intArray1 = point1.ToIntArray();
			int[] intArray2 = point2.ToIntArray();

			if (intArray1.Length != intArray2.Length)
				return null;

            for (int i = 0; i < intArray1.Length; i++)
            {
                intArray1[i] -= intArray2[i];
                if (intArray1[i] < 0)
                    return null;
            }

			newPoint = new Point(intArray1);

			return newPoint;
		}

        //sorts coordinates of current point lexicographically
        public void SortLexicographically()
        {
			char[] charArray = Coordinates.ToCharArray();

			Array.Sort(charArray);

            Coordinates = String.Join("", charArray);
        }

        //return sum of the coordinates of the current point
        public int SumOfCoordinates()
        {
            int sum = 0;

            int[] intArray = ToIntArray();

            foreach (int i in intArray)
                sum += i;

            return sum;
        }





		//not sure what symmetrycheck does
		//increments a point (in string format) and 
		public static string IncrementPoint(string inputPoint, bool symmetryCheck)
		{
			char[] temp = inputPoint.ToCharArray();

			for (int i = temp.Length - 1; i >= 0; i--)
			{
				if (Convert.ToInt16(temp[i].ToString()) < ((symmetryCheck) ? (Globals.k / 2) : (Globals.k)))
				{
					temp[i] = (char)(Convert.ToInt16(temp[i]) + 1);
					break;
				}
				else
					temp[i] = '0';
			}

			return new string(temp);
		}
	}
}
