using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer2.Parsers;
using RayTracer2.Geometry;

namespace RayTracerTests
{
    /// <summary>
    /// Summary description for ParserTests
    /// </summary>
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void PlyParser_ParseFileTest()
        {
            PlyParser testParser = new PlyParser();

            List<Triangle> triangles = new List<Triangle>();
            triangles.Add(new Triangle(new Point(1, 0, 1), new Point(1, 1, -1), new Point(1, -1, -1), null));
            
            Mesh expectedShape = new Mesh(triangles, null);

            string[] lines = {"ply",
                                "format ascii 1.0",
                                "element vertex 3",
                                "property float x",
                                "property float y",
                                "property float z",
                                "element face 1",
                                "property list uchar int vertex_indices",
                                "end_header",
                                "1 0 1",
                                "1 1 -1",
                                "1 -1 -1",
                                "3 0 1 2"
                              };

            IShape testShape = testParser.ParsePly(lines, 1, null);

            Assert.IsTrue(expectedShape.GetType() == testShape.GetType(), string.Format( "TestShape wasn't the right type. testShape type: {0}; expectedShape Type: {1}", testShape.GetType(), expectedShape.GetType()));
            Mesh testMesh = (Mesh)testShape;
            //Assert.IsTrue(expectedShape.GetTriangles()[0] == testMesh.GetTriangles()[0], "testMesh is not equal to expectedMesh.");
        }
    }
}
