using HodStudio.XitSoap.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace HodStudio.XitSoap.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BearerTokenAuthenticationTests
    {
        [TestMethod]
        public void BearerTokenAuthenticationTest()
        {
            var ws = new WebService();
            ws.SetAuthentication(new BearerTokenAuthentication("xpto1234!@#$"));

            Assert.IsNotNull(ws.AuthenticationInfo);
            Assert.AreEqual(ws.AuthenticationInfo.AuthenticationHeader, "Bearer xpto1234!@#$");
        }
    }
}
