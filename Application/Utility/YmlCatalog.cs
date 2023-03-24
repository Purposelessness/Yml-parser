using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Application.Utility
{
    [XmlRoot("yml_catalog")]
    public partial class YmlCatalog
    {
        public const string DateFormat = "yyyy-MM-dd HH:mm";

        [XmlIgnore]
        public DateTime Date { get; set; }

        [XmlAttribute("date")]
        public string DateString
        {
            get
            {
                var formattedDate = Date.ToString(DateFormat);
                return formattedDate;
            }
            set =>
                Date = DateTime.ParseExact(value, DateFormat,
                    CultureInfo.InvariantCulture);
        }

        [XmlElement("shop")]
        public Shop Shop { get; set; }

        public YmlCatalog() : this(new Shop())
        {
        }

        public YmlCatalog(Shop shop, DateTime? date = null)
        {
            Date = date ?? DateTime.Now;
            Shop = shop;
        }
    }

    [XmlRoot("shop", IsNullable = false)]
    public class Shop
    {
        [XmlElement("name", Order = 1)]
        public string Name { get; set; }

        [XmlElement("company", Order = 2)]
        public string Company { get; set; }

        [XmlElement("url", Order = 3)]
        public string Url { get; set; }

        [XmlArray("currencies", Order = 4),
         XmlArrayItem("currency")]
        public Currency[]? Currencies;

        [XmlArray("categories", Order = 5),
         XmlArrayItem("category")]
        public Category[]? Categories;

        [XmlArray("offers", Order = 6),
         XmlArrayItem("offer")]
        public Offer[]? Offers;

        public Shop()
        {
            Url = string.Empty;
            Company = string.Empty;
            Name = string.Empty;
        }

        public Shop(string name, string company, string url,
            Currency[] currencies, Category[] categories, Offer[] offers)
        {
            Name = name;
            Company = company;
            Url = url;
            Categories = categories;
            Offers = offers;
            Currencies = currencies;
        }
    }

    [XmlRoot("currency", IsNullable = false)]
    public class Currency
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("rate")]
        public int Rate { get; set; }

        [XmlAttribute("plus")]
        public int Plus { get; set; }

        public Currency() : this("")
        {
        }

        public Currency(string id = "", int rate = 0, int plus = 0)
        {
            Id = id;
            Rate = rate;
            Plus = plus;
        }
    }

    public class Category
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlIgnore]
        public int? ParentId { get; set; }

        [XmlAttribute("parentId")]
        public string ParentIdString
        {
            get => ParentId is null ? string.Empty : ParentId.ToString()!;
        }

        [XmlText]
        public string Name { get; set; }

        public Category() : this(0)
        {
        }

        public Category(int id = 0, int? parentId = null, string name = "")
        {
            Id = id;
            ParentId = parentId;
            Name = name;
        }
    }

    [XmlRoot("offer", IsNullable = false)]
    public class Offer
    {
        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }

        [XmlElement(ElementName = "url")]
        public string Url { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "price")]
        public int Price { get; set; }

        [XmlElement(ElementName = "picture", IsNullable = false)]
        public string? PictureUrl { get; set; }

        [XmlElement(ElementName = "delivery")]
        public bool Delivery { get; set; }

        // and so on...

        [XmlIgnore]
        public string? JsonString { get; set; }

        public Offer() : this(0)
        {
        }

        public Offer(int id = 0, string name = "", string description = "",
            string url = "", int price = 0,
            string? pictureUrl = null, bool delivery = true,
            string? jsonString = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Url = url;
            Price = price;
            PictureUrl = pictureUrl;
            Delivery = delivery;
            JsonString = jsonString;
        }
    }
}