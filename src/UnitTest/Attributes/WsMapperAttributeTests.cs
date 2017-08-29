using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HodStudio.XitSoap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace HodStudio.XitSoap.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WsMapperAttributeTests
    {
        [TestMethod]
        public void WsMapperAttributeTest()
        {
            var ws = new WsMapperAttribute("abc");

            Assert.AreEqual(ws.Source, "abc");
        }
    }
}
