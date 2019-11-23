using System;
using System.Collections.Generic;
using Research.Core;

namespace Research.LatticePolytopes
{
	public class GenerateUV
	{
        /// <summary>
        /// Generates all valid u points.
        /// </summary>
        /// <returns>List of valid u points </returns>
        public static List<Point2> GenerateU()
		{
			var result = new List<Point2>();
			var vertexSet = FileIO.readVertexList();
            var terminatorArray = new int[Globals.d];
            var currentPointArray = new int[Globals.d];

            //create strings of length Globals.d for first point and last point to check
			for (int i = 0; i < Globals.d; i++)
			{
				currentPointArray[i] = 0;
				terminatorArray[i] = Globals.k / 2;
			}

            var currentPoint = new Point2(currentPointArray);
            var terminator = new Point2(terminatorArray);

			while (!currentPointArray.Equals(terminatorArray))
			{
                if (vertexSet.Count == 0 || containsPoint(vertexSet, new Point2(currentPointArray)))
					result.Add(new Point2(currentPointArray));

				Point2.IncrementPointLastCoordinate(currentPointArray, false);
			}

			return result;
		}

        public static List<Point2> GenerateV(Point2 u)
        {
            var result = new List<Point2>();
            var candidateV = new Point2();
            var vCoordinates = new int[u.GetDimension()];
            var g0 = new int[Globals.d];
            var terminateFlag = false;

            //set first v candidate
            for (int i = 0; i < u.GetDimension(); i++)
                vCoordinates[i] = Globals.k - u.Coordinates[i] - Globals.gap;

            while(!terminateFlag)
			{
                candidateV = new Point2(vCoordinates);

				//same point check
				if (u.Equals(candidateV))
				{
					if (Globals.ShowMessages)
						Console.WriteLine(candidateV.ToString() + ": Eliminated. Same point as u.");
				}
	            //inverse check
	            else if (CheckInverse(u, candidateV))
	            {
	                if (Globals.ShowMessages)
	                    Console.WriteLine(candidateV.ToString() + ": Eliminated. Inverse already checked.");
	            }
                else 
                {
                    var uRedundant = false;
                    var vRedundant = false;

					//generate gi 
					for (int i = 0; i < Globals.d; i++)
						g0[i] = Globals.gap + u.Coordinates[i] + vCoordinates[i] - Globals.k;

					//check vStar, given gap i, check that both u and v exist within the vertex set of the corresponding facets.
					if (!CheckVertexSet(u, candidateV, g0, Globals.d - 1, Globals.k))
					{
                        if (Globals.ShowMessages)
							Console.WriteLine(candidateV.ToString() + ": Eliminated.  Point does not belong to vertex set.");
					}
					//check convex core
                    else if (CheckConvexCore(u, candidateV, g0, out uRedundant, out vRedundant))
					{
						result.Add(candidateV);

                        if (Globals.ShowMessages)
							Console.WriteLine("u: " + u + ". v: " + candidateV);
					}
					else
					{
                        if (Globals.ShowMessages)
						{
							Console.Write("u: " + u + ". v: ");
                            foreach (int i in vCoordinates)
								Console.Write(i + " ");

							Console.WriteLine(" eliminated, " + (uRedundant && vRedundant ? "u and v are not vertices " :
											   (uRedundant ? "u is not a vertex " : "v is not a vertex ")) +
											  "of the convex core.");
						}
					}

                    result.Add(candidateV);

                    if (Globals.ShowMessages)
						Console.WriteLine("u: " + u + ". v: " + candidateV);
				}

                //increment candidate and check termination condition
                while (symmetryCheck(u.Coordinates, vCoordinates))
                    IncrementCandidatePoint(u.Coordinates, vCoordinates, out terminateFlag);
            }
			return result;
        }

		//returns true if inverse has NOT been checked (i.e. u,v is a valid pair).
        private static bool CheckInverse(Point2 u, Point2 v)
		{
			var vCoords = v.Coordinates;
			var inverseVCoords = new int[Globals.d];

			//checks to see if any element of v is less than half of k/2
			for (int i = 0; i < Globals.d; i++)
			{
				if (vCoords[i] < Math.Ceiling((double)Globals.k / 2))
					return false;
			}

			//checks to see if v inverse ordered lexicographically is smaller than u
			for (int i = 0; i < Globals.d; i++)
				inverseVCoords[i] = Globals.k - vCoords[i];

			Array.Sort(inverseVCoords);

			if (u.CompareLexicographically(new Point2(inverseVCoords)))
				return false;

			return true;
		}

