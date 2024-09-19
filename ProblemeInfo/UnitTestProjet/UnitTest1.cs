using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ProblemeInfo;

namespace ProblemeInfo
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ProblemeInfo.Pixel pixel = new ProblemeInfo.Pixel(0,0,0);
            Assert.AreEqual(0, pixel.moyenne());
        }
        [TestMethod]
        public void TestMethod2()
        {
            ProblemeInfo.Point point = new ProblemeInfo.Point(0, 0);
            point.Carre();
            Assert.AreEqual(0, point.X);
        }
        [TestMethod]
        public void TestMethod3()
        {
            ProblemeInfo.Pixel pixel = new ProblemeInfo.Pixel(5, 5, 5);
            Assert.AreEqual("5 5 5", pixel.ToString());
        }
        [TestMethod]
        public void TestMethod4()
        {
            ProblemeInfo.Point point = new ProblemeInfo.Point(0, 0);
            Assert.AreEqual(0, point.Norme());
        }
        [TestMethod]
        public void TestMethod5()
        {
            List<string[]> tab = new List<string[]>();
            Assert.AreEqual(-1,MyImage.TestPresence(tab,"3"));
        }
    }
}