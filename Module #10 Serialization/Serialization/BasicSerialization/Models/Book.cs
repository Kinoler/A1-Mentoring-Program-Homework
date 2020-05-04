﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BasicSerialization.Enums;

namespace BasicSerialization.Models
{
    [Serializable]
    [XmlType(TypeName = "book")]
    public class Book
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("isbn")]
        public string ISBN { get; set; }

        [XmlElement("author")]
        public string Author { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("genre")]
        public GenreType Genre { get; set; }

        [XmlElement("publisher")]
        public string Publisher { get; set; }

        [XmlElement("publish_date")]
        public string PublishDateString {
            get => this.PublishDate.ToString("yyyy-MM-dd");
            set => this.PublishDate = DateTime.Parse(value);
        }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("registration_date")]
        public string RegistrationDateString
        {
            get => this.RegistrationDate.ToString("yyyy-MM-dd");
            set => this.RegistrationDate = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime PublishDate { get; set; }

        [XmlIgnore]
        public DateTime RegistrationDate { get; set; }
    }
}
