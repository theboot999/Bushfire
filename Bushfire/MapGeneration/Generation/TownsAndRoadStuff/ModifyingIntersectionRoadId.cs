using BushFire.Game;
using BushFire.Game.Controllers;
using BushFire.Game.Map;
using BushFire.MapGeneration.Containers;
using BushFire.MapGeneration.Generation.RoadStuff;
using BushFire.MapGeneration.Tech;
using BushFire.Menu.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Generation.TownsAndRoadStuff
{
    //basicaly what we are doing
    //Any of the 2 straight roads in an intersection
    //Need there roadId + 4
    //This means they wont be affect by jump pathfinding
    //And perhaps we may need to use this for turning offset

    class ModifyingIntersectionRoadId
    {
        public ModifyingIntersectionRoadId(Tile[,] tileGrid, ShrunkNode[,] shrunkMap, LoadingInfo loadingInfo)
        {
            float percentDone = 0;
            float percentJump = 100f / CreatingWorld.worldWidth;

            for (int x = 0; x < ShrunkWorldBuilder.shrunkWorldWidth; x++)
            {
                percentDone += percentJump;
                loadingInfo.UpdateLoading(LoadingType.FixingIntersections, percentDone);

                for (int y = 0; y < ShrunkWorldBuilder.shrunkWorldHeight; y++)
                {
                    int amount = CountNeighbourRoadsShrunkMap(shrunkMap, new Point(x, y));
                    Point expanded = new Point(x * 2, y * 2);

                    if (amount > 2)
                    {
                        FixSpot(expanded, tileGrid);
                    }  
                }
            }
        }


        private void FixSpot(Point point, Tile[,] tileGrid)
        {
            Point nextPoint = point;

            for (int i = 2; i < 7; i += 1)
            {           
                if (CreatingWorld.IsRoad(tileGrid, nextPoint))
                {
                    int id = tileGrid[nextPoint.X, nextPoint.Y].tileLogistic.roadId;
                    if (id < 12) //Lets try do every road
                    {
                        LandType landtype = tileGrid[nextPoint.X, nextPoint.Y].tileLogistic.landType;
                        tileGrid[nextPoint.X, nextPoint.Y].SetTileLogistic(TileLogisticsController.GetTileLogistic(landtype, id + 12));
                    }
                }
                nextPoint = AngleStuff.AddPointToDirection(point, i);
            }
        }

        private int CountNeighbourRoadsShrunkMap(ShrunkNode[,] shrunkMap, Point point)
        {
            int amount = 0;
            Point nextPoint;

            for (int i = 0; i < 8; i += 2)
            {
                nextPoint = AngleStuff.AddPointToDirection(point, i);
                if (ShrunkWorldBuilder.PointLegit(nextPoint))
                {
                    if (shrunkMap[nextPoint.X, nextPoint.Y].landType == LandType.CITYROAD || shrunkMap[nextPoint.X, nextPoint.Y].landType == LandType.COUNTRYROAD)
                    {
                        amount++;
                    }
                }
            }
            return amount;
        }

        private int CountHowManyRoadsNormalMap(Tile[,] tileGrid, Point point)
        {
            int amount = 0;

            Point nextPoint;
            //only checking in four directions;
            for (int i = 2; i < 5; i += 1)
            {
                nextPoint = AngleStuff.AddPointToDirection(point, i);
                if (CreatingWorld.IsRoad(tileGrid, nextPoint))
                {
                    amount++;
                }
            }
            return amount;
        }

    }
}
