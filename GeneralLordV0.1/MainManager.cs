using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Engine.Screens;
using SandBox.View.Map;
using GeneralLordWebApiClient.Model;
using Newtonsoft.Json.Linq;
using Helpers;
using TaleWorlds.Library;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem.Actions;
using SandBox.View.Menu;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors.Towns;
using TaleWorlds.ObjectSystem;
using GeneralLordWebApiClient;
using MatchHistory = GeneralLordWebApiClient.Model.MatchHistory;
using GeneralLord.FormationBattleTest;
using CunningLords.Interaction;
using GeneralLord.Client.Web;

namespace GeneralLord
{
    internal class MainManager : CampaignBehaviorBase
    {
        public MainManager()
        {
            this._initializeState = true;
            this._isFirstGameLaunch = false;
        }

        public void TickCampaignBehavior()
        {
            //JObject json = JObject.Parse(Serializer.ReadStringFromFile("playerprofile.json"));
            //ArmyContainer ac = Serializer.JsonDeserializeFromStringAc((string)json["ArmyContainer"]);



            if (PlayerEncounter.Current != null)
            {


                if (ScreenManager.TopScreen is MapScreen && PlayerEncounter.Current.EncounterState == PlayerEncounterState.Begin)
                {
                    PlayerEncounter.Finish(false);
                    if(MobileParty.MainParty.CurrentSettlement != null) LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);

                    int hasNPCBrother = PartyBase.MainParty.MemberRoster.FindIndexOfTroop(CharacterObject.Find("tutorial_npc_brother"));
                    if (hasNPCBrother != -1)
                    {
                        PartyBase.MainParty.MemberRoster.RemoveTroop(CharacterObject.Find("tutorial_npc_brother"));
                    }

                    _isFirstGameLaunch = true;
                }

                else if (PlayerEncounter.Current.EncounterState == PlayerEncounterState.Wait)
                {
                    /*if (BattleTestHandler.BattleTestEnabled == BattleTestHandler.BattleTestEnabledState.BattleTest)
                    {
                        JsonBattleConfig.UpdateArmyAfterBattle();
                        if (MobileParty.MainParty.CurrentSettlement != null) LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);
                        PlayerEncounter.Finish(false);
                        OpponentPartyHandler.RemoveOpponentParty();
                    }*/

                    if (BattleTestHandler.BattleTestEnabled == BattleTestHandler.BattleTestEnabledState.BattleTest)
                    {
                        CharacterHandler.HandleBattleTestlRestoreHealth();
                    }

                    if (BattleTestHandler.BattleTestEnabled == BattleTestHandler.BattleTestEnabledState.None)
                    {
                        CampaignBattleResult campaignBattleResult = CampaignBattleResult.GetResult(PlayerEncounter.Battle.BattleState);
                        MatchHistory matchHistory = new MatchHistory();
                        if (campaignBattleResult.PlayerVictory)
                        {
                            matchHistory = JsonBattleConfig.CreateMatchHistory("PlayerVictory");
                        }
                        else
                        {
                            matchHistory = JsonBattleConfig.CreateMatchHistory("PlayerDefeat");
                        }
                        var t = Task.Run(async () => await ServerRequestsHandler.SavePostBattle(matchHistory));
                        t.Wait();
                    }
                    JsonBattleConfig.UpdateArmyAfterBattle();
                    if (MobileParty.MainParty.CurrentSettlement != null) LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);

                    PlayerEncounter.Finish(false);

                    OpponentPartyHandler.RemoveOpponentParty();


                    if (BattleTestHandler.BattleTestEnabled == BattleTestHandler.BattleTestEnabledState.None) ScreenManager.PopScreen();

                    _mainManagerScreen._viewModel.MainOverview.RefreshValues();


                    //InformationManager.DisplayMessage(new InformationMessage("Wait worked"));
                    //else if (PlayerEncounter.CampaignBattleResult != null) PlayerEncounter.Update();

                }
                /*
                else if (BattleTestHandler.BattleTestEnabled == BattleTestHandler.BattleTestEnabledState.None  && (PlayerEncounter.Current.EncounterState == PlayerEncounterState.PlayerVictory || PlayerEncounter.Current.EncounterState == PlayerEncounterState.PlayerTotalDefeat ||
                    PlayerEncounter.Current.EncounterState == PlayerEncounterState.CaptureHeroes || PlayerEncounter.Current.EncounterState == PlayerEncounterState.FreeHeroes ||
                    PlayerEncounter.Current.EncounterState == PlayerEncounterState.LootParty || PlayerEncounter.Current.EncounterState == PlayerEncounterState.LootInventory 
                    ))
                {
                    if (BattleTestHandler.BattleTestEnabled == BattleTestHandler.BattleTestEnabledState.None)
                    {
                        CampaignBattleResult campaignBattleResult = CampaignBattleResult.GetResult(PlayerEncounter.Battle.BattleState);
                        MatchHistory matchHistory = new MatchHistory();
                        if (campaignBattleResult.PlayerVictory)
                        {
                            matchHistory = JsonBattleConfig.CreateMatchHistory("PlayerVictory");
                        }
                        else
                        {
                            matchHistory = JsonBattleConfig.CreateMatchHistory("PlayerDefeat");
                        }
                        var t = Task.Run(async () =>
                        {
                            await WebRequests.PostAsync(UrlHandler.GetUrlFromString(UrlHandler.PostBattleProcess), matchHistory);
                            //Serializer.JsonSerialize(result.ServerResponse, "playerprofile.json");
                        });
                        t.Wait();

                        JsonBattleConfig.UpdateArmyAfterBattle();
                    }
                    PlayerEncounter.Finish(false);

                    OpponentPartyHandler.RemoveOpponentParty();


                    if(BattleTestHandler.BattleTestEnabled == BattleTestHandler.BattleTestEnabledState.None) ScreenManager.PopScreen();
                }*/
            }

