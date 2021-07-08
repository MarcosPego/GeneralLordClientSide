using System;
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
                }

                else if (PlayerEncounter.Current.EncounterState == PlayerEncounterState.Wait)
                {

                    JsonBattleConfig.UpdateArmyAfterBattle();
                    PlayerEncounter.Finish(false);

                }

            }

            if (ScreenManager.TopScreen is MapScreen)
            {
                this._partyManager = new PartyManager();
                this._partyManagerLogic = new PartyManagerLogic();
                this._partyManagerLogic.Initialize(this._partyManager.TestRosterLeft(), this._partyManager.TestRosterRight());

                _initializeState = false;
                ScreenManager.PushScreen(new MainManagerScreen(_partyManagerLogic));
            }


            /*if (Game.Current.GameManager.Game.GameStateManager.ActiveState != null)
            {

             
                if (Game.Current.GameManager.Game.GameStateManager.ActiveState.GetType() == typeof(MapState) && this._initializeState == true)
                {
                    this._initializeState = false;

                }

                    //InformationManager.DisplayMessage(new InformationMessage(""));
 
            }
            */
            /*if (Game.Current.GameManager.Game.GameStateManager.ActiveState != null)
            {
                InformationManager.DisplayMessage(new InformationMessage(Game.Current.GameManager.Game.GameStateManager.ActiveState.GetType().ToString()));
            }*/
            //if (CampaignEvents.OnCharacterCreationIsOverEvent)
            //{
            //    CampaignEvents.OnCharacterCreationIsOverEvent
            //}

            if (Mission.Current == null && Input.IsKeyDown(InputKey.LeftControl))
            {
                if (Input.IsKeyReleased(InputKey.R))
                {
                    JObject json = JObject.Parse(Serializer.ReadStringFromFile("enemyProfile.json"));
                    //InformationManager.DisplayMessage(new InformationMessage(json.ToString()));
                    ArmyContainer ac = Serializer.JsonDeserializeFromStringAc((string)json["armyContainer"]) as ArmyContainer;

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
                    CharacterHandler.SaveCharacter();

                }
                if (Input.IsKeyReleased(InputKey.Y))
                {
                    PartyScreenState.currentState = PartyScreenStateEnum.RecruitmentScreen;
                    RecruitmentManager.OpenRecruitmentRoster();
                }
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
                    
                    /*CharacterHandler.saveLocationFile = "enemygeneral.xml";
                    CharacterHandler.saveLocationPath = CharacterHandler.SaveLocationEnum.ModuleData;
                    CharacterHandler.LoadXML();*/

                    JsonBattleConfig.ExecuteSubmitAc();
                }
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
    }
}
