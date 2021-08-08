using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralLordWebApiClient.Model
{
    public class WoundedTroopArmy
    {
        public WoundedTroopArmy()
        {
            WoundedTroopsGroup = new List<WoundedTroopGroup>();

        }


        public List<WoundedTroopGroup> WoundedTroopsGroup = new List<WoundedTroopGroup>();
    }
}
