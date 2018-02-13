using HodStudio.XitSoap.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace HodStudio.XitSoap.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BasicAuthenticationTests
    {
        [TestMethod]
        public void BasicAuthenticationTest()
        {
            var ws = new WebService();
            ws.SetAuthentication(new BasicAuthentication("user", "password"));

            Assert.IsNotNull(ws.AuthenticationInfo);
            Assert.AreEqual(ws.AuthenticationInfo.AuthenticationHeader, "Basic dXNlcjpwYXNzd29yZA==");
        }
    }
}
