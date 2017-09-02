using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace HodStudio.XitSoap.Helpers
{
    internal static class XmlHelpers
    {
        /// <summary>
        /// Remove all xmlns:* instances from the passed XmlDocument to simplify our xpath expressions
        /// </summary>
        internal static XDocument RemoveNamespaces(XDocument oldXml)
        {
            // FROM: http://social.msdn.microsoft.com/Forums/en-US/bed57335-827a-4731-b6da-a7636ac29f21/xdocument-remove-namespace?forum=linqprojectgeneral
            try
            {
                var parser = oldXml.ToString();
                parser = Regex.Replace(
                    parser,
                    StringConstants.XmlNamespaceXmlsRegex,
                    string.Empty,
                    RegexOptions.IgnoreCase | RegexOptions.Multiline);
                parser = Regex.Replace(
                    parser,
                    StringConstants.XmlNamespaceProperNameRegex,
                    string.Empty,
                    RegexOptions.IgnoreCase | RegexOptions.Multiline);
                XDocument newXml = XDocument.Parse(parser);
                return newXml;
            }
            catch (XmlException error)
            {
                throw new XmlException(error.Message + " at XmlHelpers.RemoveNamespaces");
            }
        }

        /// <summary>
        /// Remove all xmlns:* instances from the passed XmlDocument to simplify our xpath expressions
        /// </summary>
        internal static XDocument RemoveNamespaces(string oldXml)
        {
            XDocument newXml = XDocument.Parse(oldXml);
            return RemoveNamespaces(newXml);
        }

        /// <summary>
        /// Converts a string that has been HTML-enconded for HTTP transmission into a decoded string.
        /// </summary>
        /// <param name="escapedString">String to decode.</param>
        /// <returns>Decoded (unescaped) string.</returns>
        internal static string UnescapeString(string escapedString)
        {
            return HttpUtility.HtmlDecode(escapedString);
        }
    }
}
