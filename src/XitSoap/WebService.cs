using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using HodStudio.XitSoap.Helpers;

namespace HodStudio.XitSoap
{
    public class WebService
    {
        internal string Url { get; set; }
        internal string Method { get; set; }
        internal string Namespace { get; set; }
        internal Dictionary<string, string> Params = new Dictionary<string, string>();
        public XDocument ResponseSOAP = XDocument.Parse("<root/>");
        public XDocument ResultXML = XDocument.Parse("<root/>");
        public string ResultString = string.Empty;

        internal Dictionary<string, string> mappers = new Dictionary<string, string>();

        public WebService()
        {
        }
        public WebService(string baseUrl, string methodName = "", string @namespace = "http://tempuri.org/")
        {
            Url = baseUrl;
            Method = methodName;
            Namespace = @namespace;
        }

        /// <summary>
        /// Adds a parameter to the WebMethod invocation.
        /// </summary>
        /// <param name="name">Name of the WebMethod parameter (case sensitive)</param>
        /// <param name="value">Value to pass to the paramenter</param>
        public void AddParameter(string name, string value)
        {
            this.AddMethodParameter(name, value);
        }

        /// <summary>
        /// Adds a parameter to the WebMethod invocation, using a type in the value.
        /// </summary>
        /// <param name="name">Name of the WebMethod parameter (case sensitive)</param>
        /// <param name="value">Value to pass to the paramenter</param>
        public void AddParameter<InputType>(string name, InputType value)
        {
            this.AddMethodParameter(name, value);
        }

        /// <summary>
        /// Using the base url, invokes the WebMethod informed in the creation of class
        /// </summary>
        /// <param name="encode">Encode params</param>
        public void Invoke(bool encode = false)
        {
            Invoke(Method, encode);
        }

        /// <summary>
        /// Using the base url, invokes the WebMethod with the given name
        /// </summary>
        /// <param name="methodName">Web Method name</param>
        /// <param name="encode">Encode params</param>
        public void Invoke(string methodName, bool encode = false)
        {
            this.InvokeService(methodName, encode);
        }

        /// <summary>
        /// Cleans all internal data used in the last invocation, except the WebService's URL.
        /// This avoids creating a new WebService object when the URL you want to use is the same.
        /// </summary>
        public void CleanLastInvoke()
        {
            Method = string.Empty;
            Params = new Dictionary<string, string>();
            ResultXML = null;
            ResponseSOAP = ResultXML;
            ResultString = Method;
        }
    }

    /// <summary>
    /// This class is an alternative when you can't use Service References. It allows you to invoke Web Methods on a given Web Service URL.
    /// Based on the code from http://stackoverflow.com/questions/9482773/web-service-without-adding-a-reference
    /// </summary>
    public class WebService<ResultType> : WebService
    {
        public ResultType ResultObject = default(ResultType);

        public WebService(string _methodName = "")
        {
            System.Reflection.MemberInfo info = typeof(ResultType);
            var contract = ((WsContractAttribute)info.GetCustomAttributes(typeof(WsContractAttribute), true).FirstOrDefault());
            if (contract == null)
                throw new ArgumentNullException("Contract", "You tried to invoke a webservice without specifying the WebService's Contract/URL.");
            this.GetMapperAttributes(typeof(ResultType));

            Url = ServiceCatalog.GetServiceAddress(contract.ContractName);
            Method = _methodName;
            Namespace = contract.Namespace;
        }
        public WebService(string baseUrl, string methodName = "", string @namespace = "http://tempuri.org/")
            : base(baseUrl, methodName, @namespace) { }

        /// <summary>
        /// Using the base url, invokes the WebMethod informed in the creation of class
        /// </summary>
        public new void Invoke(bool encode = false)
        {
            base.Invoke(encode);
            ExtractResultClass();
        }

        /// <summary>
        /// Using the base url, invokes the WebMethod with the given name
        /// </summary>
        /// <param name="methodName">Web Method name</param>
        public new void Invoke(string methodName, bool encode = false)
        {
            var originalMethod = Method;
            Method = methodName;
            base.Invoke(methodName, encode);
            ExtractResultClass();
            Method = originalMethod;
        }

        private void ExtractResultClass()
        {
            var methodNameResult = Method + "Result";
            var xmlMapper = ResultString.Replace(methodNameResult, typeof(ResultType).Name);
            xmlMapper = this.ApplyMappers(xmlMapper);

            XmlSerializer serializer = new XmlSerializer(typeof(ResultType));
            using (var rdr = new StringReader(xmlMapper))
            {
                ResultObject = (ResultType)serializer.Deserialize(rdr);
            }
        }

        /// <summary>
        /// Cleans all internal data used in the last invocation, except the WebService's URL.
        /// This avoids creating a new WebService object when the URL you want to use is the same.
        /// </summary>
        public new void CleanLastInvoke()
        {
            base.CleanLastInvoke();
            ResultObject = default(ResultType);
        }
    }
}
