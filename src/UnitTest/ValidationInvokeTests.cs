using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HodStudio.XitSoap;
using HodStudio.XitSoap.Tests;
using System.Diagnostics.CodeAnalysis;
using HodStudio.XitSoap.Tests.Model;
using System.Reflection;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ValidationInvokeTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ServiceCatalog.Catalog.Add("ProductContract", "http://localhost/XitSoap/ProductService.asmx");
        }

        [MyExpectedException(typeof(ArgumentNullException), "You tried to invoke a webservice without specifying the WebMethod.")]
        [TestMethod]
        public void AssertCanInvokeTestFailEmptyWebMethod()
        {
            var wsCon = new WebService("test");
            wsCon.Invoke(string.Empty);
        }

        [MyExpectedException(typeof(MethodAccessException), "You tried to invoke a webservice without specifying the WebService's Contract/URL.")]
        [TestMethod]
        public void AssertCanInvokeTestFailEmptyUrl()
        {
            var wsCon = new WebService();
            wsCon.Invoke("test");
        }

        [MyExpectedException(typeof(KeyNotFoundException), "The contract was not found on the Service Catalog.")]
        [TestMethod]
        public void AssertCanInvokeTestFailDoesntExistOnCatalog()
        {
            var wsCon = new WebService();
            wsCon.Invoke<ClassWithWrongContract>("test");
        }

        [MyExpectedException(typeof(AmbiguousMatchException), "The URL's contract is different from the URL informed on the WebService constructor.")]
        [TestMethod]
        public void AssertCanInvokeTestFailDifferentUrl()
        {
            var wsCon = new WebService("http://test.com");
            wsCon.Invoke<ProductOutput>("test");
        }

        [TestCleanup]
        public void CleanUp()
        {
            ServiceCatalog.Catalog.Clear();
        }
    }
}
