using BushFire.Engine.Files;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.Controllers
{


    static class EngineController
    {
        public static bool debugMode = false;
        public static int debugTilesOnScreen;
        public static bool rebuildSettingsFileOnLoad = false;

        public static Dictionary<KeyMap, Keys> keyMapList;

        //Timings
        public static float gameUpdateTime;
        public static float timeMultiply = 1f;
        public static float drawUpdateTime;

        public static bool exitProgram;

        public static double updateFrameRate;
        public static double drawFrameRate;
        public static double totalFrameRate = 0;
        public static double frameCounter = 0;
        public static double averageFrameRate = 0;
        
        public static bool resetTimer;

        private static double inSecondUpdateFrames = 0;
        private static double inSecondDrawFrames = 0;
        private static double currentSecond = 0;
        private static double currentSecondCounter = 0;
        private static double totalUpdateFrames = 0;

        public static double mainThreadTime;
        public static double secondThreadTime;
        public static double drawTime;

        //debugging only
        public static double debugTotalDrawFrames = 1;
        public static double debugTotalUpdateFrames = 0;
        public static double debugSkippedFrames = 0;
        public static double debugFires = 0;
        public static double debugMiniFires = 0;

        private static Stopwatch stopwatchDebug;

        public static void Init()
        {
            keyMapList = new Dictionary<KeyMap, Keys>();

            foreach (KeyMap keyMap in (KeyMap[])Enum.GetValues(typeof(KeyMap)))
            {
                Keys key = Keys.Z;

                for (int i = 0; i < Data.settingsXML.keyMapEnumList.Count; i++)
                {
                    if (keyMap == Data.settingsXML.keyMapEnumList[i])       //if this enum was in the list
                    {
                        key = Data.settingsXML.keyList[i];    //set the Key to it
                    }
                }
                //if the enum was added after the save list.  default it to Z;
                keyMapList.Add(keyMap, key);
            }
        }

        public static void StartDebugTimer()
        {
            stopwatchDebug = new Stopwatch();
            stopwatchDebug.Start();
        }

        public static void StopDebugTimer(string extra)
        {
            stopwatchDebug.Stop();
            ScreenController.AddMessage(extra + stopwatchDebug.ElapsedMilliseconds.ToString() + "Milliseconds = " + stopwatchDebug.ElapsedTicks.ToString() + " Ticks", Color.White);
        }


        private static float lastGameUpdateTime;

        public static void UpdateTimer(GameTime gameTime)
        {

            drawUpdateTime = (float)(gameTime.ElapsedGameTime.TotalSeconds * 60);
            gameUpdateTime = drawUpdateTime * timeMultiply;

        //    if (gameUpdateTime > lastGameUpdateTime * 10)
          //  {
                //we have spiked from moving the window
         //       gameUpdateTime = lastGameUpdateTime;
         //   }


            lastGameUpdateTime = drawUpdateTime;

            debugSkippedFrames = debugTotalUpdateFrames - debugTotalDrawFrames;
            debugTotalUpdateFrames++;

            if (gameTime.ElapsedGameTime.TotalSeconds > 0)
            {
                inSecondUpdateFrames++;
                currentSecond += gameTime.ElapsedGameTime.TotalSeconds;

                if (currentSecond > currentSecondCounter)  // 1 second has passed
                {
                    updateFrameRate = inSecondUpdateFrames;
                    drawFrameRate = inSecondDrawFrames;

                    totalUpdateFrames += inSecondUpdateFrames;
                    inSecondUpdateFrames = 0;
                    inSecondDrawFrames = 0;

                    if (currentSecondCounter > 0)
                    {
                        EngineController.averageFrameRate = totalUpdateFrames / (currentSecondCounter);
                    }
                    currentSecondCounter++;
                }
            }

            if (resetTimer)
            {
                currentSecond = 0;
                currentSecondCounter = 1;
                totalUpdateFrames = 0;
                inSecondUpdateFrames = 0;
                resetTimer = false;
            }
        }

        public static void AddDrawFrame()
        {
            debugTotalDrawFrames++;
            inSecondDrawFrames++;
        }
    }

    public enum KeyMap
    {
        ShowHelp,
        MoveCameraUp,
        MoveCameraRight,
        MoveCameraDown,
        MoveCameraLeft,
        FastMovingCamera,
        IncreaseGameSpeed,
        DecreaseGameSpeed,
        ToggleDebug,
        ResetFramesCounter,
        OpenInfoWindow,
        AddWayPointHold,
        AddUnitToSelection,
        RemoveUnitFromSelection,
        CreateControlGroup,
        SelectControlGroupOne,
        SelectControlGroupTwo,
        SelectControlGroupThree,
        SelectControlGroupFour,
        SelectControlGroupFive

    }

}
