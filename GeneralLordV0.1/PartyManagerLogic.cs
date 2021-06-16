using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace GeneralLord
{
    public class PartyManagerLogic
    {

        public PartyManagerLogic()
        {
            //this._config = new BattleGeneralConfig();
            this.RightSideRoster = new TroopRoster[1];
            this.LeftSideRoster = new TroopRoster[4];
        }

        public void Initialize(TroopRoster[] leftTroopRoster, TroopRoster[] rightTroopRoster)
        {
            this.RightSideRoster = rightTroopRoster;
            this.LeftSideRoster = leftTroopRoster;
        }



        //public BattleGeneralConfig _config;


        public TroopRoster[] RightSideRoster { get; set; }

        public TroopRoster[] LeftSideRoster { get; set; }
    }
}
