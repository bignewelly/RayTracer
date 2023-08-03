using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer2.Geometry
{
    public class Mesh : BaseShape
    {
        private BVH.BoundingVolumeHiarchy Triangles;
        private Vector OffsetFromOrigin;

        public Mesh(List<Triangle> Triangles, Material Material, Vector Offset = null) : this(Triangles.ToArray(), Material, Offset)
        {
            //do nothing;
        }

        public Mesh(Triangle[] Triangles, Material Material, Vector Offset = null) : base(Material)
        {
            this.Triangles = new BVH.BoundingVolumeHiarchy(Triangles);
            if (Offset != null)
            {
                this.OffsetFromOrigin = Offset;
            }
            else
            {
                this.OffsetFromOrigin = new Vector(0, 0, 0);
            }
        }

        public override BoundingBox getBoundingBox()
        {
            return Triangles.getBoundingBox();
        }

        public override Vector getNormal(Point Position)
        {
            return getNormalInternal(TransformToObjectCoordinates(Position));
        }

        public override double getIntersectTime(Ray r)
        {
            return getIntersectTimeInternal(TransformToObjectCoordinates(r));
        }

        public Triangle getIntersectedTriangle(Ray r)
        {
            return getIntersectedTriangleInternal(TransformToObjectCoordinates(r));
        }

        private Ray TransformToObjectCoordinates(Ray r)
        {
            return new Ray(TransformToObjectCoordinates(r.Origin), r.Direction);
        }

        private Ray TransformToWorldCoordinates(Ray r)
        {
            return new Ray(TransformToWorldCoordinates(r.Origin), r.Direction);
        }

        private Triangle TransformToWorldCoordinates(Triangle t)
        {
            if (t != null)
            {
                Material newMaterial = t.Material;
                if (newMaterial == null)
                {
                    newMaterial = this.Material;
                }

                return new Triangle(TransformToWorldCoordinates(t.Vertex1), TransformToWorldCoordinates(t.Vertex2), TransformToWorldCoordinates(t.Vertex3), newMaterial);
            }

            return t;
        }

        private BoundingBox TransformToWorldCoordinates(BoundingBox b)
        {
            if (b != null)
            {
                return new BoundingBox(TransformToWorldCoordinates(b.x, Axis.x), TransformToWorldCoordinates(b.y, Axis.y), TransformToWorldCoordinates(b.z, Axis.z));
            }

            return null;
        }

        private Extent TransformToWorldCoordinates(Extent e, Axis a)
        {
            return new Extent(e.Min + OffsetFromOrigin[a], e.Max + OffsetFromOrigin[a]);
        }

        private Point TransformToObjectCoordinates(Point p)
        {
            return p - OffsetFromOrigin;
        }

        private Point TransformToWorldCoordinates(Point p)
        {
            return p + OffsetFromOrigin;
        }

        private Vector getNormalInternal(Point Position)
        {
            Vector Direction = new Point(0, 0, 0) - Position;
            Ray intersectRay = new Ray(Position - Direction, Direction);

            Triangle tri = getIntersectedTriangleInternal(intersectRay);

            if (tri != null)
            {
                return tri.getNormal(Position);
            }

            return null;
        }

        private double getIntersectTimeInternal(Ray r)
        {
            Triangle tri = getIntersectedTriangleInternal(r);

            if (tri != null)
            {
                return tri.getIntersectTime(r);
            }
            return -1;
        }

        private Triangle getIntersectedTriangleInternal(Ray r)
        {
            Triangle tempTriangle = null;

            tempTriangle = (Triangle) Triangles.getIntersectedShape(r);

            return tempTriangle;
        }

        public override IShape getIntersectedShape(Ray r)
        {
            return TransformToWorldCoordinates(getIntersectedTriangle(r));
        }
    }
}
