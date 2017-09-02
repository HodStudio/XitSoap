using System.Xml.Linq;

namespace HodStudio.XitSoap
{
    public class WebServiceResult
    {
        public string StringResult { get; set; }
        public XDocument XmlResult { get; set; }
        internal XDocument SoapResponse { get; set; }
    }
}
