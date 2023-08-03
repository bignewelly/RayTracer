using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using RayTracer2;
using RayTracer2.Geometry;
namespace RayTracerTests
{
    [TestClass]
    public class ObjectTests
    {
        [TestMethod]
        public void Plane_IsHitTest()
        {
            Plane testPlane = new Plane(new Point(1, 0, 0), new Vector(-1, 0, 0), new Material(new Color()));
            Ray testRay = new Ray(new Point(0, 0, 0), new Vector(1, 0, 0));
            Ray testRay2 = new Ray(new Point(0, 0, 0), new Vector(-1, 0, 0));
            Ray testRay3 = new Ray(new Point(0, 0, 0), new Vector(0, 1, 1));
            Ray testRay4 = new Ray(new Point(0, 0, 0), new Vector(0.0001f, 1, 1));
            Ray testRay5 = new Ray(new Point(0, 0, 0), new Vector(-0.0000001f, 1000000, 100000));

            Assert.IsTrue(testPlane.IsHit(testRay), "Ray pointing directly at plane unexpectedly missed plane.");
            Assert.IsFalse(testPlane.IsHit(testRay2), "Ray pointing in opposite direcion of plane unexpectedly hit plane.");
            Assert.IsFalse(testPlane.IsHit(testRay3), "Ray parallel to plane unexpectedly hit plane.");
            Assert.IsTrue(testPlane.IsHit(testRay4), "Ray pointing slightly at plane unexpectedly missed plane.");
            Assert.IsFalse(testPlane.IsHit(testRay5), "Ray pointing slightly away from plane unexpectedly hit plane.");
        }


        [TestMethod]
        public void Plane_HitTest()
        {
            HitRecord expectedRecord = new HitRecord();
            expectedRecord.Material = new Material(new Color(1, 0, 0));
            expectedRecord.Normal = new Vector(-1, 0, 0);
            expectedRecord.Time = 1;

            Plane testPlane = new Plane(new Point(1, 0, 0), expectedRecord.Normal, expectedRecord.Material);
            Ray testRay = new Ray(new Point(0, 0, 0), new Vector(1, 0, 0));
            Ray testRay2 = new Ray(new Point(0, 0, 0), new Vector(-1, 0, 0));

            HitRecord testRecord = testPlane.Hit(testRay);
            HitRecord testRecord2 = testPlane.Hit(testRay2);

            Assert.IsTrue(testRecord.Material.color == expectedRecord.Material.color, String.Format("testRecord has the wrong color. expectedRecord.Material.color: ({0}, {1}, {2}); testRecord.Material.color: ({3}, {4}, {5})", expectedRecord.Material.color.r.ToString(), expectedRecord.Material.color.g.ToString(), expectedRecord.Material.color.b.ToString(), testRecord.Material.color.r.ToString(), testRecord.Material.color.g.ToString(), testRecord.Material.color.b.ToString()));
            Assert.IsTrue(testRecord.Normal == expectedRecord.Normal, String.Format("testRecord has the wrong normal. expectedRecord.Normal: ({0}, {1}, {2}); testRecord.Normal: ({3}, {4}, {5})", expectedRecord.Normal.x.ToString(), expectedRecord.Normal.y.ToString(), expectedRecord.Normal.z.ToString(), testRecord.Normal.x.ToString(), testRecord.Normal.y.ToString(), testRecord.Normal.z.ToString()));
            Assert.AreEqual(testRecord.Time, expectedRecord.Time, 0, "testRecord.Time was not the expected value.");
            Assert.IsTrue(testRecord2 == null, "testRecord2 Is not null. missedColor: ({0}, {1}, {2});");
        }

        [TestMethod]
        public void Triangle_IsHitTest()
        {
            Triangle testTriangle = new Triangle(new Point(1, 0, 1), new Point(1, 1, -1), new Point(1, -1, -1), new Material(new Color()));
            Ray testRay = new Ray(new Point(0, 0, 0), new Vector(1, 0, 0));
            Ray testRay2 = new Ray(new Point(0, 0, 0), new Vector(-1, 0, 0));
            Ray testRay3 = new Ray(new Point(0, 0, 0), new Vector(0, 1, 1));
            Ray testRay4 = new Ray(new Point(0, 0, 0), new Vector(1, 1, 1));
            Ray testRay5 = new Ray(new Point(0, 0, 1), new Vector(1, 0, 0));

            Assert.IsTrue(testTriangle.IsHit(testRay), "Ray pointing directly at triangle unexpectedly missed triangle.");
            Assert.IsFalse(testTriangle.IsHit(testRay2), "Ray pointing in opposite direcion of triangle unexpectedly hit triangle.");
            Assert.IsFalse(testTriangle.IsHit(testRay3), "Ray parallel to triangle unexpectedly hit triangle.");
            Assert.IsFalse(testTriangle.IsHit(testRay4), "Ray pointing slightly at triangle unexpectedly hit triangle.");
            Assert.IsTrue(testTriangle.IsHit(testRay), "Ray pointing directly at triangle unexpectedly missed triangle.");
            Assert.IsTrue(testTriangle.IsHit(testRay5), "Ray pointing directly at triangle unexpectedly missed triangle.");
        }


