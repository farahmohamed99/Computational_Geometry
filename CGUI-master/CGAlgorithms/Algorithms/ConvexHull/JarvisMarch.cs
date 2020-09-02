using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public static double CalculateAngle(Point p1, Point p2, Point p3)
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

            if (points.Count <= 2)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    outPoints.Add(points[i]);
                }
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
                Point mnPoint = points[index];
                outPoints.Add(minPoint);
                double x1 = 100000000;
                double y1 = minPoint.Y;
                Point virtualPoint1 = new Point(x1, y1);
                List<KeyValuePair<double, Point>> sortedAngles1 = new List<KeyValuePair<double, Point>>();
                // bageeb el angles smallest angle
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i] != minPoint)
                    {
                        double angle = CalculateAngle(minPoint, virtualPoint1, points[i]);
                        sortedAngles1.Add(new KeyValuePair<double, Point>(angle, points[i]));

                    }
                }
                sortedAngles1.Sort((s, ss) => s.Key.CompareTo(ss.Key));
                outPoints.Add(sortedAngles1[0].Value);

                //bageeb el min point fe ba2et el list be ene ba5od a5er 2 fel list ykono wa7da  min we wa7da virtual
                // bageen el max angle ben el virtal wel point wel min 3ashan el min htkoon 3ala el sehal 5ales
                while (true)
                {
                    virtualPoint1 = outPoints[outPoints.Count - 1];
                    minPoint = outPoints[outPoints.Count - 2];
                    List<KeyValuePair<double, Point>> sortedAngles = new List<KeyValuePair<double, Point>>();
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (virtualPoint1 != points[i] || minPoint != points[i])
                        {
                            double angle = CalculateAngle(virtualPoint1, points[i], minPoint);
                            if (angle < 0)
                                angle += 360;
                            sortedAngles.Add(new KeyValuePair<double, Point>(angle, points[i]));
                        }
                    }
                    sortedAngles.Sort((s, ss) => s.Key.CompareTo(ss.Key));

                    //3ashan at2aked ene marg3tesh le awel point
                    if (sortedAngles[sortedAngles.Count - 1].Value != mnPoint)
                    {
                        outPoints.Add(sortedAngles[sortedAngles.Count - 1].Value);
                    }
                    else
                        break;
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
            return "Convex Hull - Jarvis March";
        }
    }
}