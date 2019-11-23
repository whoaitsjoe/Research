using System;
using System.Collections.Generic;
using Research.Core;

namespace Research.DeterminingAd
{
    public class Run
    {
        private List<Point2> Vertices;
        private List<Point2> CandidateVertices;
        private List<Point2> FinalCandidateVertexList;
        private List<Point2> RedVertices;
        private List<Point2> NonVertices;

        /// <summary>
        /// Main method that gets called for determining ad.
        /// </summary>
        public void run()
        {
            init(Globals.d);
            var allEdges = Initialization.initializeEdges();
            var candidateVertexQueue = new Queue<Point2>();
            var invalidVertexFlag = false; //flag to determine if one of the tests fail (i.e. candidate vertex cannot be a vertex).
            var st = DateTime.Now;  // datetime variable to track running time.
            var timeCheck = true;   // bool to display running time for each check.
            Point2 currentCandidate;

            foreach (Point2 candidateVertex in CandidateVertices)
                candidateVertexQueue.Enqueue(candidateVertex);

            //go through a number of checks to see if candidate vertex is still valid.
            while(candidateVertexQueue.Count != 0)
            {
                currentCandidate = candidateVertexQueue.Dequeue();

                foreach(Point2 possibleEdge in allEdges)
                {
                    //add possibleEdge to currentCandidate
                    Point2 thisCandidate = Point2.AddPoints(currentCandidate, possibleEdge);

                    invalidVertexFlag = false; //reset vertex flag

                    if (thisCandidate == null)
                    {
                        if(Globals.ShowMessages)
                            Console.WriteLine("ERROR: thisCandidate is null. Check possibleEdge being added to currentCandidate.");
                        
						invalidVertexFlag = true;
						continue;
					}

					thisCandidate.SortLexicographically(); //sort coordinates of points to account for symmetry

					//Testing output
					Console.WriteLine("Now checking: {0}. # of elements in queue: {1}", thisCandidate, candidateVertexQueue.Count);

					st = DateTime.Now;

                    //check to see if currentCandidate is in RedVertices List
                    foreach(Point2 thisRedVertex in RedVertices)
                    {
                        if (thisCandidate.Equals(thisRedVertex))
						{
							if (Globals.ShowMessages)
                                Console.WriteLine("{0} is a red vertex.", thisCandidate);
                            
                            invalidVertexFlag = true;
                            break;
                        }
					}

                    if(timeCheck)
					    Console.WriteLine("Checking Red Vertices: {0}ms.", st - DateTime.Now);

					st = DateTime.Now;

					if (invalidVertexFlag)
						continue;
                    
                    //check if sum of coordinates is > d*2^(d-2)
                    if (thisCandidate.SumOfCoordinates() > (Globals.d * Math.Pow(2, (Globals.d - 2))))
					{
						if (Globals.ShowMessages)
                            Console.WriteLine("{0}. Sum of coordinates exceeds d*2^(d-2).", thisCandidate);

                        NonVertices.Add(thisCandidate);

                        continue;
					}

                    if(timeCheck)
					    Console.WriteLine("Checking Sum of Coords > d*2^(d-2): {0}ms.", st - DateTime.Now);
                    
					st = DateTime.Now;

                    foreach(int coordinate in thisCandidate.Coordinates)
                    {
                        if(coordinate == 0)
                        {
							if (Globals.ShowMessages)
                                Console.WriteLine("--Eliminated. In Red Vertices List (one of the coordinates is 0).");
                            
                            invalidVertexFlag = true;
                            break;
                        }
                    }

                    if(timeCheck)
					    Console.WriteLine("Checking Each Coord for 0: {0}ms.", st - DateTime.Now);
                    
					st = DateTime.Now;

                    if (invalidVertexFlag)
                        continue;

                    //check if currentCandidate is in list of vertices
                    foreach(Point2 thisVertex in Vertices)
                    {
                        if (thisCandidate.Equals(thisVertex))
						{
                            if (Globals.ShowMessages)
                                Console.WriteLine("--Eliminated. In Vertices List.");
                            
                            invalidVertexFlag = true;
                            break;
                        }
                    }

                    if(timeCheck)
                        Console.WriteLine("In Vertex List Check: {0}ms.", st - DateTime.Now);
                    
                    st = DateTime.Now;

                    if (invalidVertexFlag)
                        continue;

					//check if currentCandidate is in list of non-vertices
					foreach (Point2 thisNonVertex in NonVertices)
					{
						if (thisCandidate.Equals(thisNonVertex))
						{
                            if (Globals.ShowMessages)
                                Console.WriteLine("--Eliminated. In Non Vertices List.");
                            
							invalidVertexFlag = true;
							break;
						}
					}

                    if(timeCheck)
                        Console.WriteLine("In Nonvertex List Check: {0}ms.", st - DateTime.Now);
                    
                    st = DateTime.Now;

					if (invalidVertexFlag)
						continue;

                    //check if currentCandidate is in list of FinalCandidate (e.g. already checked before).
                    foreach(Point2 thisFinalCandidate in FinalCandidateVertexList)
                    {
                        if(thisCandidate.Equals(thisFinalCandidate))
                        {
                            if (Globals.ShowMessages)
                                Console.WriteLine("--Eliminated. In Final Candidate List.");
                            
                            invalidVertexFlag = true;
                            break;
                        }
					}

                    if(timeCheck)
					    Console.WriteLine("In Final Candidate List Check: {0}ms.", st - DateTime.Now);
                    
					st = DateTime.Now;

					if (invalidVertexFlag)
						continue;

                    //Check if currentCandidate is in one of the cones
                    if (InsideConeCheck(thisCandidate))
					{
						if (Globals.ShowMessages)
							Console.WriteLine("{0} is inside a cone.", thisCandidate);

						NonVertices.Add(thisCandidate);
                        continue;
                    }

                    if(timeCheck)
                        Console.WriteLine("Inside Cone CHeck: {0}ms.", st - DateTime.Now);
                    
                    st = DateTime.Now;

                    //Epsilon Check -- check eps{ij}=1 while eps{i}=eps{j}=0 and eps{ij}=0 while eps{i}=eps{j}=1
                    if (EpsilonCheck(thisCandidate))
					{
						if (Globals.ShowMessages)
							Console.WriteLine("{0} failed the epsilon check.", thisCandidate);
                        
                    }

					if (timeCheck)
						Console.WriteLine("Epsilon Check: {0}ms.", st - DateTime.Now);

					st = DateTime.Now;


                    //Double decomposition heuristic
                    if (DoubleDecompositionHeuristic(thisCandidate, allEdges))
					{
						if (Globals.ShowMessages)
							Console.WriteLine("{0} has at least two decompositions.", thisCandidate);

						NonVertices.Add(thisCandidate);

                        if(timeCheck)
	                        Console.WriteLine("DDHeuristic: {0}ms.", st - DateTime.Now);
	                    st = DateTime.Now;

                        continue;
                    }

                    if(timeCheck)
					    Console.WriteLine("DDHeuristic: {0}ms.", st - DateTime.Now);
                    
					if (Globals.ShowMessages)
						Console.WriteLine("Adding {0} to Final Candidate Vertex List.", thisCandidate);
                    
                    FinalCandidateVertexList.Add(thisCandidate);
                    candidateVertexQueue.Enqueue(thisCandidate);
                }
            }

            CandidateVertexPostProcessing(FinalCandidateVertexList);

			Console.WriteLine("\n\nRed Vertices:");
            foreach (Point2 p in RedVertices)
				Console.WriteLine(p);

            Console.WriteLine("\n\nFinal Candidate Vertex List: ");
            foreach (Point2 p in FinalCandidateVertexList)
                Console.WriteLine(p);
        }

