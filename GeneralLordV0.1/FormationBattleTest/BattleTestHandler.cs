using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;

namespace GeneralLord.FormationBattleTest
{
    public class BattleTestHandler
    {
        public static BattleTestEnabledState BattleTestEnabled = BattleTestEnabledState.None;
		public static int CurrentPlayerHealth;

		public enum BattleTestEnabledState
        {
            None = 99,
            BattleTest
        }

        public static void OpenBattleTestMission()
        {
			CurrentPlayerHealth = PartyBase.MainParty.LeaderHero.HitPoints;
			BattleTestEnabled = BattleTestEnabledState.BattleTest;
			Settlement closestHideout = SettlementHelper.FindNearestSettlement((Settlement x) => x.IsHideout() && x.IsActive);
			Clan clan = Clan.BanditFactions.FirstOrDefault((Clan t) => t.Culture == closestHideout.Culture);

			//randomSettlement = SettlementHelper.FindRandomSettlement((Settlement x) => x.IsTown);
			var randomSettlement = SettlementHelper.FindNearestSettlementToPoint(MobileParty.MainParty.Position2D);

			//InformationManager.DisplayMessage(new InformationMessage(randomSettlement.Name.ToString()));
			//MobileParty.MainParty.
			EnterSettlementAction.ApplyForParty(MobileParty.MainParty, randomSettlement);
			//MobileParty mobileParty = MobilePartyHelper.SpawnLordParty(bestAvailableCommander, new Vec2(Hero.MainHero.GetPosition().x, Hero.MainHero.GetPosition().z), 1f);
			OpponentPartyHandler.RemoveOpponentParty();
			OpponentPartyHandler.CurrentOpponentParty = BanditPartyComponent.CreateBanditParty("BattleTest", clan, closestHideout.Hideout, false);
			OpponentPartyHandler.CurrentOpponentParty.InitializeMobileParty(
						 CreateBattleTestEnemyRoster(),
						 CreateBattleTestEnemyRoster(),
						OpponentPartyHandler.CurrentOpponentParty.Position2D,
						0);

			PlayerEncounter.Start();

			//InformationManager.DisplayMessage(new InformationMessage(PartyBase.MainParty.IsSettlement.ToString()));
			PlayerEncounter.Current.SetupFields(PartyBase.MainParty, OpponentPartyHandler.CurrentOpponentParty.Party);
			PlayerEncounter.StartBattle();
			BattleTestMissionManager.OpenBattleTestMission(PlayerEncounter.GetBattleSceneForMapPosition(MobileParty.MainParty.Position2D));
			PlayerEncounter.StartAttackMission();

		}

		public static TroopRoster CreateBattleTestEnemyRoster()
        {
			//TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRoster = new TroopRoster(PartyBase.MainParty);
			JsonBattleConfig.TryAddCharacterToRoster(troopRoster, "imperial_recruit", 1);

			//JsonBattleConfig.TryAddCharacterToRoster(troopRosterA, "aserai_recruit", 1);
			//this.TryAddCharacterToRoster(troopRosterA, "vlandian_recruit", 32);
			//this.TryAddCharacterToRoster(troopRosterA, "sturgian_recruit", 18);
			//this.TryAddCharacterToRoster(troopRosterA, "mercenary_1", 10);
			//this.TryAddCharacterToRoster(troopRosterA, "mercenary_2", 20);
			//InformationManager.DisplayMessage(new InformationMessage(troopRoster.TotalManCount.ToString()));

			return troopRoster;
		}
	}
}
