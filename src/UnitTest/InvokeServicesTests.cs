using Microsoft.VisualStudio.TestTools.UnitTesting;
using HodStudio.XitSoap.Tests.Model;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace HodStudio.XitSoap.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class InvokeServicesTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ServiceCatalog.Catalog.Add("ProductContract", "http://localhost/XitSoap/ProductService.asmx");
            ServiceCatalog.Catalog.Add("UsZipContract", "http://www.webservicex.net/uszip.asmx");
        }

        [TestMethod]
        public void InvokeGetProductTest()
        {
            var wsCon = new WebService();
            var result = wsCon.Invoke<ProductOutput>("GetProduct");

            var productResult = Mock.CreateProductOutput();

            var expected = JsonConvert.SerializeObject(productResult);
            var actual = JsonConvert.SerializeObject(result);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InvokeConvertSpeedTest()
        {
            var wsCon = new WebService("http://www.webservicex.net/ConvertSpeed.asmx", "http://www.webserviceX.NET/");
            wsCon.AddParameter("speed", 100D);
            wsCon.AddParameter("FromUnit", "milesPerhour");
            wsCon.AddParameter("ToUnit", "kilometersPerhour");
            var result = wsCon.Invoke<double>("ConvertSpeed");
            Assert.AreEqual(160.93470878864446D, result);
        }

        [TestMethod]
        public void InvokeUsZipWithoutContractTest()
        {
            var wsCon = new WebService("http://www.webservicex.net/uszip.asmx", "http://www.webserviceX.NET");
            wsCon.AddParameter("USZip", "85001");
            var result = JsonConvert.SerializeObject(wsCon.Invoke<CityZipSearch>("GetInfoByZIP"));
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
        public void InvokeUsZipWithoutUrlAndWithNamespaceTest()
        {
            var wsCon = new WebService(string.Empty, "http://www.webserviceX.NET");
            wsCon.AddParameter("USZip", "85001");
            var result = JsonConvert.SerializeObject(wsCon.Invoke<CityZipSearchWithContractAndWithoutNamespace>("GetInfoByZIP"));
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
        public void InvokeUsZipWithoutUrlAndNamespaceTest()
        {
            var wsCon = new WebService();
            wsCon.AddParameter("USZip", "85001");
            var result = JsonConvert.SerializeObject(wsCon.Invoke<CityZipSearchWithContractAndNamespace>("GetInfoByZIP"));
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
        public void InvokeUsZipWithUrlAndWithoutNamespaceTest()
        {
            var wsCon = new WebService("http://www.webservicex.net/uszip.asmx");
            wsCon.AddParameter("USZip", "85001");
            var result = JsonConvert.SerializeObject(wsCon.Invoke<CityZipSearchWithContractForOnlyNamespace>("GetInfoByZIP"));
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
            Assert.AreEqual("http://www.webservicex.net/uszip.asmx", wsCon.Url);
        }

        [TestCleanup]
        public void CleanUp()
        {
            ServiceCatalog.Catalog.Clear();
        }
    }
}
