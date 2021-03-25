using BushFire.Engine.Controllers;
using BushFire.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.Files
{
    [Serializable]
    public class SettingsXML
    {
        //Audio
        public List<VolumeType> volumeEnumList;
        public List<float> volumeLevelList;

        //Controls
        public List<KeyMap> keyMapEnumList;
        public List<Keys> keyList;

        //Display
        public int resolutionWidth;
        public int resolutionHeight;
        public int targetFrameRate;
        public bool fullScreen;
        public float uiScale;
        public float messageSpeed;
        public float minNightBrightness;

        public SettingsXML()
        {

        }

        public void CreateDefault()
        {
            DefaultAudioVolume();
            DefaultDisplay();
            DefaultKeyMap();
        }

        private void DefaultAudioVolume()
        {
            volumeEnumList = new List<VolumeType>();
            volumeLevelList = new List<float>();

            foreach (VolumeType volumeType in (VolumeType[])Enum.GetValues(typeof(VolumeType)))
            {
                volumeEnumList.Add(volumeType);
                volumeLevelList.Add(0.5f);
            }
        }

        private void DefaultDisplay()
        {
            resolutionWidth = 0;
            resolutionHeight = 0;
            fullScreen = false;
            targetFrameRate = 0;
            uiScale = 0.9f;
            messageSpeed = 1f;
            minNightBrightness = 1.5f;
        }

        private void DefaultKeyMap()
        {
            keyMapEnumList = new List<KeyMap>();
            keyList = new List<Keys>();

            keyMapEnumList.Add(KeyMap.ShowHelp); keyList.Add(Keys.F1);
            keyMapEnumList.Add(KeyMap.MoveCameraUp); keyList.Add(Keys.W);
            keyMapEnumList.Add(KeyMap.MoveCameraRight); keyList.Add(Keys.D);
            keyMapEnumList.Add(KeyMap.MoveCameraDown); keyList.Add(Keys.S);
            keyMapEnumList.Add(KeyMap.MoveCameraLeft); keyList.Add(Keys.A);
            keyMapEnumList.Add(KeyMap.FastMovingCamera); keyList.Add(Keys.LeftShift);
            keyMapEnumList.Add(KeyMap.IncreaseGameSpeed); keyList.Add(Keys.Add);
            keyMapEnumList.Add(KeyMap.DecreaseGameSpeed); keyList.Add(Keys.Subtract);
            keyMapEnumList.Add(KeyMap.ToggleDebug); keyList.Add(Keys.F5);
            keyMapEnumList.Add(KeyMap.ResetFramesCounter); keyList.Add(Keys.F6);
            keyMapEnumList.Add(KeyMap.OpenInfoWindow); keyList.Add(Keys.LeftControl);
            keyMapEnumList.Add(KeyMap.AddUnitToSelection); keyList.Add(Keys.LeftShift);
            keyMapEnumList.Add(KeyMap.RemoveUnitFromSelection); keyList.Add(Keys.LeftAlt);
            keyMapEnumList.Add(KeyMap.CreateControlGroup); keyList.Add(Keys.LeftControl);
            keyMapEnumList.Add(KeyMap.SelectControlGroupOne); keyList.Add(Keys.D1);
            keyMapEnumList.Add(KeyMap.SelectControlGroupTwo); keyList.Add(Keys.D2);
            keyMapEnumList.Add(KeyMap.SelectControlGroupThree); keyList.Add(Keys.D3);
            keyMapEnumList.Add(KeyMap.SelectControlGroupFour); keyList.Add(Keys.D4);
            keyMapEnumList.Add(KeyMap.SelectControlGroupFive); keyList.Add(Keys.D5);

        }



        public void PrepareForSaving()
        {
            PrepareAudioVolume();
            PrepareDisplay();
            PrepareKeyMaps();
        }

        private void PrepareAudioVolume()
        {
            volumeEnumList = new List<VolumeType>();
            volumeLevelList = new List<float>();

            foreach (VolumeType volumeType in (VolumeType[])Enum.GetValues(typeof(VolumeType)))
            {
                volumeEnumList.Add(volumeType);
                volumeLevelList.Add(AudioManager.GetVolume(volumeType));
            }
        }

        private void PrepareDisplay()
        {
            resolutionWidth = DisplayController.resolutionList[DisplayController.resolutionId].Width;
            resolutionHeight = DisplayController.resolutionList[DisplayController.resolutionId].Height;
            fullScreen = DisplayController.fullScreen;
            targetFrameRate = DisplayController.targetFrameRate;
            uiScale = DisplayController.uiScale;
            messageSpeed = DisplayController.messageSpeed;
            minNightBrightness = DisplayController.minNightBrightness;
        }

        private void PrepareKeyMaps()
        {
            keyMapEnumList = new List<KeyMap>();
            keyList = new List<Keys>();

            foreach (KeyValuePair<KeyMap, Keys> values in EngineController.keyMapList)
            {
                keyMapEnumList.Add(values.Key);
                keyList.Add(values.Value);
            }
        }
    }
}
