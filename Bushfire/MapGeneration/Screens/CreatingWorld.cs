using BushFire.Content.Game;
using BushFire.Content.Game.Screens;
using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Game;
using BushFire.Game.Controllers;
using BushFire.Game.Map;
using BushFire.Game.MapObjects;
using BushFire.Game.Tech;
using BushFire.MapGeneration.Containers;
using BushFire.MapGeneration.Generation;
using BushFire.MapGeneration.Tech;
using BushFire.Menu.Containers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BushFire.Menu.Screens
{
    class CreatingWorld : Screen
    {
        enum CreationState
        {
            WaitingForPerlin,
            StartCreatingWorld,
            CreatingWorld,
            InitMiniMap,
            CreatingMiniMap,
            Done
        }

        //Benchmark graphics memory
        //Why is it climbing.
        //Trees seem to be doing it
        //we are only references textures
        //minimap turned of so we can check it
        //also have lights bouncing around on the screen, and one following the cursor on the loadings screen

        public static int worldWidth;
        public static int worldHeight;
        public static int numberOfTowns;
        public static int minTownRoads;
        public static int maxTownRoads;
        double[,] waterMap;
        double[,] treeMap;
        Random rndPerlinOne;
        Random rndPerlinTwo;
        float scatteredWater = 2.2f;  //raise this number for more lakes. 2f is about standard for a large landmass. 10f for lots of lakes.  need water level at -1.5f or the lakes will be ocean. DEFAULT 2.2f
        int waterSmoothPasses = 6;    //number of smoothing passes.  Test out a value of 1f. Default 20f.  Can go up to 30 but CPU 
        int treeSmoothPasses = 6;
        LoadingInfo loadingInfo;
        float scatteredTrees = 10f;
        volatile public bool perlinOneCompleted;
        volatile public bool perlinTwoCompleted;
        volatile CreationState creationState;

        public CreatingWorld(int worldWidth, int worldHeight, int numberOfTowns, int minTownRoads, int maxTownRoads)
        {
            perlinOneCompleted = false;
            perlinTwoCompleted = false;
            rndPerlinOne = new Random(GameController.seed);
            rndPerlinTwo = new Random(GameController.seed);

            WorldController.InitNewWorld();
            AddContainer(new BackgroundMenu(Rectangle.Empty, DockType.SCREENRESOLUTION, TextureSheet.Loading));
            loadingInfo = new LoadingInfo(new Rectangle(0, 0, 1000, 1200));
            AddContainer(loadingInfo);
            CreatingWorld.numberOfTowns = numberOfTowns;
            CreatingWorld.worldWidth = worldWidth;
            CreatingWorld.worldHeight = worldHeight;
            CreatingWorld.minTownRoads = minTownRoads;
            CreatingWorld.maxTownRoads = maxTownRoads;
            ThreadPool.QueueUserWorkItem(Go);          
        }

        public void Go(Object stateInfo)
        {
            creationState = CreationState.WaitingForPerlin;
            ThreadPool.QueueUserWorkItem(CreateWaterMap);
            ThreadPool.QueueUserWorkItem(CreateRiverMap);
        }



        public void CreateWaterMap(Object stateInfo)
        {
            PerlinNoise perlinNoise = new PerlinNoise(rndPerlinOne);
            waterMap = perlinNoise.GenerateNoiseMap(waterSmoothPasses, scatteredWater, 10f, false, loadingInfo, true, rndPerlinOne, worldWidth, worldHeight);
            perlinOneCompleted = true;
        }

        public void CreateRiverMap(Object stateInfo)
        {
            PerlinNoise perlinNoise = new PerlinNoise(rndPerlinTwo);
            treeMap = perlinNoise.GenerateNoiseMap(treeSmoothPasses, scatteredTrees, 10f, false, loadingInfo, false, rndPerlinTwo, worldWidth, worldHeight);
            perlinTwoCompleted = true;
        }

        public static bool TileLegitX(int x)
        {
            return x >= 0 && x < CreatingWorld.worldWidth;
        }

        public static bool TileLegitY(int y)
        {
            return y >= 0 && y < CreatingWorld.worldHeight;
        }

        public static bool TileLegit(Point tile)
        {
            if (tile.X >= 0 && tile.X < CreatingWorld.worldWidth)
            {
                return tile.Y >= 0 && tile.Y < CreatingWorld.worldHeight;
            }
            return false;
        }

        public static bool IsRoad(Tile[,] tileGrid, Point tile)
        {
            if (TileLegit(tile))
            {
                return (tileGrid[tile.X, tile.Y].GetLandType() == LandType.CITYROAD || tileGrid[tile.X, tile.Y].GetLandType() == LandType.COUNTRYROAD);
            }
            return false;
        }


        public static bool IsStraightRoad(Tile[,] tileGrid, Point tile)
        {
            //okay bit dodgey, just checking the stored bit mask value
            if (TileLegit(tile))
            {
                int id = tileGrid[tile.X, tile.Y].GetRoadTileBitMaskId();
                return (id == 248 || id == 107 || id == 31 || id == 214);
            }

            return false;
        }

        private void WaitPerlin()
        {
            while (true)
            {
                if (perlinOneCompleted)
                {
                    if (perlinTwoCompleted)
                    {
                        break;
                    }
                }
                Thread.Sleep(100);
            }
        }

        private void UpdateWaitPerlin()
        {
            if (perlinOneCompleted)
            {
                if (perlinTwoCompleted)
                {
                    creationState = CreationState.StartCreatingWorld;
                }
            }
        }

        //Done on seperate thread
        private void UpdateCreatingWorld(Object stateInfo)
        {
            Tile[,] tileGrid = new Tile[worldWidth, worldHeight];
            List<Intersection> intersectionList = new List<Intersection>();
            List<Town> townList = new List<Town>();

            //Creating Mass
            LandMass landMass = new LandMass(tileGrid, waterMap, loadingInfo);

            //Creating Mass
            LandSmoothing landSmoothing = new LandSmoothing(tileGrid, loadingInfo);

            //Creating Roads
            TownsAndRoads townsAndRoads = new TownsAndRoads(tileGrid, intersectionList, townList, loadingInfo);

            AddingTrees addingTrees = new AddingTrees(tileGrid, treeMap, loadingInfo);



            //Setting the world
            WorldController.world = new World(worldWidth, worldHeight, tileGrid, intersectionList, townList);
            WorldController.world.AddVehicle();
            //Faster GC Collection
            Array.Clear(waterMap, 0, waterMap.Length);
            Array.Clear(treeMap, 0, treeMap.Length);
            GC.Collect();
            creationState = CreationState.InitMiniMap;
        }

        //Done on main thread with pauses as we need to access the graphics device
        private void UpdateInitMiniMap()
        {
            WorldController.worldMini = new WorldMini();
            creationState = CreationState.CreatingMiniMap;
        }

        private void UpdateCreatingMiniMap()
        {
            if (WorldController.worldMini.Create(loadingInfo))
            {
                creationState = CreationState.Done;
            }
            
        }

        public override void Update(Input input)
        {
            base.Update(input);

            if (creationState == CreationState.WaitingForPerlin)
            {
                UpdateWaitPerlin();
            }
            else if(creationState == CreationState.StartCreatingWorld)
            {
                ThreadPool.QueueUserWorkItem(UpdateCreatingWorld);
                creationState = CreationState.CreatingWorld;
            }
            else if (creationState == CreationState.InitMiniMap)
            {
                UpdateInitMiniMap();
            }
            else if (creationState == CreationState.CreatingMiniMap)
            {
                UpdateCreatingMiniMap();
            }
            else if (creationState == CreationState.Done)
            {
                ScreenController.ChangeScreen(new GameScreen());
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }





    }

}
