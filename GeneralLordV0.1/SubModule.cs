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

		//public static EnhancedBattleTestSubModule Instance { get; private set; }

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

			//EnhancedBattleTestSubModule.Instance = this;

			/*Module.CurrentModule.AddInitialStateOption(new InitialStateOption("GeneralLordGameMode",
			new TextObject("{=EnhancedBattleTest_singleplayerbattleoption}GeneralLordGameMode"), 0,
			() =>
			{
				IsMultiplayer = false;
				MBGameManager.StartNewGame(new GeneralLordGameManager());
			}, () => false));*/
		}

		protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
			base.OnGameStart(game, gameStarterObject);
			CampaignGameStarter campaignGameStarter = gameStarterObject as CampaignGameStarter;
			if (campaignGameStarter != null)
			{
				//this._partyManager = new PartyManager();
				this._mainManager = new MainManager();
				//campaignGameStarter.AddBehavior(this._partyManager);
				campaignGameStarter.AddBehavior(this._mainManager);

			}
		}

		public override void OnCampaignStart(Game game, object gameStarterObject)
		{

			//base.OnGameStart(game, gameStarterObject);
			base.OnCampaignStart(game, gameStarterObject);
			CampaignGameStarter campaignGameStarter = gameStarterObject as CampaignGameStarter;
			if (campaignGameStarter != null)
			{


				
				//this._partyManager = new PartyManager();
				this._mainManager = new MainManager();
				//campaignGameStarter.AddBehavior(this._partyManager);
				campaignGameStarter.AddBehavior(this._mainManager);

			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000024F4 File Offset: 0x000006F4
		public override void OnGameEnd(Game game)
		{
			base.OnGameEnd(game);
			//this._partyManager = null;
			this._mainManager = null;
		}
		

		// Token: 0x06000012 RID: 18 RVA: 0x00002508 File Offset: 0x00000708
		protected override void OnApplicationTick(float dt)
		{
			base.OnApplicationTick(dt);
			//if (this._partyManager != null)
			//{
			//	this._partyManager.TickCampaignBehavior();
			//}
			if (this._mainManager != null)
			{
				this._mainManager.TickCampaignBehavior();
			}
		}

		private PartyManager _partyManager = null;
		private MainManager _mainManager = null;
	}
}