		public void runWithCandidate(Point2 candidate)
		{
			init(Globals.d);
			List<Point2> allEdges = Initialization.initializeEdges();
			Queue<Point2> candidateVertexQueue = new Queue<Point2>();
			var invalidVertexFlag = false; //flag to dtermine if one of the tests fail (i.e. candidate vertex cannot be a vertex).
			var st = DateTime.Now;

			foreach (Point2 candidateVertex in CandidateVertices)
				candidateVertexQueue.Enqueue(candidateVertex);


			invalidVertexFlag = false; //reset vertex flag

			if (candidate == null)
			{
				if (Globals.ShowMessages)
					Console.WriteLine("ERROR: thisCandidate is null. Check possibleEdge being added to currentCandidate.");

				invalidVertexFlag = true;
				return;
			}

			candidate.SortLexicographically(); //sort coordinates of points to account for symmetry

			//Testing output
			Console.WriteLine("Now checking: {0}. # of elements in queue: {1}", candidate, candidateVertexQueue.Count);

			//check to see if currentCandidate is in RedVertices List
			foreach (Point2 thisRedVertex in RedVertices)
			{
				if (candidate.Equals(thisRedVertex))
				{
					if (Globals.ShowMessages)
						Console.WriteLine("{0} is a red vertex.", candidate);

					if (Globals.ShowMessages)
						Console.WriteLine("--Eliminated. In Red Vertices List.");

					invalidVertexFlag = true;
					break;
				}
			}

			//check if sum of coordinates is > d*2^(d-2)
			if (candidate.SumOfCoordinates() > (Globals.d * Math.Pow(2, (Globals.d - 2))))
			{
				if (Globals.ShowMessages)
					Console.WriteLine("{0}. Sum of coordinates exceedds d*2^(d-2).", candidate);

				NonVertices.Add(candidate);

				return;
			}

			foreach (int coordinate in candidate.Coordinates)
			{
				if (coordinate == 0)
				{
					if (Globals.ShowMessages)
						Console.WriteLine("--Eliminated. In Red Vertices List (one of the coordinates is 0).");

					invalidVertexFlag = true;
					break;
				}
			}

			//check if currentCandidate is in list of vertices
			foreach (Point2 thisVertex in Vertices)
			{
				if (candidate.Equals(thisVertex))
				{
					if (Globals.ShowMessages)
						Console.WriteLine("--Eliminated. In Vertices List.");

					invalidVertexFlag = true;
					break;
				}
			}

			//check if currentCandidate is in list of non-vertices
			foreach (Point2 thisNonVertex in NonVertices)
			{
				if (candidate.Equals(thisNonVertex))
				{
					if (Globals.ShowMessages)
						Console.WriteLine("--Eliminated. In Non Vertices List.");

					invalidVertexFlag = true;
					break;
				}
			}

			foreach (Point2 thisFinalCandidate in FinalCandidateVertexList)
			{
				if (candidate.Equals(thisFinalCandidate))
				{
					if (Globals.ShowMessages)
						Console.WriteLine("--Eliminated. In Final Candidate List.");

					invalidVertexFlag = true;
					break;
				}
			}

			if (invalidVertexFlag)
				return;

			//Check if currentCandidate is in one of the cones
			if (InsideConeCheck(candidate))
			{
				Console.WriteLine("{0} is inside a cone.", candidate);

				return;
			}

			//Check decomposition heuristic
			if (DoubleDecompositionHeuristic(candidate, allEdges))
			{
				Console.WriteLine("{0} has at least two decompositions.", candidate);

				return;
			}

			Console.WriteLine("Adding {0} to Final Canddiate Vertex List.", candidate);
		}

