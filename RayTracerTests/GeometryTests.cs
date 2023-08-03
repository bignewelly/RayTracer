using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using RayTracer2.Geometry;

namespace RayTracerTests
{
    [TestClass]
    public class GeometryTests
    {
        const double THRESHOLD = 0;
        [TestMethod]
        public void Extent_UnionWithDefaultTest()
        {
            Extent testExtent = Geometry.Union(new Extent(), new Extent(-1, 1));

            Assert.AreEqual(testExtent.Min, -1, THRESHOLD, String.Format("Unexpected min value. Expected Value: -1; Actual Value: {0}", testExtent.Min.ToString()));
            Assert.AreEqual(testExtent.Max, 1, THRESHOLD, String.Format("Unexpected max value. Expected Value: 1; Actual Value: {0}", testExtent.Max.ToString()));
        }

        [TestMethod]
        public void Extent_UnionTest()
        {
            Extent testExtent = Geometry.Union(new Extent(-1, 0), new Extent(0, 1));

            Assert.AreEqual(testExtent.Min, -1, THRESHOLD, String.Format("Unexpected min value. Expected Value: -1; Actual Value: {0}", testExtent.Min.ToString()));
            Assert.AreEqual(testExtent.Max, 1, THRESHOLD, String.Format("Unexpected max value. Expected Value: 1; Actual Value: {0}", testExtent.Max.ToString()));
        }

        [TestMethod]
        public void BoundingBox_UnionWithDefaultTest()
        {
            Extent x = new Extent(-1, 1);
            Extent y = new Extent(-2, 2);
            Extent z = new Extent(-3, 3);

            BoundingBox testBox = Geometry.Union(null, new BoundingBox(x,y,z));

            Assert.AreEqual(testBox.x.Min, x.Min, THRESHOLD, String.Format("Unexpected min x value. Expected Value: {0}; Actual Value: {1}", testBox.x.Min.ToString(), x.Min.ToString()));
            Assert.AreEqual(testBox.x.Max, x.Max, THRESHOLD, String.Format("Unexpected max x value. Expected Value: {0}; Actual Value: {1}", testBox.x.Max.ToString(), x.Max.ToString()));

            Assert.AreEqual(testBox.y.Min, y.Min, THRESHOLD, String.Format("Unexpected min y value. Expected Value: {0}; Actual Value: {1}", testBox.y.Min.ToString(), y.Min.ToString()));
            Assert.AreEqual(testBox.y.Max, y.Max, THRESHOLD, String.Format("Unexpected max y value. Expected Value: {0}; Actual Value: {1}", testBox.y.Max.ToString(), y.Max.ToString()));

            Assert.AreEqual(testBox.z.Min, z.Min, THRESHOLD, String.Format("Unexpected min z value. Expected Value: {0}; Actual Value: {1}", testBox.z.Min.ToString(), z.Min.ToString()));
            Assert.AreEqual(testBox.z.Max, z.Max, THRESHOLD, String.Format("Unexpected max z value. Expected Value: {0}; Actual Value: {1}", testBox.z.Max.ToString(), z.Max.ToString()));
        }

        [TestMethod]
        public void BoundingBox_UnionTest()
        {
            Extent x = new Extent(-1, 1);
            Extent y = new Extent(-2, 2);
            Extent z = new Extent(-3, 3);

            BoundingBox testBox1 = new BoundingBox(new Extent(x.Min, 0), new Extent(0, y.Max), new Extent());
            BoundingBox testBox2 = new BoundingBox(new Extent(0, x.Max), new Extent(y.Min, 0), new Extent(z.Min, z.Max));

            BoundingBox testBox = Geometry.Union(testBox1, testBox2);

            Assert.AreEqual(testBox.x.Min, x.Min, THRESHOLD, String.Format("Unexpected min x value. Expected Value: {0}; Actual Value: {1}", testBox.x.Min.ToString(), x.Min.ToString()));
            Assert.AreEqual(testBox.x.Max, x.Max, THRESHOLD, String.Format("Unexpected max x value. Expected Value: {0}; Actual Value: {1}", testBox.x.Max.ToString(), x.Max.ToString()));

            Assert.AreEqual(testBox.y.Min, y.Min, THRESHOLD, String.Format("Unexpected min y value. Expected Value: {0}; Actual Value: {1}", testBox.y.Min.ToString(), y.Min.ToString()));
            Assert.AreEqual(testBox.y.Max, y.Max, THRESHOLD, String.Format("Unexpected max y value. Expected Value: {0}; Actual Value: {1}", testBox.y.Max.ToString(), y.Max.ToString()));

            Assert.AreEqual(testBox.z.Min, z.Min, THRESHOLD, String.Format("Unexpected min z value. Expected Value: {0}; Actual Value: {1}", testBox.z.Min.ToString(), z.Min.ToString()));
            Assert.AreEqual(testBox.z.Max, z.Max, THRESHOLD, String.Format("Unexpected max z value. Expected Value: {0}; Actual Value: {1}", testBox.z.Max.ToString(), z.Max.ToString()));
        }

