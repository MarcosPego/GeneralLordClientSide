using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnhancedBattleTest.Data;
using SandBox;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;
using EnhancedBattleTest.GameMode;
using TaleWorlds.SaveSystem.Load;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;


namespace GeneralLord
{
    public class GeneralLordGameManager //: CampaignGameManager
    {
        /*private bool _loadingSavedGame;

        private LoadResult _loadedGameResult;


        private int _seed = 1234;


        public GeneralLordGameManager()
        {
            this._loadingSavedGame = false;
            //this._seed = ((int)DateTime.Now.Ticks & 65535);
        }

        // Token: 0x06000059 RID: 89 RVA: 0x00007DAF File Offset: 0x00005FAF
        public GeneralLordGameManager(int seed)
        {
            this._loadingSavedGame = false;
            this._seed = seed;
        }

        public GeneralLordGameManager(LoadResult loadedGameResult)
        {
            this._loadingSavedGame = true;
            this._loadedGameResult = loadedGameResult;
        }

        protected override void DoLoadingForGameManager(
          GameManagerLoadingSteps gameManagerLoadingStep,
          out GameManagerLoadingSteps nextStep)
        {
            nextStep = GameManagerLoadingSteps.None;
            switch (gameManagerLoadingStep)
            {
                case GameManagerLoadingSteps.PreInitializeZerothStep:
                    nextStep = GameManagerLoadingSteps.FirstInitializeFirstStep;
                    break;
                case GameManagerLoadingSteps.FirstInitializeFirstStep:
                    MBGameManager.LoadModuleData(this._loadingSavedGame);
                    nextStep = GameManagerLoadingSteps.WaitSecondStep;
                    break;
                case GameManagerLoadingSteps.WaitSecondStep:
                    StartNewGame();
                    nextStep = GameManagerLoadingSteps.SecondInitializeThirdState;
                    break;
                case GameManagerLoadingSteps.SecondInitializeThirdState:
                    MBGlobals.InitializeReferences();
                    if (this._loadingSavedGame)
                    {
                        MBDebug.Print("Initializing new game begin...");
                        TaleWorlds.CampaignSystem.Campaign campaign = new TaleWorlds.CampaignSystem.Campaign(CampaignGameMode.Campaign);
                        Game.CreateGame(campaign, this);
                        campaign.SetLoadingParameters(Campaign.GameLoadingType.NewCampaign, this._seed);
                        MBDebug.Print("Initializing new game end...");
                    } 
                    else
                    {
                        MBDebug.Print("Initializing new game begin...");
                        var campaign = new Campaign();
                        TaleWorlds.Core.Game.CreateGame(campaign, this);
                        campaign.SetLoadingParameters(TaleWorlds.CampaignSystem.Campaign.GameLoadingType.NewCampaign, _seed);
                        MBDebug.Print("Initializing new game end...");
                    }
                    TaleWorlds.Core.Game.Current.DoLoading();
                    nextStep = GameManagerLoadingSteps.PostInitializeFourthState;
                    break;
                case GameManagerLoadingSteps.PostInitializeFourthState:
                    bool flag = true;
                    foreach (MBSubModuleBase subModule in Module.CurrentModule.SubModules)
                        flag = flag && subModule.DoLoading(TaleWorlds.Core.Game.Current);
                    nextStep = flag ? GameManagerLoadingSteps.FinishLoadingFifthStep : GameManagerLoadingSteps.PostInitializeFourthState;
                    break;
                case GameManagerLoadingSteps.FinishLoadingFifthStep:
                    nextStep = TaleWorlds.Core.Game.Current.DoLoading() ? GameManagerLoadingSteps.None : GameManagerLoadingSteps.FinishLoadingFifthStep;
                    EnhancedBattleTestPartyController.Initialize();
                    break;
            }

        }

        public override void OnGameStart(TaleWorlds.Core.Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);

            InitializeGameTexts(TaleWorlds.Core.Game.Current.GameTextManager);

            //Game.Current.GameStateManager.PushState(Game.Current.GameStateManager.CreateState<GeneralLordMainGameState>());
        }

        public override void OnGameEnd(TaleWorlds.Core.Game game)
        {
            base.OnGameEnd(game);

            EnhancedBattleTestPartyController.OnGameEnd();
        }

        public override void OnLoadFinished()
        {
            base.OnLoadFinished();

            if (!this._loadingSavedGame)
            {
                MBDebug.Print("Switching to menu window...");
                this.LaunchSandboxCharacterCreation();
                return;
            }
            else
            {
                //Game.Current.GameStateManager.PushState(Game.Current.GameStateManager.CreateState<GeneralLordMainGameState>());
            }
            CampaignEvents.Instance.OnGameLoadFinished();
        }


        private void LaunchSandboxCharacterCreation()
        {
            CharacterCreationState gameState = Game.Current.GameStateManager.CreateState<CharacterCreationState>(new object[]
            {
                new SandboxCharacterCreationContent()
            });
            Game.Current.GameStateManager.CleanAndPushState(gameState, 0);
        }

        private void InitializeGameTexts(GameTextManager gameTextManager)
        {
            //gameTextManager.LoadGameTexts(BasePath.Name + "Modules/Native/ModuleData/multiplayer_strings.xml");
            //gameTextManager.LoadGameTexts(BasePath.Name + "Modules/Native/ModuleData/global_strings.xml");
            //gameTextManager.LoadGameTexts(BasePath.Name + "Modules/Native/ModuleData/module_strings.xml");
            //gameTextManager.LoadGameTexts(BasePath.Name + "Modules/Native/ModuleData/native_strings.xml");
            gameTextManager.LoadGameTexts(ModuleHelper.GetXmlPath("EnhancedBattleTest",
                "module_strings"));
        }
        */
    }
}
