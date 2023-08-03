using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer2.Geometry
{
    public class Sphere : BaseShape
    {
        Point _CenterPoint;
        double _Radius;

        public Sphere(Point c, double r, Material Material) : base(Material)
        {
            _CenterPoint = c;
            _Radius = r;
        }

        public override BoundingBox getBoundingBox()
        {
            Extent extX = new Extent(_CenterPoint.x - _Radius, _CenterPoint.x + _Radius);
            Extent extY = new Extent(_CenterPoint.y - _Radius, _CenterPoint.y + _Radius);
            Extent extZ = new Extent(_CenterPoint.z - _Radius, _CenterPoint.z + _Radius);

            return new BoundingBox(extX, extY, extZ);

        }

        public Point CenterPoint
        {
            get
            {
                return _CenterPoint;
            }
        }

        public Double Radius
        {
            get
            {
                return _Radius;
            }
        }

        public override Vector getNormal(Point Position)
        {
            return Geometry.Normalize(Position - _CenterPoint);
        }

        public override double getIntersectTime(Ray r)
        {
            Vector offset = _CenterPoint - r.Origin;
            double offsetDotDir = Geometry.Dot(offset, r.Direction);
            if (offsetDotDir < 0) return -1;

            double dot2 = Geometry.Dot(offset, offset) - offsetDotDir * offsetDotDir;

            if (dot2 > _Radius) return -1;

            double thc = Math.Sqrt(_Radius - dot2);

            if (thc <= 0) return -1;

            double time1 = offsetDotDir - thc;
            double time2 = offsetDotDir + thc;

            if (time1 > time2)
            {
                double temp = time1;
                time1 = time2;
                time2 = temp;
            }

            if (time1 < 0)
            {
                time1 = time2;
            }

            return time1;
        }

        //protected override double getIntersectTime(Ray r)
        //{
        //    Vector offset = Geometry.Normalize(r.Origin - CenterPoint);
        //    double offsetDotDir = Geometry.Dot(offset, r.Direction);
        //    double c = Geometry.Dot(offset, offset) - Radius * Radius;

        //    if (c > 0 && offsetDotDir > 0) return -1;
        //    double discrimenant = offsetDotDir * offsetDotDir - c;

        //    if (discrimenant < 0) return -1;

        //    double discRoot = (double)Math.Sqrt(discrimenant);

        //    double time1 = offsetDotDir - discRoot;
        //    double time2 = offsetDotDir + discRoot;

        //    if (time1 > time2)
        //    {
        //        double temp = time1;
        //        time1 = time2;
        //        time2 = temp;
        //    }

        //    if (time1 < 0)
        //    {
        //        time1 = time2;
        //    }

        //    return time1;
        //}
    }
}
