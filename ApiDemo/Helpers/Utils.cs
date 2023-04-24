using Newtonsoft.Json;
using System.Xml;

namespace ApiDemo.Helpers
{
    public class Utils
    {

        public Utils() { }

        public string XmlToJsonString(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var jsonTop10 = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.None, true);
            return jsonTop10.Replace("\"", "\'").Replace("\n", " ");
        }
    }
}
