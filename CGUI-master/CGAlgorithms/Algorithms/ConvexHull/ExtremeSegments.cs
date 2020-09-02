using CGUtilities;
using System.Collections.Generic;
using System.Linq;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            for (int i = 0; i < points.Count; ++i)
            {
                for (int j = i + 1; j < points.Count; ++j)
                {
                    if (points[i].X == points[j].X && points[i].Y == points[j].Y)
                    {
                        points.RemoveAt(j);
                        --j;
                    }
                }
            }
            if (points.Count <= 2)
            {
                outPoints = points;
                return;
            }

            int x = 0;
            for (int i = 0; i < points.Count; ++i)
            {
                for (int j = i + 1; j < points.Count; ++j)
                {
                    int r_dir = 0, l_dir = 0, cl = 0;
                    for (int p = 0; p < points.Count; ++p)
                    {
                        if (i == p || j == p) continue;


                        if (HelperMethods.CheckTurn(new Line(points[i], points[j]), points[p]) == Enums.TurnType.Right)
                            ++r_dir;

                        if (HelperMethods.CheckTurn(new Line(points[i], points[j]), points[p]) == Enums.TurnType.Left)
                            ++l_dir;
                        if (HelperMethods.CheckTurn(new Line(points[i], points[j]), points[p]) ==
                            Enums.TurnType.Colinear)
                            cl++;
                    }


                    if ((l_dir + cl == points.Count - 2 || r_dir + cl == points.Count - 2))
                    {
                        outLines.Add(new Line(points[i], points[j]));
                        if (!outPoints.Contains(points[i])) outPoints.Add(points[i]);
                        if (!outPoints.Contains(points[j])) outPoints.Add(points[j]);
                    }
                }
            }





            ////////////////////////////////

            List<Point> temp = new List<Point>();


            temp.Clear();

            for (int i = 0; i < outPoints.Count; ++i)
            {
                temp.Add(outPoints[i]);
            }
            outPoints.Clear();
            for (int i = 0; i < temp.Count; ++i)
            {
                List<Point> dia = new List<Point>();
                dia.Add(temp[i]);
                for (int j = 0; j < temp.Count; ++j)
                {
                    if (i != j && (temp[i].X - temp[j].X) == (temp[i].Y - temp[j].Y))
                    {
                        dia.Add(temp[j]);
                    }

                }

                dia = dia.OrderBy(z => z.X).ThenBy(z => z.Y).ToList();
                if (!outPoints.Contains(dia[0])) outPoints.Add(dia[0]);
                if (dia.Count > 1 && !outPoints.Contains(dia[dia.Count - 1]))
                    outPoints.Add(dia[dia.Count - 1]);
                for (int k = 1; k < dia.Count - 1; ++k)
                {
                    int l = outPoints.IndexOf(dia[k]);
                    if (l != -1)
                    {
                        outPoints.RemoveAt(l);
                    }

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
                List<Point> dia = new List<Point>();
                dia.Add(temp[i]);
                for (int j = 0; j < temp.Count; ++j)
                {
                    if (i != j && temp[i].X == temp[j].X)
                    {
                        dia.Add(temp[j]);
                    }

                }

                dia = dia.OrderBy(z => z.Y).ToList();
                if (!outPoints.Contains(dia[0])) outPoints.Add(dia[0]);
                if (dia.Count > 1 && !outPoints.Contains(dia[dia.Count - 1]))
                    outPoints.Add(dia[dia.Count - 1]);

                for (int k = 1; k < dia.Count - 1; ++k)
                {
                    int l = outPoints.FindIndex(a => a == dia[k]);
                    if (l != -1)
                    {
                        outPoints.RemoveAt(l);
                    }

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
                List<Point> dia = new List<Point>();
                dia.Add(temp[i]);
                for (int j = 0; j < temp.Count; ++j)
                {
                    if (i != j && temp[i].Y == temp[j].Y)
                    {
                        dia.Add(temp[j]);
                    }

                }

                dia = dia.OrderBy(z => z.X).ToList();
                if (!outPoints.Contains(dia[0])) outPoints.Add(dia[0]);
                if (dia.Count > 1 && !outPoints.Contains(dia[dia.Count - 1]))
                    outPoints.Add(dia[dia.Count - 1]);

                for (int k = 1; k < dia.Count - 1; ++k)
                {
                    int l = outPoints.IndexOf(dia[k]);
                    if (l != -1)
                    {
                        outPoints.RemoveAt(l);
                    }

                }


            }

        }
        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
