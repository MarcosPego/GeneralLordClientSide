using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Engine.Screens;

namespace GeneralLord
{
    internal class PartyManager
	{
		public PartyManager()
        {
			this._partyManagerLogic = new PartyManagerLogic();
			this._partyManagerLogic.Initialize(TestRosterLeft(), TestRosterRight());
		}

		/*public void TickCampaignBehavior()
		{
			if (Mission.Current == null && Input.IsKeyDown(InputKey.LeftControl))
			{
				if (Input.IsKeyReleased(InputKey.E))
				{
					
					ScreenManager.PushScreen(new PartyManagerScreen(this._partyManagerLogic));
				}
			}
		}*/

		public TroopRoster ReturnOpenRoster()
		{
			CustomTroopRoster settings = this.GetSettings();
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			foreach (CharacterEntry characterEntry in settings.Characters)
			{
				this.TryAddCharacterToRoster(troopRoster, characterEntry.Id, 500);
			}

            return troopRoster;
		}

		public TroopRoster[] TestRosterLeft()
		{
			//TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRosterA = new TroopRoster(PartyBase.MainParty);
			//this.TryAddCharacterToRoster(troopRosterA, "aserai_recruit", 1);
			//this.TryAddCharacterToRoster(troopRosterA, "vlandian_recruit", 32);
			//this.TryAddCharacterToRoster(troopRosterA, "sturgian_recruit", 18);
			//this.TryAddCharacterToRoster(troopRosterA, "mercenary_1", 10);
			//this.TryAddCharacterToRoster(troopRosterA, "mercenary_2", 20);
			//InformationManager.DisplayMessage(new InformationMessage(troopRoster.TotalManCount.ToString()));


			TroopRoster troopRosterB = new TroopRoster(PartyBase.MainParty);
			//this.TryAddCharacterToRoster(troopRosterB, "imperial_recruit", 1);

			TroopRoster troopRosterC = new TroopRoster(PartyBase.MainParty);
			TroopRoster troopRosterD = new TroopRoster(PartyBase.MainParty);
			//this.TryAddCharacterToRoster(troopRosterC, "imperial_recruit", 1);

			TroopRoster[] troopRosterlist = new TroopRoster[4];
			troopRosterlist[0] = troopRosterA;
			troopRosterlist[1] = troopRosterB;
			troopRosterlist[2] = troopRosterC;
			troopRosterlist[3] = troopRosterD;

			return troopRosterlist;
		}

		public TroopRoster[] TestRosterRight()
		{
			//TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRoster = new TroopRoster(PartyBase.MainParty);
			this.TryAddCharacterToRoster(troopRoster, "aserai_recruit", 5);
			this.TryAddCharacterToRoster(troopRoster, "vlandian_recruit", 5);
			this.TryAddCharacterToRoster(troopRoster, "sturgian_recruit", 10);
			this.TryAddCharacterToRoster(troopRoster, "mercenary_1", 15);
			//InformationManager.DisplayMessage(new InformationMessage(troopRoster.TotalManCount.ToString()));


			TroopRoster[] troopRosterlist = new TroopRoster[1];
			troopRosterlist[0]= PartyBase.MainParty.MemberRoster;

			return troopRosterlist;
		}


		public void TryAddCharacterToRoster(TroopRoster troopRoster, string characterId, int count)
		{

			CharacterObject characterObject = CharacterObject.Find(characterId);
			if (characterObject != null)
			{
				//InformationManager.DisplayMessage(new InformationMessage("Chegou" + characterId));
				troopRoster.AddToCounts(characterObject, count, false, 0, 0, true, -1);
				
			}
			else
			{
				InformationManager.DisplayMessage(new InformationMessage("CustomTroopRoster: " + characterId + " id not found."));
			}
		}

		private CustomTroopRoster GetSettings()
		{
			CustomTroopRoster result;
			try
			{
				string path = this.GetPath("CustomTroopRoster.xml");
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(CustomTroopRoster));
				using (StreamReader streamReader = new StreamReader(path))
				{
					result = (CustomTroopRoster)xmlSerializer.Deserialize(streamReader);
				}
			}
			catch (Exception)
			{
				InformationManager.DisplayMessage(new InformationMessage("CustomTroopRoster: Error occured while trying to retrieve mod settings."));
				result = new CustomTroopRoster();
			}
			return result;
		}

		private string GetPath(string fileName)
		{
			return Path.Combine(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", "..")), "ModuleData", fileName);
		}


		private bool TroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
		{
			return true;
		}
		private bool PartyScreenDoneClicked(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, bool isForced, List<MobileParty> leftParties, List<MobileParty> rigthParties)
		{
			return true;
		}

		private Tuple<bool, string> PartyScreenDoneCondition(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, int leftLimitNum, int rightLimitNum)
		{
			return new Tuple<bool, string>(true, "");
		}

		/*public override void RegisterEvents()
		{
		}
		public override void SyncData(IDataStore dataStore)
		{
		}*/

		public PartyManagerLogic _partyManagerLogic;
	}
}
