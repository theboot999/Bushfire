using BushFire.Game;
using BushFire.Game.Map;
using BushFire.Game.MapObjects;
using BushFire.Game.Tech;
using BushFire.MapGeneration.Containers;
using BushFire.MapGeneration.Generation.RoadStuff;
using BushFire.MapGeneration.Generation.TownsAndRoadStuff;
using BushFire.MapGeneration.Tech;
using BushFire.Menu.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Generation
{
    //So perhaps when creating the world
    //we can scroll down going creating this 
    //doing this
    //as a list that gets added
    //with a progress bar
    //also
    //i think when creating the minimap, if we create 1 cell per frame,
    //we can draw a screen update without having this bag lag affect
    //even though it is a bit faster
    //then as a debug we can add on there the milliseconds taking to
    //even cooler
    //as we are building the mini map
    //we should draw it on the screen


    class TownsAndRoads
    {
        private ShrunkNode[,] shrunkMap;
        private List<Point> connectionList;
        private List<ShrunkPlot> shrunkPlotList = new List<ShrunkPlot>();
        private List<Building> buildingList = new List<Building>();      

        public TownsAndRoads(Tile[,] tileGrid, List<Intersection> intersectionList, List<Town> townList, LoadingInfo loadingInfo)
        {
            ShrunkWorldBuilder.shrunkWorldWidth = CreatingWorld.worldWidth / 2;
            ShrunkWorldBuilder.shrunkWorldHeight = CreatingWorld.worldHeight / 2;
            shrunkMap = new ShrunkNode[ShrunkWorldBuilder.shrunkWorldWidth, ShrunkWorldBuilder.shrunkWorldHeight];

           // DebugLine("ShrunkWorldBuilder");
            ShrunkWorldBuilder shrunkWorldBuilder = new ShrunkWorldBuilder(tileGrid, shrunkMap, loadingInfo);
          //  DebugLine("TownSitesBuilder");
            TownSitesBuilder townSitesBuilder = new TownSitesBuilder(shrunkMap, townList, ref connectionList, loadingInfo);
         //   DebugLine("BuildingTownRoads");
            BuildingTownRoads buildingTownRoads = new BuildingTownRoads(townList, shrunkMap, shrunkPlotList, loadingInfo);
         //   DebugLine("ClearingShrunkPlotErrors");
            ClearingShrunkPlotErrors clearingShrunkPlotErrors = new ClearingShrunkPlotErrors(shrunkMap, shrunkPlotList);
         //   DebugLine("ExpandShrunkPlot");
            ExpandShrunkPlot expandShrunkPlot = new ExpandShrunkPlot(shrunkPlotList, townList, loadingInfo);
          //  DebugLine("ConnectingTowns");
            ConnectingTowns connectingTowns = new ConnectingTowns(ref townList, ref connectionList, shrunkMap, loadingInfo);
         //   DebugLine("ClearingRoadErrors");
            ClearingRoadErrors clearingRoadErrors = new ClearingRoadErrors(shrunkMap, loadingInfo);
        //    DebugLine("ExpandShrunkWorld");
            ExpandShrunkWorld expandShrunkWorld = new ExpandShrunkWorld(tileGrid, shrunkMap, loadingInfo);
        //    DebugLine("LoadingTownBuildings");
            LoadingTownBuildings loadingTownBuildings = new LoadingTownBuildings(buildingList);
       //     DebugLine("AddingTownBuildings");
            AddingTownBuildings addingTownBuildings = new AddingTownBuildings(buildingList, tileGrid, townList, loadingInfo);
       //     DebugLine("AddingRoadTiles");
            AddingRoadTiles addingRoadTiles = new AddingRoadTiles(tileGrid, loadingInfo);
       //     DebugLine("CreatingIntersections");
            CreatingIntersections creatingIntersections = new CreatingIntersections(tileGrid, shrunkMap, intersectionList, loadingInfo);
       //     DebugLine("ModifyingIntersectionRoadId");
            ModifyingIntersectionRoadId modifyingIntersectionRoadId = new ModifyingIntersectionRoadId(tileGrid, shrunkMap, loadingInfo);
       //     DebugLine("AddingStreetLights");
            AddingStreetLights addingStreetLights = new AddingStreetLights(tileGrid, shrunkMap, loadingInfo);
                   
        }

        Stopwatch watch;
        string next;
        private void DebugLine(string value)
        {

            if (watch != null)
            {
                watch.Stop();
                Debug.WriteLine("TIMER: " + next + " :" + watch.ElapsedMilliseconds);
            }
            watch = new Stopwatch();
            watch.Start();
            next = value;

        }

    }
}
