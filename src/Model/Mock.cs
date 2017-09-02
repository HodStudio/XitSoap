using System;
using System.Collections.Generic;

namespace HodStudio.XitSoap.Tests.Model
{
    public static class Mock
    {
        public static Product CreateProduct()
        {
            return new Product()
            {
                Name = "Product 01",
                Code = 1,
                Brand = "Brand 01",
                ExpirationDate = new DateTime(2017, 12, 23),
                DeliveryDate = new DateTime(2017, 07, 03),
                SubTitle = "Product 01 Subtitle",
                Price = 18.30M
            };
        }

        public static ProductOutput CreateProductOutput()
        {
            return new ProductOutput()
            {
                Products = new List<Product>() { CreateProduct() }
            };
        }
    }
}
