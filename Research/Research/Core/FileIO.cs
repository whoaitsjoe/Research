using System;
using System.Collections.Generic;
using System.IO;

namespace Research.Core
{
    public class FileIO
    {
        
        public static void WritePointsToFile(List<Point2> points, string outFile)
		{
            System.IO.File.WriteAllLines(outFile, FormatOutput(points));
        }

        /// <summary>
        /// Formats list of points to be proper format for CDD input.
        /// </summary>
        /// <returns>Formatted output.</returns>
        /// <param name="points">List of points.</param>
        private static String[] FormatOutput(List<Point2> points)
        {
			var result = new string[points.Count + 4];

			result[0] = "V-representation";
			result[1] = "begin";
			result[2] = points.Count + " " + (points[0].GetDimension() + 1) + " " + "integer";
			result[result.Length - 1] = "end";

			for (int i = 0; i < points.Count; i++)
			{
				var temp = " 1 ";
				foreach (int c in points[i].Coordinates)
				{
                    temp += c.ToString() + " ";
				}
				result[3 + i] = temp;
			}
			return result;
        }








		public static List<Graph> readPolytopes()
		{
			List<Graph> result;

			var path = "/Users/Joe/Code/research/latticePolytopes/Files/" + Globals.d + Globals.k + Globals.gap + "/Polytopes";

			result = readGraphFromFile(path);

			return result;
		}

		public static List<Point2> readVertexList(int gap = -1, string path = "")
		{
			List<Point2> result = new List<Point2>();

			if (path.Equals(""))
				path = "/Users/Joe/Code/research/latticePolytopes/Files/" + (Globals.d - 1) + Globals.k + ((gap == -1) ? Globals.gap : gap) + "/vertex";

			result = readPointsFromFile(path);

			return result;
		}

		//Reads list of graphs from file
		//File has format as follows:
		//Facet #
		//point 1 (e.g. 001)
		//--
		//edge 1 (e.g. 001 : 002 010)
		public static List<Graph> readGraphFromFile(string fName)
		{
			List<Graph> result = new List<Graph>();
			string[] lines = System.IO.File.ReadAllLines(fName);
			bool vertex = false;
			bool edge = false;
			List<Point> pts = new List<Point>();
			Dictionary<string, List<string>> a = new Dictionary<string, List<string>>();
			string id;

			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i].StartsWith("Facet", StringComparison.Ordinal))
				{
					vertex = true;
					edge = false;
					i++;
					if (pts.Count > 0)
					{
						result.Add(new Graph(pts, a));
					}
					pts = new List<Point>();
				}
				else if (lines[i] == "--")
				{
					vertex = false;
					edge = true;
					i++;
					a = new Dictionary<string, List<string>>();
				}
				else if (lines[i] == "end")
				{
					result.Add(new Graph(pts, a));
				}

				if (vertex)
				{
					pts.Add(new Point(lines[i]));
				}
				if (edge)
				{
					List<string> adjList = parseAdjList(lines[i], out id);
					a.Add(id, adjList);
				}
			}
			return result;
		}

		//reads points from file
		//File has one point per line (e.g. 001).
		public static List<Point2> readPointsFromFile(string fName)
		{
			List<Point2> result = new List<Point2>();
			string[] lines;

			try
			{
				lines = System.IO.File.ReadAllLines(fName);
			}
			catch (Exception e)
			{
				return new List<Point2>();
			}

			for (int i = 0; i < lines.Length; i++)
			{
				result.Add(new Point2(lines[i]));
			}

			return result;
		}

		//converts each line that's read-in into List of ints.
		//skips all elements before ':', then convert each token thereafter to an int.
		public static List<string> parseAdjList(string line, out string id)
		{
			List<string> result = new List<string>();
			string[] tokens = line.Split(' ');
			bool flag = false;
			id = tokens[0];

			foreach (string word in tokens)
			{
				if (word == ":")
				{
					flag = true;
					continue;
				}
				if (flag)
				{
					result.Add(word);
				}
			}

			return result;
		}
    }
}
