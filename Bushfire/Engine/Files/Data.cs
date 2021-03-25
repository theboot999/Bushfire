using BushFire.Content.Game;
using BushFire.Editor.Tech;
using BushFire.Engine.Controllers;
using BushFire.Game;
using BushFire.Game.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.Files
{
    //TODO: rename this. cant think of a name right now
    //So the first thing we do
    //Is load these parameters
    //that are in here  
    static class Data
    {

        public static string gameFolder = System.IO.Directory.GetCurrentDirectory() + @"\";
        public static SettingsXML settingsXML;
        private static string settingsFileName = "Settings.xml";
        private static string buildingsBinaryFolder = @"World\";
        private static string buildingsBinaryFileName = @"Buildings.bin";
        

        public static void SetSettings()
        {
            if (!EngineController.rebuildSettingsFileOnLoad)
            {
                settingsXML = (SettingsXML)FileLoad.LoadXmlFile(typeof(SettingsXML), settingsFileName);
            }

            if (settingsXML == null) //We didnt load it
            {
                settingsXML = new SettingsXML();
                settingsXML.CreateDefault();
            }
        }

        public static void SaveSettings()
        {
            settingsXML.PrepareForSaving();
            FileSave.SaveXmlFile(settingsXML, settingsFileName);
        }

        public static BuildingsBinary LoadBuildingsBinary()
        {
            BuildingsBinary buildingsBinary = (BuildingsBinary)FileLoad.LoadBinaryFile(buildingsBinaryFolder, buildingsBinaryFileName);

            if (buildingsBinary == null)
            {
                buildingsBinary = new BuildingsBinary();
            }
            return buildingsBinary;
        }

        public static void SaveBuildingsBinary(BuildingsBinary buildingsBinary)
        {
            FileSave.SaveBinaryFile(buildingsBinary, buildingsBinaryFolder, buildingsBinaryFileName);
        }
     
    }
}
