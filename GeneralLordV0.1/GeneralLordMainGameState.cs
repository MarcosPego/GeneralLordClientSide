using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace GeneralLord
{
    public class GeneralLordMainGameState : GameState
    {
        public GeneralLordMainGameState(PartyManagerLogic partyManagerLogic)
        {

            this._partyManagerLogic = partyManagerLogic;
        }

        public GeneralLordMainGameState()
        {
        }

        public PartyManagerLogic _partyManagerLogic;

    }
}