        [TestMethod]
        public void GeometricValueBase_EqualsTest()
        {
            Point point1 = new Point(1, 2, 3);
            Point point2 = new Point(1, 2, 3);

            Assert.IsTrue(point1 == point2, String.Format("Values are evaluated as not equal. Point1: ({0}, {1}, {2}); Point2: ({3}, {4}, {5})", point1.x.ToString(), point1.y.ToString(), point1.z.ToString(), point2.x.ToString(), point2.y.ToString(), point2.z.ToString()));
        }

        [TestMethod]
        public void GeometricValueBase_NotEqualsTest()
        {
            Point point1 = new Point(1, 1, 0);
            Point point2 = new Point(0, 1, 1);
            Point point3 = new Point(1, 0, 1);

            Assert.IsTrue(point1 != point2, String.Format("Values are evaluated as equal. Point1: ({0}, {1}, {2}); point2: ({3}, {4}, {5})", point1.x.ToString(), point1.y.ToString(), point1.z.ToString(), point2.x.ToString(), point2.y.ToString(), point2.z.ToString()));
            Assert.IsTrue(point1 != point3, String.Format("Values are evaluated as equal. Point1: ({0}, {1}, {2}); point3: ({3}, {4}, {5})", point1.x.ToString(), point1.y.ToString(), point1.z.ToString(), point3.x.ToString(), point3.y.ToString(), point3.z.ToString()));
            Assert.IsTrue(point3 != point2, String.Format("Values are evaluated as equal. point3: ({0}, {1}, {2}); Point2: ({3}, {4}, {5})", point3.x.ToString(), point3.y.ToString(), point3.z.ToString(), point2.x.ToString(), point2.y.ToString(), point2.z.ToString()));
        }

        [TestMethod]
        public void GeometricValueBase_DotTest()
        {
            Point point = new Point(1, 2, 3);
            Vector vector = new Vector(4, -5, 6);

            double expected1 = 12;
            double result1 = Geometry.Dot(point, vector);

            Assert.AreEqual(result1, expected1, THRESHOLD, String.Format("Unexpected result1 value. Expected Value: {1}; Actual Value: {0}", result1.ToString(), expected1.ToString()));
        }

        [TestMethod]
        public void Vector_CrossTest()
        {
            Vector vector1 = new Vector(1, 0, 0);
            Vector vector = new Vector(0, 1, 0);
            Vector expectedValue = new Vector(0, 0, 1);

            Vector testVector = Geometry.Cross(vector1 ,vector);

            Assert.IsTrue(testVector == expectedValue, String.Format("Testvector1 not expected value: ({0}, {1}, {2}); expectedValue: ({3}, {4}, {5})", testVector.x.ToString(), testVector.y.ToString(), testVector.z.ToString(), expectedValue.x.ToString(), expectedValue.y.ToString(), expectedValue.z.ToString()));
        }

        [TestMethod]
        public void VectorPoint_NotEqualsTest()
        {
            Point point = new Point(1, 2, 3);
            Vector vector = new Vector(1, 2, 3);

            Assert.IsTrue(point != vector, "A vector and a point were evaluated as equal.");
        }

        [TestMethod]
        public void PointVector_PlusOperatorTest()
        {
            Point point = new Point(0, 0, 0);
            Vector vector = new Vector(1, 2, 3);
            Point expectedValue = new Point(1, 2, 3);

            Point testPoint = point + vector;

            Assert.IsTrue(testPoint == expectedValue, String.Format("Testpoint not expected value: ({0}, {1}, {2}); expectedValue: ({3}, {4}, {5})", testPoint.x.ToString(), testPoint.y.ToString(), testPoint.z.ToString(), expectedValue.x.ToString(), expectedValue.y.ToString(), expectedValue.z.ToString()));
        }