        //checks if u (and v) touches an intersection of the hypercube, and if so,
        //returns true if that point is in the vertex set
        private static bool CheckVertexSet(Point2 u, Point2 v, int[] gap0, int d, int k)
        {
            if (!Globals.CheckVStar)
                return true;

            var index = "";
            var found = false;

            for (int i = 0; i < Globals.d; i++)
            {
                //check that point is in intersection with hypercube
                if (v.Coordinates[i] != Globals.k && v.Coordinates[i] != 0)
                    continue;

                index = d.ToString() + "," + k.ToString() + "," + (2 * Globals.gap - gap0[i]).ToString();

                //check for existence of vertex set
                if (Globals.VertexSet.ContainsKey(index) && Globals.VertexSet[index].Count == 0)
                    continue;

                Point2 q = v.DecreaseDimensionality(i);
                found = false;

                foreach (Point2 p in Globals.VertexSet[index])
                {
                    if (p.Equals(q))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    return false;
            }
            for (int i = 0; i < Globals.d; i++)
            {
                //check that point is in intersection with hypercube
                if (u.Coordinates[i] != Globals.k && u.Coordinates[i] != 0)
                    continue;

                index = d.ToString() + k.ToString() + gap0[i].ToString();

                //check for existence of vertex set
                if (Globals.VertexSet.ContainsKey(index) && Globals.VertexSet[index].Count == 0)
                    continue;

                Point2 q = u.DecreaseDimensionality(i);
                found = false;

                foreach (Point2 p in Globals.VertexSet[index])
                {
                    if (p.Equals(q))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    return false;
            }

            return true;
        }

		//returns true if points are vertices of the convex core
        private static bool CheckConvexCore(Point2 u, Point2 v, int[] gap, out bool uRedundant, out bool vRedundant)
		{
			List<Point2> corePoints = new List<Point2>();
            var index0 = "";
            var indexk = "";
			Point2 q;

			corePoints.Add(u);
			corePoints.Add(v);

			for (int i = 0; i < Globals.d; i++)
			{
                // have to finalize key format being used in dictionary.

				index0 = (Globals.d - 1).ToString() + Globals.k.ToString() + gap[i].ToString();
				indexk = (Globals.d - 1).ToString() + Globals.k.ToString() + (((2 * Globals.gap - gap[i]) > Globals.gap) ? (Globals.gap) : (2 * Globals.gap - gap[i])).ToString();

				//List<Point> corePoints0 = FileIO.readPointsFromFile(Globals.directory + "/" + index0 + "/coreSet");
				//List<Point> corePointsk = FileIO.readPointsFromFile(Globals.directory + "/" + indexk + "/coreSet");

				var corePoints0 = (Globals.CoreSet.ContainsKey(index0)) ? Globals.CoreSet[index0] : new List<Point2>();
				var corePointsk = (Globals.CoreSet.ContainsKey(indexk)) ? Globals.CoreSet[indexk] : new List<Point2>();

				foreach (Point2 p in corePoints0)
				{
					q = p.Clone();
					q.IncreaseByDimensionality(i, true);
					corePoints.Add(q);
				}
				foreach (Point2 p in corePointsk)
				{
					q = p.Clone();
					q.IncreaseByDimensionality(i, false);
					corePoints.Add(q);
				}
			}

			return CDD.CheckPointsConvexHullVertices(corePoints, u, v, out uRedundant, out vRedundant);
		}

		//find next valid point. If no valid points, then sets terminateFlag to true;
        public static void IncrementCandidatePoint(int[] uCoordinates, int[] candidateVCoordinates, out bool terminateFlag)
		{
			terminateFlag = true;

			var changeFlag = false;
            var i = candidateVCoordinates.Length - 1;

			while (!changeFlag)
			{
				if (i < 0)
					break;

                candidateVCoordinates[i]++;

				//checks for validity of coords and symmetry
                if (CheckValidCoord(uCoordinates[i], candidateVCoordinates[i]))
				{
					changeFlag = true;
					terminateFlag = false;
				}
				else
				{
                    candidateVCoordinates[i] = Math.Max(0, Globals.k - uCoordinates[i] - Globals.gap);
					i--;
				}
			}
		}

		//return true if current coord of v has not reached the max value it can be. v[i] is valid if v[i] <= MIN(k, k - u[i] + g)
        private static bool CheckValidCoord(int u, int v)
		{
			return v <= Math.Min(Globals.k, Globals.k - u + Globals.gap);
		}

		private static bool symmetryCheck(int[] uCoordinates, int[] vCoordinates)
		{
			for (int i = 0; i < uCoordinates.Length - 1; i++)
			{
				if (uCoordinates[i] == uCoordinates[i + 1])
				{
					if (vCoordinates[i] < vCoordinates[i + 1])
						return true;
				}
			}
			return false;
		}

		public static bool containsPoint(List<Point2> validPoints, Point2 p)
		{
			foreach (Point2 q in validPoints)
			{
				if (q.Equals(p))
					return true;
			}
			return false;
		}







		/// <summary>
		/// returns true if point u is symmetric (i.e. an element at [i] is larger than [i+1]. 
		/// 100 returns true since 001 should have been checked already)
		/// </summary>
		/// <returns>true, if every coordinate is equal or larger than preceding coordinate, false otherwise.</returns>
		/// <param name="u">U.</param>
		private static bool SymmetryCheck(int[] point)
		{
			for (int i = 0; i < point.Length - 1; i++)
			{
				if (point[i] > point[i + 1])
					return true;
			}

			return false;
		}



        //old version
        /*
		private static bool symmetryCheck(string u, string v)
		{
			char[] utemp = u.ToCharArray();
			char[] vtemp = v.ToCharArray();

			for (int i = 0; i < utemp.Length - 1; i++)
			{
				if (utemp[i].CompareTo(utemp[i + 1]) == 0)
				{
					if (vtemp[i].CompareTo(vtemp[i + 1]) > 0)
						return true;
				}
			}

			return false;
		}



		public static bool containsPoint(List<Point> validPoints, string s)
		{
			foreach (Point p in validPoints)
			{
				if (p.Equals(s))
					return true;
			}
			return false;
		}

		public static List<Point> generateV(Point u)
		{
			List<Point> result = new List<Point>();
			Point new_v;
			int[] g0 = new int[Globals.d];
			bool uRedundant, vRedundant, terminateFlag;

			int[] uPoints = u.getIntArray();
			int[] vPoints = new int[uPoints.Length];

			//set first point v to check
			for (int i = 0; i < uPoints.Length; i++)
			{
				vPoints[i] = Globals.k - uPoints[i] - Globals.gap;
			}

			while (true)
			{
				new_v = new Point(vPoints);

				//check same point
				if (u.Equals(new_v))
				{
					if (Globals.messageOn)
						Console.WriteLine(new_v.ToString() + ": Eliminated. Same point as u.");
				}
				//inverse check
				else if (CheckInverse(u, new_v))
				{
					if (Globals.messageOn)
						Console.WriteLine(new_v.ToString() + ": Eliminated. Inverse already checked.");
				}
				else
				{
					//generate gi 
					for (int i = 0; i < Globals.d; i++)
					{
						g0[i] = Globals.gap + u.getIntArray()[i] + vPoints[i] - Globals.k;
					}

					//check vStar, given gap i, check that both u and v exist within the vertex set of the corresponding facets.
					if (!CheckVertexSet(u, new_v, g0, Globals.d - 1, Globals.k))
					{
						if (Globals.messageOn)
							Console.WriteLine(new_v.ToString() + ": Eliminated.  Point does not belong to vertex set.");
					}

*/
					//check convex core
					/*
                    else if (checkConvexCore(u, new_v, g0, out uRedundant, out vRedundant))
                    {
                        result.Add(new_v);

                        if (Globals.messageOn)
                            Console.WriteLine("u: " + u + ". v: " + new_v);
                    }
                    else
                    {
                        if (Globals.messageOn)
                        {
                            Console.Write("u: " + u + ". v: ");
                            foreach (int i in vPoints)
                                Console.Write(i + " ");

                            Console.WriteLine(" eliminated, " + (uRedundant && vRedundant ? "u and v are not vertices " :
                                               (uRedundant ? "u is not a vertex " : "v is not a vertex ")) +
                                              "of the convex core.");
                        }
                    }*/
        /*

					result.Add(new_v);

                    if (Globals.ShowMessages)
						Console.WriteLine("u: " + u + ". v: " + new_v);
				}

				do
				{
					IncrementCandidatePoint(u.getIntArray(), vPoints, out terminateFlag);
				}
				while (symmetryCheck(u.Coordinates, Point.convertIntArrayToString(vPoints)) && !terminateFlag);

				//termination condition check
				if (terminateFlag)
					break;
			}

			return result;
		}

		



		
		

		//find next valid point. If no valid points, then sets terminateFlag to true;
		public static void incrementPoint(int[] u, int[] points, out bool terminateFlag)
		{
			terminateFlag = true;

			bool changeFlag = false;
			int i = points.Length - 1;

			while (!changeFlag)
			{
				if (i < 0)
					break;

				points[i]++;

				//checks for validity of coords and symmetry
				if (CheckValidCoord(u[i], points[i]))
				{
					changeFlag = true;
					terminateFlag = false;
				}
				else
				{
					points[i] = Math.Max(0, Globals.k - u[i] - Globals.gap);

					i--;
				}
			}
		}*/
	}
}
