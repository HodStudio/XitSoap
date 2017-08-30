using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace HodStudio.XitSoap.Helpers
{
    internal static class WebHelpers
    {
        /// <summary>
        /// Checks if the WebService's URL and the WebMethod's name are valid. If not, throws ArgumentNullException.
        /// </summary>
        /// <param name="methodName">Web Method name (optional)</param>
        internal static void AssertCanInvoke(this WebService service, string methodName = "")
        {
            if (string.IsNullOrEmpty(service.Url))
                throw new ArgumentNullException("Url", "You tried to invoke a webservice without specifying the WebService's URL.");
            if (string.IsNullOrEmpty(methodName) && string.IsNullOrEmpty(service.Method))
                throw new ArgumentNullException("Method", "You tried to invoke a webservice without specifying the WebMethod.");
        }

        internal static void ExtractResult(this WebService service, string methodName)
        {
            // Selects just the elements with namespace http://tempuri.org/ (i.e. ignores SOAP namespace)
            XmlNamespaceManager namespMan = new XmlNamespaceManager(new NameTable());
            namespMan.AddNamespace(StringConstants.XmlResultDummyNamespace, service.Namespace);
            var methodNameResult = string.Format(StringConstants.XmlResultResultFormat, methodName);

            XElement webMethodResult = service.ResponseSoap.XPathSelectElement(string.Format(StringConstants.XmlResultXPathSelectorFormat, methodNameResult), namespMan);
            // If the result is an XML, return it and convert it to string
            if (webMethodResult.FirstNode.NodeType == XmlNodeType.Element)
            {
                service.ResultXml = XDocument.Parse(webMethodResult.ToString());
                service.ResultXml = XmlHelpers.RemoveNamespaces(service.ResultXml);
                service.ResultString = service.ResultXml.ToString();
            }
            // If the result is a string, return it and convert it to XML (creating a root node to wrap the result)
            else
            {
                service.ResultString = webMethodResult.FirstNode.ToString();
                service.ResultXml = XDocument.Parse(string.Format(StringConstants.XmlResultXDocumentFormat, service.ResultString));
            }
        }

        /// <summary>
        /// Invokes a Web Method, with its parameters encoded or not.
        /// </summary>
        /// <param name="methodName">Name of the web method you want to call (case sensitive)</param>
        /// <param name="encode">Do you want to encode your parameters? (default: true)</param>
        internal static void InvokeService(this WebService service, string methodName, bool encode)
        {
            service.AssertCanInvoke(methodName);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(service.Url);
            req.Headers.Add(StringConstants.SoapHeaderName, CreateSoapHeaderName(service.Namespace, methodName));
            req.ContentType = StringConstants.SoapContentType;
            req.Accept = StringConstants.SoapAccept;
            req.Method = StringConstants.SoapMethod;

            using (Stream stm = req.GetRequestStream())
            {
                var postValues = new StringBuilder();
                foreach (var param in service.Params)
                {
                    if (encode) postValues.AppendFormat(StringConstants.SoapParamFormat, HttpUtility.HtmlEncode(param.Key), HttpUtility.HtmlEncode(param.Value));
                    else postValues.AppendFormat(StringConstants.SoapParamFormat, param.Key, param.Value);
                }
                postValues = service.ApplyMappersInput(postValues);

                var soapStr = string.Format(StringConstants.SoapStringFormat, methodName, postValues.ToString(), service.Namespace);
                using (StreamWriter stmw = new StreamWriter(stm))
                {
                    stmw.Write(soapStr);
                }
            }

            using (StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                string result = responseReader.ReadToEnd();
                service.ResponseSoap = XDocument.Parse(result);
                service.ExtractResult(methodName);
            }
        }

        private static string CreateSoapHeaderName(string @namespace, string methodName)
        {
            var fixedNamespace = @namespace;
            if (fixedNamespace.EndsWith("/"))
                fixedNamespace = fixedNamespace.Substring(0, fixedNamespace.Length - 1);
            return string.Format(StringConstants.SoapHeaderFormat, fixedNamespace, methodName);
        }
    }
}
