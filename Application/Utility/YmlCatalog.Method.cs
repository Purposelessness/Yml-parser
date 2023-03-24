using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Application.Utility
{
    public partial class YmlCatalog
    {
        public XmlDocument ToXmlDocument()
        {
            var document = new XmlDocument();
            using var stringWriter = new StringWriter();
            var serializer = new XmlSerializer(typeof(YmlCatalog));
            serializer.Serialize(stringWriter, this);
            document.LoadXml(stringWriter.ToString());
            return document;
        }

        public static async Task<YmlCatalog> LoadDocumentAsync(string url)
        {
            var document = new XmlDocument();
            await document.LoadAsync(url);

            var serializer = new XmlSerializer(typeof(YmlCatalog));
            using var reader = new StringReader(document.OuterXml);
            var yml = (YmlCatalog)serializer.Deserialize(reader)!;
            
            return yml;
        }
    }
}