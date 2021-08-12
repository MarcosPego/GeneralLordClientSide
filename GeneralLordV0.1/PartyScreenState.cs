using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralLord
{
    public class PartyScreenState
    {
        public static PartyScreenStateEnum currentState = PartyScreenStateEnum.NormalScreen;
        public static int goldToChange = 0;



    }

    public enum PartyScreenStateEnum
    {
        RecruitmentScreen,
        GarrisonScreen,
        NormalScreen
    }
}
