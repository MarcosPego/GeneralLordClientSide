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
            //InformationManager.DisplayMessage(new InformationMessage("Testasf"));

            /*InformationManager.DisplayMessage(new InformationMessage(Game.Current.GameManager.Game.GameStateManager.ActiveState.GetType().ToString()));
            */
            
            if(ScreenManager.TopScreen is MapScreen){
                this._partyManager = new PartyManager();
                this._partyManagerLogic = new PartyManagerLogic();
                this._partyManagerLogic.Initialize(this._partyManager.TestRosterLeft(), this._partyManager.TestRosterRight());


                ScreenManager.PushScreen(new MainManagerScreen(_partyManagerLogic));
            }

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

            /*if (Mission.Current == null && Input.IsKeyDown(InputKey.LeftControl))
            {
                if (Input.IsKeyReleased(InputKey.R))
                {
                    //InformationManager.DisplayMessage(new InformationMessage("Test"));

                    //ScreenManager.PushScreen(new MainManagerScreen(this._partyManagerLogic));
                    this._initializeState = false;
                    this._partyManager = new PartyManager();
                    this._partyManagerLogic = new PartyManagerLogic();
                    this._partyManagerLogic.Initialize(this._partyManager.TestRosterLeft(), this._partyManager.TestRosterRight());
                    ScreenManager.PushScreen(new MainManagerScreen(_partyManagerLogic));
                }
            }*/
        }

        public override void RegisterEvents()
        {
        }
        public override void SyncData(IDataStore dataStore)
        {
        }

        private bool _initializeState = true;
        private PartyManagerLogic _partyManagerLogic;
        private PartyManager _partyManager = null;
    }
}
