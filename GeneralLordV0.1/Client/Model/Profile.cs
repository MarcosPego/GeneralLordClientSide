using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaleWorlds.CampaignSystem;

namespace GeneralLordWebApiClient.Model
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Elo { get; set; }

        /*//change
        public TroopRoster TroopRoster { get; set; }
        public Hero Character { get; set; }*/

        public string ArmyContainer { get; set; }
    }
}
