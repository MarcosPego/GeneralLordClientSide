using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace GeneralLord
{
	[XmlRoot("CustomTroopRoster")]
	public class CustomTroopRoster
	{
		[XmlElement("Character")]
		public List<CharacterEntry> Characters = new List<CharacterEntry>();
	}
}