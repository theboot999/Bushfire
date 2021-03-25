using BushFire.Game;
using BushFire.Game.Map;
using BushFire.MapGeneration.Containers;
using BushFire.MapGeneration.Tech;
using BushFire.Menu.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Generation.RoadStuff
{
    class ShrunkWorldBuilder
    {
        public static int shrunkWorldWidth;
        public static int shrunkWorldHeight;

        public ShrunkWorldBuilder(Tile[,] tileGrid, ShrunkNode[,] shrunkMap, LoadingInfo loadingInfo)
        {
            float percentDone = 0;
            float percentJump = 100f / CreatingWorld.worldWidth;

            for (int x = 0; x < shrunkWorldWidth; x++)
            {
                percentDone += percentJump;
                loadingInfo.UpdateLoading(LoadingType.BuildingShrunkWorld, percentDone);

                for (int y = 0; y < shrunkWorldHeight; y++)
                {
                    LandType landType = LandType.OPEN;
                    for (int checkX = (x * 2); checkX < (x * 2) + 2; checkX++)
                    {
                        for (int checkY = (y * 2); checkY < (y * 2) + 2; checkY++)
                        {
                            if (tileGrid[checkX, checkY].GetLandType() == LandType.WATER)
                            {
                                landType = LandType.WATER;
                                break;
                            }
                            if (tileGrid[checkX, checkY].GetLandType() == LandType.BORDER)
                            {
                                landType = LandType.BORDER;
                                break;
                            }
                        }
                        if (landType == LandType.WATER)
                        {
                            break;
                        }
                    }
                    shrunkMap[x, y] = new ShrunkNode(landType);
                }
            }



        }

        #region METHODS


        public static bool xLegitShrunkMap(int workingX, int offSet)
        {
            return (workingX > (-1 + offSet) && workingX < (shrunkWorldWidth - offSet));

        }

        public static bool yLegitShrunkMap(int workingY, int offSet)
        {
            return (workingY > (-1 + offSet) && workingY < (shrunkWorldHeight - offSet));

        }

        public static bool PointLegit(Point point)
        {
            if (point.X > -1 && point.X < shrunkWorldWidth)
            {
                return (point.Y > -1 && point.Y < shrunkWorldHeight);
            }
            return false;           
        }

        public static bool IsRoad(Point checkPoint, ShrunkNode[,] shrunkMap)
        {
            if (PointLegit(checkPoint))
            {
                return shrunkMap[checkPoint.X, checkPoint.Y].IsRoad();
            }
            return false;
        }

        public static bool IsOpen(Point checkPoint, ShrunkNode[,] shrunkMap)
        {
            if (PointLegit(checkPoint))
            {
                return (shrunkMap[checkPoint.X, checkPoint.Y].landType == LandType.OPEN);
              }
            return false;
        }


        public static bool IsAnyNeighbourRoads(Point point, LandType[,] shrunkMap)
        {
            Point nextPoint;
            //only checking in four directions;
            for (int i = 0; i < 8; i += 2)
            {
                nextPoint = AngleStuff.AddPointToDirection(point, i);
                if (xLegitShrunkMap(nextPoint.X, 0) && yLegitShrunkMap(nextPoint.Y, 0))
                {
                    if (shrunkMap[nextPoint.X, nextPoint.Y] == LandType.CITYROAD || shrunkMap[nextPoint.X, nextPoint.Y] == LandType.COUNTRYROAD)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
        
    }
}
