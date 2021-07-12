﻿using GeneralLord;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace GeneralLordWebApiClient.Model
{
    public class ProfileHandler
    {

        public static Profile GetVerifyProfile()
        {
            Serializer.EnsureSaveDirectory();
            var filePath = Path.Combine(Serializer.SaveFolderPath(), "playerprofile.json");
            try
            {
               
                Profile profile = (Profile) Serializer.JsonDeserializeProfile(filePath);
                //profile.Elo = 2000;
                //InformationManager.DisplayMessage(new InformationMessage(profile.Id.ToString()));
                return profile;

            }
            catch
            {
                Profile profile = new Profile { Name = PartyBase.MainParty.LeaderHero.Name.ToString(), Elo = 1500, ArmyContainer = Serializer.JsonString("armyConfig.json") };
                Serializer.JsonSerialize(profile, filePath);
                return profile;
            }
        }


        public static Profile UpdateProfileAc()
        {
            Serializer.EnsureSaveDirectory();
            var filePath = Path.Combine(Serializer.SaveFolderPath(), "playerprofile.json");
            try
            {

                Profile profile = (Profile)Serializer.JsonDeserializeProfile(filePath);
                profile.ArmyContainer = Serializer.JsonString("armyConfig.json");
                if(JsonBattleConfig.UniqueId != 0)
                {
                    profile.UniqueUser = JsonBattleConfig.UniqueId;
                    InformationManager.DisplayMessage(new InformationMessage(profile.UniqueUser.ToString()));

                }

                //profile.Elo = 2000;
                //InformationManager.DisplayMessage(new InformationMessage(profile.Id.ToString()));
                return profile;

            }
            catch
            {
                Profile profile;
                if (JsonBattleConfig.UniqueId == 0 )
                {
                    profile = new Profile { Name = PartyBase.MainParty.LeaderHero.Name.ToString(), Elo = 1500, ArmyContainer = Serializer.JsonString("armyConfig.json") };
                } else
                {
                    profile = new Profile { Name = PartyBase.MainParty.LeaderHero.Name.ToString(), Elo = 1500, ArmyContainer = Serializer.JsonString("armyConfig.json"), UniqueUser = JsonBattleConfig.UniqueId };
                }

                Serializer.JsonSerialize(profile, filePath);
                return profile;
            }
        }



    }
}