        [TestMethod]
        public void PointVector_MinusOperatorTest()
        {
            Point point = new Point(0, 0, 0);
            Vector vector = new Vector(1, 2, 3);
            Point expectedValue = new Point(-1, -2, -3);

            Point testPoint = point - vector;

            Assert.IsTrue(testPoint == expectedValue, String.Format("Testpoint not expected value: ({0}, {1}, {2}); expectedValue: ({3}, {4}, {5})", testPoint.x.ToString(), testPoint.y.ToString(), testPoint.z.ToString(), expectedValue.x.ToString(), expectedValue.y.ToString(), expectedValue.z.ToString()));
        }

        [TestMethod]
        public void Point_IsBetweenTest()
        {
            Point p1 = new Point(1, 1, 1);
            Point p2 = new Point(-1, -1, -1);
            Point p3 = new Point(0, 0, 0);
            Point p4 = new Point(0, 1, 0);
            Point p5 = new Point(2, 2, 2);

            Assert.IsTrue(Geometry.IsBetween(p1, p2, p3), "Mid point not recognized as on line segment.");
            Assert.IsTrue(Geometry.IsBetween(p1, p2, p1), "Startpoint point not recognized as on line segment.");
            Assert.IsTrue(Geometry.IsBetween(p1, p2, p2), "Endpoint point not recognized as on line segment.");
            Assert.IsFalse(Geometry.IsBetween(p1, p2, p4), "Point not on line was recognized as is between.");
            Assert.IsFalse(Geometry.IsBetween(p1, p2, p5), "Point on line but not between points was recognized as in between.");
        }

        [TestMethod]
        public void Point_MinusOperatorTest()
        {
            Point point = new Point(0, 0, 0);
            Point point2 = new Point(1, 2, 3);
            Vector expectedValue = new Vector(-1, -2, -3);

            Vector testVector = point - point2;

            Assert.IsTrue(testVector == expectedValue, String.Format("Testpoint not expected value: ({0}, {1}, {2}); expectedValue: ({3}, {4}, {5})", testVector.x.ToString(), testVector.y.ToString(), testVector.z.ToString(), expectedValue.x.ToString(), expectedValue.y.ToString(), expectedValue.z.ToString()));
        }

        [TestMethod]
        public void Pointdouble_TimesOperatorTest()
        {
            Point point = new Point(-1, 2, 3);
            Point expectedValue = new Point(-2, 4, 6);

            Point testValue = point * 2;

            Assert.IsTrue(testValue == expectedValue, String.Format("Testpoint not expected value: ({0}, {1}, {2}); expectedValue: ({3}, {4}, {5})", testValue.x.ToString(), testValue.y.ToString(), testValue.z.ToString(), expectedValue.x.ToString(), expectedValue.y.ToString(), expectedValue.z.ToString()));
        }

        [TestMethod]
        public void Pointdouble_DivideOperatorTest()
        {
            Point point = new Point(-3, 6, 9);
            Point expectedValue = new Point(-1, 2, 3);

            Point testValue = point / 3;

            Assert.IsTrue(testValue == expectedValue, String.Format("Testpoint not expected value: ({0}, {1}, {2}); expectedValue: ({3}, {4}, {5})", testValue.x.ToString(), testValue.y.ToString(), testValue.z.ToString(), expectedValue.x.ToString(), expectedValue.y.ToString(), expectedValue.z.ToString()));
        }

        [TestMethod]
        public void Vectordouble_DivideOperatorTest()
        {
            Vector vector = new Vector(-3, 6, 9);
            Vector expectedValue = new Vector(-1, 2, 3);

            Vector testValue = vector / 3;

            Assert.IsTrue(testValue == expectedValue, String.Format("testValue not expected value: ({0}, {1}, {2}); expectedValue: ({3}, {4}, {5})", testValue.x.ToString(), testValue.y.ToString(), testValue.z.ToString(), expectedValue.x.ToString(), expectedValue.y.ToString(), expectedValue.z.ToString()));
        }

