using System;
using System.Collections.Generic;
using Research.Core;

namespace Research.LatticePolytopes
{
    public class Run
	{        
		/// <summary>
		/// Initialize Global parameters for this instance.
		/// </summary>
		private static void Init()
		{
			Globals.d = 3;
			Globals.k = 6;
			Globals.gap = 0;
			//Globals.maxDiameter = new int[] { 0, 2, 3, 4, 4, 5, 6, 6, 7, 8, 8 };
			//Globals.diameter = Globals.maxDiameter[Globals.k];
			//Globals.maxLength = Globals.diameter + Globals.k - Globals.gap; //delta(d-1,k)+k-gap, the number to be eliminated
			Globals.chTime = 0;
			Globals.ShowMessages = true;
			Globals.VertexSet = new Dictionary<string, List<Point2>>();
			Globals.NonVertexSet = new Dictionary<string, List<Point2>>();
			Globals.CoreSet = new Dictionary<string, List<Point2>>();
		}

        public Run()
        {
            Init();


        }

        private static void PerformShelling()
        {
            List<Point2> startPoints, endPoints;
            var startEndPairs = new List<Tuple<Point2, Point2>>();

            startPoints = GenerateUV.GenerateU();

			foreach (Point2 p in startPoints)
			{
				endPoints = GenerateUV.GenerateV(p);
				foreach (Point2 q in endPoints)
                    startEndPairs.Add(new Tuple<Point2, Point2>(p, q));
			}

            Console.WriteLine("Total u,v pairs: " + startEndPairs.Count);

        }

    }
}
