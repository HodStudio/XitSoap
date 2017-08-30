using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HodStudio.XitSoap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HodStudio.XitSoap.Tests.Model;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace HodStudio.XitSoap.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceOfTypeTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ServiceCatalog.Catalog.Add("SystemSecurityService", "http://webservice.systemsecurityservice.com.test/login.asmx");

            ServiceCatalog.Catalog.Add("ProductContract", "http://localhost/XitSoap/ProductService.asmx");
        }

        [TestMethod]
        public void WebServiceTest()
        {
            var ws = new WebService<LoginOutput>();

            Assert.AreEqual(ws.Url, "http://webservice.systemsecurityservice.com.test/login.asmx");
            Assert.AreEqual(ws.Method, string.Empty);
            Assert.AreEqual(ws.Namespace, "http://tempuri.org/");
            Assert.AreEqual(ws.mappers.Count, 0);
        }

        [TestMethod]
        public void WebServiceTest1()
        {
            var url = "http://webservice.systemsecurityservice.com.test";
            var method = "authentication";
            var xmlnamespace = "http://webservice.systemsecurityservice.com.test";
            var ws = new WebService<LoginOutput>(url, method, xmlnamespace);
            Assert.AreEqual(ws.Url, url);
            Assert.AreEqual(ws.Method, method);
            Assert.AreEqual(ws.Namespace, xmlnamespace);
        }

        [MyExpectedException(typeof(Exception), "You tried to invoke a webservice without specifying the WebService's Contract/URL.")]
        [TestMethod]
        public void WebServiceTest2()
        {
            new WebService<LoginWithoutContractOutput>();
        }

        [TestMethod]
        public void InvokeTest()
        {
            var wsCon = new WebService<ProductOutput>("GetProduct");
            wsCon.Invoke();
            var result = wsCon.ResultObject;

            var productResult = CreateProductMock();

            var expected = JsonConvert.SerializeObject(productResult);
            var actual = JsonConvert.SerializeObject(result);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InvokeTest1()
        {
            var wsCon = new WebService<ProductOutput>();
            wsCon.Invoke("GetProduct");
            var result = wsCon.ResultObject;

            var productResult = CreateProductMock();

            var expected = JsonConvert.SerializeObject(productResult);
            var actual = JsonConvert.SerializeObject(result);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InvokeTestConvertMilesToKilometers()
        {
            var wsCon = new WebService<double>("http://www.webservicex.net/ConvertSpeed.asmx", null, "http://www.webserviceX.NET/");
            wsCon.AddParameter("speed", 100D);
            wsCon.AddParameter("FromUnit", "milesPerhour");
            wsCon.AddParameter("ToUnit", "kilometersPerhour");
            wsCon.Invoke("ConvertSpeed");
            var result = wsCon.ResultObject;
            Assert.AreEqual(160.93470878864446D, result);
        }

        [TestMethod]
        public void InvokeTestGetInfoByZip()
        {
            var wsCon = new WebService<CityZipSearch>("http://www.webservicex.net/uszip.asmx", null, "http://www.webserviceX.NET");
            wsCon.AddParameter("USZip", "85001");
            wsCon.Invoke("GetInfoByZIP");
            var result = JsonConvert.SerializeObject(wsCon.ResultObject);
            var expected = JsonConvert.SerializeObject(new CityZipSearch()
            {
                Result = new CityResultSet()
                {
                    City = new CityInfo()
                    {
                        Name = "Phoenix",
                        State = "AZ",
                        ZipCode = 85001,
                        AreaCode = 602,
                        TimeZone = "M"
                    }
                }
            });
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CleanLastInvokeTest()
        {
            var wsCon = new WebService<LoginOutput>();
            WebServiceTests.AddInfoCleanLastInvokeTest(wsCon);
            wsCon.ResultObject = new LoginOutput();

            wsCon.CleanLastInvoke();

            WebServiceTests.AssertInfoCleanLastInvokeTest(wsCon);
            Assert.AreEqual(wsCon.ResultObject, null);
        }

        [TestCleanup]
        public void CleanUp()
        {
            ServiceCatalog.Catalog.Clear();
        }

        private ProductOutput CreateProductMock()
        {
            var product = new Product()
            {
                Name = "Product 01",
                Code = 1,
                Brand = "Brand 01",
                ExpirationDate = new DateTime(2017, 12, 23),
                DeliveryDate = new DateTime(2017, 07, 03),
                SubTitle = "Product 01 Subtitle",
                Price = 18.30M
            };

            var productResult = new ProductOutput()
            {
                Products = new List<Product>() { product }
            };

            return productResult;
        }
    }
}