        [TestMethod]
        public void Vectordouble_TimesOperatorTest()
        {
            Vector vector = new Vector(-1, 2, 3);
            Vector expectedValue = new Vector(-2, 4, 6);

            Vector testValue = vector * 2;

            Assert.IsTrue(testValue == expectedValue, String.Format("testValue not expected value: ({0}, {1}, {2}); expectedValue: ({3}, {4}, {5})", testValue.x.ToString(), testValue.y.ToString(), testValue.z.ToString(), expectedValue.x.ToString(), expectedValue.y.ToString(), expectedValue.z.ToString()));
        }

        [TestMethod]
        public void Vector_PlusOperatorTest()
        {
            Vector vector1 = new Vector(0, 0, 0);
            Vector vector = new Vector(1, 2, 3);
            Vector expectedValue = new Vector(1, 2, 3);

            Vector testVector = vector1 + vector;

            Assert.IsTrue(testVector == expectedValue, String.Format("Testvector1 not expected value: ({0}, {1}, {2}); expectedValue: ({3}, {4}, {5})", testVector.x.ToString(), testVector.y.ToString(), testVector.z.ToString(), expectedValue.x.ToString(), expectedValue.y.ToString(), expectedValue.z.ToString()));
        }

        [TestMethod]
        public void Vector_MinusOperatorTest()
        {
            Vector vector1 = new Vector(0, 0, 0);
            Vector vector = new Vector(1, 2, 3);
            Vector expectedValue = new Vector(-1, -2, -3);

            Vector testVector = vector1 - vector;

            Assert.IsTrue(testVector == expectedValue, String.Format("Testvector1 not expected value: ({0}, {1}, {2}); expectedValue: ({3}, {4}, {5})", testVector.x.ToString(), testVector.y.ToString(), testVector.z.ToString(), expectedValue.x.ToString(), expectedValue.y.ToString(), expectedValue.z.ToString()));
        }


        [TestMethod]
        public void Geometry_GetPositionOnRayTest()
        {
            Ray ray = new Ray(new Point(1, -1, 2), new Vector(1, 1, 1));

            Point testValue = Geometry.GetPositionOnRay(ray, 0.5f);
            Point expectedValue = ray.Origin + (ray.Direction * 0.5f) ;

            Assert.IsTrue(testValue == expectedValue, String.Format("testValue not expected value: ({0}, {1}, {2}); expectedValue: ({3}, {4}, {5})", testValue.x.ToString(), testValue.y.ToString(), testValue.z.ToString(), expectedValue.x.ToString(), expectedValue.y.ToString(), expectedValue.z.ToString()));
        }

