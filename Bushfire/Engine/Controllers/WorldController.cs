using BushFire.Content.Game;
using BushFire.Editor.Tech;
using BushFire.Engine.Controllers;
using BushFire.Engine.Files;
using BushFire.Game.Map;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game
{
    static class WorldController
    {
        //Shadow brightness will decrease as the day gets into evening

        public static World world;
        public static WorldMini worldMini;
        public static WorldFire worldFire;

        public static float time;
        public static int day;
        public static string timeString = "";

        private static Dictionary<WorldParameters, float[]> worldParametersList;

        public static float globalBrightness;
        public static float globalBlue;
        public static float globalRed;
        public static float globalGreen;

        public static float shadowLength;
        public static float shadowDarkness;



        public static readonly float timeForLightsOff = 7.75f;
        public static readonly float timeForLightsOn = 16.5f;

        public static ShadowSide currentWorldShadowSide;
       
        public static readonly float[] stopLightTimes = { 10, 2.5f, 10, 2.5f};
        public static bool lightsOn;
        private static int currentMapObjectId;

        public static Point mouseTileHover;
        public static Vector2 mouseWorldHover;
        public static bool mouseInWorldFocus;

        //vehicle top speed.  this is updated every frame. all vehicles work on a percentage of this.  Updated in worldparameters update
        public static float topVehicleSpeed;
        public static float turnTopSpeed;



        #region Initizialize

        public static void InitNewWorld()
        {
            day = 1;
            time = 10f;
            currentMapObjectId = 1;
            CreateLookupLists();
        }

        public static void DisposeWorld()
        {
            if (worldMini != null)
            {
                worldMini.Dispose();
            }
            if (world != null)
            {
                world.Dispose();
            }
            world = null;
            worldMini = null;
        }

        public static int GetNextMapObjectId()
        {
            currentMapObjectId++;
            return currentMapObjectId - 1;
        }
        
        public static void UpdateMinNightBrightness()
        {
            CreateLookupLists();

            if (world != null)
            {
                world.UpdateWorldParameters();
            }
        }

        private static void CreateLookupLists()
        {
            worldParametersList = new Dictionary<WorldParameters, float[]>();

            //so removing red adds some blue
            //i think we should reverse it
            //yellow in the morning
            //blue in the evening
            //white real early in the morning

            float[] globalBrightnessList = new float[24];
            float globalMaxLight = 0.85f;
            float globalMinLight = DisplayController.minNightBrightness;

            globalBrightnessList[0] = globalMinLight;
            globalBrightnessList[1] = globalMinLight;
            globalBrightnessList[2] = globalMinLight;
            globalBrightnessList[3] = globalMinLight;
            globalBrightnessList[4] = globalMinLight;
            globalBrightnessList[5] = globalMinLight;
            globalBrightnessList[6] = 0.3f;
            globalBrightnessList[7] = 0.7f;
            globalBrightnessList[8] = globalMaxLight;
            globalBrightnessList[9] = globalMaxLight;
            globalBrightnessList[10] = globalMaxLight;
            globalBrightnessList[11] = globalMaxLight;
            globalBrightnessList[12] = globalMaxLight;
            globalBrightnessList[13] = globalMaxLight;
            globalBrightnessList[14] = globalMaxLight;
            globalBrightnessList[15] = globalMaxLight;
            globalBrightnessList[16] = globalMaxLight;
            globalBrightnessList[17] = globalMaxLight;
            globalBrightnessList[18] = 0.7f;
            globalBrightnessList[19] = globalMinLight;
            globalBrightnessList[20] = globalMinLight;
            globalBrightnessList[21] = globalMinLight;
            globalBrightnessList[22] = globalMinLight;
            globalBrightnessList[23] = globalMinLight;
            worldParametersList.Add(WorldParameters.GLOBALBRIGHTNESS, globalBrightnessList);

            float[] globalRedList = new float[24];
            globalRedList[0] = 1f;
            globalRedList[1] = 1f;
            globalRedList[2] = 1f;
            globalRedList[3] = 1f;
            globalRedList[4] = 1f;
            globalRedList[5] = 1f;
            globalRedList[6] = 1f;
            globalRedList[7] = 1f;
            globalRedList[8] = 1f;
            globalRedList[9] = 1f;
            globalRedList[10] = 1f;
            globalRedList[11] = 1f;
            globalRedList[12] = 1f;
            globalRedList[13] = 1f;
            globalRedList[14] = 1f;
            globalRedList[15] = 1f;
            globalRedList[16] = 1f;
            globalRedList[17] = 1f;
            globalRedList[18] = 1f;
            globalRedList[19] = 1f;
            globalRedList[20] = 1f;
            globalRedList[21] = 1f;
            globalRedList[22] = 1f;
            globalRedList[23] = 1f;
            worldParametersList.Add(WorldParameters.GLOBALRED, globalRedList);

            float[] globalGreenList = new float[24];
            globalGreenList[0] = 1f;
            globalGreenList[1] = 1f;
            globalGreenList[2] = 1f;
            globalGreenList[3] = 1f;
            globalGreenList[4] = 1f;
            globalGreenList[5] = 1f;
            globalGreenList[6] = 1f;
            globalGreenList[7] = 1f;
            globalGreenList[8] = 1f;
            globalGreenList[9] = 1f;
            globalGreenList[10] = 1f;
            globalGreenList[11] = 1f;
            globalGreenList[12] = 1f;
            globalGreenList[13] = 1f;
            globalGreenList[14] = 1f;
            globalGreenList[15] = 1f;
            globalGreenList[16] = 1f;
            globalGreenList[17] = 1f;
            globalGreenList[18] = 1f;
            globalGreenList[19] = 1f;
            globalGreenList[20] = 1f;
            globalGreenList[21] = 1f;
            globalGreenList[22] = 1f;
            globalGreenList[23] = 1f;
            worldParametersList.Add(WorldParameters.GLOBALGREEN, globalGreenList);

            //lowering from 1 adds a yellow tinge
            float[] globalBlueList = new float[24];
            globalBlueList[0] = 1f;
            globalBlueList[1] = 1f;
            globalBlueList[2] = 1f;
            globalBlueList[3] = 1f;
            globalBlueList[4] = 1f;
            globalBlueList[5] = 1f;
            globalBlueList[6] = 0.85f;
            globalBlueList[7] = 0.7f;
            globalBlueList[8] = 0.85f;
            globalBlueList[9] = 1f;
            globalBlueList[10] = 1f;
            globalBlueList[11] = 1f;
            globalBlueList[12] = 1f;
            globalBlueList[13] = 1f;
            globalBlueList[14] = 1f;
            globalBlueList[15] = 1f;
            globalBlueList[16] = 1f;
            globalBlueList[17] = 1f;
            globalBlueList[18] = 1f;
            globalBlueList[19] = 1f;
            globalBlueList[20] = 1f;
            globalBlueList[21] = 1f;
            globalBlueList[22] = 1f;
            globalBlueList[23] = 1f;
            worldParametersList.Add(WorldParameters.GLOBALBLUE, globalBlueList);

            float[] shadowLengthList = new float[24];
            shadowLengthList[0] = 0f;
            shadowLengthList[1] = 0f;
            shadowLengthList[2] = 0f;
            shadowLengthList[3] = 0f;
            shadowLengthList[4] = 0f;
            shadowLengthList[5] = 2.8f;
            shadowLengthList[6] = 2.4f;
            shadowLengthList[7] = 2f;
            shadowLengthList[8] = 1.6f;
            shadowLengthList[9] = 1.2f;
            shadowLengthList[10] = 0.8f;
            shadowLengthList[11] = 0.4f;
            shadowLengthList[12] = 0f;
            shadowLengthList[13] = 0.4f;
            shadowLengthList[14] = 0.8f;
            shadowLengthList[15] = 1.2f;
            shadowLengthList[16] = 1.6f;
            shadowLengthList[17] = 2.0f;
            shadowLengthList[18] = 2.4f;
            shadowLengthList[19] = 2.8f;
            shadowLengthList[20] = 0f;
            shadowLengthList[21] = 0f;
            shadowLengthList[22] = 0f;
            shadowLengthList[23] = 0f;
            worldParametersList.Add(WorldParameters.SHADOWLENGTH, shadowLengthList);

            float[] shadowBrightness = new float[24];
            shadowBrightness[0] = 1f;
            shadowBrightness[1] = 1f;
            shadowBrightness[2] = 1f;
            shadowBrightness[3] = 1f;
            shadowBrightness[4] = 1f;
            shadowBrightness[5] = 1f;
            shadowBrightness[6] = 1f;
            shadowBrightness[7] = 0.95f;
            shadowBrightness[8] = 0.85f;
            shadowBrightness[9] = 0.7f;
            shadowBrightness[10] = 0.6f;
            shadowBrightness[11] = 0.7f;
            shadowBrightness[12] = 0.7f;
            shadowBrightness[13] = 0.6f;
            shadowBrightness[14] = 0.7f;
            shadowBrightness[15] = 0.8f;
            shadowBrightness[16] = 0.85f;
            shadowBrightness[17] = 1f;
            shadowBrightness[18] = 1f;
            shadowBrightness[19] = 1f;
            shadowBrightness[20] = 1f;
            shadowBrightness[21] = 1f;
            shadowBrightness[22] = 1f;
            shadowBrightness[23] = 1f;
            worldParametersList.Add(WorldParameters.SHADOWBRIGHTNESS, shadowBrightness);
        }

        #endregion

        public static TileLogistic GetTileLogistic(Point point)
        {
            return world.tileGrid[point.X, point.Y].tileLogistic;
        }

        public static List<Point> GetDebugListStraightRoadTiles()
        {
            List<Point> debugList = new List<Point>();
            for(int x = 0; x < world.worldWidth; x++)
            {
                for (int y = 0; y < world.worldHeight; y++)
                {
                    if (world.tileGrid[x, y].IsStraightRoad())
                    {
                        debugList.Add(new Point(x, y));
                    }
                }
            }
            return debugList;
        }

        public static bool IsInWorldBounds(int x, int y)
        {
            return x > 0 && x < world.worldWidth && y > 0 && y < world.worldWidth;
        }

        public static float GetParameterValue(WorldParameters parameter, int firstIndex, int secondIndex, float lerpAmount)
        {
            float valueOne = worldParametersList[parameter][firstIndex];
            float valueTwo = worldParametersList[parameter][secondIndex];
            return MathHelper.Lerp(valueOne, valueTwo, lerpAmount);

        }

        public static void AddDebugMessage(Point point)
        {
            Tile tile = world.tileGrid[point.X, point.Y];
            string p = tile.GetDebugString();
                

            ScreenController.AddMessage(p, Color.White);
        }

        public static Point ClampPointToWorld(Point point)
        {
            if (point.X < 0) { point.X = 0; }
            if (point.X > world.worldWidth) { point.X = world.worldWidth; }
            if (point.Y < 0) { point.Y = 0; }
            if (point.Y > world.worldHeight) { point.Y = world.worldHeight; }
            return point;
        }
    }

    enum WorldParameters
    {
        GLOBALBRIGHTNESS,   
        GLOBALRED,
        GLOBALGREEN,
        GLOBALBLUE,
        SHADOWLENGTH,
        SHADOWBRIGHTNESS
    }



    //Plot is only used for the shrunk map
    enum LandType   //Plot is only used for map generation
    {
        WATER,
        OPEN,
        BORDER,
        TREE,
        CITYROAD,
        COUNTRYROAD,
        BUILDING,
        PLOT
    }
}
