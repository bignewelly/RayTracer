using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using RayTracer2.Geometry;

namespace RayTracer2
{
    public class Camera
    {
        private const double WidthToHeightRatio = 1;

        public Camera(Point Location, Vector Lookat, Vector Up, double Width, double Height, int xPixels, int yPixels)
        {
            this.Location = Location;
            this.LookAt = Geometry.Geometry.Normalize(Lookat);
            this.Up = Geometry.Geometry.Normalize(Up);
            this.Height = Height;
            this.Width = Width * WidthToHeightRatio;
            this.xPixels = xPixels;
            this.yPixels = yPixels;
        }

        public Color[,] render(Scene scene, int recursionDepth = 4)
        {
            Color[,] result = new Color[xPixels, yPixels];
            Point FilmCenter = GetFilmCenter();
            Vector Horizontal = Geometry.Geometry.Cross(Up, LookAt);

            Vector PerPixelU = Up * (Height / yPixels);
            Vector PerPixelV = Horizontal * (Width / xPixels);

            Point TopLeft = FilmCenter + ((PerPixelU * (yPixels / 2)) - (PerPixelV * (xPixels / 2)));

            Parallel.For(0, yPixels, j =>
            {
                Parallel.For(0, xPixels, i => 
                {
                    // find point on film
                    Point filmPoint = TopLeft + ((PerPixelV * (double)i) - (PerPixelU * (double)j));
                    Ray r = new Ray(Location, filmPoint - Location);

                    result[i, j] = scene.Intersect(r, recursionDepth);
                }); //Parallel for inner loop
            }); //Parallel for

            return result;
        }

        private Point GetFilmCenter()
        {
            return Location + LookAt;
        }

        public Point Location;
        public Vector LookAt;
        public Vector Up;

        public double Height, Width;
        public int xPixels, yPixels;

    }
}
