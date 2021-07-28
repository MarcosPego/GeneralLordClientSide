using GeneralLord.FormationBattleTest;
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
        private static readonly TextObject _renownStr = new TextObject("{=eiWQoW9j}You gained {A0} renown.", null);

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
                if(BattleTestHandler.BattleTestEnabled == BattleTestHandler.BattleTestEnabledState.None)
                {
                    ExplainedNumber renownExplained = new ExplainedNumber(0f, true, null);
                    ExplainedNumber influencExplained = new ExplainedNumber(0f, true, null);
                    ExplainedNumber moraleExplained = new ExplainedNumber(0f, true, null);
                    float num;
                    float num2;
                    float num3;
                    float num4;
                    float playerEarnedLootPercentage;
                    PlayerEncounter.GetBattleRewards(out num, out num2, out num3, out num4, out playerEarnedLootPercentage, ref renownExplained, ref influencExplained, ref moraleExplained);

                    OpponentPartyHandler.GoldToAdd = OpponentPartyHandler.VerifyGoldPerKilled();
                    ExplainedNumber goldExplained = new ExplainedNumber(0f, true, null);
                    __instance.BattleResults.Add(new BattleResultVM(_goldStr.Format(OpponentPartyHandler.GoldToAdd),
                        () => SandBoxUIHelper.GetExplainedNumberTooltip(ref goldExplained), null));

                    if (num > 0.1f)
                    {
                        __instance.BattleResults.Add(new BattleResultVM(_renownStr.Format(num), () => SandBoxUIHelper.GetExplainedNumberTooltip(ref renownExplained), null));
                    }
                }



            }
        }
    }
}
