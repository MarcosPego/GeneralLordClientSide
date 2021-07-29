using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace GeneralLord
{
    public class CharacterTierHandler
    {
        public static CultureObject CharacterMainCulture = PartyBase.MainParty.LeaderHero.Culture;
        public static int ClanTier = PartyBase.MainParty.LeaderHero.Clan.Tier;

        public static void InitializeCharacterTierHandler()
        {
            CharacterMainCulture = PartyBase.MainParty.LeaderHero.Culture;
            ClanTier = PartyBase.MainParty.LeaderHero.Clan.Tier;
        }

    }
}
