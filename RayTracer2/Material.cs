using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer2
{
    public enum SurfaceType
    {
        Reflective,
        Refractive,
        Diffuse
    }

    public struct RefractionIndeces
    {
        public const double Vacuum = 1;
        public const double Air = 1.000293;
        public const double Water = 1.330;
        public const double Glass = 1.52;
    }

    public class Material
    {

        public Material(Color color = null, SurfaceType type = SurfaceType.Diffuse, double RefractiveIndex = 1)
        {
            this.color = color;
            this.surfaceType = type;
            this.RefractiveIndex = RefractiveIndex;
        }
        public Color color;
        public SurfaceType surfaceType;
        public double RefractiveIndex;
    }

    public class Color
    {
        public Color(double r = 0, double g = 0, double b = 0)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        public double r, g, b;

        public static bool operator ==(Color a, Color b)
        {
            return a.r == b.r && a.g == b.g && a.b == b.b;
        }

        public static bool operator !=(Color a, Color b)
        {
            return !(a == b);
        }

        public static Color operator +(Color a, Color b)
        {
            return new Color(Math.Min(a.r + b.r, 1), Math.Min(a.g + b.g, 1), Math.Min(a.b + b.b, 1));
        }

        public static Color operator -(Color a, Color b)
        {
            return new Color(Math.Max(a.r - b.r, 0), Math.Max(a.g - b.g, 0), Math.Max(a.b - b.b, 0));
        }

        public static Color operator *(Color a, Color b)
        {
            return new Color(a.r * b.r, a.g * b.g, a.b * b.b);
        }

        public static Color operator *(Color a, double b)
        {
            return new Color(a.r * b, a.g * b, a.b * b);
        }
    }
}
