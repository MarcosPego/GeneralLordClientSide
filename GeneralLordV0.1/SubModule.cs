using TaleWorlds.MountAndBlade;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.CampaignSystem;
using EnhancedBattleTest.GameMode;
using System;
using HarmonyLib;
using TaleWorlds.ObjectSystem;
using GeneralLordWebApiClient.Model;
using GeneralLordWebApiClient;
using System.IO;
using System.Reflection;
using GeneralLord.FormationPlanHandler;

namespace GeneralLord
{
	public class SubModule : MBSubModuleBase
	{
		public static bool IsMultiplayer;
		protected override void OnSubModuleLoad()
		{
			base.OnSubModuleLoad();

			try
            {
                new Harmony("mod.generallord").PatchAll();
            }
            catch (Exception ex)
            {
				InformationManager.DisplayMessage(new InformationMessage(ex.Message));//GenericHelpers.LogException("Patch Failed", ex);
			}
			Serializer.EnsureSaveDirectory();

			string formation_path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));
			string formation_filePath;


			EnemyFormationHandler.AttackSelectedFormation = -1;
			EnemyFormationHandler.DefensiveSelectedFormation = -1;
			formation_filePath = Path.Combine(formation_path, "ModuleData", "AttackerSelectedFormation.txt");
			if (File.Exists(formation_filePath))
			{
				EnemyFormationHandler.AttackSelectedFormation = Int32.Parse(File.ReadAllText(formation_filePath));
			}
			formation_filePath = Path.Combine(formation_path, "ModuleData", "DefenderSelectedFormation.txt");
			if (File.Exists(formation_filePath))
			{
				EnemyFormationHandler.DefensiveSelectedFormation = Int32.Parse(File.ReadAllText(formation_filePath));
			}


			UrlHandler.ReleaseVersion(false);
		}

		protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
			base.OnGameStart(game, gameStarterObject);
			CampaignGameStarter campaignGameStarter = gameStarterObject as CampaignGameStarter;
			if (campaignGameStarter != null)
			{
				this._mainManager = new MainManager();
				campaignGameStarter.AddBehavior(this._mainManager);

			}
		}

		public override void OnCampaignStart(Game game, object gameStarterObject)
		{
			base.OnCampaignStart(game, gameStarterObject);
			CampaignGameStarter campaignGameStarter = gameStarterObject as CampaignGameStarter;
			if (campaignGameStarter != null)
			{
				this._mainManager = new MainManager();
				campaignGameStarter.AddBehavior(this._mainManager);

			}
		}

		public override void OnGameEnd(Game game)
		{
			base.OnGameEnd(game);
			this._mainManager = null;
		}
		

		protected override void OnApplicationTick(float dt)
		{
			base.OnApplicationTick(dt);
			if (this._mainManager != null)
			{
				this._mainManager.TickCampaignBehavior();
			}
		}

		private MainManager _mainManager = null;
	}
}