using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using RayTracer2.Geometry;
using RayTracer2.BVH;

namespace RayTracerTests
{
    [TestClass]
    public class BoundVolumeHiarchyTests
    {
        [TestMethod]
        public void BoundVolumeHiarchy_IntersectedShape_4OrLessTest()
        {
            IShape[] Shapes = new IShape[4];
            Ray[] Rays = new Ray[5];

            Shapes[0] = new Sphere(new Point(-10, 0, 0), 1, null);
            Shapes[1] = new Sphere(new Point(10, 0, 0), 1, null);
            Shapes[2] = new Sphere(new Point(0, -10, 0), 1, null);
            Shapes[3] = new Sphere(new Point(0, 10, 0), 1, null);

            Point o = new Point(0, 0, 0);

            Rays[0] = new Ray(o, new Vector(-1, 0, 0));
            Rays[1] = new Ray(o, new Vector(1, 0, 0));
            Rays[2] = new Ray(o, new Vector(0, -1, 0));
            Rays[3] = new Ray(o, new Vector(0, 1, 0));
            Rays[4] = new Ray(o, new Vector(0, 0, 1));

            IShape[] Shapes2 = new IShape[4];

            Shapes.CopyTo(Shapes2, 0);

            BoundingVolumeHiarchy bvh = new BoundingVolumeHiarchy(Shapes2);

            for (int i = 0; i < Shapes.Length; i++)
            {
                Sphere result = (Sphere)bvh.getIntersectedShape(Rays[i]);
                Sphere expectedValue = (Sphere)Shapes[i];

                Assert.IsNotNull(result, String.Format("Unexpectedly returned nothing.  Ray: {0}", i));

                Assert.AreEqual(result.CenterPoint.x, expectedValue.CenterPoint.x, String.Format("The x value for the center Points weren't equal.  Ray: {0}", i));
                Assert.AreEqual(result.CenterPoint.y, expectedValue.CenterPoint.y, String.Format("The y value for the center Points weren't equal.  Ray: {0}", i));
                Assert.AreEqual(result.CenterPoint.z, expectedValue.CenterPoint.z, String.Format("The z value for the center Points weren't equal.  Ray: {0}", i));
                Assert.AreEqual(result.Radius, expectedValue.Radius, String.Format("The radii weren't equal.  Ray: {0}", i));
            }

            Assert.IsNull(bvh.getIntersectedShape(Rays[4]), "Missing ray unexpectedly hit an object.");
        }

        [TestMethod]
        public void BoundVolumeHiarchy_IntersectedShape_MoreThan4Test()
        {
            IShape[] Shapes = new IShape[6];
            Ray[] Rays = new Ray[7];

            Shapes[0] = new Sphere(new Point(-10, 10, 0), 1, null);
            Shapes[1] = new Sphere(new Point(-10, -10, 0), 1, null);
            Shapes[2] = new Sphere(new Point(0, -10, 0), 1, null);
            Shapes[3] = new Sphere(new Point(0, 10, 0), 1, null);
            Shapes[4] = new Sphere(new Point(10, -10, 0), 1, null);
            Shapes[5] = new Sphere(new Point(10, 10, 0), 1, null);

            Point o = new Point(0, 0, 0);

            Rays[0] = new Ray(o, new Vector(-1, 1, 0));
            Rays[1] = new Ray(o, new Vector(-1, -1, 0));
            Rays[2] = new Ray(o, new Vector(0, -1, 0));
            Rays[3] = new Ray(o, new Vector(0, 1, 0));
            Rays[4] = new Ray(o, new Vector(1, -1, 0));
            Rays[5] = new Ray(o, new Vector(1, 1, 0));
            Rays[6] = new Ray(o, new Vector(1, 1, 1));

            IShape[] Shapes2 = new IShape[6];

            Shapes.CopyTo(Shapes2, 0);

            BoundingVolumeHiarchy bvh = new BoundingVolumeHiarchy(Shapes2);

            for (int i = 0; i < Shapes.Length; i++)
            {
                Sphere result = (Sphere)bvh.getIntersectedShape(Rays[i]);
                Sphere expectedValue = (Sphere)Shapes[i];

                Assert.IsNotNull(result, String.Format("Unexpectedly returned nothing.  Ray: {0}", i));

                Assert.AreEqual(result.CenterPoint.x, expectedValue.CenterPoint.x, String.Format("The x value for the center Points weren't equal.  Ray: {0}", i));
                Assert.AreEqual(result.CenterPoint.y, expectedValue.CenterPoint.y, String.Format("The y value for the center Points weren't equal.  Ray: {0}", i));
                Assert.AreEqual(result.CenterPoint.z, expectedValue.CenterPoint.z, String.Format("The z value for the center Points weren't equal.  Ray: {0}", i));
                Assert.AreEqual(result.Radius, expectedValue.Radius, String.Format("The radii weren't equal.  Ray: {0}", i));
            }

            Assert.IsTrue(bvh.getIntersectedShape(Rays[6]) == null, "Missing ray unexpectedly hit an object.");
        }


        [TestMethod]
        public void BoundVolumeHiarchy_IntersectedShape_ManyTest()
        {
            int Amount = 1000;
            IShape[] Shapes = new IShape[Amount];
            Ray[] Rays = new Ray[Amount];

            Point o = new Point(0, 0, 0);

            Random rand = new Random();

            for (int i = 0; i < Rays.Length; i++)
            {
                Rays[i] = new Ray(o, new Vector(rand.NextDouble(), rand.NextDouble(), rand.NextDouble()));
                Shapes[i] = new Sphere(o + Rays[i].Direction * Amount * 10, 1, null);
            }

            IShape[] Shapes2 = new IShape[Shapes.Length];

            Shapes.CopyTo(Shapes2, 0);

            BoundingVolumeHiarchy bvh = new BoundingVolumeHiarchy(Shapes2);

            for (int i = 0; i < Shapes.Length; i++)
            {
                Sphere result = (Sphere)bvh.getIntersectedShape(Rays[i]);
                Sphere expectedValue = (Sphere)Shapes[i];

                Assert.IsNotNull(result, String.Format("Unexpectedly returned nothing.  Ray: {0}", i));

                Assert.AreEqual(result.CenterPoint.x, expectedValue.CenterPoint.x, String.Format("The x value for the center Points weren't equal.  Ray: {0}", i));
                Assert.AreEqual(result.CenterPoint.y, expectedValue.CenterPoint.y, String.Format("The y value for the center Points weren't equal.  Ray: {0}", i));
                Assert.AreEqual(result.CenterPoint.z, expectedValue.CenterPoint.z, String.Format("The z value for the center Points weren't equal.  Ray: {0}", i));
                Assert.AreEqual(result.Radius, expectedValue.Radius, String.Format("The radii weren't equal.  Ray: {0}", i));
            }
        }

    }
}
