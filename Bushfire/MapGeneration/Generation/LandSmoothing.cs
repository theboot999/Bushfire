using BushFire.Game;
using BushFire.Game.Map;
using BushFire.Game.Storage;
using BushFire.MapGeneration.Containers;
using BushFire.Menu.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Generation
{
    class LandSmoothing
    {
        public LandSmoothing(Tile[,] tileGrid, LoadingInfo loadingInfo)
        {
            int nX;
            int nY;
            int numberOfLayers = Enum.GetNames(typeof(LayerType)).Length;
            float percentDone = 0;
            float percentJump = 100f / (numberOfLayers * CreatingWorld.worldWidth);

            //loadingInfo.UpdateLoading(LoadingType.BuildingLandMass, percentDone);



            for (int cLayer = numberOfLayers; cLayer > -1; cLayer--)
            {
                for (int x = 0; x < CreatingWorld.worldWidth; x++)
                {
                    percentDone += percentJump;
                    loadingInfo.UpdateLoading(LoadingType.SmoothingLandMass, percentDone);
                    for (int y = 0; y < CreatingWorld.worldHeight; y++)
                    {
                        if (cLayer == tileGrid[x, y].GetBaseSmoothingLayer())
                        {
                            //Straight edges first
                            for (int xScan = -1; xScan < 2; xScan++)
                            {
                                for (int yScan = -1; yScan < 2; yScan++)
                                {
                                    nX = xScan + x;
                                    nY = yScan + y;

                                    if (xScan + yScan == 1 || xScan + yScan == -1)
                                    {
                                        if (NextTileAdd(nX, nY, cLayer, out Tile tile, tileGrid)) { AddLayer(nX, nY, tileGrid[x, y].GetBaseLayerType(), BitMaskValue(x, y, nX, nY), tileGrid); }
                                    }
                                }
                            }

                            //Corner edges second.  We only add the corner edges if there is no corresponding straight edge
                            if (CheckForCorners(tileGrid, x, y))  //We only add corners to the water tiles
                            {
                                for (int xScan = -1; xScan < 2; xScan++)
                                {
                                    for (int yScan = -1; yScan < 2; yScan++)
                                    {
                                        nX = xScan + x;
                                        nY = yScan + y;

                                        if (xScan != 0 && yScan != 0)
                                        {
                                            if (NextTileAdd(nX, nY, cLayer, out Tile tile, tileGrid)) { AddLayerCorner(nX, nY, tileGrid[x, y].GetBaseLayerType(), BitMaskValue(x, y, nX, nY), tileGrid); }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool CheckForCorners(Tile[,] tileGrid, int x, int y)
        {
            if (tileGrid[x, y].GetBaseLayerType() == (LayerType)LandMass.waterCoastline)
            {
                return true;
            }
            if (x == 1 && y == 1) //topleft
            {
                return true;
            }
            if (x == CreatingWorld.worldWidth - 2 && y == 1) //top right
            {
                return true;
            }
            if (x == CreatingWorld.worldWidth - 2 && y == CreatingWorld.worldHeight - 2) //bottom right
            {
                return true;
            }
            if (x == 1 && y == CreatingWorld.worldHeight - 2) //bottom left
            {
                return true;
            }

            return false;
        }

        private void AddLayer(int nX, int nY, LayerType layerType, int bitMaskValue, Tile[,] tileGrid)
        {
            tileGrid[nX, nY].AddLayer(GroundLayerController.GetLayerByBitMask(layerType, bitMaskValue));
        }


        private void AddLayerCorner(int nX, int nY, LayerType layerType, int bitMaskValue, Tile[,] tileGrid)
        {
            Tile tile = tileGrid[nX, nY];

            if (bitMaskValue == 1) //NorthWest
            {
                if (!tile.IsBitMaskIdInList(layerType, 128) && !tile.IsBitMaskIdInList(layerType, 2))
                {
                    tileGrid[nX, nY].AddLayer(GroundLayerController.GetLayerByBitMask(layerType, bitMaskValue));
                }
            }
            if (bitMaskValue == 4) //NorhtEast
            {
                if (!tile.IsBitMaskIdInList(layerType, 2) && !tile.IsBitMaskIdInList(layerType, 8))
                {
                    tileGrid[nX, nY].AddLayer(GroundLayerController.GetLayerByBitMask(layerType, bitMaskValue));
                }
            }
            if (bitMaskValue == 16) //SouthEast
            {
                if (!tile.IsBitMaskIdInList(layerType, 8) && !tile.IsBitMaskIdInList(layerType, 32))
                {
                    tileGrid[nX, nY].AddLayer(GroundLayerController.GetLayerByBitMask(layerType, bitMaskValue));
                }
            }
            if (bitMaskValue == 64) //SouthWest
            {
                if (!tile.IsBitMaskIdInList(layerType, 32) && !tile.IsBitMaskIdInList(layerType, 64))
                {
                    tileGrid[nX, nY].AddLayer(GroundLayerController.GetLayerByBitMask(layerType, bitMaskValue));
                }
            }
        }

        private int BitMaskValue(int x, int y, int checkX, int checkY)
        {
            if (checkX < x && checkY < y) { return 1; } //northwest
            if (checkX == x && checkY < y) { return 2; } //north
            if (checkX > x && checkY < y) { return 4; } //northeast
            if (checkX > x && checkY == y) { return 8; } //east
            if (checkX > x && checkY > y) { return 16; } //southeast
            if (checkX == x && checkY > y) { return 32; } //south
            if (checkX < x && checkY > y) { return 64; } //southwest
            if (checkX < x && checkY == y) { return 128; } //west
            return 0;
        }

        public bool NextTileAdd(int tileX, int tileY, int cLayer, out Tile tile, Tile[,] tileGrid)
        {
            if (CreatingWorld.TileLegitX(tileX) && CreatingWorld.TileLegitY(tileY))
            {
                tile = tileGrid[tileX, tileY];
                if (tile.GetBaseSmoothingLayer() > cLayer)
                {
                    return true;
                }
            }
            tile = null;
            return false;
        }

    }
}
