using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer2.Geometry
{
    public class Triangle : BaseShape
    {
        public Point Vertex1, Vertex2, Vertex3;

        public Triangle(Point p1, Point p2, Point p3, Material m = null) : base (m)
        {
            Vertex1 = p1;
            Vertex2 = p2;
            Vertex3 = p3;
        }

        public override BoundingBox getBoundingBox()
        {
            Extent extX = new Extent(Math.Min(Vertex1.x, Math.Min(Vertex2.x, Vertex3.x)), Math.Max(Vertex1.x, Math.Max(Vertex2.x, Vertex3.x)));
            Extent extY = new Extent(Math.Min(Vertex1.y, Math.Min(Vertex2.y, Vertex3.y)), Math.Max(Vertex1.y, Math.Max(Vertex2.y, Vertex3.y)));
            Extent extZ = new Extent(Math.Min(Vertex1.z, Math.Min(Vertex2.z, Vertex3.z)), Math.Max(Vertex1.z, Math.Max(Vertex2.z, Vertex3.z)));

            return new BoundingBox(extX, extY, extZ);
        }

        public override Vector getNormal(Point Position)
        {
            return Geometry.Normalize(Geometry.Cross(Vertex1 - Vertex2, Vertex1 - Vertex3));
        }

        public override double getIntersectTime(Ray r)
        {

            // Let's find out if this ray intersects the plane that our triangle's on
            Vector normal = getNormal(Vertex1);
            Plane triPlane = new Plane(Vertex1, normal, getMaterial(Vertex1));

            // If it doesn't intersect our plane then go no further
            double planeIntersect = triPlane.getIntersectTime(r);
            if (planeIntersect < 0) return -1;

            // Get our intersect point
            Point IntersectPoint = Geometry.GetPositionOnRay(r, planeIntersect);

            // Let's test and see if this ray is within the bounds of our triangle
            // Get our edges
            Vector edge1 = Vertex2 - Vertex1;
            Vector edge2 = Vertex3 - Vertex2;
            Vector edge3 = Vertex1 - Vertex3;

            // get the lines from the point to the vertices
            Vector pToV1 = IntersectPoint - Vertex1;
            Vector pToV2 = IntersectPoint - Vertex2;
            Vector pToV3 = IntersectPoint - Vertex3;

            // Let's test the cross products of the edges and the new lines.  If it's less than 0 it's outside of the triangle.
            if ((Geometry.Dot(normal, Geometry.Cross(edge1, pToV1)) >= 0 &&
                Geometry.Dot(normal, Geometry.Cross(edge2, pToV2)) >= 0 &&
                Geometry.Dot(normal, Geometry.Cross(edge3, pToV3)) >= 0) ||
                Geometry.IsBetween(Vertex1, Vertex2, IntersectPoint) ||
                Geometry.IsBetween(Vertex2, Vertex3, IntersectPoint) ||
                Geometry.IsBetween(Vertex3, Vertex1, IntersectPoint)
                )
            {
                // If it's within the triangle, return the plane interesct.
                return planeIntersect;
            }

            return -1;
        }

        public override bool Equals(object Obj)
        {
            Triangle testTri;
            try
            {
                testTri = (Triangle)Obj;
            }
            catch
            {
                return false;
            }

            return this.Vertex1 == testTri.Vertex1 && this.Vertex2 == testTri.Vertex2 && this.Vertex3 == testTri.Vertex3;

        }

        public static bool operator ==(Triangle a, Triangle b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            } else if (object.ReferenceEquals(b, null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Triangle a, Triangle b)
        {
            return !(a == b);
        }
    }
}
