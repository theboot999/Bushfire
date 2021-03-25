using BushFire.Content.Game.Screens.Containers;
using BushFire.Game.Map;
using BushFire.Game.Map.FireStuff;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Screens
{
    class MapObjectsDraw
    {
        private readonly int elevationCount = 6;
        List<MapObject>[] visibleMapObjectLists;
        HashSet<Vehicle> visibleVehicleList;
        List<Fire> visibleFireList;
        List<Tile> burntTileList;

        public MapObjectsDraw()
        {
            visibleMapObjectLists = new List<MapObject>[elevationCount];
            visibleVehicleList = new HashSet<Vehicle>();
            visibleFireList = new List<Fire>();
            burntTileList = new List<Tile>();

            for (int i = 0; i < elevationCount; i++)
            {
                visibleMapObjectLists[i] = new List<MapObject>();
            }
        }

        public void ResetLists()
        {
            for (int i = 0; i < elevationCount; i++)
            {
                visibleMapObjectLists[i].Clear();
            }
            visibleVehicleList.Clear();
            visibleFireList.Clear();
            burntTileList.Clear();
        }
        
        public void AddToList(MapObject mapObject)
        {
            visibleMapObjectLists[mapObject.mapObjectProperties.elevation].Add(mapObject);
        }
    
        public void AddToVehicleLIst(Vehicle vehicle)
        {
            if (!visibleVehicleList.Contains(vehicle))
            {
                visibleVehicleList.Add(vehicle);
            }
        }

        public void AddToFireList(Fire fire)
        {
            visibleFireList.Add(fire);
        }

        public bool IsBurntTiles()
        {
            return burntTileList.Count > 0;
        }

        public void AddBurntTileToList(Tile tile)
        {
            burntTileList.Add(tile);
        }

        public void DrawBurntLayer(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < burntTileList.Count; i++)
            {
                burntTileList[i].DrawBurntLayer(spriteBatch);
            }
        }

        public void DrawObjects(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < elevationCount; i++)
            {
                if (i == 1)
                {
                    //DrawVehicles
                    foreach (Vehicle vehicle in visibleVehicleList)
                    {
                        vehicle.Draw(spriteBatch);
                        GameView.debugVehiclesDrawn++;
                    }
                }

                for (int c = 0; c < visibleMapObjectLists[i].Count; c++)
                {
                    visibleMapObjectLists[i][c].DrawObject(spriteBatch);
                    GameView.debugMapObjectsDrawn++;
                }
 
            }

            //To do we are drawing all visible fires
            //however a fire could also be a burnt out fire as we need to store our information on it if its burnt out
            //so i think we need to do a check here if its not burnt out

        }

        //THERSE HAVE TO BE ITTERATED THROGUH SEPERATELY
        public void DrawShadows(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < elevationCount; i++)
            {
                for (int c = 0; c < visibleMapObjectLists[i].Count; c++)
                {
                    visibleMapObjectLists[i][c].DrawShadows(spriteBatch);
                }

                for (int c = 0; c < visibleMapObjectLists[i].Count; c++)
                {
                    visibleMapObjectLists[i][c].DrawVisibleBlock(spriteBatch);
                }
            }
        }

        public void DrawLighting(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < elevationCount; i++)
            {
                if (i == 1)
                {
                    //DrawVehicles
                    foreach (Vehicle vehicle in visibleVehicleList)
                    {
                        vehicle.DrawLighting(spriteBatch);
                    }
                }


                for (int c = 0; c < visibleMapObjectLists[i].Count; c++)
                {
                    visibleMapObjectLists[i][c].DrawVisibleBlock(spriteBatch);
                }

                for (int c = 0; c < visibleMapObjectLists[i].Count; c++)
                {
                    visibleMapObjectLists[i][c].DrawLighting(spriteBatch);
                }

            }
        }

        public void DrawLightsOverShadows(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < elevationCount; i++)
            {
                if (i == 1)
                {
                    //DrawVehicles
                    foreach (Vehicle vehicle in visibleVehicleList)
                    {
                        vehicle.DrawLighting(spriteBatch);
                    }
                }

                for (int c = 0; c < visibleMapObjectLists[i].Count; c++)
                {
                    visibleMapObjectLists[i][c].DrawLighting(spriteBatch);
                }

            }
        }

        public void DrawFires(SpriteBatch spriteBatch)
        {
            foreach (Fire fire in visibleFireList)
            {
                if (!fire.isCompleteBurntOut)
                {
                    fire.Draw(spriteBatch);
                }
            }
        }

    }
}
