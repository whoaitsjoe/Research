using System;
using System.Collections.Generic;
using System.Linq;


//Used to store a point in d dimension and associated state
namespace Research.Core
{
	public class Point2 
	{
		//Variables
		public int[] Coordinates
		{
			get;
			set;
		}

        public HashSet<string> Decomposition1
        {
            get;
            set;
        }
        public HashSet<string> Decomposition2
        {
            get;
            set;
        }

		//Constructors
		public Point2()
		{
			Coordinates = new int[0];
            Decomposition1 = new HashSet<string>();
            Decomposition2 = new HashSet<string>();
		}

		public Point2(string coordinates)
		{
			Coordinates = coordinates.Split(',').Select(Int32.Parse).ToArray();
			Decomposition1 = new HashSet<string>();
			Decomposition2 = new HashSet<string>();
		}

		public Point2(int[] input)
		{
			Coordinates = input;
			Decomposition1 = new HashSet<string>();
			Decomposition2 = new HashSet<string>();
		}

        public Point2(int dimension)
        {
            Coordinates = new int[dimension];

            for (int i = 0; i < dimension; i++)
                Coordinates[i] = 0;


            Decomposition1 = new HashSet<string>();
            Decomposition2 = new HashSet<string>();
        }

		//Methods
		public int GetDimension()
		{
			return Coordinates.Length;
		}

		public override string ToString()
		{
            return String.Join(",", Coordinates);
		}

		public string ToLexicographicalString()
		{
            int[] tempArray = new int[Coordinates.Length];

            for (int i = 0; i < tempArray.Length; i++)
                tempArray[i] = Coordinates[i];

            Array.Sort(tempArray);

            return String.Join(",",tempArray);
		}

		public bool Equals(Point2 otherPoint)
		{
            for (int i = 0; i < Coordinates.Length; i++)
            {
                if (Coordinates[i] != otherPoint.Coordinates[i])
                    return false;
            }

            return true;
		}

		public Point2 Clone()
		{
            int[] intArray = new int[Coordinates.Length];

            for (int i = 0; i < Coordinates.Length; i++)
                intArray[i] = Coordinates[i];

            return new Point2(intArray);
		}

		//returns true if this point shares at least one outer facet with otherPoint2
		//(e.g. same dimension has the same value of either 0 or k)
		public bool ShareFacet(Point2 otherPoint)
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
        public void IncreaseByDimensionality(int dimension, bool kEqualsZero)
		{
            int[] tempArray = new int[Coordinates.Length + 1];

            for (int i = 0; i < dimension; i++)
                tempArray[i] = Coordinates[i];

            tempArray[dimension] = (kEqualsZero) ? 0 : Globals.k;

            for (int i = dimension + 1; i < Coordinates.Length; i++)
                tempArray[i + 1] = Coordinates[i];

			Coordinates = tempArray;
		}

		//increase point by 1 dimension where new dimension have a value of newDimensionValue
        public void IncreaseDimensionality(int dimension, int newDimensionValue)
		{
			int[] tempArray = new int[Coordinates.Length + 1];

			for (int i = 0; i < dimension; i++)
				tempArray[i] = Coordinates[i];

            tempArray[dimension] = newDimensionValue;

			for (int i = dimension; i < Coordinates.Length; i++)
				tempArray[i + 1] = Coordinates[i];

			Coordinates = tempArray;
		}

		//returns projection of point onto plane in 1 lower dimension where the 
		//dimension being reduced is defined by input parameter
		public Point2 DecreaseDimensionality(int dimension)
		{
            int[] tempArray = new int[Coordinates.Length - 1];

            for (int i = 0; i < Coordinates.Length - 1; i++)
            {
                if (i < dimension)
                    tempArray[i] = Coordinates[i];
                else
                    tempArray[i] = Coordinates[i + 1];
            }

            return new Point2(tempArray);
		}

		public static string ConvertIntArrayToString(int[] array)
		{
			return string.Join(",", array);
		}

        /*
		//increase coordinates by 1 dimension where kEqualsZero specifies
		//whether a 0 or k is added.
		public static string IncreaseDimensionality(string coordinates, int dimension, bool kEqualsZero)
		{
			string temp;

			temp = coordinates.Substring(0, dimension);
			temp += (kEqualsZero) ? ("0") : Globals.k.ToString();
			temp += coordinates.Substring(dimension);

			return temp;jhgjh
		}*/

