using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer2.Geometry
{
    public interface IShape
    {
        BoundingBox getBoundingBox();
        Vector getNormal(Point Position);
        Material getMaterial(Point Position);
        bool IsHit(Ray r);
        HitRecord Hit(Ray r);
        double getIntersectTime(Ray r);
        IShape getIntersectedShape(Ray r);
    }
}
