using System.Collections.Generic;

namespace HodStudio.XitSoap.Tests.Model
{
    [WsContract("ProductContract")]
    public class ProductOutput : ApiResult
    {
        /// <summary>
        /// Products' list
        /// </summary>
        public List<Product> Products { get; set; }
    }
}
