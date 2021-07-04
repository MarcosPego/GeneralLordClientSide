using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace GeneralLord
{
    public class CharacterHandler
    {
		public static void SaveCharacter()
        {
			OutputCharacterBaseTraits();
			OutputCharacterFaceProperties();
			OutputEquipmentSet();
			OutputHorseEquipments();
			OutputCharacterEnd();
		}

		public static void OutputCharacterBaseTraits()
        {
			string enemyLordStringId = "enemy_general_lord";

			TryOutputLines(new List<string>
			{
				"\t<NPCCharacter",
				"\t\tid=\""+ enemyLordStringId+ "\"",
				"\t\tage=\""+ Math.Round(Hero.MainHero.Age, 0).ToString() + "\"", 
				//"\t\tdefault_group=\"\"",
				"\t\tlevel=\""+ Hero.MainHero.Level.ToString() +"\"",
				"\t\tname=\""+ Hero.MainHero.Name.ToString() +"\"",
				"\t\tis_hero=\"true\"",
				"\t\tis_female=\""+ Hero.MainHero.IsFemale.ToString().ToLower() +"\"",
				"\t\tculture=\"Culture."+  Hero.MainHero.Culture.ToString().ToLower() +"\"",
				"\t\toccupation=\"Lord\"",

				//"\t\tbanner_symbol_mesh_name=\""+ Hero.MainHero.ClanBanner.BannerDataList.First().MeshId.ToString()+ "\"",
				//"\t\t banner_symbol_color=\""+ Hero.MainHero.ClanBanner.BannerDataList.First().ColorId.ToString()+ "\"",

				
				"\t\tbanner_symbol_mesh_name=\"test_symbol_a\"",
				"\t\t banner_symbol_color=\"FF000000\"",
			}, "NPCCharacter base traits added.", "Error occured while trying to output NPCCharacter  base traits.");

		}

		public static void OutputCharacterFaceProperties()
		{
			TryOutputLines(new List<string>
			{
				//"\t\tskill_template=\"SkillSet.\" >",
				"\t\t<face>",
				"\t\t\t<face_key_template value=\"BodyProperty.\" />",
				"\t\t</face>",

			}, "NPCCharacter face properties added.", "Error occured while trying to output NPCCharacter  base traits.");

		}

		public static void OutputCharacterSkills()
		{

			TryOutputLines(new List<string>
			{
				"\t\t<skills>",
				"\t\t\t<skill id=\"Athletics\" value=\"\" />",
				"\t\t\t<skill id=\"Riding\" value=\"\" />",
				"\t\t\t<skill id=\"OneHanded\" value=\"\" />",
				"\t\t\t<skill id=\"TwoHanded\" value=\"\" />",
				"\t\t\t<skill id=\"Polearm\" value=\"\" />",
				"\t\t\t<skill id=\"Bow\" value=\"\" />",
				"\t\t\t<skill id=\"Crossbow\" value=\"\" />",
				"\t\t\t<skill id=\"Throwing\" value=\"\" />",
				"\t\t</skills>",
				"\t\t<upgrade_targets>",
				"\t\t</upgrade_targets>",
				"\t\t<Equipments>"

			}, "NPCCharacter face properties added.", "Error occured while trying to output NPCCharacter  base traits.");

		}


		public static void OutputCharacterStart()
		{
			TryOutputLines(new List<string>
			{
				"\t<NPCCharacter",
				"\t\tid=\"\"",
				"\t\tdefault_group=\"\"",
				"\t\tlevel=\"\"",
				"\t\tname=\"\"",
				"\t\tis_basic_troop=\"\"",
				"\t\tis_female=\"\"",
				"\t\tupgrade_requires=\"\"",
				"\t\toccupation=\"\"",
				"\t\tculture=\"Culture.\"",
				"\t\tskill_template=\"SkillSet.\" >",
				"\t\t<face>",
				"\t\t\t<face_key_template value=\"BodyProperty.\" />",
				"\t\t</face>",
				"\t\t<skills>",
				"\t\t\t<skill id=\"Athletics\" value=\"\" />",
				"\t\t\t<skill id=\"Riding\" value=\"\" />",
				"\t\t\t<skill id=\"OneHanded\" value=\"\" />",
				"\t\t\t<skill id=\"TwoHanded\" value=\"\" />",
				"\t\t\t<skill id=\"Polearm\" value=\"\" />",
				"\t\t\t<skill id=\"Bow\" value=\"\" />",
				"\t\t\t<skill id=\"Crossbow\" value=\"\" />",
				"\t\t\t<skill id=\"Throwing\" value=\"\" />",
				"\t\t</skills>",
				"\t\t<upgrade_targets>",
				"\t\t</upgrade_targets>",
				"\t\t<Equipments>"
			}, "NPCCharacter first part added.", "Error occured while trying to output NPCCharacter first part.");
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002488 File Offset: 0x00000688
		public static void OutputCharacterEnd()
		{
			TryOutputLines(new List<string>
			{
				"\t\t</Equipments>",
				"\t</NPCCharacter>"
			}, "NPCCharacter end part added.", "Error occured while trying to output NPCCharacter end part.");
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000024C8 File Offset: 0x000006C8
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

		// Token: 0x0600000C RID: 12 RVA: 0x00002578 File Offset: 0x00000778
		public static void OutputHorseEquipments()
		{
			List<string> lines = new List<string>();
			Equipment battleEquipment = Hero.MainHero.BattleEquipment;
			AddEquipment(lines, battleEquipment, EquipmentIndex.ArmorItemEndSlot, true);
			AddEquipment(lines, battleEquipment, EquipmentIndex.HorseHarness, true);
			TryOutputLines(lines, "Horse equipments added.", "Error occured while trying to output Horse equipments.");
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000025C4 File Offset: 0x000007C4
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

		// Token: 0x0600000E RID: 14 RVA: 0x00002698 File Offset: 0x00000898
		public static void TryOutputLines(List<string> lines, string SucceedMessage, string FailedMessage)
		{
			try
			{
				File.AppendAllLines(GetPath("GeneralLordCharacterOutput.xml"), lines);
				InformationManager.DisplayMessage(new InformationMessage("GeneralLordCharacterOutput: " + SucceedMessage));
			}
			catch (Exception)
			{
				InformationManager.DisplayMessage(new InformationMessage("GeneralLordCharacterOutput: " + FailedMessage));
			}
		}

		public static string GetPath(string fileName)
		{
			return Path.Combine(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", "..")), "ModuleData", fileName);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002700 File Offset: 0x00000900
		/*public override void RegisterEvents()
		{
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002703 File Offset: 0x00000903
		public override void SyncData(IDataStore dataStore)
		{
		}*/

		// Token: 0x04000001 RID: 1
		public static bool isFirstPart = true;

	}
}
