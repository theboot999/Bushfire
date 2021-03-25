using BushFire.Game;
using BushFire.Game.Controllers;
using BushFire.Game.Map;
using BushFire.Game.Map.MapObjectComponents;
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
    class AddingStreetLights
    {
        private readonly int citySkip = 3;
        private readonly int countrySkip = 5;
        private readonly int distanceFromIntersection = 1;

        public AddingStreetLights(Tile[,] tileGrid, ShrunkNode[,] shrunkMap, LoadingInfo loadingInfo)
        {
            ScanHorizontal(tileGrid, shrunkMap, loadingInfo);
            ScanVertical(tileGrid, shrunkMap, loadingInfo);         
        }

        private void ScanHorizontal(Tile[,] tileGrid, ShrunkNode[,] shrunkMap, LoadingInfo loadingInfo)
        {
            float percentDone = 0;
            float percentJump = (100f / CreatingWorld.worldWidth) / 2f;

            int skipper;
            bool lightNorth = true;
            int counter = 0;



            for (int y = 0; y < ShrunkWorldBuilder.shrunkWorldWidth; y++)
            {
                percentDone += percentJump;
                loadingInfo.UpdateLoading(LoadingType.AddingStreetLights, percentDone);

                for (int x = 0; x < ShrunkWorldBuilder.shrunkWorldHeight; x++)
                {
                    if (shrunkMap[x, y].IsRoad())
                    {
                        counter++;
                        skipper = CalculateSkip(shrunkMap, x, y);
                        
                        if (counter % skipper == 0 && !AnyRoadsUpOrDown(x, y, shrunkMap) && !AnyIntersectionsLeftOrRight(x, y, shrunkMap, distanceFromIntersection))
                        {
                            lightNorth = !lightNorth;

                            if (lightNorth)
                            {
                                AddLight(tileGrid, x * 2, y * 2, 1, skipper);
                            }
                            else
                            {
                                AddLight(tileGrid, x * 2, (y * 2) + 1, 3, skipper);
                            }
                        }
                    }
                    else
                    {
                        counter = 0;
                    }
                }
            }
        }



        private void ScanVertical(Tile[,] tileGrid, ShrunkNode[,] shrunkMap, LoadingInfo loadingInfo)
        {
            float percentDone = 50;
            float percentJump = (100f / CreatingWorld.worldWidth) / 2f;

            int skipper;
            bool lightWest = true;
            int counter = 0;

            for (int x = 0; x < ShrunkWorldBuilder.shrunkWorldWidth; x++)
            {
                percentDone += percentJump;
                loadingInfo.UpdateLoading(LoadingType.AddingStreetLights, percentDone);

                for (int y = 0; y < ShrunkWorldBuilder.shrunkWorldHeight; y++)
                {
                    if (shrunkMap[x, y].IsRoad())
                    {
                        counter++;
                        skipper = CalculateSkip(shrunkMap, x, y);
                       
                        if (counter % skipper == 0 && !AnyRoadsLeftOrRight(x, y, shrunkMap) && !AnyIntersectionsUpOrDown(x, y, shrunkMap, distanceFromIntersection))
                        {
                            lightWest = !lightWest;

                            if (lightWest)
                            {
                                AddLight(tileGrid, x * 2, y * 2, 0, skipper);
                            }
                            else
                            {
                                AddLight(tileGrid, (x * 2) + 1, y * 2, 2, skipper);
                            }
                        }
                    }
                    else
                    {
                        counter = 0;
                    }
                }
            }
        }

        private int CalculateSkip(ShrunkNode[,] shrunkMap, int x, int y)
        {
            if (shrunkMap[x,y].landType == LandType.CITYROAD)
            {
                return citySkip;
            }
            else
            {
                return countrySkip;
            }
        }

        private void AddLight(Tile[,] tileGrid, int tileX, int tileY, int bitIndex, int skipper)
        {
            StreetLightType streetLightType;

            if (skipper == citySkip)
            {
                streetLightType = StreetLightType.STREETLIGHTCITY;
            }
            else
            {
                streetLightType = StreetLightType.STREETLIGHTCOUNTRY;
            }

            StreetLight streetLight = MapObjectController.GetStreetLight(streetLightType, bitIndex);
            MapObject mapObject = new MapObject(streetLight, tileX, tileY, Color.White, 1f);
            tileGrid[tileX, tileY].AddMapObject(mapObject, false, false, true, null, false);
        }

        private bool AnyRoadsUpOrDown(int x, int y, ShrunkNode[,] shrunkMap)
        {
            if (!ShrunkWorldBuilder.IsRoad(new Point(x, y - 1), shrunkMap))
            {
                return ShrunkWorldBuilder.IsRoad(new Point(x, y + 1), shrunkMap);
            }
            return true;
        }

        private bool AnyRoadsLeftOrRight(int x, int y, ShrunkNode[,] shrunkMap)
        {
            if (!ShrunkWorldBuilder.IsRoad(new Point(x - 1, y), shrunkMap))
            {
                return ShrunkWorldBuilder.IsRoad(new Point(x + 1, y), shrunkMap);
            }
            return true;
        }

        private bool AnyIntersectionsLeftOrRight(int x, int y,ShrunkNode[,] shrunkMap, int distance)
        {
            for(int currentX = x - distance; currentX <= x + distance; currentX++)
            {
                if (AnyRoadsUpOrDown(currentX, y, shrunkMap))
                {
                    return true;
                }
            }
            return false;
        }

        private bool AnyIntersectionsUpOrDown(int x, int y, ShrunkNode[,] shrunkMap, int distance)
        {
            for (int currentY = y - distance; currentY <= y + distance; currentY++)
            {
                if (AnyRoadsLeftOrRight(x, currentY, shrunkMap))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
