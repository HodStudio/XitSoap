using HodStudio.XitSoap.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
namespace HodStudio.XitSoap
{
    class WebService
    {
        #region Properties
        internal Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
        internal Dictionary<string, string> ParametersMappers { get; set; } = new Dictionary<string, string>();
        internal Dictionary<string, string> ResponseMappers { get; set; } = new Dictionary<string, string>();

        public string Url { get; private set; }
        public string Namespace { get; private set; }

        internal WebServiceResult Result { get; set; }

        private readonly string DefaultNamespace;
        private readonly string DefaultUrl;
        #endregion

        #region Constructors
        public WebService()
            : this(string.Empty, StringConstants.DefaultNamespace) { }
        public WebService(string url)
           : this(url, StringConstants.DefaultNamespace) { }
        public WebService(string url, string @namespace)
        {
            Url = url;
            Namespace = @namespace;
            DefaultUrl = url;
            DefaultNamespace = @namespace;
        }
        #endregion

        #region Parameters
        /// <summary>
        /// Adds a parameter to the WebMethod invocation, using a type in the value.
        /// </summary>
        /// <param name="name">Name of the WebMethod parameter (case sensitive)</param>
        /// <param name="value">Value to pass to the paramenter</param>
        public void AddParameter<InputType>(string name, InputType value)
        {
            this.AddMethodParameter(name, value);
        }
        #endregion

        #region Invoke Methods
        /// <summary>
        /// Using the base url, invokes the WebMethod with the given name
        /// </summary>
        /// <param name="methodName">Web Method name</param>
        public WebServiceResult Invoke(string methodName)
        {
            return Invoke(methodName, false);
        }
        /// <summary>
        /// Using the base url, invokes the WebMethod with the given name
        /// </summary>
        /// <param name="methodName">Web Method name</param>
        /// <param name="encode">Encode params</param>
        public WebServiceResult Invoke(string methodName, bool encode)
        {
            Invoke<object>(methodName, encode);
            return Result;
        }
        /// <summary>
        /// Using the base url, invokes the WebMethod with the given name
        /// </summary>
        /// <param name="methodName">Web Method name</param>
        public ResultType Invoke<ResultType>(string methodName)
        { return Invoke<ResultType>(methodName, false); }
        /// <summary>
        /// Using the base url, invokes the WebMethod with the given name
        /// </summary>
        /// <param name="methodName">Web Method name</param>
        /// <param name="encode">Encode params</param>
        public ResultType Invoke<ResultType>(string methodName, bool encode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(methodName))
                    throw new ArgumentNullException("methodName", "You tried to invoke a webservice without specifying the WebMethod.");
                MemberInfo info = typeof(ResultType);
                var contract = ((WsContractAttribute)info.GetCustomAttributes(typeof(WsContractAttribute), true).FirstOrDefault());
                if (string.IsNullOrEmpty(Url) && contract == null)
                    throw new MethodAccessException("You tried to invoke a webservice without specifying the WebService's Contract/URL.");
                ValidateContract(contract);
                
                Result = new WebServiceResult();
                ResponseMappers.GetMapperAttributes(typeof(ResultType));
                this.InvokeService(methodName, encode);
                return ExtractResultClass<ResultType>(methodName);
            }
            finally
            {
                CleanLastInvoke();
            }
        }
        #endregion

        private void ValidateContract(WsContractAttribute contract)
        {
            if (contract == null)
                return;

            string contractUrl = null;

            if (!ServiceCatalog.Catalog.ContainsKey(contract.ContractName))
            {
                if (string.IsNullOrEmpty(Url))
                    throw new KeyNotFoundException("The contract was not found on the Service Catalog.");
            }
            else
                contractUrl = ServiceCatalog.Catalog[contract.ContractName];

            if (!string.IsNullOrEmpty(contractUrl))
            {
                if (!string.IsNullOrEmpty(Url) && contractUrl != Url)
                    throw new AmbiguousMatchException("The URL's contract is different from the URL informed on the WebService constructor.");
                else
                    Url = contractUrl;
            }

            if (!string.IsNullOrWhiteSpace(contract.Namespace))
                Namespace = contract.Namespace;
        }

        private ResultType ExtractResultClass<ResultType>(string methodName)
        {
            if (typeof(ResultType) == typeof(object))
                return default(ResultType);
            var methodNameResult = methodName + "Result";
            if (!Result.StringResult.Contains(methodNameResult))
                return (ResultType)Convert.ChangeType(Result.StringResult, typeof(ResultType));
            var xmlMapper = Result.StringResult.Replace(methodNameResult, typeof(ResultType).Name);
            xmlMapper = ResponseMappers.ApplyMappers(xmlMapper);
            XmlSerializer serializer = new XmlSerializer(typeof(ResultType));
            var rdr = new StringReader(xmlMapper);
            var result = (ResultType)serializer.Deserialize(rdr);
            rdr.Close();
            return result;
        }

        private void CleanLastInvoke()
        {
            Url = DefaultUrl;
            Namespace = DefaultNamespace;
            Parameters.Clear();
            ResponseMappers.Clear();
            ParametersMappers.Clear();
        }
    }
}