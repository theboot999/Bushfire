using BushFire.Game.Map;
using BushFire.Game.Map.MapObjects;
using BushFire.Menu.Screens;
using BushFire.Game.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BushFire.Game;
using System.Diagnostics;
using BushFire.Engine.Controllers;
using Microsoft.Xna.Framework;
using BushFire.Game.Tech;
using BushFire.MapGeneration.Containers;

namespace BushFire.MapGeneration.Generation
{
    class AddingTrees
    {
        //Instead of just having a level
        //make the level determine the chance of a tree landing there

        public AddingTrees(Tile[,] tileGrid, double[,] treeMap, LoadingInfo loadingInfo)
        {
            Random rnd = GameController.GetRandomWithSeed();
            double treeLevel = 0;
            //-6 to positive 6

            float percentDone = 0;
            float percentJump = 100f / CreatingWorld.worldWidth;


            for (int x = 0; x < CreatingWorld.worldWidth; x++)
            {
                percentDone += percentJump;
                loadingInfo.UpdateLoading(LoadingType.AddingTrees, percentDone);

                for (int y = 0; y < CreatingWorld.worldHeight; y++)
                {
                    //basically between 0 and 200

                    int i = (int)((treeMap[x,y] + 8)) * 10;
                    if (rnd.Next(0, 200) < i)
                    {
                        if (treeMap[x, y] > treeLevel)
                        {
                            if (tileGrid[x, y].GetChildObject() == null && tileGrid[x, y].GetLandType() == Game.LandType.OPEN)
                            {
                                float p = (float)(rnd.Next(500, 800)) / 1000f;

                                int type = rnd.Next(0, 8);

                                Tree tree = MapObjectController.GetTree(type);
                                MapObject mapObject = new MapObject(tree, x, y, Color.White, p);
                                TileLogistic tileLogistic = TileLogisticsController.GetTileLogistic(LandType.TREE, 0);
                                tileGrid[x, y].AddMapObject(mapObject, false, false, true, tileLogistic, true);
                            }
                        }
                    }
                }
            }

        }
    }
        

    
}
