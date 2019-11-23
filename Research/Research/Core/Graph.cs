using System;
using System.Collections.Generic;

//Used to store graphs and related functions.
namespace Research.Core
{
    public class Graph
    {
        //Variables
        //---------
        public List<Point> Points 
        {
            get;
            set;
        }
        public Dictionary<string, List<string>> AdjacencyList
        {
            get;
            set;
        }

        //Constructors
        //------------
        public Graph()
        {
            Points = new List<Point>();
            AdjacencyList = new Dictionary<string, List<string>>();
        }

        public Graph(List<Point> points, Dictionary<string, List<string>> adjacencyList)
        {
            Points = points;
            AdjacencyList = adjacencyList;
        }

        //Functions
        //---------
        public List<Point> GetAllPoints()
        {
            List<Point> result = new List<Point>();
            foreach (Point p in Points)
                result.Add(p.Clone());
            return result;
        }

        public List<Point> GetAllPoints(int nextFacet)
        {
            List<Point> result = new List<Point>();
            foreach (Point p in Points)
            {
                if (Convert.ToInt32(p.Coordinates[nextFacet / 2].ToString()) == ((nextFacet % 2 == 0) ? 0 : Globals.k))
                    result.Add(p.Clone());
            }
            return result;
        }

        public List<string> GetAdjacencyList(int index)
        {
            if (index < 1)
                return null;

            return AdjacencyList[Points[index].Coordinates];
        }

        public int NumberOfPoints()
        {
            return Points.Count;
        }

        public void AddFacet(Graph newFacet)
        {
            bool found;

            //copies points which are currently not in the points list
            foreach (Point newPoint in newFacet.Points)
            {
                found = false;

                foreach (Point existingPoint in Points)
                {
                    if (newPoint.Equals(existingPoint))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    Points.Add(newPoint.Clone());
            }

            //copies over new entries in adjList
            foreach (KeyValuePair<string, List<string>> adjacencyListEntry in newFacet.AdjacencyList)
            {
                //if entry exists for key (e.g. point was previously in graph and has edges)
                if (AdjacencyList.ContainsKey(adjacencyListEntry.Key))
                {
                    foreach (string newFacetAdjacencyListValue in adjacencyListEntry.Value)
                    {
                        found = false;

                        foreach (string existingAdjacencyListValue in AdjacencyList[adjacencyListEntry.Key])
                        {
                            if (existingAdjacencyListValue.Equals(newFacetAdjacencyListValue, StringComparison.Ordinal))
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                            AdjacencyList[adjacencyListEntry.Key].Add(newFacetAdjacencyListValue);
                    }
                }
                else
                {
                    AdjacencyList.Add(adjacencyListEntry.Key, adjacencyListEntry.Value);
                }
            }
        }

        public override string ToString()
        {
            string returnString = "Points: \n";

            foreach (Point thisPoint in Points)
                returnString += thisPoint + "\n";

            returnString += "Adjacency List: \n";

            foreach (KeyValuePair<string, List<string>> adjacencyListEntry in AdjacencyList)
            {
                returnString += adjacencyListEntry.Key + " : ";

                foreach (string adjacencyListValue in adjacencyListEntry.Value)
                    returnString += adjacencyListValue + " ";
                
                returnString += "\n";
            }

            return returnString;
        }

        public Graph Clone()
        {
            List<Point> points = new List<Point>();
            Dictionary<string, List<string>> adjacencyList = new Dictionary<string, List<string>>();

            foreach (Point thisPoint in Points)
                points.Add(thisPoint.Clone());

            foreach (KeyValuePair<string, List<string>> adjacencyListEntry in AdjacencyList)
            {
                List<string> valueList = new List<string>();

                foreach (string adjacencyListValue in adjacencyListEntry.Value)
                    valueList.Add(adjacencyListValue);

                adjacencyList.Add(adjacencyListEntry.Key, valueList);
            }

            return new Graph(points, adjacencyList);
        }

        public bool Contains(Point point)
        {
            return Points.Contains(point);
        }

        public List<string> ToStringArray()
        {
            List<string> result = new List<string>();

            foreach (Point point in Points)
                result.Add(point.Coordinates);

            return result;
        }

        public List<Point> getAllContainedPoints(int dimension, int minMax)
        {
            List<Point> result = new List<Point>();
            foreach (Point thisPoint in Points)
            {
                if (Convert.ToInt32(thisPoint.Coordinates[dimension].ToString()) == ((minMax == 0) ? 0 : Globals.k))
                    result.Add(thisPoint.Clone());
            }
            return result;
        }




        /*


        //todo -- test
        //constructor taking in adj list in integer format where key references point in list and value references neighbouring points. (1-based indexing assumed).
        public Graph(List<Point> pts, Dictionary<int, List<int>> aL)
        {
            Points = new List<Point>();
            AdjacencyList = new Dictionary<string, List<string>>();

            foreach (Point p in pts)
                Points.Add(p);

            foreach (KeyValuePair<int, List<int>> entry in aL)
            {
                List<string> temp = new List<string>();

                foreach (int i in entry.Value)
                {
                    temp.Add(Points[i - 1].Coordinates);
                }

                AdjacencyList.Add(Points[entry.Key - 1].ToString(), temp);
            }
        }



        //todo -- check needs to be updated.


        public void addDimensionality(int dim, bool fZero)
        {
            //add dimension to all points
            foreach (Point p in Points)
            {
                p.increaseDimensionality(dim, fZero);
            }

            //add dimension to all entries in adjList
            Dictionary<string, List<string>> newAL = new Dictionary<string, List<string>>();
            foreach (KeyValuePair<string, List<string>> entry in AdjacencyList)
            {
                string tempKey = Point.increaseDimensionality(entry.Key, dim, fZero);
                List<string> tempVal = new List<string>();

                foreach (string s in entry.Value)
                {
                    tempVal.Add(Point.increaseDimensionality(s, dim, fZero));
                }

                newAL.Add(tempKey, tempVal);
            }

            AdjacencyList = newAL;
        }

        public int diameter()
        {
            int max = 0;
            int curr;

            foreach (Point p in Points)
            {
                curr = ShortestPath.BFS(this, p);
                if (curr > max)
                    max = curr;
            }

            return max;
        }

        */
    }
}