        [TestMethod]
        public void Geometry_IntersectTest()
        {
            Point origin = new Point(0, 0, 0);

            Ray rayX = new Ray(origin, new Vector(1, 0, 0));
            Ray rayY = new Ray(origin, new Vector(0, 1, 0));
            Ray rayZ = new Ray(origin, new Vector(0, 0, 1));

            BoundingBox bBoxX = new BoundingBox(new Extent(1, 2), new Extent(-1, 1), new Extent(-1, 1));
            BoundingBox bBoxY = new BoundingBox(new Extent(-1, 1), new Extent(1, 2), new Extent(-1, 1));
            BoundingBox bBoxZ = new BoundingBox(new Extent(-1, 1), new Extent(-1, 1), new Extent(1, 2));

            //Test x Ray
            Assert.IsTrue(Geometry.Intersect(rayX, bBoxX), String.Format("Values uncexpectedly didn't intersect. Ray: ({0}, {1}, {2}); Bounding Box: x({3}, {4}), y({5}, {6}), z({7}, {8})", rayX.Direction.x.ToString(), rayX.Direction.y.ToString(), rayX.Direction.z.ToString(), bBoxX.x.Min.ToString(), bBoxX.x.Max.ToString(), bBoxX.y.Min.ToString(), bBoxX.y.Max.ToString(), bBoxX.z.Min.ToString(), bBoxX.z.Max.ToString()));
            Assert.IsFalse(Geometry.Intersect(rayX, bBoxY), String.Format("Values uncexpectedly intersected. Ray: ({0}, {1}, {2}); Bounding Box: x({3}, {4}), y({5}, {6}), z({7}, {8})", rayX.Direction.x.ToString(), rayX.Direction.y.ToString(), rayX.Direction.z.ToString(), bBoxY.x.Min.ToString(), bBoxY.x.Max.ToString(), bBoxY.y.Min.ToString(), bBoxY.y.Max.ToString(), bBoxY.z.Min.ToString(), bBoxY.z.Max.ToString()));
            Assert.IsFalse(Geometry.Intersect(rayX, bBoxZ), String.Format("Values uncexpectedly intersected. Ray: ({0}, {1}, {2}); Bounding Box: x({3}, {4}), y({5}, {6}), z({7}, {8})", rayX.Direction.x.ToString(), rayX.Direction.y.ToString(), rayX.Direction.z.ToString(), bBoxZ.x.Min.ToString(), bBoxZ.x.Max.ToString(), bBoxZ.y.Min.ToString(), bBoxZ.y.Max.ToString(), bBoxZ.z.Min.ToString(), bBoxZ.z.Max.ToString()));

            //Test y Ray
            Assert.IsTrue(Geometry.Intersect(rayY, bBoxY), String.Format("Values uncexpectedly didn't intersect. Ray: ({0}, {1}, {2}); Bounding Box: x({3}, {4}), y({5}, {6}), z({7}, {8})", rayY.Direction.x.ToString(), rayY.Direction.y.ToString(), rayY.Direction.z.ToString(), bBoxY.x.Min.ToString(), bBoxY.x.Max.ToString(), bBoxY.y.Min.ToString(), bBoxY.y.Max.ToString(), bBoxY.z.Min.ToString(), bBoxY.z.Max.ToString()));
            Assert.IsFalse(Geometry.Intersect(rayY, bBoxX), String.Format("Values uncexpectedly intersected. Ray: ({0}, {1}, {2}); Bounding Box: x({3}, {4}), y({5}, {6}), z({7}, {8})", rayY.Direction.x.ToString(), rayY.Direction.y.ToString(), rayY.Direction.z.ToString(), bBoxX.x.Min.ToString(), bBoxX.x.Max.ToString(), bBoxX.y.Min.ToString(), bBoxX.y.Max.ToString(), bBoxX.z.Min.ToString(), bBoxX.z.Max.ToString()));
            Assert.IsFalse(Geometry.Intersect(rayY, bBoxZ), String.Format("Values uncexpectedly intersected. Ray: ({0}, {1}, {2}); Bounding Box: x({3}, {4}), y({5}, {6}), z({7}, {8})", rayY.Direction.x.ToString(), rayY.Direction.y.ToString(), rayY.Direction.z.ToString(), bBoxZ.x.Min.ToString(), bBoxZ.x.Max.ToString(), bBoxZ.y.Min.ToString(), bBoxZ.y.Max.ToString(), bBoxZ.z.Min.ToString(), bBoxZ.z.Max.ToString()));

            // Test z Ray
            Assert.IsTrue(Geometry.Intersect(rayZ, bBoxZ), String.Format("Values uncexpectedly didn't intersect. Ray: ({0}, {1}, {2}); Bounding Box: x({3}, {4}), y({5}, {6}), z({7}, {8})", rayZ.Direction.x.ToString(), rayZ.Direction.y.ToString(), rayZ.Direction.z.ToString(), bBoxZ.x.Min.ToString(), bBoxZ.x.Max.ToString(), bBoxZ.y.Min.ToString(), bBoxZ.y.Max.ToString(), bBoxZ.z.Min.ToString(), bBoxZ.z.Max.ToString()));
            Assert.IsFalse(Geometry.Intersect(rayZ, bBoxX), String.Format("Values uncexpectedly intersected. Ray: ({0}, {1}, {2}); Bounding Box: x({3}, {4}), y({5}, {6}), z({7}, {8})", rayZ.Direction.x.ToString(), rayZ.Direction.y.ToString(), rayZ.Direction.z.ToString(), bBoxX.x.Min.ToString(), bBoxX.x.Max.ToString(), bBoxX.y.Min.ToString(), bBoxX.y.Max.ToString(), bBoxX.z.Min.ToString(), bBoxX.z.Max.ToString()));
            Assert.IsFalse(Geometry.Intersect(rayZ, bBoxY), String.Format("Values uncexpectedly intersected. Ray: ({0}, {1}, {2}); Bounding Box: x({3}, {4}), y({5}, {6}), z({7}, {8})", rayZ.Direction.x.ToString(), rayZ.Direction.y.ToString(), rayZ.Direction.z.ToString(), bBoxY.x.Min.ToString(), bBoxY.x.Max.ToString(), bBoxY.y.Min.ToString(), bBoxY.y.Max.ToString(), bBoxY.z.Min.ToString(), bBoxY.z.Max.ToString()));
        }
    }
}
