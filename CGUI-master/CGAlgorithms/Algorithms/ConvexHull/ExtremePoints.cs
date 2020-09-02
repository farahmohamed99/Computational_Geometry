using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
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
            List<int> plist = new List<int>();
            for (int i = 0; i < points.Count; ++i)
            {
                for (int j = 0; j < points.Count; ++j)
                {
                    for (int k = 0; k < points.Count; ++k)
                    {
                        for (int l = 0; l < points.Count; ++l)
                        {
                            if (i != j && i != k && i != l && j != k && j != l && k != l && !plist.Contains(l))
                            {
                                if (HelperMethods.PointInTriangle(points[l], points[i], points[j], points[k]) == Enums.PointInPolygon.Inside || HelperMethods.PointInTriangle(points[l], points[i], points[j], points[k]) == Enums.PointInPolygon.OnEdge)
                                {
                                    plist.Add(l);
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < points.Count; ++i)
            {
                if (!plist.Contains(i))
                    outPoints.Add(points[i]);
            }
        }
        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}