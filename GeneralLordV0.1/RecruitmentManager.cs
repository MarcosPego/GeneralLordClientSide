using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
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

		public static TroopRoster RecruitmentTest()
		{
			//TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRosterA = new TroopRoster(PartyBase.MainParty);
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

			return troopRosterA;
		}
	}
}
