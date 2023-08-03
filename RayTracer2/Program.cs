using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RayTracer2.Geometry;
using RayTracer2.Illumination;
using RayTracer2.Parsers;

namespace RayTracer2
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Creating scene...");
                //Scene scene = GetScene();
                //Scene scene = GetSceneSquare();
                //Scene scene = GetSceneCircle();
                Scene scene = GetSceneShperesRelectance();
                Console.WriteLine("Done Creating Scene.");

                Console.WriteLine("Creating camera...");
                Camera camera = GetCamera();
                Console.WriteLine("Done Creating Camera.");

                Console.WriteLine("Rendering Frame...");
                Color[,] results = camera.render(scene);

                Console.WriteLine("Done Rendering Frame.");

                Console.WriteLine("Writing rendered immage to file.");
                String File = @"D:\Dev\RayTracer\RayTracer2\Test3.ppm";
                Console.WriteLine(File);
                WriteFile(File, results);
                Console.WriteLine("Done writing immage to file.");

                Console.WriteLine("Done!");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();
        }

        static private Scene GetScene()
        {
            PlyParser parser = new PlyParser();

            Console.Write("Adding BasePlane...   ");
            Point v1 = new Point(-5, -0.5, 5);
            Point v2 = new Point(5, -0.5, 5);
            Point v3 = new Point(-5, -0.5, -5);
            Point v4 = new Point(5, -0.5, -5);
            Color floorColor = new Color(0, 1, 1);

            Triangle[] triangles = new Triangle[2];
            triangles[0] = new Triangle(v1, v2, v3);
            triangles[1] = new Triangle(v3, v2, v4);

            Mesh floor = new Mesh(triangles, new Material(floorColor));

            Console.WriteLine("Done.");

            Console.WriteLine("Parsing object files...   ");

            String Path = @"D:\Dev\RayTracer\RayTracer2\Scenes\bunny.ply";

            Console.Write(Path + " ...   ");

            IShape mesh = parser.Parse(Path, new Material(new Color(1, 0, 0)), new Vector(0, 0, 1));
            Console.WriteLine("Done.");

            Console.Write("Add objects to scene...   ");
            IShape[] Objects = { mesh, floor };
            Scene scene = new Scene(Objects, new List<ILight>());

            Console.WriteLine("Done.");

            return scene;
        }

        static private Camera GetCamera()
        {
            //return new Camera(new Point(-1, 1, -1), new Vector(1, 0, 1), new Vector(0, 1, 0), 1, 1, 200, 200);
            //return new Camera(new Point(0, 0, 0), new Vector(0, 0, 1), new Vector(0, 1, 0), 3, 3, 300, 300);
            return new Camera(new Point(0, 0, 0), new Vector(0, 0, 1), new Vector(0, 1, 0), 5 * .5 , 3 * .5, 5 * 500, 3 * 500);
        }

        static private void WriteFile(String FileName, Color[,] image)
        {
            Console.WriteLine("Converting image to text.");
            string[] lines = {"P3", (image.GetLength(0)).ToString() + " " + (image.GetLength(1)).ToString(), "255", "" };
            StringBuilder builder = new StringBuilder();

            for (int j = 0; j < image.GetLength(1); j++)
            {
                for (int i = 0; i < image.GetLength(0); i++)
                {
                    Color pxl = image[i, j];
                    builder.Append(pxl.r * 256 + " " + pxl.g * 256 + " " + pxl.b * 256 + " ");
                }
            }

            lines[3] = builder.ToString();

            Console.WriteLine("Done converting image to text.");

            Console.WriteLine("Writing text to file.");
            System.IO.File.WriteAllLines(FileName, lines);
            Console.WriteLine("Done writing text to file.");
        }

        static private Scene GetSceneSquare()
        {

            Console.Write("Adding Square...   ");
            Point v1 = new Point(-1, 1, 2);
            Point v2 = new Point(-1, -1, 2);
            Point v3 = new Point(1, 1, 2);
            Point v4 = new Point(1, -1, 2);

            Triangle[] triangles = new Triangle[2];
            triangles[0] = new Triangle(v2, v1, v3, new Material(new Color(0, 0, 1)));
            triangles[1] = new Triangle(v2, v3, v4, new Material(new Color(1, 0, 0)));

            Mesh square = new Mesh(triangles, null);

            Console.WriteLine("Done.");

            Console.Write("Add objects to scene...   ");

            IShape[] objects = { square };
            Scene scene = new Scene(objects, new List<ILight>());

            Console.Write("Adding lights...   ");
            scene.AddLight(new PointLight(new Point(1, 4, 0), new Color(1, 1, 1)));
            Console.WriteLine("Done.");

            Console.WriteLine("Done.");

            return scene;
        }

        static private Scene GetSceneShpere()
        {

            Point v1 = new Point(-5, -1.5, 5);
            Point v2 = new Point(5, -1.5, 5);
            Point v3 = new Point(-5, -1.5, -5);
            Point v4 = new Point(5, -1.5, -5);

            Color floorColor = new Color(0, 1, 1);

            IShape[] objects = { new Sphere(new Point(0, 0, 2), 1, new Material(new Color(1, 0, 0))), new Triangle(v1, v2, v3, new Material(floorColor)), new Triangle(v3, v2, v4, new Material(floorColor)) };

            Scene scene = new Scene(objects, new List<ILight>());

            scene.AddLight(new PointLight(new Point(1, 4, 0), new Color(1, 1, 1)));

            return scene;
        }

        static private Scene GetSceneShperesRelectance()
        {
            Point v1 = new Point(-5, -1.5, 7);
            Point v2 = new Point(5, -1.5, 7);
            Point v3 = new Point(-5, -1.5, -7);
            Point v4 = new Point(5, -1.5, -7);
            Point v5 = new Point(-5, 5, 7);
            Point v6 = new Point(5, 5, 7);
            Point v7 = new Point(-5, 5, -7);
            Point v8 = new Point(5, 5, -7);

            Color floorColor = new Color(1, 1, 0);
            Color backWallColor = new Color(0, 1, 0);
            Color rightWallColor = new Color(1, 0, 0);
            Color lefttWallColor = new Color(0, 0, 1);

            Mesh Floor = new Mesh(new Triangle[] { new Triangle(v1, v2, v3), new Triangle(v3, v2, v4) }, new Material(floorColor));
            Mesh BackWall = new Mesh(new Triangle[] { new Triangle(v1, v5, v6), new Triangle(v1, v6, v2) }, new Material(backWallColor));
            Mesh RightWall = new Mesh(new Triangle[] { new Triangle(v2, v6, v4), new Triangle(v4, v6, v8) }, new Material(rightWallColor));
            Mesh LeftWall = new Mesh(new Triangle[] { new Triangle(v1, v3, v5), new Triangle(v5, v3, v7) }, new Material(lefttWallColor));

            IShape[] objects = { new Sphere(new Point(1.3, 0, 3), 1, new Material(new Color(.9, .9, .9), SurfaceType.Refractive, RefractionIndeces.Glass)), new Sphere(new Point(-1.3, 0, 3), 1, new Material(new Color(.9, .9, .9), SurfaceType.Reflective)), Floor, BackWall, RightWall, LeftWall };

            Scene scene = new Scene(objects, new List<ILight>());

            scene.AddLight(new PointLight(new Point(1, 4, 0), new Color(1, 1, 1)));

            return scene;
        }
    }
}
