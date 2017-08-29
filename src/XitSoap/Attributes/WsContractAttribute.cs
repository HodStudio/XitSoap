using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HodStudio.XitSoap
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WsContractAttribute : System.Attribute
    {
        public readonly string ContractName;
        public readonly string Namespace;

        public WsContractAttribute(string _contractName)
        {
            this.ContractName = _contractName;
            this.Namespace = "http://tempuri.org/";
        }

        public WsContractAttribute(string _contractName, string _namespace)
        {
            this.ContractName = _contractName;
            this.Namespace = _namespace;
        }
    }
}
