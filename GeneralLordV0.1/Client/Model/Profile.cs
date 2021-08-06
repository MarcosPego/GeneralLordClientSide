using System.ComponentModel.DataAnnotations;

namespace GeneralLordWebApiClient.Model
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Elo { get; set; }

        public int UniqueUser { get; set; }

        public float ArmyStrength { get; set; }
        public int TotalTroopCount { get; set; }

        /*//change
        public TroopRoster TroopRoster { get; set; }
        public Hero Character { get; set; }*/

        public string ArmyContainer { get; set; }


        public int UseDefensiveOrder { get; set; }

        public int SelectedFormation { get; set; }

        public string DefensiveFormation { get; set; }
        public string DefensiveOrders { get; set; }

    }
}