        [TestMethod]
        public void Triangle_HitTest()
        {
            HitRecord expectedRecord = new HitRecord();
            expectedRecord.Material = new Material(new Color(1, 0, 0));
            expectedRecord.Normal = new Vector(-1, 0, 0);
            expectedRecord.Time = 1;

            Point v1 = new Point(1, 0, 1);
            Point v2 = new Point(1, 1, -1);
            Point v3 = new Point(1, -1, -1);

            Triangle testTriangle = new Triangle(v1, v2, v3, expectedRecord.Material);
            Triangle testTriangle2 = new Triangle(v2, v1, v3, expectedRecord.Material);
            Ray testRay = new Ray(new Point(0, 0, 0), new Vector(1, 0, 0));
            Ray testRay2 = new Ray(new Point(0, 0, 0), new Vector(-1, 0, 0));

            HitRecord testRecord = testTriangle.Hit(testRay);
            HitRecord testRecord2 = testTriangle.Hit(testRay2);

            HitRecord testRecordMiss = testTriangle2.Hit(testRay);

            Assert.IsTrue(testRecord.Material.color == expectedRecord.Material.color, String.Format("testRecord has the wrong color. expectedRecord.Material.color: ({0}, {1}, {2}); testRecord.Material.color: ({3}, {4}, {5})", expectedRecord.Material.color.r.ToString(), expectedRecord.Material.color.g.ToString(), expectedRecord.Material.color.b.ToString(), testRecord.Material.color.r.ToString(), testRecord.Material.color.g.ToString(), testRecord.Material.color.b.ToString()));
            Assert.IsTrue(testRecord.Normal == expectedRecord.Normal, String.Format("testRecord has the wrong normal. expectedRecord.Normal: ({0}, {1}, {2}); testRecord.Normal: ({3}, {4}, {5})", expectedRecord.Normal.x.ToString(), expectedRecord.Normal.y.ToString(), expectedRecord.Normal.z.ToString(), testRecord.Normal.x.ToString(), testRecord.Normal.y.ToString(), testRecord.Normal.z.ToString()));
            Assert.AreEqual(testRecord.Time, expectedRecord.Time, 0, "testRecord.Time was not the expected value.");
            Assert.IsTrue(testRecord2 == null, "testRecord2 Is not null.");
            Assert.IsTrue(testRecordMiss == null, "testRecord2 Is not null. ");
        }

        [TestMethod]
        public void Sphere_IsHitTest()
        {
            Sphere testSphere = new Sphere(new Point(2, 0, 0), 1, new Material(new Color()));
            Ray testRay = new Ray(new Point(0, 0, 0), new Vector(1, 0, 0));
            Ray testRay2 = new Ray(new Point(0, 0, 0), new Vector(-1, 0, 0));
            Ray testRay3 = new Ray(new Point(0, 0, 0), new Vector(0, 1, 1));
            Ray testRay4 = new Ray(new Point(0, 0, 0), new Vector(1, 1, 0));
            Ray testRay5 = new Ray(new Point(0, 1, 0), new Vector(1, 0, 0));
            Ray testRay6 = new Ray(new Point(0, -1, 0), new Vector(1, 0, 0));
            Ray testRay7 = new Ray(new Point(0, 0, 1), new Vector(1, 0, 0));
            Ray testRay8 = new Ray(new Point(0, 0, -1), new Vector(1, 0, 0));

            Assert.IsTrue(testSphere.IsHit(testRay), "Ray pointing directly at sphere unexpectedly missed sphere.");
            Assert.IsFalse(testSphere.IsHit(testRay2), "Ray pointing in opposite direcion of sphere unexpectedly hit sphere.");
            Assert.IsFalse(testSphere.IsHit(testRay3), "Ray parallel to sphere unexpectedly hit sphere.");
            Assert.IsFalse(testSphere.IsHit(testRay4), "Ray pointing slightly at sphere unexpectedly hit sphere.");
            Assert.IsFalse(testSphere.IsHit(testRay5), "Ray pointing Grazing the sphere unexpectedly hit sphere.");
            Assert.IsFalse(testSphere.IsHit(testRay6), "Ray pointing Grazing the sphere unexpectedly hit sphere.");
            Assert.IsFalse(testSphere.IsHit(testRay7), "Ray pointing Grazing the sphere unexpectedly hit sphere.");
            Assert.IsFalse(testSphere.IsHit(testRay8), "Ray pointing Grazing the sphere unexpectedly hit sphere.");
        }


