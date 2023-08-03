using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer2.Geometry
{
    public abstract class BaseShape : IShape
    {

        public BaseShape(Material Material)
        {
            this._Material = Material;
        }

        abstract public BoundingBox getBoundingBox();

        public virtual Material getMaterial(Point Position)
        {
            return _Material;
        }

        public abstract Vector getNormal(Point Position);

        public virtual HitRecord Hit(Ray r)
        {
            if (!Geometry.Intersect(r, getBoundingBox())) return null;

            HitRecord record = null;
            double Time = getIntersectTime(r);

            if (Time > 0)
            {
                record = new HitRecord();
                record.Time = Time;
                Point position = Geometry.GetPositionOnRay(r,Time);
                record.Normal = getNormal(position);

                record.Material = getMaterial(position);

                if (record.Normal == null)
                {
                    record.Normal = getNormal(position);

                    record = null;
                }
            }
            return record;
        }

        public virtual bool IsHit(Ray r)
        {
            if (!Geometry.Intersect(r, getBoundingBox())) return false;
            return IsIntersected(r);
        }

        protected virtual bool IsIntersected(Ray r)
        {
            return (getIntersectTime(r) > 0);
        }

        public abstract double getIntersectTime(Ray r);

        public virtual IShape getIntersectedShape(Ray r)
        {
            if (this.IsHit(r))
            {
                return this;
            }

            return null;
        }

        public Material Material
        {
            get
            {
                return _Material;
            }
        }

        private Material _Material;
    }
}
