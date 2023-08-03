using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer2.Geometry
{
    public enum Axis
    {
        x = 0,
        y = 1,
        z = 2
    }

    public class Geometry
    {

        public static double Dot(GeometricValueBase a, GeometricValueBase b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static Vector Cross(Vector a, Vector b)
        {
            return new Vector(a.y * b.z - a.z * b.y,
                                        a.z * b.x - a.x * b.z,
                                        a.x * b.y - a.y * b.x);
        }

        public static double Distance(Point a, Point b)
        {
            double xdiff = (b.x - a.x);
            double ydiff = (b.y - a.y);
            double zdiff = (b.z - a.z);

            return (double)Math.Sqrt(xdiff * xdiff + ydiff * ydiff + zdiff * zdiff);
        }

        public static Vector Normalize(Vector a)
        {
            double distance = (double)Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
            return new Vector(a.x / distance, a.y /distance, a.z / distance);
        }

        public static Extent Union(Extent a, Extent b)
        {
            return new Extent(Math.Min(a.Min, b.Min), Math.Max(a.Max, b.Max));
        }

        public static Extent Union(Extent a, Double b)
        {
            return new Extent(Math.Min(a.Min, b), Math.Max(a.Max, b));
        }

        public static Point GetPositionOnRay(Ray Ray, double Time)
        {
            return Ray.Origin + (Ray.Direction * Time);
        }


        public static BoundingBox Union(BoundingBox a, BoundingBox b)
        {
            if (a != null && b != null)
            {
                return new BoundingBox(Union(a.x, b.x), Union(a.y, b.y), Union(a.z, b.z));
            } else if (a == null)
            {
                return b;
            } else
            {
                return a;
            }
        }

        public static BoundingBox Union(BoundingBox a, Point b)
        {
            if (b != null)
            {
                return (Union(a, new BoundingBox(b)));
            } else
            {
                return a;
            }
        }

        public static Boolean PointInBoundBox(Point P, BoundingBox BBox)
        {
            return BBox.x.Min <= P.x && P.x <= BBox.x.Max 
                && BBox.y.Min <= P.y && P.y <= BBox.y.Max 
                && BBox.z.Min <= P.z && P.z <= BBox.z.Max;
        }

        public static Boolean IsBetween(Point StartPoint, Point EndPoint, Point TestPoint)
        {
            double segmentDist = Distance(StartPoint, EndPoint);
            double stDist = Distance(StartPoint, TestPoint);
            double teDist = Distance(TestPoint, EndPoint);

            if (segmentDist == stDist + teDist)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public static bool Intersect(Ray r, BoundingBox b)
        {
            double tMin = 0;
            double tMax = 0;

            // Test for intersections with the x axis
            if (r.Direction.x >= 0)
            {
                tMin = (b.x.Min - r.Origin.x) * r.DirectionInverse.x;
                tMax = (b.x.Max - r.Origin.x) * r.DirectionInverse.x;
            } else
            {
                tMin = (b.x.Max - r.Origin.x) * r.DirectionInverse.x;
                tMax = (b.x.Min - r.Origin.x) * r.DirectionInverse.x;
            }

            double yMin = 0;
            double yMax = 0;

            // Test for intersections with the y axis
            if (r.Direction.y >= 0)
            {
                yMin = (b.y.Min - r.Origin.y) * r.DirectionInverse.y;
                yMax = (b.y.Max - r.Origin.y) * r.DirectionInverse.y;
            }
            else
            {
                yMin = (b.y.Max - r.Origin.y) * r.DirectionInverse.y;
                yMax = (b.y.Min - r.Origin.y) * r.DirectionInverse.y;
            }

            if ((tMin > yMax) || (yMin > tMax)) return false;

            if (yMin > tMin) tMin = yMin;
            if (yMax < tMax) tMax = yMax;

            double zMin = 0;
            double zMax = 0;

            // Test for intersections with the y axis
            if (r.Direction.z >= 0)
            {
                zMin = (b.z.Min - r.Origin.z) * r.DirectionInverse.z;
                zMax = (b.z.Max - r.Origin.z) * r.DirectionInverse.z;
            }
            else
            {
                zMin = (b.z.Max - r.Origin.z) * r.DirectionInverse.z;
                zMax = (b.z.Min - r.Origin.z) * r.DirectionInverse.z;
            }

            if ((tMin > zMax) || (zMin > tMax)) return false;


            return true;
        }

    }

    public struct Extent
    {
        public Extent(double Min = double.MaxValue, double Max = double.MinValue)
        {
            this.Min = Min;
            this.Max = Max;
            this.Center = 0.5 * (Min + Max);
        }
        public double Center;
        public double Min, Max;
    }

    public class BoundingBox
    {
        public BoundingBox(Extent x, Extent y, Extent z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public BoundingBox(Point p) : this(new Extent(p.x, p.x), new Extent(p.y, p.y), new Extent(p.z, p.z))
        { }

        public Extent this[Axis a]
        {
            get
            {
                switch (a)
                {
                    case Axis.x:
                        return x;
                    case Axis.y:
                        return y;
                    case Axis.z:
                        return z;
                }

                return x;
            }
            set
            {
                switch (a)
                {
                    case Axis.x:
                        x = value;
                        break;
                    case Axis.y:
                        y = value;
                        break;
                    case Axis.z:
                        z = value;
                        break;
                }

            }
        }

        public Vector GetDiagnal()
        {
            return GetMax() - GetMin();
        }

        public double GetSurfaceArea()
        {
            Vector diag = GetDiagnal();

            return 2 * (diag.x * diag.y + diag.y * diag.z + diag.x * diag.z);
        }

        public Vector Offset(Point p)
        {
            Point Min = GetMin();
            Point Max = GetMax();
            Vector o = p - GetMin();
            Vector diff = Max - Min;

            if (x.Max > x.Min) o.x /= diff.x;
            if (y.Max > y.Min) o.y /= diff.y;
            if (z.Max > z.Min) o.z /= diff.z;

            return o;
        }

        public Axis GetMaxextent()
        {
            double diffx = x.Max - x.Min;
            double diffy = y.Max - y.Min;
            double diffz = z.Max - z.Min;

            if (diffx > diffy)
            {
                if (diffx > diffz)
                {
                    return Axis.x;
                } 
            } else
            {
                if (diffy > diffz)
                {
                    return Axis.y;
                }
            }

            return Axis.z;
        }

        public Point GetMin()
        {
            return new Point(x.Min, y.Min, z.Min);
        }

        public Point GetMax()
        {
            return new Point(x.Max, y.Max, z.Max);
        }

        public Extent x, y, z;
        public Point Centroid
        {
            get
            {
                return new Point(x.Center, y.Center, z.Center);
            }
        }
    }

    public class GeometricValueBase
    {
        public GeometricValueBase(double xVal, double yVal, double zVal)
        {
            x = xVal;
            y = yVal;
            z = zVal;
        }

        public double x, y, z;

        public double this[Axis a]
        {
            get
            {
                switch (a)
                {
                    case Axis.x:
                        return x;
                    case Axis.y:
                        return y;
                    case Axis.z:
                        return z;
                }

                return x;
            }
            set
            {
                switch (a)
                {
                    case Axis.x:
                        x = value;
                        break;
                    case Axis.y:
                        y = value;
                        break;
                    case Axis.z:
                        z = value;
                        break;
                }

            }
        }


        public override bool Equals(object Obj)
        {
            GeometricValueBase testVal;
            try
            {
                testVal = (GeometricValueBase)Obj;
            }
            catch
            {
                return false;
            }

            return this.GetType() == testVal.GetType() && this.x == testVal.x && this.y == testVal.y && this.z == testVal.z;

        }

        public static bool operator ==(GeometricValueBase a, GeometricValueBase b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }
            else if (object.ReferenceEquals(b, null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(GeometricValueBase a, GeometricValueBase b)
        {
            return !(a == b);
        }
    }

    public class Point : GeometricValueBase
    {
        public Point (double xVal, double yVal, double zVal) : base(xVal, yVal, zVal)
        {
            //Do Nothing
        }

        public static Point operator +(Point a, Vector b)
        {
            return new Point(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Point operator -(Point a, Vector b)
        {
            return new Point(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector operator -(Point a, Point b)
        {
            return new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
        }


        public static Point operator *(Point a, double b)
        {
            return new Point(a.x * b, a.y * b, a.z * b);
        }

        public static Point operator /(Point a, double b)
        {

            return new Point(a.x / b, a.y / b, a.z / b);
        }

    }

    public class Vector : GeometricValueBase
    {
        public Vector (double xVal, double yVal, double zVal) : base(xVal, yVal, zVal)
        {
            //Do Nothing
        }

        public static Vector operator /(Vector a, double b)
        {

            return new Vector(a.x / b, a.y / b, a.z / b);
        }

        public static Vector operator *(Vector a, double b)
        {
            return new Vector(a.x * b, a.y * b, a.z * b);
        }

        public static Vector operator *(double a, Vector b)
        {
            return b * a;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
        }
    }

    public class Ray
    {
        public Ray(Point o, Vector d, double RefractionIndex = 1)
        {
            this.Origin = o;
            this.Direction = Geometry.Normalize(d);
            this.DirectionInverse = new Vector(1 / this.Direction.x, 1 / this.Direction.y, 1 / this.Direction.z);
            this.CurrentRefractionIndex = RefractionIndex;
        }
        public Point Origin;
        public Vector Direction;
        public Vector DirectionInverse;
        public double CurrentRefractionIndex;
    }

}
