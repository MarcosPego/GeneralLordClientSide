using System;
using System.Xml.Serialization;

namespace GeneralLord
{
    public class CharacterEntry
    {
		[XmlAttribute("id")]
		public string Id = "";
	}
}
