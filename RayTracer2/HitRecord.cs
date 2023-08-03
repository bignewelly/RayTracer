using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer2.Geometry;

namespace RayTracer2
{
    public class HitRecord
    {
        public double Time;
        public Vector Normal;
        public Material Material;
    }
}
