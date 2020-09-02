using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        public static double CalcAngle(Point p1, Point p2, Point p3)
        {
            double angle = 0.0;

            Line l1 = new Line(p1, p2);
            Line l2 = new Line(p1, p3);

            Point a = HelperMethods.GetVector(l1);
            Point b = HelperMethods.GetVector(l2);
            double dotProduct = (a.X * b.X + a.Y * b.Y);
            double crossProduct = HelperMethods.CrossProduct(a, b);

            angle = Math.Atan2(dotProduct, crossProduct) * 180 / Math.PI;

            return angle;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            if (points.Count == 1)
            {
                outPoints.Add(points[0]);
            }
            else
            {

                double minY = 100000;
                double minX = -1000;
                int index = -1;
                //bageeb el min y with max x
                for (int i = 0; i < points.Count; i++)
                {
                    if ((points[i].Y < minY) || (points[i].Y == minY && points[i].X >= minX))
                    {
                        index = i;
                        minY = points[i].Y;
                        minX = points[i].X;
                    }
                }
                
                Point minPoint = points[index];
                double x = minPoint.X + 100000000;
                double y = minPoint.Y;
                Point virtualPoint = new Point(x, y);
                points.RemoveAt(index);
                List<KeyValuePair<double, Point>> sortedAngles = new List<KeyValuePair<double, Point>>();
                for (int i = 0; i < points.Count; i++)
                {

                    double angle = CalcAngle(minPoint, virtualPoint, points[i]);

                    sortedAngles.Add(new KeyValuePair<double, Point>(angle, points[i]));
                }

                sortedAngles.Sort((s, ss) => s.Key.CompareTo(ss.Key));
                Stack<Point> myStack = new Stack<Point>();
                myStack.Push(minPoint);
                myStack.Push(sortedAngles[0].Value);

                for (int i = 1; i < points.Count; i++)
                {
                    Point p1 = myStack.Pop();
                    Point p2 = myStack.Pop();
                    myStack.Push(p2);
                    myStack.Push(p1);
                    Line l = new Line(p1, p2);
                    Enums.TurnType t = HelperMethods.CheckTurn(l, sortedAngles[i].Value);

                    while (myStack.Count > 2 && t != Enums.TurnType.Left)
                    {
                        myStack.Pop();

                        p1 = myStack.Pop();
                        p2 = myStack.Pop();

                        myStack.Push(p2);
                        myStack.Push(p1);
                        l = new Line(p1, p2);
                        t = HelperMethods.CheckTurn(l, sortedAngles[i].Value);

                    }
                    
                    myStack.Push(sortedAngles[i].Value);
                }


                while (myStack.Count != 0)
                {
                    outPoints.Add(myStack.Pop());
                }
            }

            // special cases
            /////////////////////////////
            List<Point> temp = new List<Point>();
            temp.Clear();
            for (int i = 0; i < outPoints.Count; ++i)
            {
                temp.Add(outPoints[i]);
            }
            outPoints.Clear();
            for (int i = 0; i < temp.Count; ++i)
            {
                List<Point> diagonal = new List<Point>();
                diagonal.Add(temp[i]);
                for (int j = 0; j < temp.Count; ++j)
                {
                    if ((i != j) && (temp[i].X - temp[j].X == temp[i].Y - temp[j].Y))
                    {
                        diagonal.Add(temp[j]);
                    }

                }
                // keda ma3aya kol el no2at el 3ala nfs el 5at
                // 3amalna sort bel x wel y
                diagonal = diagonal.OrderBy(x => x.X).ThenBy(x => x.Y).ToList();
                // 3ashan lw el point mtkarara maslan 3ala kaza diagonal
                if (!outPoints.Contains(diagonal[0]))
                {
                    outPoints.Add(diagonal[0]);
                }
                // lazem nt2aked size el diagonal akbar men 1 3ashan mn3melsh access le 7aga out of range
                if (diagonal.Count > 1 && !outPoints.Contains(diagonal[diagonal.Count - 1]))
                {
                    outPoints.Add(diagonal[diagonal.Count - 1]);
                }
                    

            }
            ///////////////////////////////

            temp.Clear();
            for (int i = 0; i < outPoints.Count; ++i)
            {
                temp.Add(outPoints[i]);
            }
            outPoints.Clear();
            for (int i = 0; i < temp.Count; ++i)
            {
                List<Point> verticalLine = new List<Point>();
                verticalLine.Add(temp[i]);
                for (int j = 0; j < temp.Count; ++j)
                {
                    if (i != j && temp[i].X == temp[j].X)
                    {
                        verticalLine.Add(temp[j]);
                    }

                }

                verticalLine = verticalLine.OrderBy(x => x.Y).ToList();
                if (!outPoints.Contains(verticalLine[0]))
                {
                    outPoints.Add(verticalLine[0]);
                }
                if (verticalLine.Count > 1 && !outPoints.Contains(verticalLine[verticalLine.Count - 1]))
                {
                    outPoints.Add(verticalLine[verticalLine.Count - 1]);
                }

            }

            ////////////////////////////////////////////////////////////
            temp.Clear();

            for (int i = 0; i < outPoints.Count; ++i)
            {
                temp.Add(outPoints[i]);
            }
            outPoints.Clear();
            for (int i = 0; i < temp.Count; ++i)
            {
                List<Point> horizontalLine = new List<Point>();
                horizontalLine.Add(temp[i]);
                for (int j = 0; j < temp.Count; ++j)
                {
                    if (i != j && temp[i].Y == temp[j].Y)
                    {
                        horizontalLine.Add(temp[j]);
                    }

                }

                horizontalLine = horizontalLine.OrderBy(x => x.X).ToList();
                if (!outPoints.Contains(horizontalLine[0]))
                {
                    outPoints.Add(horizontalLine[0]);
                }
                if (horizontalLine.Count > 1 && !outPoints.Contains(horizontalLine[horizontalLine.Count - 1]))
                {
                    outPoints.Add(horizontalLine[horizontalLine.Count - 1]);
                }
                    

            }



        }
        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
