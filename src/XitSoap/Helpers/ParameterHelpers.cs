using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HodStudio.XitSoap.Helpers
{
    internal static class ParameterHelpers
    {
        internal static void AddMethodParameter<InputType>(this WebService service, string name, InputType value)
        {
            service.GetMapperAttributes(typeof(InputType));
            var xmlSerializer = new XmlSerializer(typeof(InputType));
            var completeXml = new XmlDocument();
            using (var mr = new StringWriter())
            {
                xmlSerializer.Serialize(mr, value);
                completeXml.LoadXml(XmlHelpers.RemoveNamespaces(mr.ToString()).ToString());
            }
            service.Params.Add(name, completeXml.DocumentElement.InnerXml);
        }

        internal static void AddMethodParameter(this WebService service, string name, string value)
        {
            service.Params.Add(name, value);
        }
    }
}
