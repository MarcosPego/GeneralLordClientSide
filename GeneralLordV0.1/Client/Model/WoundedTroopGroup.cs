using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralLordWebApiClient.Model
{
    public class WoundedTroopGroup
    {
        public WoundedTroopGroup()
        {
            woundedTroops = new List<WoundedTroop>();
            totalWoundedTroops = 0;
        }

        public int totalWoundedTroops;
        public List<WoundedTroop> woundedTroops;
        public DateTime timeUntilRecovery;
    }
}
