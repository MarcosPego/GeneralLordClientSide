
using GeneralLord.FormationBattleTest;
using GeneralLordWebApiClient;
using GeneralLordWebApiClient.Model;
using Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;

namespace GeneralLord
{
    public class JsonBattleConfig
    {
		public static int UniqueId = 0;

		public static string dateFormat = "G";
		public static string dateCulture = "en-GB";

		public static float healingRatio = 0.5f;

		public static List<TroopRosterElement> copyOfTroopRosterPreviousToBattle = new List<TroopRosterElement>();

		public static int recoveryCooldown = 30;

		public static List<TooltipProperty> GetPartyTroopInfo(TroopRoster troopRoster, FormationClass formationClass)
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			list.Add(new TooltipProperty("", GameTexts.FindText("str_formation_class_string", formationClass.GetName()).ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.Title));
			foreach (TroopRosterElement troopRosterElement in troopRoster.GetTroopRoster())
			{
				if (!troopRosterElement.Character.IsHero && troopRosterElement.Character.DefaultFormationClass.Equals(formationClass))
				{
					list.Add(new TooltipProperty(troopRosterElement.Character.Name.ToString(), troopRosterElement.Number.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				}
			}
			return list;
		}

		public static List<TooltipProperty> GetPartyTroopHealthyInfo(PartyBase party, FormationClass formationClass)
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			list.Add(new TooltipProperty("", GameTexts.FindText("str_formation_class_string", formationClass.GetName()).ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.Title));
			foreach (TroopRosterElement troopRosterElement in party.MemberRoster.GetTroopRoster())
			{
				if (!troopRosterElement.Character.IsHero && troopRosterElement.Character.DefaultFormationClass.Equals(formationClass))
				{
					list.Add(new TooltipProperty(troopRosterElement.Character.Name.ToString(), (troopRosterElement.Number - troopRosterElement.WoundedNumber).ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				}
			}
			return list;
		}

		public static List<TooltipProperty> GetTroopsToRecoverInfo(WoundedTroopGroup woundedTroopGroup)
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			list.Add(new TooltipProperty("", "Troops To Recover", 0, false, TooltipProperty.TooltipPropertyFlags.Title));

			foreach(WoundedTroop woundedTroop in woundedTroopGroup.woundedTroops)
            {
				list.Add(new TooltipProperty(CharacterObject.Find(woundedTroop.stringId).Name.ToString(), woundedTroop.troopCount.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			}
			return list;
		}

		public static void VerifyUniqueFile()
        {
			var filePath = Path.Combine(Serializer.SaveFolderPath(false), "uniqueid.txt");

			if (!File.Exists(filePath))
            {
				ExecuteSubmitAc();
				JObject json = JObject.Parse(Serializer.ReadStringFromFile("playerprofile.json"));
				string text = json["Id"].ToString();
				File.WriteAllText(filePath, text);
			}

			UniqueId = Int32.Parse(File.ReadAllText(filePath));
			//InformationManager.DisplayMessage(new InformationMessage(UniqueId.ToString()));
			ExecuteSubmitAc();
		}

		public static void ExecuteSubmitPartyUtils()
		{
			JObject json = JObject.Parse(Serializer.ReadStringFromFile("playerprofile.json"));

			List<TroopContainer> troopContainers = new List<TroopContainer>();
			foreach (TroopRosterElement tre in PartyUtilsHandler.GarrisonedTroops.GetTroopRoster())
			{

				troopContainers.Add(new TroopContainer { stringId = tre.Character.StringId, troopCount = tre.Number, troopXP = tre.Xp });

			}
			ArmyContainer ac = new ArmyContainer { TroopContainers = troopContainers, CharacterXML = ""};

			string woundedTroopGroupString = JsonConvert.SerializeObject(PartyUtilsHandler.WoundedTroopArmy);
			string garrisonedTroopsString = JsonConvert.SerializeObject(ac);

			PartyUtils partyUtils = new PartyUtils { WoundedTroopsGroup = woundedTroopGroupString, GarrisonedTroops = garrisonedTroopsString, Id = (int)json["Id"] };


			var t = Task.Run(async () =>
			{	
				var result = await WebRequests.PostAsync(UrlHandler.GetUrlFromString(UrlHandler.SubmitPartyUtils), partyUtils);
			});
			t.Wait();
		}

		public static void ExecuteSubmitAc()
		{


			List<TroopContainer> troopContainers = new List<TroopContainer>();
			foreach (TroopRosterElement tre in PartyBase.MainParty.MemberRoster.GetTroopRoster())
			{

				troopContainers.Add(new TroopContainer { stringId = tre.Character.StringId, troopCount = tre.Number, troopXP = tre.Xp });

			}

			CharacterHandler.saveLocationFile = "playerprofile.xml";
			CharacterHandler.saveLocationPath = CharacterHandler.SaveLocationEnum.Configs;
			ArmyContainer ac = new ArmyContainer { TroopContainers = troopContainers, CharacterXML = CharacterHandler.SaveCharacterAndLoadToString() };

			//XDocument xd = ArmyContainerSerializer.LoadArmyContainerXML(ac);
			Serializer.JsonSerialize(ac);

			var t = Task.Run(async () =>
			{
				Profile profile = ProfileHandler.UpdateProfileAc();
				var result = await WebRequests.PostAsync<Profile>(UrlHandler.GetUrlFromString(UrlHandler.SaveProfile), profile);
				Serializer.JsonSerialize(result.ServerResponse, "playerprofile.json");
			});
			t.Wait();


			//SAVE GAME
			Campaign.Current.SaveHandler.QuickSaveCurrentGame();
		}

		public static void ReceivePartyUtils()
        {
			IEnumerable<PartyUtils> partyUtilsList;
			var task = Task.Run(async () =>
			{

				Profile profile = ProfileHandler.UpdateProfileAc();
				var result = await WebRequests.PostAsync<IEnumerable<PartyUtils>>(UrlHandler.GetUrlFromString(UrlHandler.GetPartyUtils), profile.Id);
				partyUtilsList = result.ServerResponse;

                if (partyUtilsList.Any())
                {
					PartyUtils partyUtils = partyUtilsList.First();
					ArmyContainer ac = JsonConvert.DeserializeObject<ArmyContainer>(partyUtils.GarrisonedTroops);


					TroopRoster troopRoster = new TroopRoster(PartyBase.MainParty);
					foreach (TroopContainer tc in ac.TroopContainers)
					{
						if (tc.stringId != "main_hero" && tc.stringId != "tutorial_npc_brother")
						{
							TryAddCharacterToRoster(troopRoster, tc.stringId, tc.troopCount);
						}

					}

					PartyUtilsHandler.GarrisonedTroops = troopRoster;
					PartyUtilsHandler.WoundedTroopArmy = JsonConvert.DeserializeObject<WoundedTroopArmy>(partyUtils.WoundedTroopsGroup);

				}
			});
			task.Wait();
		}

		/*public static void ExecuteSubmit()
		{


			List<TroopContainer> troopContainers = new List<TroopContainer>();
			foreach (TroopRosterElement tre in PartyBase.MainParty.MemberRoster.GetTroopRoster())
			{

				troopContainers.Add(new TroopContainer { stringId = tre.Character.StringId, troopCount = tre.Number, troopXP = tre.Xp });

			}

			CharacterHandler.saveLocationFile = "playerprofile.xml";
			CharacterHandler.saveLocationPath = CharacterHandler.SaveLocationEnum.Configs;
			ArmyContainer ac = new ArmyContainer { TroopContainers = troopContainers, CharacterXML = CharacterHandler.SaveCharacterAndLoadToString(), ArmyStrength = PartyBase.MainParty.TotalStrength };
			//ArmyContainer ac = new ArmyContainer { TroopContainers = troopContainers };

			//XDocument xd = ArmyContainerSerializer.LoadArmyContainerXML(ac);
			Serializer.JsonSerialize(ac);

			var t =  Task.Run(async () =>
			{
				Profile profile = ProfileHandler.GetVerifyProfile();
				var result = await WebRequests.PostAsync<Profile>(UrlHandler.GetUrlFromString(UrlHandler.SaveProfile), profile);
				Serializer.JsonSerialize(result.ServerResponse, "playerprofile.json");
			});
			t.Wait();

			//SAVE GAME
			// Campaign.Current.SaveHandler.QuickSaveCurrentGame();
		}*/

		public static void CommitGeneralLordPartyXP()
        {
					if (PlayerEncounter.Battle.BattleState == BattleState.AttackerVictory || PlayerEncounter.Battle.BattleState == BattleState.DefenderVictory)
			{
				((PlayerEncounter.Battle.BattleState == BattleState.AttackerVictory) ? PlayerEncounter.Battle.AttackerSide : PlayerEncounter.Battle.DefenderSide).DistributeRenown(null, false);
			}

			//CampaignEventDispatcher.Instance.OnPlayerBattleEnd(PlayerEncounter.Battle);
			foreach (MapEventParty mapEventParty in PlayerEncounter.Battle.AttackerSide.Parties)
			{
				PartyBase party = mapEventParty.Party;
				Hero hero = (party == PartyBase.MainParty) ? Hero.MainHero : party.LeaderHero;
				if (hero != null)
				{
					if (mapEventParty.GainedRenown > 0.001f)
					{
						GainRenownAction.Apply(hero, mapEventParty.GainedRenown, true);
					}
				}
				mapEventParty.CommitXpGain();
			}

			PlayerEncounter.Battle.AttackerSide.CommitSkillXpGains();



			foreach (MapEventParty mapEventParty in PlayerEncounter.Battle.DefenderSide.Parties)
			{
				mapEventParty.CommitXpGain();
			}

			PlayerEncounter.Battle.DefenderSide.CommitSkillXpGains();

		}

		public static void UpdateArmyAfterBattle()
        {
			//Serializer.JsonSerialize(result.ServerResponse, "playerprofile.json");

			JObject json = JObject.Parse(Serializer.ReadStringFromFile("playerprofile.json"));
			ArmyContainer ac = Serializer.JsonDeserializeFromStringAc((string)json["ArmyContainer"]);

			WoundedTroopGroup woundedTroopGroup = new WoundedTroopGroup();

			///woundedTroopGroup.timeUntilRecovery = DateTime.Now.AddHours(recoveryCooldown);
			woundedTroopGroup.timeUntilRecovery = DateTime.Now.AddMinutes(recoveryCooldown);

			foreach (TroopRosterElement tc in copyOfTroopRosterPreviousToBattle)
			{
				if (tc.Character.StringId != "main_hero")
				{
					CharacterObject characterObject = CharacterObject.Find(tc.Character.StringId);

					int healthyNumber = tc.Number - tc.WoundedNumber;

					//InformationManager.DisplayMessage(new InformationMessage("Total Previous : " + tc.Number +" Healthy Previous : "+ healthyNumber + " Wounded Previous: " + tc.WoundedNumber));

					if (PartyBase.MainParty.MemberRoster.FindIndexOfTroop(characterObject) == -1)
					{
						int troopsToRecover = (int)(healingRatio * healthyNumber);
						int downedTroops = healthyNumber - troopsToRecover;


						PartyBase.MainParty.AddMember(characterObject, troopsToRecover);
						PartyBase.MainParty.AddMember(characterObject, downedTroops, downedTroops);

						if (downedTroops > 0)
						{
							WoundedTroop woundedTroop = new WoundedTroop { stringId = tc.Character.StringId, troopCount = downedTroops };
							woundedTroopGroup.woundedTroops.Add(woundedTroop);
							woundedTroopGroup.totalWoundedTroops += downedTroops;
						}
						
						//InformationManager.DisplayMessage(new InformationMessage("Character" + characterObject.Name + " recovered and lost:" + troopsToRecover + "; " + downedTroops));
					}
                    else
                    {
						int index1 = PartyBase.MainParty.MemberRoster.FindIndexOfTroop(characterObject);

						int numberThatWentToBattle = tc.Number - tc.WoundedNumber;
						int woundedInBattle = PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(index1) - tc.WoundedNumber;

						//int survingHealthySoldiers = PartyBase.MainParty.MemberRoster.GetElementNumber(characterObject) - PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(index1);
						int deadSoldiers = numberThatWentToBattle - PartyBase.MainParty.MemberRoster.GetElementNumber(characterObject);
						int woundedSoldiers = PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(index1) - tc.WoundedNumber;

						//int dead = healthyNumber - (PartyBase.MainParty.MemberRoster.GetElementNumber(characterObject) - PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(index1));

						//InformationManager.DisplayMessage(new InformationMessage("Previous Wounded :" +  tc.WoundedNumber + " Wounded :" + woundedSoldiers + " Dead :" + deadSoldiers));

						//InformationManager.DisplayMessage(new InformationMessage("Current Wounded :" + PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(index1) + " Current Alive :" + PartyBase.MainParty.MemberRoster.GetElementNumber(characterObject)));

						int troopsToRecover = (int) (healingRatio * (deadSoldiers + woundedSoldiers));
						int downedTroops = (deadSoldiers + woundedSoldiers) - troopsToRecover;

						//PartyBase.MainParty.MemberRoster.WoundTroop(characterObject, -woundedSoldiers);
						PartyBase.MainParty.MemberRoster.AddToCounts(characterObject, -woundedSoldiers, false, -woundedSoldiers, 0, true, -1);

						//InformationManager.DisplayMessage(new InformationMessage("Post change --- Current Wounded :" + PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(index1) + " Current Alive :" + PartyBase.MainParty.MemberRoster.GetElementNumber(characterObject)));

						PartyBase.MainParty.AddMember(characterObject, troopsToRecover);
						PartyBase.MainParty.AddMember(characterObject, downedTroops, downedTroops);

						if(tc.Number != PartyBase.MainParty.MemberRoster.GetElementNumber(index1))
                        {
							PartyBase.MainParty.AddMember(characterObject, tc.Number - PartyBase.MainParty.MemberRoster.GetElementNumber(index1));
						}

						if (downedTroops > 0)
						{
							WoundedTroop woundedTroop = new WoundedTroop { stringId = tc.Character.StringId, troopCount = downedTroops};
							woundedTroopGroup.woundedTroops.Add(woundedTroop);
							woundedTroopGroup.totalWoundedTroops += downedTroops;
						}

						//InformationManager.DisplayMessage(new InformationMessage("Character" + characterObject.Name + " recovered and lost:" + troopsToRecover+ "; " + downedTroops));

					}
				}
			}

			PartyUtilsHandler.WoundedTroopArmy.WoundedTroopsGroup.Add( woundedTroopGroup);

			ExecuteSubmitPartyUtils();

			//SAVE
			//Campaign.Current.SaveHandler.QuickSaveCurrentGame();
			if (BattleTestHandler.BattleTestEnabled == BattleTestHandler.BattleTestEnabledState.None)
			{
				OpponentPartyHandler.AddGoldToParty();
				CommitGeneralLordPartyXP();
				CharacterHandler.HandleAfterBattleHealth();
			}
			ExecuteSubmitAc();
		}

		public static MatchHistory CreateMatchHistory(string battleResult)
        {
			MatchHistory matchHistory = new MatchHistory();

			matchHistory.BattleResult = battleResult;

			JObject playerJson = JObject.Parse(Serializer.ReadStringFromFile("playerprofile.json"));
			ArmyContainer playerAC = Serializer.JsonDeserializeFromStringAc((string)playerJson["ArmyContainer"]);

			matchHistory.Id = (int) playerJson["Id"];
			matchHistory.PlayerElo = (int) playerJson["Elo"];
			matchHistory.PlayerArmyStrength = (float )playerJson["ArmyStrength"];
			matchHistory.PlayerTroopCount = (int)playerJson["TotalTroopCount"];
			matchHistory.PlayerName = (string)playerJson["Name"];

			JObject enemyJson = JObject.Parse(Serializer.ReadStringFromFile("enemyProfile.json"));
			ArmyContainer enemyAC = Serializer.JsonDeserializeFromStringAc((string)enemyJson["ArmyContainer"]);


			matchHistory.EnemyId = (int)enemyJson["Id"];
			matchHistory.EnemyElo = (int)enemyJson["Elo"];
			matchHistory.EnemyArmyStrength = (float)enemyJson["ArmyStrength"];
			matchHistory.EnemyTroopCount = (int)enemyJson["TotalTroopCount"];
			matchHistory.EnemyName = (string)enemyJson["Name"];

			DateTime localDateCurrent = DateTime.Now;
			//String culture = "en-GB";
			//matchHistory.LocalTimeDatePostMatch = localDateCurrent.ToString(dateFormat, CultureInfo.CreateSpecificCulture(dateCulture));
			matchHistory.LocalTimeDatePostMatch = DateTime.Now;
			return matchHistory;
		}


		public static int XpToUpdate()
        {
			return 1000;
        }

		public static int GoldToUpdate()
		{
			return 100;
		}

		
		public static TroopRoster EnemyParty(ArmyContainer armyContainer, int IsRaiderNPCArmy = 0)
		{
			//TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRoster = new TroopRoster(PartyBase.MainParty);


			//CharacterHandler.saveLocationFile = "enemygeneral.xml";
			//CharacterHandler.saveLocationPath = CharacterHandler.SaveLocationEnum.ModuleData;
			//CharacterHandler.LoadXML();

			if (IsRaiderNPCArmy != 2) {

				CharacterHandler.saveLocationFile = "enemygeneral.xml";
				CharacterHandler.saveLocationPath = CharacterHandler.SaveLocationEnum.ModuleData;
				CharacterHandler.WriteToFile(armyContainer.CharacterXML);
				CharacterHandler.LoadXML();
				TryAddCharacterObjectToRoster(troopRoster, CharacterHandler.characterObject, 1); 
			
			}
			foreach (TroopContainer tc in armyContainer.TroopContainers)
            {
				if(tc.stringId != "main_hero" && tc.stringId != "tutorial_npc_brother")

				{
					TryAddCharacterToRoster(troopRoster, tc.stringId, tc.troopCount);
				}
			
			}


			return troopRoster;
		}

		public static void TryAddCharacterObjectToRoster(TroopRoster troopRoster, CharacterObject characterObject, int count)
		{
			if (characterObject != null)
			{
				//InformationManager.DisplayMessage(new InformationMessage("Chegou" + characterId));
				troopRoster.AddToCounts(characterObject, count, false, 0, 0, true, -1);

			}
			else
			{
				InformationManager.DisplayMessage(new InformationMessage("CustomTroopRoster: " + characterObject + " id not found."));
			}
		}


		public static void TryAddCharacterToRoster(TroopRoster troopRoster, string characterId, int count)
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

		public static void TryAddItemToRoster(ItemRoster itemRoster, string itemId, int count)
		{
			foreach (ItemObject item in Items.All)
            {
				if(item.StringId == itemId)
                {
					itemRoster.AddToCounts(item, 999);
					return;
				}

			}

			InformationManager.DisplayMessage(new InformationMessage("Item Id: " + itemId + " id not found."));
		}
	}
}