        private void init(int dimension)
        {
            CandidateVertices = Initialization.InitializeVertices(dimension);

            //removes first element of CandidateVertices if it's 000
            if (CandidateVertices[0].SumOfCoordinates() == 0)
                CandidateVertices.RemoveAt(0);
            
            Vertices = new List<Point2>();
            NonVertices = new List<Point2>();
            RedVertices = Initialization.InitializeRedVertices(dimension);
            FinalCandidateVertexList = new List<Point2>();
        }

		//returns true if candidate vertex can be decomposed in multiple ways from set of possible edges.
		private bool DoubleDecompositionHeuristic(Point2 candidateVertex, List<Point2> possibleEdges)
        {

            return (DoubleDecompositionBruteForce(candidateVertex, possibleEdges));
            //possibleEdges may not be required.

            var numberOfNonZeroCoordinates = candidateVertex.GetDimension();

            List<Point2> usedVectors = new List<Point2>();
            //create data structure to store (2) decompositions of candidate vertices.

            //make a copy of candidateVertex that will be modified (i.e. subtracting vectors).

            while(true)     //need to determine termination condition for loop
            {
				//find first nonzero coordinate of current vector
				var firstNonZeroIndex = candidateVertex.FirstNonZeroIndex();
                List<Point2> splitVectors = new List<Point2>();

                //generate corresponding unit vector
                Point2 unitVector = Point2.GenerateOnesVector(candidateVertex.GetDimension(), firstNonZeroIndex);

                //ensure unit vector is not in usedVector list, if vector is used, perform split. Check to make sure split vectors are not used before.
                if (usedVectors.Contains(unitVector))
                    splitVectors = Point2.SplitVectors(unitVector, usedVectors);

                if(splitVectors.Count == 0)
                {
                    //Cannot split the current vector, hence no decomposition possible
                }

                //subtract unit vector from current vector

                //add unit vector to usedVeectorList
            }
            return DoubleDecompositionBruteForce(candidateVertex, possibleEdges);
        }

        //helper function to generate unit vector of length d, with the first x coordinates as 0
        private Point2 GenerateUnitVector(int d, int x)
        {
            var intArray = new int[d];

            for (int i = 0; i < d; i++)
                intArray[i] = (i < x) ? 0 : 1;

            return new Point2(intArray);
        }

