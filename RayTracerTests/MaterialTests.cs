using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using RayTracer2;

namespace RayTracerTests
{
    [TestClass]
    public class MaterialTests
    {
        [TestMethod]
        public void Color_AdditionTest()
        {
            Color color1 = new Color();
            Color color2 = new Color(1, 0, 0);
            Color color3 = new Color(0, 1, 0);
            Color color4 = new Color(0, 0, 1);
            Color color5 = new Color(1, 1, 1);

            Color result1 = color1 + color2;
            Color result2 = color1 + color3;
            Color result3 = color1 + color4;

            Color result4 = color1 + color5;
            Color result5 = color2 + color5;
            Color result6 = color3 + color5;
            Color result7 = color4 + color5;

            Assert.IsTrue(result1 == color2, String.Format("Values don't match.  Expected Value: ({0}, {1}, {2}); Actual Value: ({3}, {4}, {5})", color2.r, color2.g, color2.b, result1.r, result1.g, result1.b));
            Assert.IsTrue(result2 == color3, String.Format("Values don't match.  Expected Value: ({0}, {1}, {2}); Actual Value: ({3}, {4}, {5})", color3.r, color3.g, color3.b, result2.r, result2.g, result2.b));
            Assert.IsTrue(result3 == color4, String.Format("Values don't match.  Expected Value: ({0}, {1}, {2}); Actual Value: ({3}, {4}, {5})", color4.r, color4.g, color4.b, result3.r, result3.g, result3.b));

            Assert.IsTrue(result4 == color5, String.Format("Values don't match.  Expected Value: ({0}, {1}, {2}); Actual Value: ({3}, {4}, {5})", color5.r, color5.g, color5.b, result4.r, result4.g, result4.b));
            Assert.IsTrue(result5 == color5, String.Format("Values don't match.  Expected Value: ({0}, {1}, {2}); Actual Value: ({3}, {4}, {5})", color5.r, color5.g, color5.b, result5.r, result5.g, result5.b));
            Assert.IsTrue(result6 == color5, String.Format("Values don't match.  Expected Value: ({0}, {1}, {2}); Actual Value: ({3}, {4}, {5})", color5.r, color5.g, color5.b, result6.r, result6.g, result6.b));
            Assert.IsTrue(result7 == color5, String.Format("Values don't match.  Expected Value: ({0}, {1}, {2}); Actual Value: ({3}, {4}, {5})", color5.r, color5.g, color5.b, result7.r, result7.g, result7.b));
        }
    }
}
