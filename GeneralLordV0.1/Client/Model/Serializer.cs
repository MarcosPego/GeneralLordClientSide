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
    public class Serializer
    {
        public static ArmyContainer Deserialize()
        {
            try
            {
                EnsureSaveDirectory();
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


        public static void WriteJsonToFile(string jsondata, string endFile = "armyConfig.json")
        {
            var filePath = Path.Combine(SaveFolderPath(), endFile);
            File.WriteAllText(filePath, jsondata);

        }

        public static string ReadStringFromFile(string endFile = "armyConfig.json")
        {
            var filePath = Path.Combine(SaveFolderPath(), endFile);
            return File.ReadAllText(filePath);

        }

        public static Profile JsonDeserializeProfile(string endFile = "armyConfig.json")
        {
            var filePath = Path.Combine(SaveFolderPath(), endFile);

            string jsonString = File.ReadAllText(filePath);
            using (JsonReader reader = new JsonTextReader(new StringReader(jsonString)))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<Profile>(reader);
            }

                //JsonSerializer serializer = new JsonSerializer();
                //serializer.Serialize(writer, armyContainer);
        }

        public static ArmyContainer JsonDeserializeAc(string endFile = "armyConfig.json")
        {
            var filePath = Path.Combine(SaveFolderPath(), endFile);

            string jsonString = File.ReadAllText(filePath);
            using (JsonReader reader = new JsonTextReader(new StringReader(jsonString)))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<ArmyContainer>(reader);
            }

            //JsonSerializer serializer = new JsonSerializer();
            //serializer.Serialize(writer, armyContainer);
        }

        public static ArmyContainer JsonDeserializeFromStringAc( string jsonString)
        {
            //var filePath = Path.Combine(SaveFolderPath(), "armyConfig.json");
            //string jsonString = File.ReadAllText(filePath);

            using (JsonReader reader = new JsonTextReader(new StringReader(jsonString)))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<ArmyContainer>(reader);
            }

            //JsonSerializer serializer = new JsonSerializer();
            //serializer.Serialize(writer, armyContainer);
        }
        public static Profile JsonDeserializeFromStringProfile(string jsonString)
        {
            //var filePath = Path.Combine(SaveFolderPath(), "armyConfig.json");
            //string jsonString = File.ReadAllText(filePath);

            using (JsonReader reader = new JsonTextReader(new StringReader(jsonString)))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<Profile>(reader);
            }

            //JsonSerializer serializer = new JsonSerializer();
            //serializer.Serialize(writer, armyContainer);
        }

        public static string JsonString(string fileName)
        {
            var filePath = Path.Combine(SaveFolderPath(), fileName);

            return File.ReadAllText(filePath);

        }

        public static void JsonSerialize(object objectToSerialize, string endFile = "armyConfig.json")
        {
            var filePath = Path.Combine(SaveFolderPath(), endFile);


            using (TextWriter writer = new StreamWriter(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, objectToSerialize);
            }
        }

        public static void JsonSerializeString(string json, string endFile = "armyConfig.json")
        {
            var filePath = Path.Combine(SaveFolderPath(), endFile);


            using (TextWriter writer = new StreamWriter(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, json);
            }
        }

        public static void SaveString(string json, string endFile = "armyConfig.json")
        {
            var filePath = Path.Combine(SaveFolderPath(), endFile);


            using (TextWriter writer = new StreamWriter(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, json);
            }
        }

        public static XDocument LoadArmyContainerXML(ArmyContainer armyContainer)
        {
            var filePath = Path.Combine(SaveFolderPath(), "armyConfig.xml");

            try
            {
                EnsureSaveDirectory();
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

        public static void EnsureSaveDirectory()
        {
            Directory.CreateDirectory(SaveFolderPath());
        }
        public static string SaveFolderPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "Mount and Blade II Bannerlord", "Configs", "GeneralLord");

        }

    }
}
