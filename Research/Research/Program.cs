using System;
using System.Collections.Generic;
using Research.Core;
using Research.DeterminingAd;

namespace Research
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Globals.d = 5;
			Globals.ShowMessages = true;

            //Tester2();
            DeterminingAd.Run runner = new Run();
            //runner.run();

            //Run runner = new Run();
            //runner.runWithCandidate(new Point2("1,3,5,6"));

            /*var testVar = CoreOperations.ReadRedVerticesFromFile();

            foreach(Tuple<int, List<Point2>> thisVar in testVar)
            {
                Console.WriteLine("Dimension: {0}", thisVar.Item1);

                foreach (Point2 p2 in thisVar.Item2)
                    Console.WriteLine(p2);
            }*/

            //List<Point2> thesePoints = runner.InitializeRedVertices(Globals.d);

            runner.run();

            //Point2 testPoint = new Point2("0,1,3,3");
            //Console.WriteLine(Point2.SplitVector(testPoint, new List<Point2>()));

            //display(thesePoints);
            //check to see if point2 equals works or not
        }

        public static void display(List<Point2> points)
        {
            foreach (Point2 p in points)
                Console.WriteLine(p);

            Console.WriteLine("---Total Points: {0}", points.Count);
        }

        public static void display(Dictionary<string, int> points)
        {
            foreach(KeyValuePair<string, int> entry in points)
            {
                Console.WriteLine(entry.Key);
            }
        }




        public static void Tester2()
        {
            DeterminingAd.Run thisTester = new DeterminingAd.Run();

            thisTester.run();
        }

        public static void Tester()
        {
            //init
            Globals.d = 3;
            Globals.k = (int)Math.Pow(2, Globals.d - 1);

            List<Point> A2 = new List<Point>();
            A2.Add(new Point("00"));
            A2.Add(new Point("01"));

            List<Point> redVertices = new List<Point>();
            redVertices.Add(new Point("000"));
            redVertices.Add(new Point("001"));
            redVertices.Add(new Point("012"));
            redVertices.Add(new Point("022"));
        }
    }
}
