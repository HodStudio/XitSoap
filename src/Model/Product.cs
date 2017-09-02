using System;

namespace HodStudio.XitSoap.Tests.Model
{
    public class Product
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public string Brand { get; set; }
        public string SubTitle { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal Price { get; set; }
    }
}
