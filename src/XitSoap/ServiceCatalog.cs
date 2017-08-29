using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HodStudio.XitSoap
{
    public static class ServiceCatalog
    {
        public static readonly Dictionary<string, string> Catalog = new Dictionary<string, string>();

        public static string GetServiceAddress(string contractName)
        {
            return Catalog[contractName];
        }
    }
}
