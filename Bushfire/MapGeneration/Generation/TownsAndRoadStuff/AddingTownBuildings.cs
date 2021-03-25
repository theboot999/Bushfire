using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Game;
using BushFire.Game.Controllers;
using BushFire.Game.Map;
using BushFire.Game.MapObjects;
using BushFire.Game.Tech;
using BushFire.MapGeneration.Containers;
using BushFire.MapGeneration.Tech;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Generation.TownsAndRoadStuff
{
    class AddingTownBuildings
    {
        //OKAY SO WE NOW HAVE
        //SOMETHING CALLED A MAP OBJECT
        //THIS MAP OBJECT WILL INCLUDE THE BUILDING
        //BECAUSE THIS MAP OBJECT CAN HAVE SOME UNIQUE INFORMATION IN IT
        //RATHER THAN THE SPECIFIC BUILDING

        public AddingTownBuildings(List<Building> buildingList, Tile[,] tileGrid, List<Town> townList, LoadingInfo loadingInfo)
        {
            float percentDone = 0;
            float percentJump = 100f / townList.Count;

            Random rnd = GameController.GetRandomWithSeed();
            int failed = 1;

            foreach (Town town in townList)
            {
                percentDone += percentJump;
                loadingInfo.UpdateLoading(LoadingType.AddingTownBuildings, percentDone);

                foreach (Plot plot in town.plotList)
                {
                    Building building = GetBuilding(buildingList, plot.roadFaceDirection, plot.width, plot.height, rnd);

                    if (building != null)
                    {
                        AddBuildingToMap(GetTopLeft(building, plot), building, tileGrid, rnd);
                        town.IncreaseBuildingCount();
                    }
                    else
                    {
                        failed++;
                    }
                }
                town.DestroyPlotList();
            }
        }

        private Point GetTopLeft(Building building, Plot plot)
        {
            int topLeftX = plot.topLeft.X;
            int topLeftY = plot.topLeft.Y;

            if (plot.roadFaceDirection == 0) //up
            {
                topLeftX = plot.topLeft.X + ((plot.width - building.width) / 2);
                topLeftY = plot.topLeft.Y;
            }
            else if (plot.roadFaceDirection == 2) //right
            {
                topLeftX = plot.topLeft.X + (plot.width - building.width);
                topLeftY = plot.topLeft.Y + ((plot.height - building.height) / 2);
            }
            else if (plot.roadFaceDirection == 4) //down
            {
                topLeftX = plot.topLeft.X + ((plot.width - building.width) / 2);
                topLeftY = plot.topLeft.Y + (plot.height - building.height);
            }
            else if (plot.roadFaceDirection == 6) //left
            {
                topLeftX = plot.topLeft.X;
                topLeftY = plot.topLeft.Y + ((plot.height - building.height) / 2);
            }

            return new Point(topLeftX, topLeftY);
        }

        private void AddBuildingToMap(Point topLeft, Building building, Tile[,] tileGrid, Random rnd)
        {
            Color color = building.GetColorFromAvailable(rnd);
            MapObject mapObject = new MapObject(building, topLeft.X, topLeft.Y, color, 1f);
            TileLogistic tileLogistic = TileLogisticsController.GetTileLogistic(LandType.BUILDING, 0);

            tileGrid[topLeft.X, topLeft.Y].AddMapObject(mapObject, false, false, false, tileLogistic, true);

            for (int x = 0; x < building.width; x++)
            {
                for (int y = 0; y < building.height; y++)
                {
                    if (building.isPiece(x, y))
                    {
                        tileGrid[x + topLeft.X, y+ topLeft.Y].SetChildObject(mapObject, false, true, tileLogistic, true);
                    }

                    //add burn rates for hte tile etc
                    //here we also need to switch the tile type to what it is
                    //in this case, a 
                }
            }
        }

        private Building GetBuilding(List<Building> buildingList, int directionFacing, int width, int height, Random rnd)
        {
            for (int i = 0; i < 100; i++)
            {
                int p = rnd.Next(0, buildingList.Count);
                {
                    Building building = buildingList[p];
                    if (building.directionFacing == directionFacing)
                    {
                        if(building.width <= width)
                        {
                            if (building.height <= height)
                            {
                                return building;
                            }
                        }
                    }
                }
            }
            return null;


        }
    }
}
