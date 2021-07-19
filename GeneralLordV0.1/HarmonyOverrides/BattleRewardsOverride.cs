using HarmonyLib;
using SandBox.ViewModelCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard;

namespace GeneralLord.HarmonyOverrides
{
    class BattleRewardsOverride
    {

        private static readonly TextObject _goldStr = new TextObject("{=WAKz9xX8}You gained {A0} gold.", null);

        [HarmonyPatch(typeof(SPScoreboardVM))]
        [HarmonyPatch("GetBattleRewards")]
        class ResetOverride
        {
            static void Prefix(SPScoreboardVM __instance, ref bool playerVictory)
            {
                playerVictory = false;


            }

            static void Postfix(SPScoreboardVM __instance)
            {
                OpponentPartyHandler.GoldToAdd = OpponentPartyHandler.VerifyGoldPerKilled();
                ExplainedNumber goldExplained = new ExplainedNumber(0f, true, null);
                __instance.BattleResults.Add(new BattleResultVM(_goldStr.Format(OpponentPartyHandler.GoldToAdd), 
                    () => SandBoxUIHelper.GetExplainedNumberTooltip(ref goldExplained), null));


            }
        }
    }
}
