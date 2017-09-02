using System;

namespace HodStudio.XitSoap
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WsContractAttribute : System.Attribute
    {
        public readonly string ContractName;
        public readonly string Namespace;

        public WsContractAttribute(string contractName)
            : this(contractName, string.Empty) { }
        public WsContractAttribute(string contractName, string @namespace)
        {
            ContractName = contractName;
            Namespace = @namespace;
        }
    }
}
