using System.Collections.Generic;

namespace HodStudio.XitSoap
{
    public static class ServiceCatalog
    {
        public static Dictionary<string, string> Catalog { get; private set; } = new Dictionary<string, string>();

        public static string GetServiceAddress(string contractName)
        {
            return Catalog[contractName];
        }
    }
}
