using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace GeneralLord
{
    public class RecruitmentManager
    {
		public static void TesteRoster()
        {
			PartyScreenManager.OpenScreenWithCondition(new PartyScreenLogic.IsTroopTransferableDelegate(TroopTransferableDelegate), 
				new PartyPresentationDoneButtonConditionDelegate(PartyScreenDoneCondition), 
				new PartyPresentationDoneButtonDelegate(PartyScreenDoneClicked), 
				PartyScreenLogic.TransferState.TransferableWithTrade, PartyScreenLogic.TransferState.NotTransferable, new TextObject("Test"), 150, false);
		}

		public static void OpenNPCRecruitmentRoster()
        {

			TroopRoster recruitmentRoster = NPCRecruitmentTest();


			PartyScreenManager.OpenScreenAsQuest(recruitmentRoster, new TextObject("{=}Recruit Troops", null),
				0, new PartyPresentationDoneButtonConditionDelegate(PartyScreenDoneCondition),
				new PartyScreenClosedDelegate(SellPrisonersDoneHandler),
				new PartyScreenLogic.IsTroopTransferableDelegate(RightTransferableDelegate), null);
		}

		public static void OpenRecruitmentRoster()
		{

			TroopRoster recruitmentRoster = RecruitmentTest();


			PartyScreenManager.OpenScreenAsQuest(recruitmentRoster, new TextObject("{=}Recruit Troops", null),
				0, new PartyPresentationDoneButtonConditionDelegate(PartyScreenDoneCondition),
				new PartyScreenClosedDelegate(SellPrisonersDoneHandler), 
				new PartyScreenLogic.IsTroopTransferableDelegate(RightTransferableDelegate), null);
		}

		private static bool TroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
		{
			return true;
		}

		private static bool PartyScreenDoneClicked(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, List<MobileParty> leftParties, List<MobileParty> rigthParties)
		{
			return true;
		}

		private static Tuple<bool, TextObject> PartyScreenDoneCondition(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, int leftLimitNum, int rightLimitNum)
		{
			return new Tuple<bool, TextObject>(true, new TextObject("", null));
		}

		private static void SellPrisonersDoneHandler(PartyBase leftOwnerParty, TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, PartyBase rightOwnerParty, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster)
		{
			//SellPrisonersAction.ApplyForSelectedPrisoners(MobileParty.MainParty, leftPrisonRoster, Hero.MainHero.CurrentSettlement);

			//GiveGoldAction.ApplyBetweenCharacters(null, PartyBase.MainParty.LeaderHero, 100, false);
			//return true;
		}

		public static bool RightTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
		{
			return side == PartyScreenLogic.PartyRosterSide.Left && type == PartyScreenLogic.TroopType.Member;
		}

		public static bool VerifyIfValidCulture(string stringIdObject)
		{
			//InformationManager.DisplayMessage(new InformationMessage(CharacterTierHandler.CharacterMainCulture.StringId));

			if (PartyBase.MainParty.LeaderHero.Culture.StringId == stringIdObject)
			{
				return true;
			}

			if (PartyBase.MainParty.LeaderHero.Clan.Tier >= 1)
			{
				return true;
			}

			return false;
		}

		public static TroopRoster RecruitmentTest()
		{
			//TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRosterA = new TroopRoster(PartyBase.MainParty);

			if (VerifyIfValidCulture("aserai")) JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "aserai_recruit", 999);
			if (VerifyIfValidCulture("battania")) JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "battanian_volunteer", 999);
			if (VerifyIfValidCulture("khuzait")) JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "khuzait_nomad", 999);
			if (VerifyIfValidCulture("vlandia")) JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "vlandian_recruit", 999);
			if (VerifyIfValidCulture("sturgia")) JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "sturgian_recruit", 999);
			if (VerifyIfValidCulture("empire")) JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "imperial_recruit", 999);


			if (PartyBase.MainParty.LeaderHero.Clan.Tier >= 2)
            {
				JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "imperial_vigla_recruit", 999);
				JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "sturgian_warrior_son", 999);
				JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "vlandian_squire", 999);
				JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "aserai_youth", 999);
				JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "battanian_highborn_youth", 999);
			}

			if (PartyBase.MainParty.LeaderHero.Clan.Tier >= 3)
			{
				JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "karakhuzaits_tier_1", 999);
				JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "beni_zilal_tier_1", 999);
				JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "ghilman_tier_1", 999);
				JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "skolderbrotva_tier_1", 999);
			}

			//JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "imperial_archer", 999);
			//this.TryAddCharacterToRoster(troopRosterA, "vlandian_recruit", 32);
			//this.TryAddCharacterToRoster(troopRosterA, "sturgian_recruit", 18);
			//this.TryAddCharacterToRoster(troopRosterA, "mercenary_1", 10);
			//this.TryAddCharacterToRoster(troopRosterA, "mercenary_2", 20);
			//InformationManager.DisplayMessage(new InformationMessage(troopRoster.TotalManCount.ToString()));

			return troopRosterA;
		}



		public static TroopRoster NPCRecruitmentTest()
		{
			//TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRosterA = new TroopRoster(PartyBase.MainParty);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "looter", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "deserter", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "sea_raiders_bandit", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "sea_raiders_raider", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "sea_raiders_chief", 999);

			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "mountain_bandits_bandit", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "mountain_bandits_raider", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "mountain_bandits_chief", 999);

            JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "sea_raiders_boss", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "mountain_bandits_boss", 999);

			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "imperial_recruit", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "imperial_archer", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "aserai_recruit", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "vlandian_recruit", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "sturgian_recruit", 999);
			//this.TryAddCharacterToRoster(troopRosterA, "vlandian_recruit", 32);
			//this.TryAddCharacterToRoster(troopRosterA, "sturgian_recruit", 18);
			//this.TryAddCharacterToRoster(troopRosterA, "mercenary_1", 10);
			//this.TryAddCharacterToRoster(troopRosterA, "mercenary_2", 20);
			//InformationManager.DisplayMessage(new InformationMessage(troopRoster.TotalManCount.ToString()));


			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "imperial_trained_infantryman", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "imperial_trained_infantryman", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "imperial_legionary", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "imperial_equite", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "imperial_cataphract", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "imperial_elite_cataphract", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "imperial_legionary", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "imperial_trained_archer", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "imperial_veteran_archer", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "imperial_palatine_guard", 999);

			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "khuzait_raider", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "khuzait_hunter", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "khuzait_horseman", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "khuzait_qanqli", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "khuzait_horse_archer", 999);
			JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "khuzait_heavy_lancer", 999);

			return troopRosterA;
		}
	}
}
