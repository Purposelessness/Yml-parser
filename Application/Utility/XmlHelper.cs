using System;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace Application.Utility
{
    public static class XmlHelper
    {
        public static async Task LoadAsync(this XmlDocument document,
            string url)
        {
            var client = new WebClient();
            await using var stream = client.OpenRead(url);
            if (stream is { CanRead: true })
            {
                document.Load(stream);
            }
        }
    }
}