            if (ScreenManager.TopScreen is MapScreen)
            {
                Serializer.ThisCharacterName = PartyBase.MainParty.LeaderHero.Name.ToString();
                JsonBattleConfig.VerifyUniqueFile();
                if(this._initializeState && !this._isFirstGameLaunch) JsonBattleConfig.ReceivePartyUtils();

                _initializeState = false;
                _mainManagerScreen = new MainManagerScreen();


                ScreenManager.PushScreen(_mainManagerScreen);

            }
            if(ScreenManager.TopScreen is MainManagerScreen)  { PartyUtilsHandler.TickForRecovery(_mainManagerScreen); } /**/

            if (Mission.Current == null && Input.IsKeyDown(InputKey.LeftControl))
            {            
                if (false && Input.IsKeyReleased(InputKey.T))
                {
                    Serializer.ThisCharacterName = PartyBase.MainParty.LeaderHero.Name.ToString();
                    JsonBattleConfig.VerifyUniqueFile();
                    if (this._initializeState && !this._isFirstGameLaunch) JsonBattleConfig.ReceivePartyUtils();

                    _initializeState = false;
                    _mainManagerScreen = new MainManagerScreen();


                    ScreenManager.PushScreen(_mainManagerScreen);
                    //PartyBase.MainParty.LeaderHero.HitPoints = PartyBase.MainParty.LeaderHero.CharacterObject.MaxHitPoints();
                }
                if (false && Input.IsKeyReleased(InputKey.G))
                {
                    foreach  (TroopRosterElement element in  PartyBase.MainParty.MemberRoster.GetTroopRoster())
                    {
                        int index = PartyBase.MainParty.MemberRoster.FindIndexOfTroop(element.Character);

                        int wounded = PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(index);


                        PartyBase.MainParty.MemberRoster.WoundTroop(element.Character, -wounded);
                      
                    }

                }
                if (false && Input.IsKeyReleased(InputKey.K))
                {
                    PartyBase.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(3);
                }
            }
        }

        public override void RegisterEvents()
        {
        }
        public override void SyncData(IDataStore dataStore)
        {
        }

        private Settlement randomSettlement;

        private bool _initializeState = true;
        private bool _isFirstGameLaunch;
        private object localDate;

        private MainManagerScreen _mainManagerScreen;
    }
}
