using System;
using System.Collections.Generic;

namespace Research.Core
{
    public class Globals
    {
        public static int d;
        public static int k;
        public static int gap;
		public static int[] MaximumDiameter;

		public static bool ShowMessages;

        public static long chTime;

        //Determining a(d)
        public Dictionary<string, int> Vertices;
        public Dictionary<string, int> RedVertices;
        public Dictionary<string, int> NonVertices;

        //Delta(d,k)
        public static Dictionary<string, List<Point2>> VertexSet;
        public static Dictionary<string, List<Point2>> NonVertexSet;
        public static Dictionary<string, List<Point2>> CoreSet;
        public static bool CheckVStar;
        public static int convexHullCount;
    }
}
