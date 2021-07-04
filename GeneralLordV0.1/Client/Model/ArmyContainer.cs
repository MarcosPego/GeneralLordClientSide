using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace GeneralLordWebApiClient.Model
{
    [Serializable]
    public class ArmyContainer
    {
        public int ArmyStrenght;
        public List<TroopContainer> TroopContainers { get; set; }

    //public Hero Character { get; set; }
    }
}
