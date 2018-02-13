using HodStudio.XitSoap.Tests.Model;
using System;
using System.Collections.Generic;
using System.Web.Services;

namespace Web
{
    /// <summary>
    /// Summary description for Products
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ProductService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public ProductOutput GetProduct()
        {
            return CreateProduct();
        }

        [WebMethod]
        public bool IsValid()
        {
            return true;
        }

        [WebMethod]
        public ProductOutput SearchProduct(ProductFilter input)
        {
            return CreateProduct();
        }

        private ProductOutput CreateProduct()
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


            var productOutput = new ProductOutput() { Products = new List<Product>() { product } };
            return productOutput;
        }
    }
}
