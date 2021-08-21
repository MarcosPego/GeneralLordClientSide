
using GeneralLord.Client.Model;
using GeneralLord.Client.Web;
using GeneralLord.FormationBattleTest;
using GeneralLord.FormationPlanHandler;
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

		public static int recoveryMinuteCooldown = 30;
		public static int rankedHourCooldown = 12;


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

		public static List<TooltipProperty> GetPartyTroopInfoFromTwoRosters(TroopRoster troopRoster, TroopRoster troopRoster2, FormationClass formationClass)
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

			foreach (TroopRosterElement troopRosterElement in troopRoster2.GetTroopRoster())
			{
				if (!troopRosterElement.Character.IsHero && troopRosterElement.Character.DefaultFormationClass.Equals(formationClass))
				{
					list.Add(new TooltipProperty("Wounded " + troopRosterElement.Character.Name.ToString(), troopRosterElement.Number.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				}
			}
			return list;
		}

		public static List<TooltipProperty> GetPartyTroopInfoFromThreeRosters(TroopRoster troopRoster, TroopRoster troopRoster2, TroopRoster troopRoster3, FormationClass formationClass)
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

			foreach (TroopRosterElement troopRosterElement in troopRoster2.GetTroopRoster())
			{
				if (!troopRosterElement.Character.IsHero && troopRosterElement.Character.DefaultFormationClass.Equals(formationClass))
				{
					list.Add(new TooltipProperty("Wounded " + troopRosterElement.Character.Name.ToString(), troopRosterElement.Number.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				}
			}


			foreach (TroopRosterElement troopRosterElement in troopRoster3.GetTroopRoster())
			{
				if (!troopRosterElement.Character.IsHero && troopRosterElement.Character.DefaultFormationClass.Equals(formationClass))
				{
					list.Add(new TooltipProperty("Garrisoned " + troopRosterElement.Character.Name.ToString(), troopRosterElement.Number.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
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

		public static List<TooltipProperty> GetPartyTroopWoundedInfo(PartyBase party, FormationClass formationClass)
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			list.Add(new TooltipProperty("", GameTexts.FindText("str_formation_class_string", formationClass.GetName()).ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.Title));
			foreach (TroopRosterElement troopRosterElement in party.MemberRoster.GetTroopRoster())
			{
				if (!troopRosterElement.Character.IsHero && troopRosterElement.Character.DefaultFormationClass.Equals(formationClass))
				{
					list.Add(new TooltipProperty(troopRosterElement.Character.Name.ToString(), troopRosterElement.WoundedNumber.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
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
				ExecuteSubmitProfileWithAc();
				JObject json = JObject.Parse(Serializer.ReadStringFromFile("playerprofile.json"));
				string text = json["Id"].ToString();
				File.WriteAllText(filePath, text);
			}

			UniqueId = Int32.Parse(File.ReadAllText(filePath));
			//InformationManager.DisplayMessage(new InformationMessage(UniqueId.ToString()));
			ExecuteSubmitProfileWithAc();
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


			var t = Task.Run(async () => await ServerRequestsHandler.SubmitPartyUtilsToServer(partyUtils));
			//Information Saved in Server only, doesn't need to wait
			//t.Wait();
		}

		public static void ExecuteSubmitProfileWithAc(bool isInitialSave = false)
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
			Profile profile = ProfileHandler.UpdateProfileAc();


			var t = Task.Run(async () => await ServerRequestsHandler.SubmitPlayerProfile(profile));
			t.Wait();
			//SAVE GAME
			Campaign.Current.SaveHandler.QuickSaveCurrentGame();
		}

		public static void ReceivePartyUtils()
        {
			Profile profile = ProfileHandler.UpdateProfileAc();
			var task = Task.Run(async () => await ServerRequestsHandler.ReceivePlayerPartyUtils(profile.Id));
			task.Wait();
			if (task.Result != null)
            {
				IEnumerable<PartyUtils> partyUtilsList = task.Result;

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
					PartyUtilsHandler.TransferWoundedTroopArmyToRoster();
				}
			}

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

			PartyUtilsHandler.UpdateWoundedTroopsReforged();
			ExecuteSubmitPartyUtils();

			//SAVE
			//Campaign.Current.SaveHandler.QuickSaveCurrentGame();
			if (BattleTestHandler.BattleTestEnabled == BattleTestHandler.BattleTestEnabledState.None)
			{
				OpponentPartyHandler.AddGoldToParty();
				CommitGeneralLordPartyXP();
				CharacterHandler.HandleAfterBattleHealth();
			}
			ExecuteSubmitProfileWithAc();
		}

		public static ArmyContainer GetPlayerFallenArmyContainer()
        {

			List<TroopContainer> troopContainers = new List<TroopContainer>();
			foreach (TroopRosterElement tc in copyOfTroopRosterPreviousToBattle)
			{
				if (tc.Character.StringId != "main_hero")
				{
					CharacterObject characterObject = CharacterObject.Find(tc.Character.StringId);

					int healthyNumber = tc.Number - tc.WoundedNumber;

					//InformationManager.DisplayMessage(new InformationMessage("Total Previous : " + tc.Number +" Healthy Previous : "+ healthyNumber + " Wounded Previous: " + tc.WoundedNumber));

					if (PartyBase.MainParty.MemberRoster.FindIndexOfTroop(characterObject) == -1)
					{
						int fallenTroops = healthyNumber;
						troopContainers.Add(new TroopContainer { stringId = tc.Character.StringId, troopCount = fallenTroops, troopXP = 0 });
					}
					else
					{
						int index1 = PartyBase.MainParty.MemberRoster.FindIndexOfTroop(characterObject);

						int numberThatWentToBattle = tc.Number - tc.WoundedNumber;

						int deadSoldiers = numberThatWentToBattle - PartyBase.MainParty.MemberRoster.GetElementNumber(characterObject);
						int woundedSoldiers = PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(index1) - tc.WoundedNumber;

						troopContainers.Add(new TroopContainer { stringId = tc.Character.StringId, troopCount = deadSoldiers + woundedSoldiers, troopXP = 0 });
					}
				}
			}
			ArmyContainer ac = new ArmyContainer { TroopContainers = troopContainers, CharacterXML = "" };
			return ac;
        }

		public static ArmyContainer GetEnemyFallenArmyContainer(ArmyContainer enemyAC)
		{
			List<TroopContainer> troopContainers = new List<TroopContainer>();
			TroopRoster currentRoster = OpponentPartyHandler.CurrentOpponentParty.Party.MemberRoster;
			foreach (TroopContainer tc in enemyAC.TroopContainers)
			{
				if (tc.stringId != "main_hero")
				{
					CharacterObject characterObject = CharacterObject.Find(tc.stringId);

					if (currentRoster.FindIndexOfTroop(characterObject) == -1)
					{
						int fallenTroops = tc.troopCount;
						troopContainers.Add(new TroopContainer { stringId = tc.stringId, troopCount = fallenTroops, troopXP = 0 });
					}
					else
					{
						int index1 = currentRoster.FindIndexOfTroop(characterObject);

						int deadSoldiers = tc.troopCount - currentRoster.GetElementNumber(characterObject);
						int woundedSoldiers = currentRoster.GetElementWoundedNumber(index1);

						troopContainers.Add(new TroopContainer { stringId = tc.stringId, troopCount = deadSoldiers + woundedSoldiers, troopXP = 0 });
					}
				}
			}
			ArmyContainer ac = new ArmyContainer { TroopContainers = troopContainers, CharacterXML = "" };
			return ac;
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
			matchHistory.PlayerArmyContainer = (string)playerJson["ArmyContainer"];
			matchHistory.PlayerFallenArmyContainer = JsonConvert.SerializeObject(GetPlayerFallenArmyContainer());


			JObject enemyJson = JObject.Parse(Serializer.ReadStringFromFile("enemyProfile.json"));
			ArmyContainer enemyAC = Serializer.JsonDeserializeFromStringAc((string)enemyJson["ArmyContainer"]);


			matchHistory.EnemyId = (int)enemyJson["Id"];
			matchHistory.EnemyElo = (int)enemyJson["Elo"];
			matchHistory.EnemyArmyStrength = (float)enemyJson["ArmyStrength"];
			matchHistory.EnemyTroopCount = (int)enemyJson["TotalTroopCount"];
			matchHistory.EnemyName = (string)enemyJson["Name"];
			matchHistory.EnemyArmyContainer = (string)enemyJson["ArmyContainer"];
			matchHistory.EnemyFallenArmyContainer = JsonConvert.SerializeObject(GetEnemyFallenArmyContainer(enemyAC));

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
