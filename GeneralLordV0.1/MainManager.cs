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

namespace GeneralLord
{
    internal class MainManager : CampaignBehaviorBase
    {
        public MainManager()
        {
            this._initializeState = true;

            //JsonBattleConfig.ExecuteSubmitAc();

            //this._partyManager = new PartyManager();
            //this._partyManagerLogic = new PartyManagerLogic();
            //this._partyManagerLogic.Initialize(this._partyManager.TestRosterLeft(), this._partyManager.TestRosterRight());
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
                        var t = Task.Run(async () =>
                        {
                            await WebRequests.PostAsync(UrlHandler.GetUrlFromString(UrlHandler.PostBattleProcess), matchHistory);
                            //Serializer.JsonSerialize(result.ServerResponse, "playerprofile.json");
                        });
                        t.Wait();

                        //JsonBattleConfig.UpdateArmyAfterBattle();
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
                if(this._initializeState) JsonBattleConfig.ReceivePartyUtils();
                this._partyManager = new PartyManager();
                this._partyManagerLogic = new PartyManagerLogic();
                this._partyManagerLogic.Initialize(this._partyManager.TestRosterLeft(), this._partyManager.TestRosterRight());

                _initializeState = false;
                _mainManagerScreen = new MainManagerScreen(_partyManagerLogic);


                ScreenManager.PushScreen(_mainManagerScreen);

            }
            if(ScreenManager.TopScreen is MainManagerScreen)  { PartyUtilsHandler.TickForRecovery(_mainManagerScreen); }

            if (Mission.Current == null && Input.IsKeyDown(InputKey.LeftControl))
            {
                if (false && Input.IsKeyReleased(InputKey.R))
                {
                    JObject json = JObject.Parse(Serializer.ReadStringFromFile("enemyProfile.json"));
                    //InformationManager.DisplayMessage(new InformationMessage(json.ToString()));
                    ArmyContainer ac = Serializer.JsonDeserializeFromStringAc((string)json["ArmyContainer"]) as ArmyContainer;

                    //Serializer.JsonDeserialize("enemyProfile.json");
                    //string jsonString = profile.ArmyContainer;
                    //Clan clan = Clan.All.First();


                    //Hero bestAvailableCommander = clan.Heroes.First();
                    //Hero bestAvailableCommander = CharacterObject.Find("enemy_general_lord").HeroObject;

                    //Hero bestAvailableCommander = Hero.CreateHero(CharacterObject.Find("lordTest").StringId);
                    //bestAvailableCommander.SetCharacterObject(CharacterObject.Find("lordTest"));

                    Settlement closestHideout = SettlementHelper.FindNearestSettlement((Settlement x) => x.IsHideout() && x.IsActive);
                    Clan clan = Clan.BanditFactions.FirstOrDefault((Clan t) => t.Culture == closestHideout.Culture);
                    //this._questBanditMobileParty = 


                    //MobileParty mobileParty = MobilePartyHelper.SpawnLordParty(bestAvailableCommander, new Vec2(Hero.MainHero.GetPosition().x, Hero.MainHero.GetPosition().z), 1f);
                    MobileParty mobileParty = BanditPartyComponent.CreateBanditParty("EnemyClan", clan, closestHideout.Hideout, false);
                    mobileParty.InitializeMobileParty(
                                JsonBattleConfig.EnemyParty(ac),
                                JsonBattleConfig.EnemyParty(ac),
                                mobileParty.Position2D,
                                0);
                    PlayerEncounter.Start();

                    //InformationManager.DisplayMessage(new InformationMessage(PartyBase.MainParty.IsSettlement.ToString()));
                    PlayerEncounter.Current.SetupFields(PartyBase.MainParty, mobileParty.Party);
                    PlayerEncounter.StartBattle();
                    CampaignMission.OpenBattleMission(PlayerEncounter.GetBattleSceneForMapPosition(MobileParty.MainParty.Position2D));
                }
                if (Input.IsKeyReleased(InputKey.E))
                {
                    //RecruitmentManager.OpenNPCRecruitmentRoster();
                    ScreenManager.PushScreen(new CunningLordsPlanDefinitionScreen());
                }
                if (Input.IsKeyReleased(InputKey.W))
                {
                    //RecruitmentManager.OpenNPCRecruitmentRoster();
                    ScreenManager.PushScreen(new CunningLordsPlanDefinitionScreen(false));
                }
                if (Input.IsKeyReleased(InputKey.T))
                {
                    PartyBase.MainParty.LeaderHero.HitPoints = PartyBase.MainParty.LeaderHero.CharacterObject.MaxHitPoints();
                }
                if (Input.IsKeyReleased(InputKey.G))
                {
                    foreach  (TroopRosterElement element in  PartyBase.MainParty.MemberRoster.GetTroopRoster())
                    {
                        int index = PartyBase.MainParty.MemberRoster.FindIndexOfTroop(element.Character);

                        int wounded = PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(index);


                        PartyBase.MainParty.MemberRoster.WoundTroop(element.Character, -wounded);
                      
                    }

                }
                if (Input.IsKeyReleased(InputKey.K))
                {
                    PartyBase.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(3);
                }
                /*
                if (Input.IsKeyReleased(InputKey.T))
                {
                    if (ScreenManager.TopScreen is MapScreen)
                    {
                        this._partyManager = new PartyManager();
                        this._partyManagerLogic = new PartyManagerLogic();
                        this._partyManagerLogic.Initialize(this._partyManager.TestRosterLeft(), this._partyManager.TestRosterRight());

                        _initializeState = false;
                        ScreenManager.PushScreen(new MainManagerScreen(_partyManagerLogic));
                    }
                }
                if (Input.IsKeyReleased(InputKey.U))
                {
                    DateTime localDateCurrent = DateTime.Now;
                    //String culture = "en-GB";
                    InformationManager.DisplayMessage(new InformationMessage(localDateCurrent.ToString("G", CultureInfo.CreateSpecificCulture("en-GB"))));
                    InformationManager.DisplayMessage(new InformationMessage(localDateCurrent.ToString() + "   " + localDateCurrent.Kind));


                    //InformationManager.DisplayMessage(new InformationMessage(DateTime.Parse(localDateCurrent.ToString("G", CultureInfo.CreateSpecificCulture("en-GB")));
                    /*CharacterHandler.saveLocationFile = "enemygeneral.xml";
                    CharacterHandler.saveLocationPath = CharacterHandler.SaveLocationEnum.ModuleData;
                    CharacterHandler.LoadXML();

                }*/
            }






        }

        private void GauntletMenuRecruitVolunteers()
        {
            throw new NotImplementedException();
        }

        public override void RegisterEvents()
        {
        }
        public override void SyncData(IDataStore dataStore)
        {
        }

        private Settlement randomSettlement;

        private bool _initializeState = true;
        private PartyManagerLogic _partyManagerLogic;
        private PartyManager _partyManager = null;
        private object localDate;

        private MainManagerScreen _mainManagerScreen;
    }
}
