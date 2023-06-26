using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlueDragon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueDragon.Data.Tests
{
    [TestClass()]
    public class HccContextTests
    {
        [TestMethod()]
        public void HccContextTest()
        {
            var a = 1;
            Assert.IsTrue(a == 1);
        }

        [TestMethod()]
        public void HccContextTest1()
        {
            var a = 1;
            Assert.IsTrue(a == 2);
        }
    }
}