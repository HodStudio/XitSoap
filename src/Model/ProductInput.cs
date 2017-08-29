using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HodStudio.XitSoap.Tests.Model
{
    public class ProductInput
    {
        /// <summary>
        /// Code for search product. In this case, the property on the webservice has a different name from the model.
        /// </summary>
        [WsMapper("ProductCode")]
        public string Code { get; set; }
    }
}
