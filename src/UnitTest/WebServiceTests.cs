using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HodStudio.XitSoap;
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
        public void WebServiceTest()
        {
            var ws = new WebService();
            Assert.AreEqual(ws.Url, null);
            Assert.AreEqual(ws.Method, null);
            Assert.AreEqual(ws.Namespace, null);
        }

        [TestMethod]
        public void WebServiceTest1()
        {
            var url = "http://webservice.systemsecurityservice.com.test";
            var method = "authentication";
            var xmlnamespace = "http://webservice.systemsecurityservice.com.test";
            var ws = new WebService(url, method, xmlnamespace);
            Assert.AreEqual(ws.Url, url);
            Assert.AreEqual(ws.Method, method);
            Assert.AreEqual(ws.Namespace, xmlnamespace);
        }

        [TestMethod]
        public void AddParameterTest()
        {
            var wsCon = new WebService();
            var paramName = "abc";
            var paramValue = "xyz";
            wsCon.AddParameter(paramName, paramValue);

            Assert.AreEqual(wsCon.Params.Count, 1);
            Assert.AreEqual(wsCon.Params[paramName], paramValue);
        }

        [TestMethod]
        public void AddParameterTest1()
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

            Assert.AreEqual(wsCon.Params.Count, 1);
            Assert.AreEqual(wsCon.Params[paramName], completeXml.DocumentElement.InnerXml);
        }

        [TestMethod]
        public void InvokeTest()
        {
            var wsCon = new WebService("http://localhost/XitSoap/ProductService.asmx", "GetProduct");
            wsCon.Invoke();
            var actual = wsCon.ResultXml;

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

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void InvokeTest1()
        {
            var wsCon = new WebService("http://localhost/XitSoap/ProductService.asmx", "IsValid");
            wsCon.Invoke();
            var actualString = wsCon.ResultString;
            var expectedString = "true";

            var actualXml = wsCon.ResultXml;
            var expectedXml = "<root>true</root>";

            Assert.AreEqual(expectedString, actualString);
            Assert.AreEqual(expectedXml, actualXml.ToString());
        }

        [TestMethod]
        public void InvokeTestUsingGlobalWeatherReturningString()
        {
            var wsCon = new WebService("http://www.webservicex.com/globalweather.asmx", "GetCitiesByCountry", "http://www.webserviceX.NET");
            wsCon.AddParameter("CountryName", "CZE");
            wsCon.Invoke();
            var actualString = wsCon.ResultString;
            var expectedString = "&lt;NewDataSet&gt;\r\n  &lt;Table&gt;\r\n    &lt;Country&gt;Czech Republic&lt;/Country&gt;\r\n    &lt;City&gt;Holesov&lt;/City&gt;\r\n  &lt;/Table&gt;\r\n  &lt;Table&gt;\r\n    &lt;Country&gt;Czech Republic&lt;/Country&gt;\r\n    &lt;City&gt;Karlovy Vary&lt;/City&gt;\r\n  &lt;/Table&gt;\r\n  &lt;Table&gt;\r\n    &lt;Country&gt;Czech Republic&lt;/Country&gt;\r\n    &lt;City&gt;Ostrava / Mosnov&lt;/City&gt;\r\n  &lt;/Table&gt;\r\n  &lt;Table&gt;\r\n    &lt;Country&gt;Czech Republic&lt;/Country&gt;\r\n    &lt;City&gt;Praha / Ruzyne&lt;/City&gt;\r\n  &lt;/Table&gt;\r\n  &lt;Table&gt;\r\n    &lt;Country&gt;Czech Republic&lt;/Country&gt;\r\n    &lt;City&gt;Brno / Turany&lt;/City&gt;\r\n  &lt;/Table&gt;\r\n&lt;/NewDataSet&gt;";
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void CleanLastInvokeTest()
        {
            var wsCon = new WebService();
            AddInfoCleanLastInvokeTest(wsCon);

            wsCon.CleanLastInvoke();

            AssertInfoCleanLastInvokeTest(wsCon);
        }

        internal static void AddInfoCleanLastInvokeTest(WebService wsCon)
        {
            var paramName = "abc";
            var paramValue = "xyz";
            wsCon.AddParameter(paramName, paramValue);

            wsCon.ResultString = "true";
            wsCon.ResultXml = new System.Xml.Linq.XDocument();
            wsCon.ResponseSoap = new System.Xml.Linq.XDocument();
        }

        internal static void AssertInfoCleanLastInvokeTest(WebService wsCon)
        {
            Assert.AreEqual(wsCon.ResultString, string.Empty);
            Assert.AreEqual(wsCon.ResultXml, null);
            Assert.AreEqual(wsCon.ResponseSoap, null);
            Assert.AreEqual(wsCon.Method, string.Empty);
            Assert.AreEqual(wsCon.Params.Count, 0);
        }
    }
}
