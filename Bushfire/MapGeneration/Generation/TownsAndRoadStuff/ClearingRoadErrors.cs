using BushFire.Game;
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
    class ClearingRoadErrors
    {
        public ClearingRoadErrors(ShrunkNode[,] shrunkMap, LoadingInfo loadingInfo)
        {
            float percentDone = 0;
            float percentJump = 100f / CreatingWorld.worldWidth;

            for (int x = 0; x < ShrunkWorldBuilder.shrunkWorldWidth; x++)
            {
                percentDone += percentJump;
                loadingInfo.UpdateLoading(LoadingType.SmoothingTowns, percentDone);
                for (int y = 0; y < ShrunkWorldBuilder.shrunkWorldHeight; y++)
                {
                    if (!IsAnyNeighbourRoads(new Point(x, y), shrunkMap) && shrunkMap[x, y].landType != LandType.PLOT)
                    {
                        if (ShrunkWorldBuilder.IsRoad(new Point(x, y), shrunkMap))
                        {
                            shrunkMap[x, y].SetLandType(LandType.OPEN);
                        }
                    }
                }
            }
        }

        private bool IsAnyNeighbourRoads(Point point, ShrunkNode[,] shrunkMap)
        {
            Point nextPoint;
            //only checking in four directions;
            for (int i = 0; i < 8; i += 2)
            {
                nextPoint = AngleStuff.AddPointToDirection(point, i);
                if(ShrunkWorldBuilder.PointLegit(nextPoint))
                    {
                    if (shrunkMap[nextPoint.X, nextPoint.Y].landType == LandType.CITYROAD || shrunkMap[nextPoint.X, nextPoint.Y].landType == LandType.COUNTRYROAD)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
