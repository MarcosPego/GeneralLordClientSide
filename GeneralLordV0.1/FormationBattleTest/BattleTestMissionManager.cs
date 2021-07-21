using Helpers;
using SandBox;
using SandBox.Source.Missions;
using SandBox.Source.Missions.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic;

namespace GeneralLord.FormationBattleTest
{
    [MissionManager]
    public class BattleTestMissionManager
    {

		[MissionMethod]
		public static Mission OpenBattleTestMission(string scene)
		{
			return OpenBattleTestMission(SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, "", false));
		}

		[MissionMethod]
		public static Mission OpenBattleTestMission(MissionInitializerRecord rec)
		{
			bool isPlayerSergeant = MobileParty.MainParty.MapEvent.IsPlayerSergeant();
			bool isPlayerInArmy = MobileParty.MainParty.Army != null;
			List<string> heroesOnPlayerSideByPriority = HeroHelper.OrderHeroesOnPlayerSideByPriority();
			return MissionState.OpenNew("BattleTest", rec, delegate (Mission mission)
			{
				MissionBehaviour[] array = new MissionBehaviour[27];
				array[0] = new MissionOptionsComponent();
				array[1] = new CampaignMissionComponent();
				array[2] = new BattleEndLogic();
				array[3] = new MissionCombatantsLogic(MobileParty.MainParty.MapEvent.InvolvedParties, PartyBase.MainParty, MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender), MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker), Mission.MissionTeamAITypeEnum.FieldBattle, isPlayerSergeant);
				array[4] = new MissionDefaultCaptainAssignmentLogic();
				array[5] = new BattleMissionStarterLogic();
				array[6] = new BattleSpawnLogic("battle_set");
				array[7] = new AgentBattleAILogic();
                array[8] = CreateCampaignMissionAgentSpawnLogic(false);
				array[9] = new BaseMissionTroopSpawnHandler();
				array[10] = new BattleObserverMissionLogic();
				array[11] = new BattleAgentLogic();
				array[12] = new MountAgentLogic();
				array[13] = new AgentVictoryLogic();
				array[14] = new MissionDebugHandler();
				array[15] = new MissionAgentPanicHandler();
				array[16] = new MissionHardBorderPlacer();
				array[17] = new MissionBoundaryPlacer();
				array[18] = new MissionBoundaryCrossingHandler();
				array[19] = new BattleMissionAgentInteractionLogic();
				array[20] = new FieldBattleController();
				array[21] = new AgentMoraleInteractionLogic();
				array[22] = new HighlightsController();
				array[23] = new BattleHighlightsController();
				array[24] = new AssignPlayerRoleInTeamMissionController(!isPlayerSergeant, isPlayerSergeant, isPlayerInArmy, heroesOnPlayerSideByPriority, FormationClass.NumberOfRegularFormations);
				int num = 25;
				Hero leaderHero = MapEvent.PlayerMapEvent.AttackerSide.LeaderParty.LeaderHero;
				string attackerGeneralName = (leaderHero != null) ? leaderHero.Name.ToString() : null;
				Hero leaderHero2 = MapEvent.PlayerMapEvent.DefenderSide.LeaderParty.LeaderHero;
				array[num] = new CreateBodyguardMissionBehavior(attackerGeneralName, (leaderHero2 != null) ? leaderHero2.Name.ToString() : null, null, null, true);
				array[26] = new EquipmentControllerLeaveLogic();
				return array;
			}, true, true);
		}


		private static MissionAgentSpawnLogic CreateCampaignMissionAgentSpawnLogic(bool isSiege = false)
		{
			return new MissionAgentSpawnLogic(new IMissionTroopSupplier[]
			{
				new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Defender, null),
				new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Attacker, null)
			}, PartyBase.MainParty.Side, isSiege);
		}
	}
}