        //returns true if candidate vertex can be decomposed in multiple ways from set of possible edges.
        private bool DoubleDecompositionBruteForce(Point2 candidateVertex, List<Point2> possibleEdges)
        {
            int helperResult = DoubleDecompositionBruteForceHelper(candidateVertex, possibleEdges, 0);

            if (helperResult == 1)
                return false;
            else if (helperResult < 1)
                Console.WriteLine("ERROR: DoubleDecompositionHelper did not find a single decomposition.");
            
            return true;
        }

        //recursive helper for DDHeuristic
        private int DoubleDecompositionBruteForceHelper(Point2 candidateVertex, List<Point2> possibleEdges, int index)
        {
            if (candidateVertex.SumOfCoordinates() == 0)
                return 1;
            
            if (index >= possibleEdges.Count)
                return 0;

			//subtract current edge from candidateVertex
			Point2 newCandidateVertex = Point2.SubtractPoints(candidateVertex, possibleEdges[index]);

            if (newCandidateVertex == null)
                return DoubleDecompositionBruteForceHelper(candidateVertex, possibleEdges, index + 1);
            else
                return DoubleDecompositionBruteForceHelper(candidateVertex, possibleEdges, index + 1) +
                    DoubleDecompositionBruteForceHelper(newCandidateVertex, possibleEdges, index + 1);
		}

        //return true if the point is inside one of the cones
        private bool InsideConeCheck(Point2 candidateVertex)
		{
            int[] candidateIntArray = candidateVertex.Coordinates;
            int sum = 0;

            for (int i = 0; i < candidateIntArray.Length; i++)
            {
                sum += candidateIntArray[i];
            }
            if (sum > Globals.d * 2 - 1 || sum > ((Globals.d - 1)*Math.Pow(2, (Globals.d - 1))/2))
                return false;

			//Checking 111d cone
			for (int i = 0; i < candidateIntArray.Length; i++)
            {
                sum = 0;
                for (int j = 0; j < candidateIntArray.Length; j++)
                {
                    if (j == i)
                        continue;
                    
                    sum += candidateIntArray[j];
                }
                if (sum > candidateIntArray[i] * (Globals.d * 2 - 2))
                    return false;
            }

            //Checking 0,k/2,k/2 cone
            for (int i = 0; i < candidateIntArray.Length; i++)
			{
				sum = 0;
                for (int j = 0; j < candidateIntArray.Length; j++)
                {
                    if (j == i)
                        continue;
                    
                    sum += candidateIntArray[j];
                }
                if (sum > candidateIntArray[i] * (Globals.d - 2))
                    return false;
            }

            return true;
        }

        //post-processing for final candidate vertex list
        private void CandidateVertexPostProcessing(List<Point2> CandidateVertexList)
        {
            //remove duplicates (ie. when sum of coordinates = d*2^(d-2), min x_i >= 2^(d-2))
            Stack<int> MaxCoordinateIndices = new Stack<int>();
            int coordinateLimit = (int)Math.Pow(2, (Globals.d - 2));

            for (int i = 0; i < CandidateVertexList.Count; i++)
            {
                if (CandidateVertexList[i].SumOfCoordinates() == Globals.d * coordinateLimit)
                    MaxCoordinateIndices.Push(i);
            }

            //remove dups if there are more than 1 vertex with max coord sum
            if(MaxCoordinateIndices.Count > 0)
            {
                int GreaterThanLimitCoordCount = Globals.d;
                int previousIndex = -1;

                while(MaxCoordinateIndices.Count > 0)
                {
                    int currentIndex = MaxCoordinateIndices.Pop();

                    int[] currentPointIntArray = CandidateVertexList[currentIndex].Coordinates;

                    int currentPointGreaterThanLimitCount = 0;

                    foreach(int currentCoord in currentPointIntArray)
                    {
                        if (currentCoord >= coordinateLimit)
                            currentPointGreaterThanLimitCount++;
                    }

                    if(currentPointGreaterThanLimitCount < GreaterThanLimitCoordCount)
                    {
                        GreaterThanLimitCoordCount = currentPointGreaterThanLimitCount;

                        if (previousIndex >= 0)
                            CandidateVertexList.RemoveAt(previousIndex);

                        previousIndex = currentIndex;
                    }
                    else if(currentPointGreaterThanLimitCount == GreaterThanLimitCoordCount)
                    {
						Console.WriteLine("ERROR - CandidateVertex Post Processing: Two points have the same number of coordinates above 2^(d-2).");
                        CandidateVertexList.RemoveAt(currentIndex);
                    }
					else
						CandidateVertexList.RemoveAt(currentIndex);
                }
            }
		}

        //epsilon check - returns true if it fails
        private static bool EpsilonCheck(Point2 candidatePoint)
        {
            return false;   
        }
    }
}
