using BushFire.Game;
using BushFire.Game.Controllers;
using BushFire.Game.Map;
using BushFire.Game.Storage;
using BushFire.MapGeneration.Containers;
using BushFire.Menu.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Generation.RoadStuff
{
    class AddingRoadTiles
    {
        public AddingRoadTiles(Tile[,] tileGrid, LoadingInfo loadingInfo)
        {
            float percentDone = 0;
            float percentJump = 100f / CreatingWorld.worldWidth;


            List<Point> pointsToUpdateList = new List<Point>();

            for (int x = 0; x < CreatingWorld.worldWidth; x++)
            {
                percentDone += percentJump;
                loadingInfo.UpdateLoading(LoadingType.AddingRoadTiles, percentDone);

                for (int y = 0; y < CreatingWorld.worldHeight; y++)
                {
                    if (CheckRoad(x, y, 0, tileGrid))
                    {
                        int bitCount = BitMaskValue(x, y, tileGrid);

                        if (bitCount > -1)
                        {
                            Tile tile = tileGrid[x, y];

                            int index = BitMask.GetTileIndexFromBitmask(bitCount);


                            if (index > -1 && index < 56)
                            {
                                if (tile.GetLandType() == LandType.CITYROAD)
                                {
                                    tile.AddLayer(GroundLayerController.GetLayerByIndex(LayerType.CITYROAD, index));
                                    tile.SetTileLogistic(TileLogisticsController.GetTileLogistic(LandType.CITYROAD, index));
                                }
                                else if (tile.GetLandType() == LandType.COUNTRYROAD)
                                {
                                    tile.AddLayer(GroundLayerController.GetLayerByIndex(LayerType.COUNTRYROAD, index));
                                    tile.SetTileLogistic(TileLogisticsController.GetTileLogistic(LandType.COUNTRYROAD, index));
                                }

                            }
                            /*else if (index == 56)
                            {
                                pointsToUpdateList.Add(new Point(x, y));
                            }*/

                            }
                        }
                    }
                }
          
        }

        private Vector2 GetNode(int index)
        {
            switch (index)
            {
                case 0:
                    return new Vector2(64, 93);
                 case 1:
                    return new Vector2(38, 64);
                case 2:
                    return new Vector2(64, 38);
                case 3:
                    return new Vector2(93, 64);
                case 4:
                    return new Vector2(93, 93);
                case 5:
                    return new Vector2(39, 93);
                case 6:
                    return new Vector2(39, 39);
                case 7:
                    return new Vector2(93, 39);
                case 8:
                    return new Vector2(93, 39);
                case 9:
                    return new Vector2(93, 93);
                case 10:
                    return new Vector2(93, 39);
                case 11:
                    return new Vector2(39, 39);
            }
            return new Vector2(GroundLayerController.halfTileSize, GroundLayerController.halfTileSize);
        }

        private int BitMaskValue(int centerX, int centerY, Tile[,] tileGrid)
        {
            int count = 0;
            if (CheckRoad(centerX - 1, centerY - 1, 1, tileGrid)) { count += 1; } //northwest
            if (CheckRoad(centerX, centerY - 1, 0, tileGrid)) { count += 2; } //north
            if (CheckRoad(centerX + 1, centerY - 1, 2, tileGrid)) { count += 4; } //northeast
            if (CheckRoad(centerX + 1, centerY, 0, tileGrid)) { count += 16; } //east
            if (CheckRoad(centerX + 1, centerY + 1, 3, tileGrid)) { count += 128; } //southeast
            if (CheckRoad(centerX, centerY + 1, 0, tileGrid)) { count += 64; } //south
            if (CheckRoad(centerX - 1, centerY + 1, 4, tileGrid)) { count += 32; } //southwest
            if (CheckRoad(centerX - 1, centerY, 0, tileGrid)) { count += 8; ; } //west
            return count;
        }

        private Boolean CheckRoad(int x, int y, int dir, Tile[,] tileGrid)
        {
            Tile tile;

            if (CreatingWorld.TileLegitX(x) && CreatingWorld.TileLegitY(y))
            {
                tile = tileGrid[x, y];
                if (tile.IsRoad())
                {
                    if (dir > 0)
                    {
                        return (CheckCorners(x, y, dir, tileGrid));
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckCorners(int x, int y, int dir, Tile[,] tileGrid)  
        {
            bool isRoad = true;

            if (CreatingWorld.TileLegitX(x) && CreatingWorld.TileLegitY(y))
            {
                if (dir == 1)     //Northwest
                {
                    if (CreatingWorld.TileLegitX(x + 1) && CreatingWorld.TileLegitY(y))
                    {
                        if (CheckRoadCorner(x + 1, y, tileGrid) == false) { isRoad = false; }
                    }
                    if (CreatingWorld.TileLegitX(x) && CreatingWorld.TileLegitY(y + 1))
                    {
                        if (CheckRoadCorner(x, y + 1, tileGrid) == false) { isRoad = false; }
                    }
                }
                if (dir == 2) //Northeast
                {
                    if (CreatingWorld.TileLegitX(x - 1) && CreatingWorld.TileLegitY(y))
                    {
                        if (CheckRoadCorner(x - 1, y, tileGrid) == false) { isRoad = false; }
                    }
                    if (CreatingWorld.TileLegitX(x) && CreatingWorld.TileLegitY(y + 1))
                    {
                        if (CheckRoadCorner(x, y + 1, tileGrid) == false) { isRoad = false; }
                    }
                }
                if (dir == 3) //SouthEast
                {
                    if (CreatingWorld.TileLegitX(x - 1) && CreatingWorld.TileLegitY(y))
                    {
                        if (CheckRoadCorner(x - 1, y, tileGrid) == false) { isRoad = false; }
                    }
                    if (CreatingWorld.TileLegitX(x) && CreatingWorld.TileLegitY(y - 1))
                    {
                        if (CheckRoadCorner(x, y - 1, tileGrid) == false) { isRoad = false; }
                    }
                }
                if (dir == 4) //Southwest
                {
                    if (CreatingWorld.TileLegitX(x + 1) && CreatingWorld.TileLegitY(y))
                    {
                        if (CheckRoadCorner(x + 1, y, tileGrid) == false) { isRoad = false; }
                    }
                    if (CreatingWorld.TileLegitX(x) && CreatingWorld.TileLegitY(y - 1))
                    {
                        if (CheckRoadCorner(x, y - 1, tileGrid) == false) { isRoad = false; }
                    }
                }
            }
            return isRoad;
        }

        private bool CheckRoadCorner(int x, int y, Tile[,] tileGrid)
        {
            bool check = false;
            if (CreatingWorld.TileLegitX(x) && CreatingWorld.TileLegitY(y))
            {
                Tile tile = tileGrid[x, y];
                if (tile.IsRoad())
                {
                    check = true;
                }
            }
            return check;
        }

    }
}
