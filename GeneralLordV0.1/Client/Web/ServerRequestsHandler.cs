using GeneralLord.FormationPlanHandler;
using GeneralLordWebApiClient;
using GeneralLordWebApiClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace GeneralLord.Client.Web
{
    public class ServerRequestsHandler
    {
        public static async Task SubmitPartyUtilsToServer(PartyUtils partyUtils, int counter = 0)
        {
            try
            {
                await WebRequests.PostAsync(UrlHandler.GetUrlFromString(UrlHandler.SubmitPartyUtils), partyUtils);
            } catch(Exception e)
            {
                
                if(counter < 3)
                {

                    InformationManager.DisplayMessage(new InformationMessage("Attempting to connect to server!"));
                    await SubmitPartyUtilsToServer(partyUtils, counter + 1);
                }
                else
                {
                    InformationManager.DisplayMessage(new InformationMessage("Unexpected error trying to submit wounded or garrisoned soldiers! Please try again later or contact the support in Discord"));
                }
            }

        }

        public static async Task SubmitPlayerProfile(Profile profile, int counter = 0)
        {
            try
            {
                var result = await WebRequests.PostAsync<Profile>(UrlHandler.GetUrlFromString(UrlHandler.SaveProfile), profile);
                EnemyFormationHandler.UseDefensiveSettings = result.ServerResponse.UseDefensiveOrder;
                Serializer.JsonSerialize(result.ServerResponse, "playerprofile.json");
            }
            catch (Exception e)
            {

                if (counter < 3)
                {

                    InformationManager.DisplayMessage(new InformationMessage("Attempting to connect to server!"));
                    await SubmitPlayerProfile(profile, counter + 1);
                }
                else
                {
                    InformationManager.DisplayMessage(new InformationMessage("Unexpected error trying to save profile in server! Please try again later or contact the support in Discord"));
                }
            }


        }

        public static async Task<IEnumerable<PartyUtils>> ReceivePlayerPartyUtils(int playerId, int counter = 0)
        {
            try
            {
                var result = await WebRequests.PostAsync<IEnumerable<PartyUtils>>(UrlHandler.GetUrlFromString(UrlHandler.GetPartyUtils), playerId);
                return result.ServerResponse;
            }
            catch (Exception e)
            {

                if (counter < 3)
                {

                    InformationManager.DisplayMessage(new InformationMessage("Attempting to connect to server!"));
                    return await ReceivePlayerPartyUtils(playerId, counter + 1);
                }
                else
                {
                    InformationManager.DisplayMessage(new InformationMessage("Unexpected error trying to retrieve wounded or garrisoned soldiers! Please try again later or contact the support in Discord"));
                    return null;
                  
                }
            }

        }

        public static async Task SavePostBattle(MatchHistory matchHistory, int counter = 0)
        {
            try
            {
                await WebRequests.PostAsync(UrlHandler.GetUrlFromString(UrlHandler.PostBattleProcess), matchHistory);
            }
            catch (Exception e)
            {

                if (counter < 3)
                {

                    InformationManager.DisplayMessage(new InformationMessage("Attempting to connect to server!"));
                    await SavePostBattle(matchHistory, counter + 1);
                }
                else
                {
                    InformationManager.DisplayMessage(new InformationMessage("Unexpected error trying to save match history! Please try again later or contact the support in Discord"));
                }
            }

        }

        public static async Task<IEnumerable<MatchHistory>> GetMatchHistory(int profileId, int counter = 0)
        {
            try
            {
                var result = await WebRequests.PostAsync<IEnumerable<MatchHistory>>(UrlHandler.GetUrlFromString(UrlHandler.GetMatchHistory), profileId);
                return result.ServerResponse;
            }
            catch (Exception e)
            {

                if (counter < 3)
                {

                    InformationManager.DisplayMessage(new InformationMessage("Attempting to connect to server!"));
                    return await GetMatchHistory(profileId, counter + 1);
                }
                else
                {
                    InformationManager.DisplayMessage(new InformationMessage("Unexpected error trying to save match history! Please try again later or contact the support in Discord"));
                    return null;
                }
            }
        }

        public static async Task<IEnumerable<Profile>> GetMatchMakingProfiles(Profile profile, bool isRankingScreen, int counter = 0)
        {
            try
            {
                if (isRankingScreen)
                {
                    var profileResult = await WebRequests.PostAsync<IEnumerable<Profile>>(UrlHandler.GetUrlFromString(UrlHandler.GetRankingProfiles));
                    return profileResult.ServerResponse;
                }
                else
                {
                    var profileResult = await WebRequests.PostAsync<IEnumerable<Profile>>(UrlHandler.GetUrlFromString(UrlHandler.MultipleFromProfile), profile);
                    return profileResult.ServerResponse;
                }
            }
            catch (Exception e)
            {

                if (counter < 3)
                {

                    InformationManager.DisplayMessage(new InformationMessage("Attempting to connect to server!"));
                    return await GetMatchMakingProfiles(profile, isRankingScreen, counter + 1);
                }
                else
                {
                    InformationManager.DisplayMessage(new InformationMessage("Unexpected error trying to open opponent selection! Please try again later or contact the support in Discord"));
                    return null;
                }
            }
        }
        public static async Task<bool> GetIsCurrentVersion(string Version, int counter = 0)
        {
            try
            {

                var boolResult = await WebRequests.PostAsync<bool>(UrlHandler.GetUrlFromString(UrlHandler.IsCurrentVersion), Version);
                return boolResult.ServerResponse;

            }
            catch (Exception e)
            {

                if (counter < 3)
                {

                    InformationManager.DisplayMessage(new InformationMessage("Attempting to connect to server!"));
                    return await GetIsCurrentVersion(Version, counter + 1);
                }
                else
                {
                    InformationManager.DisplayMessage(new InformationMessage("Unexpected error trying to open opponent selection! Please try again later or contact the support in Discord"));
                    return false;
                }
            }
        }

    }
}
