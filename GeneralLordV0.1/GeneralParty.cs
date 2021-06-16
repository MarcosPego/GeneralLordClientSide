using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace GeneralLord
{
    public class GeneralParty
    {
        public MobileParty MobileParty;
        public string Name;
        internal readonly Banner Banner;
        internal readonly string BannerKey;
        internal Hero Hero;

        public GeneralParty(MobileParty mobileParty)
        {
            MobileParty = mobileParty;
            //Banner = Banners.GetRandomElement();
            BannerKey = Banner.Serialize();
            Hero = mobileParty.LeaderHero;
            //LogMilitiaFormed(MobileParty);
        }

    }
}
