using GeneralLordWebApiClient;
using GeneralLordWebApiClient.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
		public static void ExecuteSubmitAc()
		{


			List<TroopContainer> troopContainers = new List<TroopContainer>();
			foreach (TroopRosterElement tre in PartyBase.MainParty.MemberRoster.GetTroopRoster())
			{

				troopContainers.Add(new TroopContainer { stringId = tre.Character.StringId, troopCount = tre.Number, troopXP = tre.Xp });

			}
			ArmyContainer ac = new ArmyContainer { TroopContainers = troopContainers };

			//XDocument xd = ArmyContainerSerializer.LoadArmyContainerXML(ac);
			Serializer.JsonSerialize(ac);

			Task.Run(async () =>
			{
				Profile profile = ProfileHandler.UpdateProfileAc();
				var result = await WebRequests.PostAsync<Profile>("http://localhost:40519/values/save", profile);
				Serializer.JsonSerialize(result.ServerResponse, "playerprofile.json");
			});

		}

		public static void ExecuteSubmit()
		{


			List<TroopContainer> troopContainers = new List<TroopContainer>();
			foreach (TroopRosterElement tre in PartyBase.MainParty.MemberRoster.GetTroopRoster())
			{

				troopContainers.Add(new TroopContainer { stringId = tre.Character.StringId, troopCount = tre.Number, troopXP = tre.Xp });

			}
			ArmyContainer ac = new ArmyContainer { TroopContainers = troopContainers };

			//XDocument xd = ArmyContainerSerializer.LoadArmyContainerXML(ac);
			Serializer.JsonSerialize(ac);

			Task.Run(async () =>
			{
				Profile profile = ProfileHandler.GetVerifyProfile();
				var result = await WebRequests.PostAsync<Profile>("http://localhost:40519/values/save", profile);
				Serializer.JsonSerialize(result.ServerResponse, "playerprofile.json");
			});

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

					int index = PartyBase.MainParty.MemberRoster.FindIndexOfTroop(characterObject);
					PartyBase.MainParty.MemberRoster.SetElementXp(index, tc.troopXP + XpToUpdate());

					//string wtf = "Char:" + characterObject.Name + " IndexFound:" + index.ToString();
					//InformationManager.DisplayMessage(new InformationMessage(wtf));
				}
			}

			//SAVE
			//Campaign.Current.SaveHandler.QuickSaveCurrentGame();
			ExecuteSubmitAc();
			GiveGoldAction.ApplyBetweenCharacters(null, PartyBase.MainParty.LeaderHero, GoldToUpdate(), true);
		}

		public static int XpToUpdate()
        {
			return 1000;
        }

		public static int GoldToUpdate()
		{
			return 100;
		}


		public static TroopRoster EnemyParty(ArmyContainer armyContainer)
		{
			//TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRoster = new TroopRoster(PartyBase.MainParty);

            foreach (TroopContainer tc in armyContainer.TroopContainers)
            {
				if(tc.stringId != "main_hero")
                {
					TryAddCharacterToRoster(troopRoster, tc.stringId,1);
				}
				
			}
			return troopRoster;
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
