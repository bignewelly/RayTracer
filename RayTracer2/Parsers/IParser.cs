using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RayTracer2.Geometry;

namespace RayTracer2.Parsers
{
    public interface IParser
    {
        IShape Parse(String FileName);
    }
}
