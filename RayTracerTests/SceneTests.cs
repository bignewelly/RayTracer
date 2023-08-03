using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using RayTracer2;
using RayTracer2.Geometry;
using RayTracer2.Illumination;

namespace RayTracerTests
{
    [TestClass]
    public class SceneTests
    {
        [TestMethod]
        public void Scene_intersectNothingTest()
        {
            Scene testScene = new Scene(new IShape[0], new List<ILight>());

            Assert.IsTrue(testScene.Intersect(new Ray(new Point(0, 0, 0), new Vector(0, 0, 1))) == new Color(0, 0, 0), "Intersect value was not what was expected.");
        }

        [TestMethod]
        public void Scene_IntersectPlaneTest()
        {
            Color expectedColor = new Color(0, 1, 0);
            IShape[] objects = { new Plane(new Point(10, 0, 0), new Vector(-1, 0, 0), new Material(expectedColor)) };
            Scene testScene = new Scene(objects, new List<ILight>());
            testScene.AddLight(new PointLight(new Point(0, 0, 0), new Color(1, 1, 1)));

            Color testColor = testScene.Intersect(new Ray(new Point(0, 0, 0), new Vector(1, 0, 0)));
            Color testColor2 = testScene.Intersect(new Ray(new Point(0, 0, 0), new Vector(-1, 0, 0)));
            Color expectedColor2 = new Color();

            Assert.IsTrue(testColor == expectedColor, String.Format("Intersect value was not what was expected. expectedColor: ({0}, {1}, {2}); testColor: ({3}, {4}, {5})", expectedColor.r, expectedColor.g, expectedColor.b, testColor.r, testColor.g, testColor.b));
            Assert.IsTrue(testColor2 == expectedColor2, String.Format("Intersect value was not what was expected. expectedColor: ({0}, {1}, {2}); testColor2: ({3}, {4}, {5})", expectedColor2.r, expectedColor2.g, expectedColor2.b, testColor2.r, testColor2.g, testColor2.b));
        }

        public void Scene_IntersectPlaneNoLightTest()
        {
            Color expectedColor = new Color(0, 0, 0);
            IShape[] objects = { new Plane(new Point(10, 0, 0), new Vector(-1, 0, 0), new Material(new Color(0, 1, 0))) };
            Scene testScene = new Scene(objects, new List<ILight>());

            Color testColor = testScene.Intersect(new Ray(new Point(0, 0, 0), new Vector(1, 0, 0)));

            Assert.IsTrue(testColor == expectedColor, String.Format("Intersect value was not what was expected. expectedColor: ({0}, {1}, {2}); testColor: ({3}, {4}, {5})", expectedColor.r, expectedColor.g, expectedColor.b, testColor.r, testColor.g, testColor.b));
        }
    }
}
