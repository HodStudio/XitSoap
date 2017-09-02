using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace HodStudio.XitSoap.Helpers
{
    internal static class WebHelpers
    {
        internal static void ExtractResult(this WebService service, string methodName)
        {
            // Selects just the elements with namespace http://tempuri.org/ (i.e. ignores SOAP namespace)
            XmlNamespaceManager namespMan = new XmlNamespaceManager(new NameTable());
            namespMan.AddNamespace(StringConstants.XmlResultDummyNamespace, service.Namespace);
            var methodNameResult = string.Format(StringConstants.XmlResultResultFormat, methodName);

            XElement webMethodResult = service.Result.SoapResponse.XPathSelectElement(string.Format(StringConstants.XmlResultXPathSelectorFormat, methodNameResult), namespMan);
            // If the result is an XML, return it and convert it to string
            if (webMethodResult.FirstNode.NodeType == XmlNodeType.Element)
            {
                var xmlResult = XDocument.Parse(webMethodResult.ToString());
                service.Result.XmlResult = XmlHelpers.RemoveNamespaces(xmlResult);
                service.Result.StringResult = service.Result.XmlResult.ToString();
            }
            // If the result is a string, return it and convert it to XML (creating a root node to wrap the result)
            else
            {
                service.Result.StringResult = webMethodResult.FirstNode.ToString();
                service.Result.XmlResult = XDocument.Parse(string.Format(StringConstants.XmlResultXDocumentFormat, service.Result.StringResult));
            }
        }

        /// <summary>
        /// Invokes a Web Method, with its parameters encoded or not.
        /// </summary>
        /// <param name="methodName">Name of the web method you want to call (case sensitive)</param>
        /// <param name="encode">Do you want to encode your parameters? (default: true)</param>
        internal static void InvokeService(this WebService service, string methodName, bool encode)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(service.Url);
            req.Headers.Add(StringConstants.SoapHeaderName, CreateSoapHeaderName(service.Namespace, methodName));
            req.ContentType = StringConstants.SoapContentType;
            req.Accept = StringConstants.SoapAccept;
            req.Method = StringConstants.SoapMethod;

            var stm = req.GetRequestStream();
            var postValues = new StringBuilder();
            foreach (var param in service.Parameters)
            {
                if (encode) postValues.AppendFormat(StringConstants.SoapParamFormat, HttpUtility.HtmlEncode(param.Key), HttpUtility.HtmlEncode(param.Value));
                else postValues.AppendFormat(StringConstants.SoapParamFormat, param.Key, param.Value);
            }
            postValues = service.ParametersMappers.ApplyMappers(postValues);

            var soapStr = string.Format(StringConstants.SoapStringFormat, methodName, postValues.ToString(), service.Namespace);

            using (StreamWriter stmw = new StreamWriter(stm))
                stmw.Write(soapStr);

            stm.Close();

            var responseReader = new StreamReader(req.GetResponse().GetResponseStream());
            string result = responseReader.ReadToEnd();
            service.Result.SoapResponse = XDocument.Parse(result);
            service.ExtractResult(methodName);
            responseReader.Close();
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
