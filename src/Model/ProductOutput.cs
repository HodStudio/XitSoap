using HodStudio.XitSoap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
