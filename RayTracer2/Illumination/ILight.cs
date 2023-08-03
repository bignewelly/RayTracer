using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RayTracer2.Geometry;

namespace RayTracer2.Illumination
{
    public interface ILight
    {
        Color GetColor();
        Point GetPosition();
    }
}
