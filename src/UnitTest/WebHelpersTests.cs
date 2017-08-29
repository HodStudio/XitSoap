using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HodStudio.XitSoap;
using HodStudio.XitSoap.Tests;
using System.Diagnostics.CodeAnalysis;
using HodStudio.XitSoap.Tests.Model;

namespace UnitTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebHelpersTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ServiceCatalog.Catalog.Add("ProductContract", "http://localhost/XitSoap/ProductService.asmx");
        }

        [MyExpectedException(typeof(ArgumentNullException), "You tried to invoke a webservice without specifying the WebService's URL.")]
        [TestMethod]
        public void AssertCanInvokeTest()
        {
            var wsCon = new WebService();
            wsCon.Invoke();
        }

        [MyExpectedException(typeof(ArgumentNullException), "You tried to invoke a webservice without specifying the WebMethod.")]
        [TestMethod]
        public void AssertCanInvokeTest1()
        {
            var wsCon = new WebService("abc");
            wsCon.Invoke();
        }

        [TestMethod]
        public void InputTest()
        {
            var wsCon = new WebService<ProductOutput>("SearchProduct");
            wsCon.AddParameter("input", new ProductInput() { Code = "xpto" });
            wsCon.Invoke();

            var actual = wsCon.ResultXml;

            var expected = System.Xml.Linq.XDocument.Parse(@"<SearchProductResult>
  <ReturnCode>1</ReturnCode>
  <Errors />
  <Products>
    <Product>
      <Name>Product 01</Name>
      <Code>1</Code>
      <Brand>Brand 01</Brand>
      <SubTitle>Product 01 Subtitle</SubTitle>
      <ExpirationDate>2017-12-23T00:00:00</ExpirationDate>
      <DeliveryDate>2017-07-03T00:00:00</DeliveryDate>
      <Price>18.30</Price>
    </Product>
  </Products>
</SearchProductResult>");

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestCleanup]
        public void CleanUp()
        {
            ServiceCatalog.Catalog.Clear();
        }
    }
}