		//increases the coordinate in the given dimension by 1. 0 based indexing
		public void IncrementCoordinateByDimension(int dimension)
		{
            Coordinates[dimension]++;
		}

        //**FOR UV GENERATION**
        //increments coordinate represented in string format. 
        //Second parameter symmetryCheck accounts for symmetry, upper bound is limited to k/2 instead of k.
		public static string incrementPointForUVGeneration(string coordinateArray, bool symmetryCheck)
		{
			char[] temp = coordinateArray.ToCharArray();

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

		//returns sum of coordinates of both points as a new point
		public static Point2 AddPoints(Point2 point1, Point2 point2)
		{
			Point2 newPoint;

            if (point1.Coordinates.Length != point2.Coordinates.Length)
				return null;

            int[] intArray = new int[point1.Coordinates.Length];

            for (int i = 0; i < intArray.Length; i++)
                intArray[i] = point1.Coordinates[i] + point2.Coordinates[i];

            newPoint = new Point2(intArray); 

			return newPoint;
		}

		//returns Point1 - Point2, if one of the coordinates is negative, returns null
		public static Point2 SubtractPoints(Point2 point1, Point2 point2)
		{
			Point2 newPoint;

            if (point1.Coordinates.Length != point2.Coordinates.Length)
				return null;

            int[] intArray = new int[point1.Coordinates.Length];
			for (int i = 0; i < intArray.Length; i++)
			{
                intArray[i] = point1.Coordinates[i] - point2.Coordinates[i];
				if (intArray[i] < 0)
					return null;
			}

			newPoint = new Point2(intArray);

			return newPoint;
		}

        //returns true if current point is strictly less than otherPoint (lexicographically)
        public bool CompareLexicographically(Point2 otherPoint)
        {
            for (int i = 0; i < Math.Min(Coordinates.Length, otherPoint.Coordinates.Length); i++)
            {
                if (Coordinates[i] < otherPoint.Coordinates[i])
                    return true;
                else if (Coordinates[i] > otherPoint.Coordinates[i])
                    return false;
            }

            return false;
        }

		//sorts coordinates of current point lexicographically
		public void SortLexicographically()
		{
            Array.Sort(Coordinates);
		}

		//return sum of the coordinates of the current point
		public int SumOfCoordinates()
		{
			int sum = 0;

            foreach (int i in Coordinates)
				sum += i;

			return sum;
		}

        //return index of first non-zero coordinate. 0-based indexing. Returns -1 if 0 vector.
        public int FirstNonZeroIndex()
        {
            for (int i = 0; i < Coordinates.Length; i++)
            {
                if (Coordinates[i] > 0)
                    return i;
            }

            return -1;
        }

        //returns Point representation of unit vector in dimension dimension and with 0 as coordinates until index startingIndex
        //after which all ccoordinates are 1.
        public static Point2 GenerateOnesVector(int dimension, int startingIndex)
        {
            var intArray = new int[dimension];

            for (int i = 0; i < dimension; i++)
                intArray[i] = (i > startingIndex) ? 1 : 0;

            return new Point2(intArray);
        }

        //generates a unit (or binary if binaryIndex > 0) vector of length equals to dimension
        public static Point2 GenerateUnitVector(int dimension, int unitIndex, int binaryIndex = -1)
        {
            var intArray = new int[dimension];

            for (int i = 0; i < dimension; i++)
                intArray[i] = (i == unitIndex || i == binaryIndex) ? 1 : 0;

            return new Point2(intArray);
        }

        //Takes a point and uses a greeedy heuristic to determinee if it has multiple decompositions
        public static List<Point2> DecomposePoint(Point2 currentPoint, List<Point2> usedVectors)
        {

            return null;

        }

        //takes a unitVector and splits it greedily into as few vectors as possible, none of which appear in usedVectors.
        //returns an empty list if a split is not possible.
        public static List<Point2> SplitVectors(Point2 unitVector, List<Point2> usedVectors)
        {
            List<Point2> result = new List<Point2>();

            var found = false;
            var unitVectorFirstNonZero = unitVector.FirstNonZeroIndex();
            var unitVectorDimension = unitVector.GetDimension();

            while (!found)
            {
                //First split vector into two, if a vector has been used before, see if it's splitable, then recursively split.
				var intArray = new int[unitVectorDimension];
				var intArray2 = new int[unitVectorDimension];

                for (int i = 0; i < unitVectorDimension; i++)
                {
                    intArray[i] = (i == unitVectorFirstNonZero) ? 1 : 0;
                    intArray[i] = (i <= unitVectorFirstNonZero) ? 0 : 1;
                }

                var split1 = new Point2(intArray);
                var split2 = new Point2(intArray2);

                if(usedVectors.Contains(split1) || usedVectors.Contains(split2))
                {
                    
                }
            }

            return result;
        }

        //Recursive helper method to determine if unitVector can be split. If not, returns empty list.
        private static List<Point2> SplitVectorsHelper(Point2 unitVector, List<Point2> usedVectors)
        {
            List<Point2> result = new List<Point2>();

			var found = false;
			var unitVectorFirstNonZero = unitVector.FirstNonZeroIndex();
			var unitVectorDimension = unitVector.GetDimension();
            var splitIndex = unitVectorFirstNonZero;

            while (!found)
            {
                if (unitVectorFirstNonZero == -1 || unitVectorFirstNonZero >= unitVectorDimension)
                    return result;
                
				//First split vector into two, if a vector has been used before, see if it's splitable, then recursively split.
				var intArray = new int[unitVectorDimension];
				var intArray2 = new int[unitVectorDimension];

				for (int i = 0; i < unitVectorDimension; i++)
				{
					intArray[i] = (i >= unitVectorFirstNonZero && i <= splitIndex) ? 1 : 0;
                    intArray2[i] = (i < splitIndex) ? 0 : 1;
				}

				var split1 = new Point2(intArray);
				var split2 = new Point2(intArray2);

                if (usedVectors.Contains(split1))
                {
                    unitVectorFirstNonZero++;
                    continue;
                }
                else if (usedVectors.Contains(split2))
                {
                    //recursively breakdown the second half of the vector into a list of unused vectors.

                }
                else
				{
					found = true;

					result.Add(split1);
					result.Add(split2);
                }
            }

            foreach (Point2 vector in result)
                VectorCheck(vector);

            return result;
        }

        //Take a given point, and a list of points as the usedVectors and generates one (if unique)
        //or two (if more than one) decompositions, or zero (if error occurs or no decomposition is found).
        //Input point is assumed to have sorted coordinates.
        //Returns 1 if unique decompsition, 0 if none is found, or 2 if more than 1 is found.
        public static int SplitVector(Point2 inputPoint, List<Point2> usedVectors)
        {
            //usedVectors may not be required?? or it's empty and this function populates it.

            //variables
            var firstNonZero = 0;
            var unitVectorIndex = Globals.d - 1;
            var binaryVectorIndex = 0;
            var onesVector = new Point2();
			var found = false;
			var vectorDimension = inputPoint.GetDimension();
            var vector1 = new Point2();
            var vector2 = new Point2();
            var localInputPoint = inputPoint.Clone();
            var decomposition1 = new List<Point2>();

            while (!found)
			{
                Console.WriteLine(localInputPoint);
				firstNonZero = localInputPoint.FirstNonZeroIndex();
				onesVector = Point2.GenerateOnesVector(vectorDimension, firstNonZero);

                var intArray1 = new int[vectorDimension];
                var intArray2 = new int[vectorDimension];

                //generate v1 and v2
                for (int i = 0; i < vectorDimension; i++)
                {
                    intArray1[i] = (i == unitVectorIndex || (binaryVectorIndex > unitVectorIndex && i == binaryVectorIndex)) ? 1 : 0;
                    intArray2[i] = (i < firstNonZero || i == unitVectorIndex || (binaryVectorIndex > unitVectorIndex && i == binaryVectorIndex)) ? 0 : 1;
                }
                vector1 = new Point2(intArray1);
                vector2 = new Point2(intArray2);

                //one of the vectors is used
                if(usedVectors.Contains(vector1) || usedVectors.Contains(vector2))
                {
                    //unitVectorIndex can still be decremented
                    if (unitVectorIndex > firstNonZero)
                        unitVectorIndex--;
                    //reset unitVectorIndex and move binaryVectorIndex
                    else
                    {
                        if(binaryVectorIndex > unitVectorIndex)
                        {
                            if (binaryVectorIndex > firstNonZero + 1)
                            {
                                binaryVectorIndex--;
                                unitVectorIndex = binaryVectorIndex - 1;
                            }
                            else
                                return 0;
                        }
                        else
                        {
                            binaryVectorIndex = Globals.d - 1;
                            unitVectorIndex = binaryVectorIndex - 1;
                        }
                    }
                }
                else
                {
                    var tempLocalInputPoint = SubtractPoints(localInputPoint, vector1);

                    //subtracting the vector results in a negative coordinate, don't consider current vector
                    if (VectorCheck(tempLocalInputPoint))
					{                    
                        //unitVectorIndex can still be decremented
						if (unitVectorIndex > firstNonZero)
							unitVectorIndex--;
						//reset unitVectorIndex and move binaryVectorIndex
						else
						{
							if (binaryVectorIndex > unitVectorIndex)
							{
								if (binaryVectorIndex > firstNonZero + 1)
								{
									binaryVectorIndex--;
									unitVectorIndex = binaryVectorIndex - 1;
								}
								else
									return 0;
							}
							else
							{
								binaryVectorIndex = Globals.d - 1;
								unitVectorIndex = binaryVectorIndex - 1;
							}
						}
                        continue;
                    }
                    else
                        localInputPoint = tempLocalInputPoint;
                    
                    //Only 1 decomposition?
                    if(localInputPoint.SumOfCoordinates() == 0)
                    {
                        return SplitVectorFinalCheck(decomposition1, inputPoint);
                    }

					tempLocalInputPoint = SubtractPoints(localInputPoint, vector2);

                    //subtracting the vector results in a negative coordinate, don't consider current vector
                    if (VectorCheck(tempLocalInputPoint))
					{                    
                        //unitVectorIndex can still be decremented
						if (unitVectorIndex > firstNonZero)
							unitVectorIndex--;
						//reset unitVectorIndex and move binaryVectorIndex
						else
						{
							if (binaryVectorIndex > unitVectorIndex)
							{
								if (binaryVectorIndex > firstNonZero + 1)
								{
									binaryVectorIndex--;
									unitVectorIndex = binaryVectorIndex - 1;
								}
								else
									return 0;
							}
							else
							{
								binaryVectorIndex = Globals.d - 1;
								unitVectorIndex = binaryVectorIndex - 1;
							}
						}
                        continue;
                    }
					else
						localInputPoint = tempLocalInputPoint;

					//Only 1 decomposition?
					if (localInputPoint.SumOfCoordinates() == 0)
					{
						return SplitVectorFinalCheck(decomposition1, inputPoint);
					}
                }
            }
            return 0;
        }

        //gets called when SplitVector finds a list of vectors
        //checks that decomposition is unique by sorting vectors and greedily checking them 
        //returns 0 for no decomposition/negative coord, 1 for unique decomp, 2 for multi decomp
        private static int SplitVectorFinalCheck(List<Point2> vectors, Point2 inputPoint)
        {


            return 0;
        }

        //checks vectors to see if non-zero coordinates appeear
        //if they do, then need to account for cases where vectors do not have lexicographically
        //ordered coordinates
        private static bool VectorCheck(Point2 vector)
        {
            if (vector == null)
                return true;
            
            var invalid = false;
            for (int i = 0; i < vector.GetDimension(); i++)
            {
                if (vector.Coordinates[i] < 0)
                {
                    invalid = true;
                    break;
                }
            }

            return invalid;
        }

        //takes in list of vectors and checks to see if all unit and binary vectors are used (i.e. is it a unique decomposition)
        //may be redundant? see if logic can be included in SplitVector
        private static bool CheckSingleDecomposition(List<Point2> usedVectors)
        {

            return true;
        }

        //takes in list of usedVectors and finds another decomposition for it.
        private static List<Point2> FindSecondDecomposition(List<Point2> usedVectors)
        {
            return null;   
        }


        /// <summary>
        /// Increments point.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <param name="resetToZero">If set to <c>true</c> reset to zero.</param>
        /// returns null if incremented point becomes larger
        public static void IncrementPointLastCoordinate(int[] pointAsArray, bool resetToZero)
        {
            pointAsArray[pointAsArray.Length - 1]++;

            for (int i = pointAsArray.Length - 1; i > 0; i--)
            {
                if(pointAsArray[i] > Globals.k)
                {
                    pointAsArray[i - 1]++;
                    pointAsArray[i] = (resetToZero) ? 0 : 
                        ((pointAsArray[i - 1] > Globals.k) ? 0 : pointAsArray[i - 1]);
                }
                else
                    return;
            }

            //check first coordinate to see if it exceeded the limit
            if (pointAsArray[0] > Globals.k)
                pointAsArray = null;
        }






        //symmetry check is for u generation to only go up to k/2 instead of k for each coord
        //increments a point (in string format)
        //May need to be rewritten
        //outdated*
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
