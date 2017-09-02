using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace HodStudio.XitSoap.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WsContractAttributeTests
    {
        [TestMethod]
        public void WsContractAttributeTest()
        {
            var ws = new WsContractAttribute("abc");

            Assert.AreEqual(ws.ContractName, "abc");
        }

        [TestMethod]
        public void WsContractAttributeTest1()
        {
            var ws = new WsContractAttribute("abc", "xyz");

            Assert.AreEqual(ws.ContractName, "abc");
            Assert.AreEqual(ws.Namespace, "xyz");
        }
    }
}
