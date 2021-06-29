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

namespace GeneralLord
{
    internal class MainManager : CampaignBehaviorBase
    {
        public MainManager()
        {
            this._initializeState = true;

           
            //this._partyManager = new PartyManager();
            //this._partyManagerLogic = new PartyManagerLogic();
            //this._partyManagerLogic.Initialize(this._partyManager.TestRosterLeft(), this._partyManager.TestRosterRight());
        }

        public void TickCampaignBehavior()
        {
            JObject json = JObject.Parse(Serializer.ReadStringFromFile("playerprofile.json"));
            ArmyContainer ac = Serializer.JsonDeserializeFromStringAc((string)json["ArmyContainer"]);


            
            if (PlayerEncounter.Current != null)
            {
                if (PlayerEncounter.Current.EncounterState == PlayerEncounterState.Wait)
                {

                    JsonBattleConfig.UpdateArmyAfterBattle();
                    PlayerEncounter.Finish(false);

                }

            }

            /*if (ScreenManager.TopScreen is MapScreen )
            {
                this._partyManager = new PartyManager();
                this._partyManagerLogic = new PartyManagerLogic();
                this._partyManagerLogic.Initialize(this._partyManager.TestRosterLeft(), this._partyManager.TestRosterRight());

                _initializeState = false;
                ScreenManager.PushScreen(new MainManagerScreen(_partyManagerLogic));
            }*/

            /*if (Game.Current.GameManager.Game.GameStateManager.ActiveState != null)
            {

             
                if (Game.Current.GameManager.Game.GameStateManager.ActiveState.GetType() == typeof(MapState) && this._initializeState == true)
                {
                    this._initializeState = false;

                }

                    //InformationManager.DisplayMessage(new InformationMessage("Testasf"));
 
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
                    json = JObject.Parse(Serializer.ReadStringFromFile("enemyProfile.json"));
                    //InformationManager.DisplayMessage(new InformationMessage(json.ToString()));
                    ac = Serializer.JsonDeserializeFromStringAc((string)json["armyContainer"]) as ArmyContainer;

                    //Serializer.JsonDeserialize("enemyProfile.json");
                    //string jsonString = profile.ArmyContainer;
                    Clan clan = Clan.All.First();
                    Hero bestAvailableCommander = clan.Heroes.First();
                    MobileParty mobileParty = MobilePartyHelper.SpawnLordParty(bestAvailableCommander, new Vec2(Hero.MainHero.GetPosition().x, Hero.MainHero.GetPosition().z), 1f);
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
                    //randomSettlement = SettlementHelper.FindRandomSettlement((Settlement x) => x.IsTown);
                    randomSettlement = SettlementHelper.FindNearestSettlementToPoint(MobileParty.MainParty.Position2D, (Settlement x) => x.IsTown);

                    InformationManager.DisplayMessage(new InformationMessage(randomSettlement.Name.ToString()));
                    //MobileParty.MainParty.
                    //EnterSettlementAction.ApplyForParty(MobileParty.MainParty, randomSettlement);
                    //GauntletMenuRecruitVolunteers gmr = new GauntletMenuRecruitVolunteers();
                    //MobileParty.MainParty.Position2D = randomSettlement.GetPosition2D;
                    //PlayerEncounter.EnterSettlement();
                    InformationManager.DisplayMessage(new InformationMessage(Campaign.Current.CurrentMenuContext.StringId));
                    Campaign.Current.HandleSettlementEncounter(MobileParty.MainParty, randomSettlement);
                    //Campaign.Current.CurrentMenuContext.OpenRecruitVolunteers();

                    //if (this._menuRecruitVolunteers == null)
                    //{
                    //    this._menuRecruitVolunteers = this.AddMenuView<MenuRecruitVolunteers>(Array.Empty<object>());
                    //}
                    InformationManager.DisplayMessage(new InformationMessage(Campaign.Current.CurrentMenuContext.StringId));
                   

                    Campaign.Current.CurrentMenuContext.OpenRecruitVolunteers();


                    //this.AddMenuView<MenuRecruitVolunteers>(Array.Empty<object>());

                    //LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);
                    //GauntletMenuRecruitVolunteers gmc = new GauntletMenuRecruitVolunteers();

                }

                if (Input.IsKeyReleased(InputKey.T))
                {
                    PlayerEncounter.LeaveSettlement();
                    PlayerEncounter.Finish(true);
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
