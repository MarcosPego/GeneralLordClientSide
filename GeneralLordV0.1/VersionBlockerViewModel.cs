using GeneralLordWebApiClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace GeneralLord
{
    public class VersionBlockerViewModel : ViewModel
    {

        public VersionBlockerViewModel()
        {
            RefreshValues();
        }


        public override void RefreshValues()
        {
            base.RefreshValues();
        }

		public void ExectuteLeaveGme()
		{


			PartyUtilsHandler.GarrisonedTroops = new TroopRoster(PartyBase.MainParty);
			PartyUtilsHandler.WoundedTroops = new TroopRoster(PartyBase.MainParty);
			PartyUtilsHandler.WoundedTroopArmy = new WoundedTroopArmy();

			OpponentPartyHandler.CurrentOpponentParty = null;

			OpponentPartyHandler.PreBattleTroopRoster = null;

			//GameMetrics.currenLastPlaythroughStart = DateTime.Now;
			GameMetrics.currentLastPlaythroughEnd = DateTime.Now;
			GameMetrics.timePlayed = GameMetrics.currentLastPlaythroughEnd - GameMetrics.currentLastPlaythroughStart;

			InformationManager.DisplayMessage(new InformationMessage("Exiting to Main Menu!"));
			GameMetrics.savedAndExited++;

			MBGameManager.EndGame();
			//Campaign.Current.SaveHandler.QuickSaveCurrentGame();
			//JsonBattleConfig.ExecuteSubmitProfileWithAc();
			//this._exitOnSaveOver = true;
		}

	}
}
