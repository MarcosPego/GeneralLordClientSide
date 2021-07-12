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

        /*//change
        public TroopRoster TroopRoster { get; set; }
        public Hero Character { get; set; }*/

        public string ArmyContainer { get; set; }
    }
}
