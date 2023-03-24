using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Android.App;
using Application.Utility;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace Application.MVP.Model
{
    public class MainModel
    {
        [XmlRoot("offers")]
        public class Offers
        {
            [XmlElement("offer")]
            public Offer[]? OfferArray { get; set; }
        }

        public event Action<string>? DebugAction = delegate { };
        public event Action<string[]>? DataIsReady = delegate { };

        private readonly Dictionary<int, Offer> _offerMap;

        public MainModel()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _offerMap = new Dictionary<int, Offer>();
        }

        public async Task LoadData(string url)
        {
            var document = new XmlDocument();
            try
            {
                await document.LoadAsync(url);
            }
            catch (Exception ex)
            {
                DebugAction?.Invoke(ex.Message);
                return;
            }

            var offersNode = document.SelectSingleNode("//offers");
            if (offersNode is null)
                return;

            var offersNodeChildren = offersNode.ChildNodes;

            var serializer = new XmlSerializer(typeof(Offers));
            using var reader = new StringReader(offersNode.OuterXml);
            var offers = (Offers?)serializer.Deserialize(reader);
            if (offers?.OfferArray is null ||
                offers.OfferArray.Length != offersNodeChildren.Count)
                return;

            var idData = new string[offersNodeChildren.Count];
            for (var i = 0; i < offersNodeChildren.Count; ++i)
            {
                if (_offerMap.ContainsKey(offers.OfferArray[i].Id))
                    continue;

                _offerMap.Add(offers.OfferArray[i].Id, offers.OfferArray[i]);
                var offerNode = offersNodeChildren.Item(i);
                var json = JsonConvert.SerializeXmlNode(offerNode,
                    Formatting.Indented);
                offers.OfferArray[i].JsonString = json;
                idData[i] = offers.OfferArray[i].Id.ToString();
            }

            DataIsReady?.Invoke(idData);
        }

        public bool TryGetOffer(string idStr, out string? offerJson)
        {
            offerJson = null;
            if (!int.TryParse(idStr, out var id) ||
                !_offerMap.ContainsKey(id))
                return false;

            offerJson = _offerMap[id].JsonString;
            return true;
        }
    }
}