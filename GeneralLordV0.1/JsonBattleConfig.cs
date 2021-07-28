using GeneralLordWebApiClient;
using GeneralLordWebApiClient.Model;
using Helpers;
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

namespace GeneralLord
{
    public class JsonBattleConfig
    {
		public static int UniqueId = 0;

		public static string dateFormat = "G";
		public static string dateCulture = "en-GB";

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
			/*int num = 0;
			foreach (FlattenedTroopRosterElement troopRosterElement in PartyBase.MainParty.MemberRoster.ToFlattenedRoster())
			{
				CharacterObject troop = troopRosterElement.Troop;
				bool flag = Campaign.Current.Models.PartyTroopUpgradeModel.CanTroopGainXp(PartyBase.MainParty, troop);
				InformationManager.DisplayMessage(new InformationMessage(troopRosterElement.XpGained.ToString()));
				if (!troopRosterElement.IsKilled && troopRosterElement.XpGained > 0 && flag)
				{
					int num2 = Campaign.Current.Models.PartyTrainingModel.CalculateXpGainFromBattles(troopRosterElement, PartyBase.MainParty);
					InformationManager.DisplayMessage(new InformationMessage(num2.ToString()));
					int num3 = Campaign.Current.Models.PartyTrainingModel.GenerateSharedXp(troop, num2, MobileParty.MainParty);
					InformationManager.DisplayMessage(new InformationMessage(num2.ToString()));
					if (num3 > 0)
					{
						num += num3;
						num2 -= num3;
					}
					if (!troop.IsHero)
					{
						PartyBase.MainParty.MemberRoster.AddXpToTroop(num2, troop);
					}
				}
			}
			MobilePartyHelper.PartyAddSharedXp(MobileParty.MainParty, (float)num);*/

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

			foreach (TroopContainer tc in ac.TroopContainers)
			{
				if (tc.stringId != "main_hero")
				{
					CharacterObject characterObject = CharacterObject.Find(tc.stringId);
					if(PartyBase.MainParty.MemberRoster.FindIndexOfTroop(characterObject) == -1)
                    {
						PartyBase.MainParty.AddMember(characterObject, tc.troopCount);

					}
                    else
                    {
						int index1 = PartyBase.MainParty.MemberRoster.FindIndexOfTroop(characterObject);

						PartyBase.MainParty.AddMember(characterObject, tc.troopCount - PartyBase.MainParty.MemberRoster.GetElementNumber(characterObject));
						

					}

					//int index = PartyBase.MainParty.MemberRoster.FindIndexOfTroop(characterObject);
					//PartyBase.MainParty.MemberRoster.SetElementXp(index, tc.troopXP + XpToUpdate());

					//string wtf = "Char:" + characterObject.Name + " IndexFound:" + index.ToString();
					//InformationManager.DisplayMessage(new InformationMessage(wtf));
				}
			}

			//SAVE
			//Campaign.Current.SaveHandler.QuickSaveCurrentGame();
			OpponentPartyHandler.AddGoldToParty();
			CommitGeneralLordPartyXP();
			ExecuteSubmitAc();


			//GiveGoldAction.ApplyBetweenCharacters(null, PartyBase.MainParty.LeaderHero, GoldToUpdate(), true);
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


			CharacterHandler.saveLocationFile = "enemygeneral.xml";
			CharacterHandler.saveLocationPath = CharacterHandler.SaveLocationEnum.ModuleData;
			CharacterHandler.LoadXML();

			if(IsRaiderNPCArmy != 2) TryAddCharacterObjectToRoster(troopRoster, CharacterHandler.characterObject, 1);
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
	}
}
