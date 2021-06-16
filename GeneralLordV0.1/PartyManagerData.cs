using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace GeneralLord
{
    class PartyManagerData
    {
        public void InitializeCopyFrom(MobileParty rightParty)
        {
            this.RightSideRoster = TroopRoster.CreateDummyTroopRoster();
            this.FormationRosterA = TroopRoster.CreateDummyTroopRoster();
            this.FormationRosterB = TroopRoster.CreateDummyTroopRoster();
            this.FormationRosterC = TroopRoster.CreateDummyTroopRoster();
            this.FormationRosterD = TroopRoster.CreateDummyTroopRoster();
            this.FormationRosterE = TroopRoster.CreateDummyTroopRoster();
        }

        public TroopRoster RightSideRoster;

        public TroopRoster FormationRosterA;
        public TroopRoster FormationRosterB;
        public TroopRoster FormationRosterC;
        public TroopRoster FormationRosterD;
        public TroopRoster FormationRosterE;
    }
}