        [TestMethod]
        public void Sphere_HitTest()
        {
            HitRecord expectedRecord = new HitRecord();
            expectedRecord.Material = new Material(new Color(1, 0, 0));
            expectedRecord.Normal = new Vector(-1, 0, 0);
            expectedRecord.Time = 1;

            Sphere testSphere = new Sphere(new Point(2, 0, 0), 1, expectedRecord.Material);
            Ray testRay = new Ray(new Point(0, 0, 0), new Vector(1, 0, 0));
            Ray testRay2 = new Ray(new Point(0, 0, 0), new Vector(-1, 0, 0));

            HitRecord testRecord = testSphere.Hit(testRay);
            HitRecord testRecord2 = testSphere.Hit(testRay2);

            Assert.IsTrue(testRecord.Material.color == expectedRecord.Material.color, String.Format("testRecord has the wrong color. expectedRecord.Material.color: ({0}, {1}, {2}); testRecord.Material.color: ({3}, {4}, {5})", expectedRecord.Material.color.r.ToString(), expectedRecord.Material.color.g.ToString(), expectedRecord.Material.color.b.ToString(), testRecord.Material.color.r.ToString(), testRecord.Material.color.g.ToString(), testRecord.Material.color.b.ToString()));
            Assert.IsTrue(testRecord.Normal == expectedRecord.Normal, String.Format("testRecord has the wrong normal. expectedRecord.Normal: ({0}, {1}, {2}); testRecord.Normal: ({3}, {4}, {5})", expectedRecord.Normal.x.ToString(), expectedRecord.Normal.y.ToString(), expectedRecord.Normal.z.ToString(), testRecord.Normal.x.ToString(), testRecord.Normal.y.ToString(), testRecord.Normal.z.ToString()));
            Assert.AreEqual(testRecord.Time, expectedRecord.Time, 0, "testRecord.Time was not the expected value.");
            Assert.IsTrue(testRecord2 == null, "testRecord2 Is not null. missedColor: ({0}, {1}, {2});");
        }

        [TestMethod]
        public void Mesh_IsHitTest()
        {
            Triangle[] Triangles = new Triangle[4];

            Point vert1 = new Point(0, 1, 0);
            Point vert2 = new Point(-1, -1, -1);
            Point vert3 = new Point(1, -1, -1);
            Point vert4 = new Point(1, -1, 1);
            Point vert5 = new Point(-1, -1, 1);

            Triangles[0] = new Triangle(vert1, vert3, vert2);
            Triangles[1] =new Triangle(vert1, vert4, vert3);
            Triangles[2] = new Triangle(vert1, vert5, vert4);
            Triangles[3] = new Triangle(vert1, vert2, vert5);

            Mesh testMesh = new Mesh(Triangles, null);
            Mesh testMesh2 = new Mesh(Triangles, null, new Vector(0, -2, 0));

            Ray testRay = new Ray(new Point(0, 0, -1), new Vector(0, 0, 1));
            Ray testRay2 = new Ray(new Point(0, 0, -2), new Vector(0, 0, -1));
            Ray testRay3 = new Ray(new Point(0, 2, 0), new Vector(0, -1, 0));
            Ray testRay4 = new Ray(new Point(-1, 0, 0), new Vector(1, 0, 0));
            Ray testRay5 = new Ray(new Point(1, 0, 0), new Vector(-1, 0, 0));

            Assert.IsTrue(testMesh.IsHit(testRay), "testRay unexpectedly missed.");
            Assert.IsFalse(testMesh.IsHit(testRay2), "testRay2 unexpectedly hit.");
            Assert.IsTrue(testMesh.IsHit(testRay3), "testRay3 unexpectedly missed.");
            Assert.IsTrue(testMesh.IsHit(testRay4), "testRay4 unexpectedly missed.");
            Assert.IsTrue(testMesh.IsHit(testRay5), "testRay5 unexpectedly missed.");

            Assert.IsFalse(testMesh2.IsHit(testRay), "testRay unexpectedly hit translated mesh.");
            Assert.IsFalse(testMesh2.IsHit(testRay2), "testRay2 unexpectedly hit translated mesh.");
            Assert.IsTrue(testMesh2.IsHit(testRay3), "testRay3 unexpectedly missed translated mesh.");
            Assert.IsFalse(testMesh2.IsHit(testRay4), "testRay4 unexpectedly hit translated mesh.");
            Assert.IsFalse(testMesh2.IsHit(testRay5), "testRay5 unexpectedly hit translated mesh.");
        }


