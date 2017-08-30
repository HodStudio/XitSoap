using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HodStudio.XitSoap.Helpers
{
    internal class StringConstants
    {
        internal const string SoapStringFormat = @"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                   xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                   xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                  <soap:Body>
                    <{0} xmlns=""{2}"">
                      {1}
                    </{0}>
                  </soap:Body>
                </soap:Envelope>";
        internal const string SoapHeaderName = "SOAPAction";
        internal const string SoapHeaderFormat = "\"{0}/{1}\"";
        internal const string SoapContentType = "text/xml;charset=\"utf-8\"";
        internal const string SoapAccept = "text/xml";
        internal const string SoapMethod = "POST";
        internal const string SoapParamFormat = "<{0}>{1}</{0}>";

        internal const string XmlResultDummyNamespace = "guisgjkjtlcenzpabjfm";
        internal const string XmlResultResultFormat = "{0}Result";
        internal const string XmlResultXPathSelectorFormat = "//" + XmlResultDummyNamespace + ":{0}";
        internal const string XmlResultXDocumentFormat = "<root>{0}</root>";
        internal const string XmlNamespaceXmlsRegex = @"(xmlns:?[^=]*=[""][^""]*[""])";
        internal const string XmlNamespaceProperNameRegex = @"(<[A-z]*(\s+xsi:?[^=]*=[""][^""]*[""])+\s+/>)";
    }
}
