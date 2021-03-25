using BushFire.Game;
using BushFire.Game.Controllers;
using BushFire.Game.Map;
using BushFire.Game.Storage;
using BushFire.MapGeneration.Containers;
using BushFire.MapGeneration.Tech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Generation.RoadStuff
{
    class ExpandShrunkWorld
    {
        public ExpandShrunkWorld(Tile[,] tileGrid, ShrunkNode[,] shrunkMap, LoadingInfo loadingInfo)
        {
            float percentDone = 0;
            float percentJump = 100f / ShrunkWorldBuilder.shrunkWorldWidth;

            for (int x = 0; x < ShrunkWorldBuilder.shrunkWorldWidth; x++)
            {
                percentDone += percentJump;
                loadingInfo.UpdateLoading(LoadingType.ExpandingShrunkWorld, percentDone);
                for (int y = 0; y < ShrunkWorldBuilder.shrunkWorldHeight; y++)
                {
                    for (int useX = (x * 2); useX < (x * 2) + 2; useX++)
                    {
                        for (int useY = (y * 2); useY < (y * 2) + 2; useY++)
                        {
                            LandType landType = shrunkMap[x, y].landType;

                            if (landType == LandType.PLOT)
                            {
                                landType = LandType.OPEN;
                            }
                            tileGrid[useX, useY].SetTileLogistic(TileLogisticsController.GetTileLogistic(landType, 0));                          
                        }
                    }
                }
            }
        }
    }
}