        [TestMethod]
        public void Mesh_getIntersectedShapeTest()
        {
            Triangle[] Triangles = new Triangle[4];

            Point vert1 = new Point(0, 1, 0);
            Point vert2 = new Point(-1, -1, -1);
            Point vert3 = new Point(1, -1, -1);
            Point vert4 = new Point(1, -1, 1);
            Point vert5 = new Point(-1, -1, 1);

            Triangles[0] = new Triangle(vert1, vert3, vert2);
            Triangles[1] = new Triangle(vert1, vert4, vert3);
            Triangles[2] = new Triangle(vert1, vert5, vert4);
            Triangles[3] = new Triangle(vert1, vert2, vert5);

            Triangle[] tri2 = new Triangle[4];

            Triangles.CopyTo(tri2, 0);

            Mesh testMesh = new Mesh(tri2, null);

            Ray testRay = new Ray(new Point(0, 0, -1), new Vector(0, 0, 1));

            Triangle testTriangle = (Triangle)testMesh.getIntersectedShape(testRay);
            Triangle expectedTriangle = Triangles[0];

            Assert.IsNotNull(testTriangle, "testTriangle was unexpectedly null.");
            Assert.IsTrue(testTriangle.Vertex1 == expectedTriangle.Vertex1 && testTriangle.Vertex2 == expectedTriangle.Vertex2 && testTriangle.Vertex3 == expectedTriangle.Vertex3, "testTriangle was not equal to expected triangle.");
        }

        [TestMethod]
        public void Mesh_HitTest()
        {
            List<Triangle> Triangles = new List<Triangle>();

            Point vert1 = new Point(0, 1, 0);
            Point vert2 = new Point(-1, -1, -1);
            Point vert3 = new Point(1, -1, -1);
            Point vert4 = new Point(1, -1, 1);
            Point vert5 = new Point(-1, -1, 1);

            Triangles.Add(new Triangle(vert1, vert3, vert2));
            Triangles.Add(new Triangle(vert1, vert4, vert3));
            Triangles.Add(new Triangle(vert1, vert5, vert4));
            Triangles.Add(new Triangle(vert1, vert2, vert5));

            HitRecord expectedRecord = new HitRecord();
            expectedRecord.Material = new Material(new Color(1, 0, 0));
            expectedRecord.Normal = Triangles[0].getNormal(Triangles[0].Vertex1);
            expectedRecord.Time = 0.5;

            Mesh testMesh = new Mesh(Triangles, expectedRecord.Material);

            Ray testRay = new Ray(new Point(0, 0, -1), new Vector(0, 0, 1));
            Ray testRay2 = new Ray(new Point(0, 0, -2), new Vector(0, 0, -1));

            HitRecord testRecord = testMesh.Hit(testRay);
            HitRecord testRecord2 = testMesh.Hit(testRay2);

            Assert.IsTrue(testRecord.Material.color == expectedRecord.Material.color, String.Format("testRecord has the wrong color. expectedRecord.Material.color: ({0}, {1}, {2}); testRecord.Material.color: ({3}, {4}, {5})", expectedRecord.Material.color.r.ToString(), expectedRecord.Material.color.g.ToString(), expectedRecord.Material.color.b.ToString(), testRecord.Material.color.r.ToString(), testRecord.Material.color.g.ToString(), testRecord.Material.color.b.ToString()));
            Assert.IsTrue(testRecord.Normal == expectedRecord.Normal, String.Format("testRecord has the wrong normal. expectedRecord.Normal: ({0}, {1}, {2}); testRecord.Normal: ({3}, {4}, {5})", expectedRecord.Normal.x.ToString(), expectedRecord.Normal.y.ToString(), expectedRecord.Normal.z.ToString(), testRecord.Normal.x.ToString(), testRecord.Normal.y.ToString(), testRecord.Normal.z.ToString()));
            Assert.AreEqual(testRecord.Time, expectedRecord.Time, 0, "testRecord.Time was not the expected value.");
            Assert.IsTrue(testRecord2 == null, "testRecord2 Is not null. missedColor: ({0}, {1}, {2});");
        }
    }
}
