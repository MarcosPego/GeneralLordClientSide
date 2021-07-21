using GeneralLord.FormationBattleTest.BattleTestCustomView;
using SandBox.View;
using SandBox.View.Missions;
using SandBox.ViewModelCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.LegacyGUI.Missions;
using TaleWorlds.MountAndBlade.View.Missions;

namespace GeneralLord.FormationBattleTest
{
	[ViewCreatorModule]
	public class BattleTestMissionView
    {

		[ViewMethod("BattleTest")]
		public static MissionView[] OpenBattleMission(Mission mission)
		{
			return new List<MissionView>
			{
				new CampaignMissionView(),
				ViewCreator.CreateMissionSingleplayerEscapeMenu(CampaignOptions.IsIronmanMode),
				ViewCreator.CreateOptionsUIHandler(),
				ViewCreator.CreateMissionMainAgentGamepadEquipDropView(mission),
				ViewCreator.CreateMissionBattleScoreUIHandler(mission, new SPScoreboardVM(null)),
				ViewCreator.CreateMissionAgentLabelUIHandler(mission),
				ViewCreator.CreateMissionOrderUIHandler(null),
				ViewCreator.CreatePlayerRoleSelectionUIHandler(null),
				new OrderTroopPlacer(),
				new MissionSingleplayerUIHandler(),
				ViewCreator.CreateMissionAgentStatusUIHandler(mission),
				ViewCreator.CreateMissionMainAgentEquipmentController(mission),
				ViewCreator.CreateMissionMainAgentCheerBarkControllerView(mission),
				ViewCreator.CreateMissionAgentLockVisualizerView(mission),
				new MusicBattleMissionView(false),
				ViewCreator.CreateMissionBoundaryCrossingView(),
				new MissionBoundaryWallView(),
				ViewCreator.CreateMissionFormationMarkerUIHandler(mission),
				ViewCreator.CreateSingleplayerMissionKillNotificationUIHandler(),
				ViewCreator.CreateMissionSpectatorControlView(mission),
				new MissionItemContourControllerView(),
				new MissionAgentContourControllerView(),
				new MissionPreloadView(),
				new CampaignBattleSpectatorView(),
				ViewCreator.CreatePhotoModeView(),
				new BattleTestView()
			}.ToArray();
		}

	}


}
