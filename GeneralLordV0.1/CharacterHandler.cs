using GeneralLord.FormationBattleTest;
using GeneralLordWebApiClient.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace GeneralLord
{
    public class CharacterHandler
    {
		public static CharacterObject characterObject;
		public static string saveLocationFile = "playergeneral.xml";
		public static SaveLocationEnum saveLocationPath = SaveLocationEnum.Configs;
		public static bool debugMode = false;

		public static float healthRegainPercentageAfterBattle = 0.3f;
		public static int pricePerHealthPoint = 2;

		public enum SaveLocationEnum
		{
			ModuleData,
			Configs
		}

		public static void HandleHealthBuy()
		{
			if (PartyBase.MainParty.LeaderHero.Gold - PriceToFullHealth() < 0)
			{
				InformationManager.DisplayMessage(new InformationMessage("Not Enough Money To Go to the Doctor!"));
			} else if (PartyBase.MainParty.LeaderHero.HitPoints == PartyBase.MainParty.LeaderHero.CharacterObject.MaxHitPoints())
            {
				InformationManager.DisplayMessage(new InformationMessage("You feel in perfect condition"));
			}
			else
			{
				GiveGoldAction.ApplyBetweenCharacters(null, PartyBase.MainParty.LeaderHero, -PriceToFullHealth(), false);
				PartyBase.MainParty.LeaderHero.HitPoints = PartyBase.MainParty.LeaderHero.CharacterObject.MaxHitPoints();
				InformationManager.DisplayMessage(new InformationMessage("Healed to full health! You feel refreshed!"));
			}
		}


		public static int PriceToFullHealth()
        {
			return (PartyBase.MainParty.LeaderHero.CharacterObject.MaxHitPoints() - PartyBase.MainParty.LeaderHero.HitPoints) * 5;
		}

		public static void HandleAfterBattleHealth()
        {
			int maxPossibleHealthGain = (int) (healthRegainPercentageAfterBattle *  PartyBase.MainParty.LeaderHero.CharacterObject.MaxHitPoints());



			if(PartyBase.MainParty.LeaderHero.CharacterObject.MaxHitPoints() - PartyBase.MainParty.LeaderHero.HitPoints > maxPossibleHealthGain)
            {
				PartyBase.MainParty.LeaderHero.HitPoints = PartyBase.MainParty.LeaderHero.HitPoints + maxPossibleHealthGain;

			}
			else
            {
				PartyBase.MainParty.LeaderHero.HitPoints = PartyBase.MainParty.LeaderHero.CharacterObject.MaxHitPoints();
			}

		}

		public static void HandleBattleTestlRestoreHealth()
		{
			PartyBase.MainParty.LeaderHero.HitPoints = BattleTestHandler.CurrentPlayerHealth;

		}


		public static string SaveCharacterAndLoadToString()
        {
			SaveCharacter();
			StreamReader streamReader = new StreamReader(DynamicPath());
			string xml = streamReader.ReadToEnd();

			return xml;
        }

		public static void SaveCharacter()
        {
			TryClearFile("Deleted Original File", "Couldn't delete original file");

			OutputCharacterBaseTraits();
			OutputCharacterFaceProperties();
			OutputCharacterSkills();
			OutputEquipmentSet();
			OutputHorseEquipments();
			OutputCharacterEnd();
		}

		public static void OutputCharacterBaseTraits()
        {
			string enemyLordStringId = "enemyGeneral";

			TryOutputLines(new List<string>
			{

				"<?xml version=\"1.0\" encoding=\"utf-8\"?>",
				"<NPCCharacters>",
				"\t<NPCCharacter",
				"\t\tid=\""+ enemyLordStringId+ "\"",
				"\t\tage=\""+ Math.Round(Hero.MainHero.Age, 0).ToString() + "\"", 
				"\t\tlevel=\""+ Hero.MainHero.Level.ToString() +"\"",
				"\t\tname=\""+ Hero.MainHero.Name.ToString()+"\"",
				"\t\tis_female=\""+ Hero.MainHero.IsFemale.ToString().ToLower() +"\"",
				"\t\tculture=\"Culture."+  Hero.MainHero.Culture.ToString().ToLower() +"\"",
				"\t\toccupation=\"Soldier\">",

			}, "NPCCharacter base traits added.", "Error occured while trying to output NPCCharacter  base traits.");

		}

		public static void OutputCharacterFaceProperties()
		{
			BodyProperties heroBP = Hero.MainHero.BodyProperties;

			/*TryOutputLines(new List<string>
			{
				"\t\t<face>",
				"\t\t\t<face_key_template value=\"BodyProperty.fighter_" + Hero.MainHero.Culture.ToString().ToLower() + "\" />",
				"\t\t</face>",

			}, "NPCCharacter face properties added.", "Error occured while trying to output NPCCharacter  base traits.");
			*/

			TryOutputLines(new List<string>
			{
				"\t\t<face>",
				"\t\t\t<face_key_template value=\"BodyProperty.fighter_" + Hero.MainHero.Culture.ToString().ToLower() + "\" />",
				"\t\t</face>",

			}, "NPCCharacter face properties added.", "Error occured while trying to output NPCCharacter  base traits.");
		}

		public static void OutputCharacterSkills()
		{

			Hero mainHero = Hero.MainHero;

			TryOutputLines(new List<string>
			{
				"\t\t<skills>",
				"\t\t\t<skill id=\"Athletics\" value=\""+ mainHero.GetSkillValue(DefaultSkills.Athletics).ToString()+ "\" />",
				"\t\t\t<skill id=\"Riding\" value=\""+ mainHero.GetSkillValue(DefaultSkills.Riding).ToString()+ "\" />",
				"\t\t\t<skill id=\"OneHanded\" value=\""+ mainHero.GetSkillValue(DefaultSkills.OneHanded).ToString()+ "\" />",
				"\t\t\t<skill id=\"TwoHanded\" value=\""+ mainHero.GetSkillValue(DefaultSkills.TwoHanded).ToString()+ "\" />",
				"\t\t\t<skill id=\"Polearm\" value=\""+ mainHero.GetSkillValue(DefaultSkills.Polearm).ToString()+"\" />",
				"\t\t\t<skill id=\"Bow\" value=\""+ mainHero.GetSkillValue(DefaultSkills.Bow).ToString()+ "\" />",
				"\t\t\t<skill id=\"Crossbow\" value=\""+ mainHero.GetSkillValue(DefaultSkills.Crossbow).ToString()+ "\" />",
				"\t\t\t<skill id=\"Throwing\" value=\""+ mainHero.GetSkillValue(DefaultSkills.Throwing).ToString()+ "\" />",
				"\t\t</skills>",
				"\t\t<upgrade_targets>",
				"\t\t</upgrade_targets>",
				"\t\t<Equipments>"

			}, "NPCCharacter face properties added.", "Error occured while trying to output NPCCharacter  base traits.");

		}

		public static void OutputCharacterEnd()
		{
			TryOutputLines(new List<string>
			{
				"\t\t</Equipments>",
				"\t</NPCCharacter>",
				"</NPCCharacters>",
			}, "NPCCharacter end part added.", "Error occured while trying to output NPCCharacter end part.");
		}


		public static void OutputEquipmentSet()
		{
			List<string> list = new List<string>();
			Equipment battleEquipment = Hero.MainHero.BattleEquipment;
			list.Add("\t\t\t<EquipmentRoster>");
			AddEquipment(list, battleEquipment, EquipmentIndex.WeaponItemBeginSlot, false);
			AddEquipment(list, battleEquipment, EquipmentIndex.Weapon1, false);
			AddEquipment(list, battleEquipment, EquipmentIndex.Weapon2, false);
			AddEquipment(list, battleEquipment, EquipmentIndex.Weapon3, false);
			AddEquipment(list, battleEquipment, EquipmentIndex.NumAllWeaponSlots, false);
			AddEquipment(list, battleEquipment, EquipmentIndex.Cape, false);
			AddEquipment(list, battleEquipment, EquipmentIndex.Body, false);
			AddEquipment(list, battleEquipment, EquipmentIndex.Gloves, false);
			AddEquipment(list, battleEquipment, EquipmentIndex.Leg, false);
			list.Add("\t\t\t</EquipmentRoster>");
			TryOutputLines(list, "EquipmentRoster added.", "Error occured while trying to output EquipmentRoster.");
		}


		public static void OutputHorseEquipments()
		{
			List<string> lines = new List<string>();
			Equipment battleEquipment = Hero.MainHero.BattleEquipment;
			AddEquipment(lines, battleEquipment, EquipmentIndex.ArmorItemEndSlot, true);
			AddEquipment(lines, battleEquipment, EquipmentIndex.HorseHarness, true);
			TryOutputLines(lines, "Horse equipments added.", "Error occured while trying to output Horse equipments.");
		}


		public static void AddEquipment(List<string> lines, Equipment equipment, EquipmentIndex equipmentIndex, bool isHorse = false)
		{
			EquipmentElement equipmentFromSlot = equipment.GetEquipmentFromSlot(equipmentIndex);
			bool flag = equipmentFromSlot.Item != null;
			if (flag)
			{
				string text = (equipmentIndex == EquipmentIndex.NumAllWeaponSlots) ? "Head" : ((equipmentIndex == EquipmentIndex.WeaponItemBeginSlot) ? "Item0" : ((equipmentIndex == EquipmentIndex.Weapon1) ? "Item1" : ((equipmentIndex == EquipmentIndex.Weapon2) ? "Item2" : ((equipmentIndex == EquipmentIndex.Weapon3) ? "Item3" : ((equipmentIndex == EquipmentIndex.Weapon4) ? "Item4" : ((equipmentIndex == EquipmentIndex.ArmorItemEndSlot) ? "Horse" : equipmentIndex.ToString()))))));
				lines.Add(string.Concat(new string[]
				{
					isHorse ? "" : "\t",
					"\t\t\t<equipment slot=\"",
					text,
					"\" id=\"Item.",
					equipmentFromSlot.Item.StringId,
					"\" />"
				}));
			}
		}

		public static void TryClearFile(string SucceedMessage, string FailedMessage)
		{
			try
			{
				Serializer.EnsureSaveDirectory();
				File.Delete(DynamicPath());
				//File.Delete(GetPath(saveFileLocation));
				if (debugMode) InformationManager.DisplayMessage(new InformationMessage("GeneralLordCharacterOutput: " + SucceedMessage));
			}
			catch (Exception)
			{
				if (debugMode) InformationManager.DisplayMessage(new InformationMessage("GeneralLordCharacterOutput: " + FailedMessage));
			}
		}

		public static void TryOutputLines(List<string> lines, string SucceedMessage, string FailedMessage)
		{
			try
			{
				Serializer.EnsureSaveDirectory();
				File.AppendAllLines(DynamicPath(), lines);
				if(debugMode) InformationManager.DisplayMessage(new InformationMessage("GeneralLordCharacterOutput: " + SucceedMessage));
			}
			catch (Exception)
			{
				if (debugMode) InformationManager.DisplayMessage(new InformationMessage("GeneralLordCharacterOutput: " + FailedMessage));
			}
		}

		public static string DynamicPath()
        {
			Serializer.EnsureSaveDirectory();
			switch (saveLocationPath)
			{
				case SaveLocationEnum.ModuleData:
					return GetPath(saveLocationFile);
					//break;
				case SaveLocationEnum.Configs:
					return Path.Combine(Serializer.SaveFolderPath(), saveLocationFile);
					//break;
				default:
					Console.WriteLine("Error");
					return "";
					//break;
			}
		}


		public static void WriteToFile(string data)
		{
			//TryClearFile("Deleted Original File", "Couldn't delete original file");
			File.WriteAllText(DynamicPath(), data);
		}

		public static string GetPath(string fileName)
		{
			return Path.Combine(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", "..")), "ModuleData", "EnemyGeneral", fileName);
		}

		public static void LoadXML()
        {
			string xmlPath = DynamicPath();

			XmlDocument xmlDocument = new XmlDocument();
			StreamReader streamReader = new StreamReader(xmlPath);
			string xml = streamReader.ReadToEnd();
			xmlDocument.LoadXml(xml);

			foreach (object obj2 in xmlDocument)
            {
				XmlNode xmlNode2 = (XmlNode)obj2;

				foreach (object obj3 in xmlNode2)
				{
					XmlNode xmlNode3 = (XmlNode)obj3;
					XmlAttributeCollection attributes2 = xmlNode3.Attributes;
					if (attributes2 != null)
					{
						string innerText2 = attributes2["id"].InnerText;
						CharacterObject object2 = Game.Current.ObjectManager.GetObject<CharacterObject>(innerText2);
						//MBObjectManager.Instance.UnregisterObject(object2);
						if (object2 != null)
						{

							object2.Deserialize(Game.Current.ObjectManager, xmlNode3);
							characterObject = object2;
					
						}
					}
				}

				
			}
		}

	}
}
