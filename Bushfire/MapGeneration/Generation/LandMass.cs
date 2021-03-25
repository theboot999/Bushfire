using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Game;
using BushFire.Game.Controllers;
using BushFire.Game.Map;
using BushFire.Game.Storage;
using BushFire.MapGeneration.Containers;
using BushFire.Menu.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Generation
{
    class LandMass
    {
        double[] groundLevels;

        public static int waterCoastline = 10;

        public LandMass(Tile[,] tileGrid, double[,] waterMap, LoadingInfo loadingInfo)
        {
            //following number is amount of ground layers
            groundLevels = new double[50];
            float value = -5.5f;
            for (int i = 0; i < groundLevels.Length; i++)
            {
                groundLevels[i] = value;
                value += 0.20f;
            }

            float percentDone = 0;
            float percentJump = 100f / CreatingWorld.worldWidth;

            for (int x = 0; x < CreatingWorld.worldWidth; x++)
            {
                percentDone += percentJump;
                loadingInfo.UpdateLoading(LoadingType.BuildingLandMass, percentDone);

                for (int y = 0; y < CreatingWorld.worldHeight; y++)
                {
                    if (tileGrid[x, y] == null)
                    {                       
                        int layerIndex = GetLayerIndex(waterMap[x, y]);
                        LayerType layerType = (LayerType)layerIndex;
                      

                        if (layerIndex > waterCoastline)
                        {
                            Tile tile = new Tile(GroundLayerController.GetRandomLayer(layerType), TileLogisticsController.GetTileLogistic(LandType.OPEN, 0), x, y);
                            tileGrid[x, y] = tile;
                        }
                        else
                        {
                            Tile tile = new Tile(GroundLayerController.GetRandomLayer(layerType), TileLogisticsController.GetTileLogistic(LandType.WATER, 0), x, y);
                            tileGrid[x, y] = tile;
                        }

                        if (x == 0 || x == CreatingWorld.worldWidth  - 1|| y == 0 || y == CreatingWorld.worldHeight - 1)
                        {
                            Tile tile = new Tile(GroundLayerController.GetLayerByIndex(LayerType.BORDER, 0), TileLogisticsController.GetTileLogistic(LandType.BORDER, 0), x, y);
                            tileGrid[x, y] = tile;
                        }

                    }
                }
            }
        }

     /*   //If we are water and we have land nearby we have to swtich to coastline layer
        public bool IsNeighbourLand(int x, int y, Double[,] waterMap)
        {
            for (int xScan = -1; xScan < 2; xScan++)
            {
                for (int yScan = -1; yScan < 2; yScan++)
                {
                    int nX = xScan + x;
                    int nY = yScan + y;

                    if (CreatingWorld.TileLegitX(nX) && CreatingWorld.TileLegitY(nY))
                    {
                        if (nX != x || nY != y)
                        {
                            int layerIndex = GetLayerIndex(waterMap[nX, nY]);

                            if (layerIndex > waterCoastline)
                            {
                                return true;
                            }

                        }
                    }
                }
            }
            return false;
        }*/

        public int GetLayerIndex(double input)
        {
            for (int i = 0; i < groundLevels.Length; i++)
            {
                if (input < groundLevels[i])
                {
                    return i;
                }
            }

            return groundLevels.Length;
        }

    }
}
