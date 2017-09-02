using Microsoft.VisualStudio.TestTools.UnitTesting;
using HodStudio.XitSoap.Tests.Model;
using HodStudio.XitSoap.Helpers;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Diagnostics.CodeAnalysis;

namespace HodStudio.XitSoap.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceTests
    {
        [TestMethod]
        public void WebServiceWithoutParametersTest()
        {
            var ws = new WebService();
            Assert.AreEqual(string.Empty, ws.Url);
            Assert.AreEqual(StringConstants.DefaultNamespace, ws.Namespace);
        }

        [TestMethod]
        public void WebServiceWithParametersTest()
        {
            var url = "http://webservice.systemsecurityservice.com.test";
            var xmlnamespace = "http://webservice.systemsecurityservice.com.test";
            var ws = new WebService(url, xmlnamespace);

            Assert.AreEqual(ws.Url, url);
            Assert.AreEqual(ws.Namespace, xmlnamespace);
        }

        [TestMethod]
        public void AddSimpleParameterTest()
        {
            var wsCon = new WebService();
            var paramName = "abc";
            var paramValue = "xyz";
            wsCon.AddParameter(paramName, paramValue);

            Assert.AreEqual(wsCon.Parameters.Count, 1);
            Assert.AreEqual(wsCon.Parameters[paramName], paramValue);
        }

        [TestMethod]
        public void AddComplexParameterTest()
        {
            var wsCon = new WebService();
            var paramName = "abc";
            var paramValue = new LoginInput()
            {
                Username = "usu01",
                Password = "password01",
                System = "XitSoap"
            };
            wsCon.AddParameter(paramName, paramValue);

            var xmlSerializer = new XmlSerializer(typeof(LoginInput));
            var completeXml = new XmlDocument();
            using (var mr = new StringWriter())
            {
                xmlSerializer.Serialize(mr, paramValue);
                completeXml.LoadXml(XmlHelpers.RemoveNamespaces(mr.ToString()).ToString());
            }

            Assert.AreEqual(wsCon.Parameters.Count, 1);
            Assert.AreEqual(wsCon.Parameters[paramName], completeXml.DocumentElement.InnerXml);
        }

        [TestMethod]
        public void InvokeReturningDefaultTest()
        {
            var wsCon = new WebService("http://localhost/XitSoap/ProductService.asmx");
            var actual = wsCon.Invoke("GetProduct").XmlResult;

            var expected = System.Xml.Linq.XDocument.Parse(@"<GetProductResult>
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
</GetProductResult>");

            //TODO: show how to use the library and convert the XML to Json

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void InvokeReturningBoolTest()
        {
            var wsCon = new WebService("http://localhost/XitSoap/ProductService.asmx");
            var actual = wsCon.Invoke<bool>("IsValid");
            var expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InvokeTestUsingGlobalWeatherReturningString()
        {
            var wsCon = new WebService("http://www.webservicex.com/globalweather.asmx", "http://www.webserviceX.NET");
            wsCon.AddParameter("CountryName", "CZE");
            var actualString = wsCon.Invoke("GetCitiesByCountry").StringResult;
            var expectedString = "&lt;NewDataSet&gt;\r\n  &lt;Table&gt;\r\n    &lt;Country&gt;Czech Republic&lt;/Country&gt;\r\n    &lt;City&gt;Holesov&lt;/City&gt;\r\n  &lt;/Table&gt;\r\n  &lt;Table&gt;\r\n    &lt;Country&gt;Czech Republic&lt;/Country&gt;\r\n    &lt;City&gt;Karlovy Vary&lt;/City&gt;\r\n  &lt;/Table&gt;\r\n  &lt;Table&gt;\r\n    &lt;Country&gt;Czech Republic&lt;/Country&gt;\r\n    &lt;City&gt;Ostrava / Mosnov&lt;/City&gt;\r\n  &lt;/Table&gt;\r\n  &lt;Table&gt;\r\n    &lt;Country&gt;Czech Republic&lt;/Country&gt;\r\n    &lt;City&gt;Praha / Ruzyne&lt;/City&gt;\r\n  &lt;/Table&gt;\r\n  &lt;Table&gt;\r\n    &lt;Country&gt;Czech Republic&lt;/Country&gt;\r\n    &lt;City&gt;Brno / Turany&lt;/City&gt;\r\n  &lt;/Table&gt;\r\n&lt;/NewDataSet&gt;";
            Assert.AreEqual(expectedString, actualString);
        }

        //[TestMethod]
        //public void CleanLastInvokeTest()
        //{
        //    var wsCon = new WebService();
        //    AddInfoCleanLastInvokeTest(wsCon);

        //    wsCon.CleanLastInvoke();

        //    AssertInfoCleanLastInvokeTest(wsCon);
        //}

        //internal static void AddInfoCleanLastInvokeTest(WebService wsCon)
        //{
        //    var paramName = "abc";
        //    var paramValue = "xyz";
        //    wsCon.AddParameter(paramName, paramValue);

        //    wsCon.ResultString = "true";
        //    wsCon.ResultXml = new System.Xml.Linq.XDocument();
        //    wsCon.ResponseSoap = new System.Xml.Linq.XDocument();
        //}

        //internal static void AssertInfoCleanLastInvokeTest(WebService wsCon)
        //{
        //    Assert.AreEqual(wsCon.ResultString, string.Empty);
        //    Assert.AreEqual(wsCon.ResultXml, null);
        //    Assert.AreEqual(wsCon.ResponseSoap, null);
        //    Assert.AreEqual(wsCon.Method, string.Empty);
        //    Assert.AreEqual(wsCon.Params.Count, 0);
        //}
    }
}
