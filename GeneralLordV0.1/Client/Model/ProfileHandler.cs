using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace GeneralLordWebApiClient.Model
{
    public class ProfileHandler
    {

        public static Profile GetVerifyProfile()
        {
            EnsureSaveDirectory();
            var filePath = Path.Combine(SaveFolderPath(), "playerprofile.json");
            try
            {
               
                Profile profile = (Profile) Serializer.JsonDeserializeProfile(filePath);
                //profile.Elo = 2000;
                //InformationManager.DisplayMessage(new InformationMessage(profile.Id.ToString()));
                return profile;

            }
            catch
            {
                Profile profile = new Profile {Name = "GeneralLordTest2", Elo = 1500, ArmyContainer = Serializer.JsonString("armyConfig.json") };
                Serializer.JsonSerialize(profile, filePath);
                return profile;
            }
        }


        public static Profile UpdateProfileAc()
        {
            EnsureSaveDirectory();
            var filePath = Path.Combine(SaveFolderPath(), "playerprofile.json");
            try
            {

                Profile profile = (Profile)Serializer.JsonDeserializeProfile(filePath);
                profile.ArmyContainer = Serializer.JsonString("armyConfig.json");
                //profile.Elo = 2000;
                //InformationManager.DisplayMessage(new InformationMessage(profile.Id.ToString()));
                return profile;

            }
            catch
            {
                Profile profile = new Profile { Name = "GeneralLordTest2", Elo = 1500, ArmyContainer = Serializer.JsonString("armyConfig.json") };
                Serializer.JsonSerialize(profile, filePath);
                return profile;
            }
        }


        private static void EnsureSaveDirectory()
        {

            Directory.CreateDirectory(SaveFolderPath());
        }
        private static string SaveFolderPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "Mount and Blade II Bannerlord", "Configs", "GeneralLord");

        }

    }
}
