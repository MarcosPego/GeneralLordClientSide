using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using System.Xml.Serialization;



namespace GeneralLordWebApiClient.Model
{
    public class ArmyContainerSerializer
    {
        public static ArmyContainer Deserialize()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ArmyContainer));
                var filePath = Path.Combine(SaveFolderPath(), "armyConfig.xml");
                using (TextReader reader = new StreamReader(filePath))
                {
                    var result = (ArmyContainer)serializer.Deserialize(reader);
                    return result;
                }

            }
            catch
            {
                var result = new ArmyContainer();
                Serialize(result);
                return result;
            }
        }

        public static void Serialize(ArmyContainer armyContainer )
        {
            try
            {
                
                EnsureSaveDirectory();
                var filePath = Path.Combine(SaveFolderPath(), "armyConfig.xml");
                using (TextWriter writer = new StreamWriter(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ArmyContainer));
                    serializer.Serialize(writer, armyContainer);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        public static ArmyContainer JsonDeserialize()
        {
            var filePath = Path.Combine(SaveFolderPath(), "armyConfig.json");

            string jsonString = File.ReadAllText(filePath);
            using (JsonReader reader = new JsonTextReader(new StringReader(jsonString)))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (ArmyContainer)serializer.Deserialize<ArmyContainer>(reader);
            }

                //JsonSerializer serializer = new JsonSerializer();
                //serializer.Serialize(writer, armyContainer);
        }

        public static ArmyContainer JsonDeserializeFromString(string jsonString)
        {
            //var filePath = Path.Combine(SaveFolderPath(), "armyConfig.json");
            //string jsonString = File.ReadAllText(filePath);

            using (JsonReader reader = new JsonTextReader(new StringReader(jsonString)))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (ArmyContainer)serializer.Deserialize<ArmyContainer>(reader);
            }

            //JsonSerializer serializer = new JsonSerializer();
            //serializer.Serialize(writer, armyContainer);
        }

        public static string JsonString()
        {
            var filePath = Path.Combine(SaveFolderPath(), "armyConfig.json");

            return File.ReadAllText(filePath);

        }

        public static void JsonSerialize(ArmyContainer armyContainer)
        {
            var filePath = Path.Combine(SaveFolderPath(), "armyConfig.json");


            using (TextWriter writer = new StreamWriter(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, armyContainer);
            }
        }

        public static XDocument LoadArmyContainerXML(ArmyContainer armyContainer)
        {
            var filePath = Path.Combine(SaveFolderPath(), "armyConfig.xml");

            try
            {
                Serialize(armyContainer);
                return XDocument.Load(filePath);


            }
            catch (Exception e)
            {
                var result = new ArmyContainer();
                Serialize(result);
                return XDocument.Load(filePath);
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
