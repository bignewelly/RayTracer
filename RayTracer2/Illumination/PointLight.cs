using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RayTracer2.Geometry;

namespace RayTracer2.Illumination
{
    public class PointLight : ILight
    {
        public PointLight(Point p, Color c)
        {
            this.Position = p;
            this.Color = c;
        }

        public Color GetColor()
        {
            return Color;
        }

        public Point GetPosition()
        {
            return Position;
        }


        public Point Position;
        public Color Color;
    }
}
