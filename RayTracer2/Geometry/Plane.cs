using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer2.Geometry
{
    public class Plane : BaseShape
    {
        public Plane(Point O, Vector N, Material M) : base (M)
        {
            this.Origin = O;
            this.Normal = N;
        }

        public Point Origin;
        public Vector Normal;

        public override BoundingBox getBoundingBox()
        {
            return new BoundingBox(new Extent(double.MinValue, double.MaxValue), new Extent(double.MinValue, double.MaxValue), new Extent(double.MinValue, double.MaxValue));
        }

        public override Vector getNormal(Point Position)
        {
            return Normal;
        }

        public override double getIntersectTime(Ray r)
        {
            Point newOrig = r.Origin - (this.Origin - new Point(0,0,0));
            if (-Geometry.Dot(r.Direction, Normal) < 0.0000001f) return -1;
            return - (Geometry.Dot(newOrig, Normal) / Geometry.Dot(r.Direction, Normal));
        }
    }
}
