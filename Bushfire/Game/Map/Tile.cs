using BushFire.Content.Game.Screens.Containers;
using BushFire.Editor.Tech;
using BushFire.Engine.Controllers;
using BushFire.Game.Controllers;
using BushFire.Game.Map.FireStuff;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.Map.MapObjects;
using BushFire.Game.MapObjects;
using BushFire.Game.Screens;
using BushFire.Game.Storage;
using BushFire.Game.Tech;
using BushFire.MapGeneration.Generation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map
{
    //so the burnt layer has nothign to do with the fire. its simply visual.  if a spot starts burning it becomes a burnt layer
    //we could probably even reference the fire for a burnt darkness rating? maybe dont need to
    //burnt layer is purely visual. the fire is what were interested in

    class Tile
    {
        private GroundLayer baseLayer;   //Baselayer cannot be null
        private GroundLayer[] layerList;
        private GroundLayer burntLayer;
        private MapObject mapObject; //This is for the main square the object is on
        public MapObject childObject { get; private set; }
        public TileLogistic tileLogistic { get; private set; }
        public Fire fire { get; set; }
        public Vehicle vehicle { get; set; }
        private Vector2 location;
        //   public bool canBurn;

        //we have 6 spare bytes in our tile class
        //    public ushort spare1 = 1;
        //   public ushort spare2 = 1;
        //   public bool spare3 = true;
        //   public bool spare4 = true;



        public string GetDebugString()
        {
           // string p = GetStopLightCounter().ToString();

            

            
            string p = "Pathfinding Score - " + tileLogistic.pathFindingScore + System.Environment.NewLine +
        "Direction Block One - " + tileLogistic.directionBlockOne + System.Environment.NewLine +
        "Direction Block Two - " + tileLogistic.directionBlockTwo + System.Environment.NewLine +
        "Road Id - " + tileLogistic.roadId + System.Environment.NewLine +
        "LandType - " + tileLogistic.landType.ToString() + System.Environment.NewLine +
        "LandType - " + tileLogistic.isRoad + System.Environment.NewLine +
        "CenterVector - " + tileLogistic.center + System.Environment.NewLine +
        "CornerVector - " + tileLogistic.turnOverrideAmount + System.Environment.NewLine +
        "isVehicle" + Convert.ToString(vehicle != null) + System.Environment.NewLine +
        GetStopLightCounter().ToString();
            return p;
        }




        // public Layer vehicle1;
        //  public Layer vehicle2;

        public Tile(GroundLayer baseLayer, TileLogistic tileLogistic, int tileX, int tileY)
        {
            //landtype just all starts as water
            //when we expand our shunk map the landtype gets set?
            this.tileLogistic = tileLogistic;
            this.baseLayer = baseLayer;

            location = new Vector2((tileX * GroundLayerController.tileSize) + GroundLayerController.halfTileSize, (tileY * GroundLayerController.tileSize) + GroundLayerController.halfTileSize);
        }
       
        public void AddLayer(GroundLayer groundLayer)
        {
            if (layerList == null)
            {
                layerList = new GroundLayer[0];
            }
            ResizeLayerList(1);
            layerList[layerList.Length - 1] = groundLayer;
        }

        public void RemoveLastLayer()
        {
            ResizeLayerList(-1);
        }

        private void ResizeLayerList(int amount)
        {
            GroundLayer[] tempList = layerList;
            layerList = new GroundLayer[layerList.Length + amount];
            for (int i = 0; i < tempList.Length; i++)
            {
                if (i < layerList.Length)
                {
                    layerList[i] = tempList[i];
                }
            }
        }
        
        public MapObjectType GetMapObjectType()
        {
            if (childObject != null)
            {
                return childObject.GetMapObjectType();
            }
            else if (mapObject != null)
            {
                return mapObject.GetMapObjectType();
            }
            else
            {
                return MapObjectType.NONE;
            }
        }


        public GroundLayer GetBaseLayer()
        {
            return baseLayer;
        }

        public GroundLayer[] GetLayerList()
        {
            return layerList;
        }

        public GroundLayer GetBurntLayer()
        {
            return burntLayer;
        }

        public void SetTileLogistic(TileLogistic tileLogistic)
        {
            this.tileLogistic = tileLogistic;
        }

        public int GetBaseSmoothingLayer()
        {
            return baseLayer.GetSmoothingLayer();
        }

        public LayerType GetBaseLayerType()
        {
            return baseLayer.layerType;
        }

        public LandType GetLandType()
        {
            return tileLogistic.landType;
        }

        public Intersection GetIntersection()
        {
            MapObjectProperties mapObjectProperties = GetMapObjectProperties();

            if (mapObjectProperties != null && mapObject.GetMapObjectType() == MapObjectType.STOPLIGHT)
            {
                StopLight stopLight = (StopLight)mapObjectProperties;
                return stopLight.GetControllingIntersection();
            }
            return null;
        }

        private MapObjectProperties GetMapObjectProperties()
        {
            if (mapObject != null)
            {
                return mapObject.mapObjectProperties;
            }
            if (childObject != null)
            {
                return mapObject.mapObjectProperties;
            }
            return null;
        }


        public int GetRoadTileBitMaskId()
        {
            foreach (GroundLayer layer in layerList)
            {
                if (layer.layerType == LayerType.CITYROAD || layer.layerType == LayerType.COUNTRYROAD)
                {
                    return layer.bitMaskId;
                }    
            }
            return -1;
        }

        public bool IsBitMaskIdInList(LayerType layerType, int bitmaskId)
        {
            if (layerList != null)
            {
                foreach (GroundLayer layer in layerList)
                {
                    if (layer.layerType == layerType && layer.bitMaskId == bitmaskId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsRoad()
        {
            //We can use is road here if we want
            return tileLogistic.landType == LandType.CITYROAD || tileLogistic.landType == LandType.COUNTRYROAD;
        }

        public bool IsStraightRoad()
        {
            return tileLogistic.isRoad && tileLogistic.roadId < 4;
        }

        public bool IsOpen()
        {
            return tileLogistic.landType == LandType.OPEN;
        }

        public bool IsDrivable()
        {
            return tileLogistic.landType == LandType.CITYROAD || tileLogistic.landType == LandType.COUNTRYROAD || tileLogistic.landType == LandType.OPEN;     
        }

        //> -1 means a block
        public bool IsStopLightBlockDirection(int vehicleDirection, out bool isStopLight)
        {
            isStopLight = false;
            if (mapObject != null)
            {
                if (mapObject.mapObjectProperties.mapObjectType == MapObjectType.STOPLIGHT)
                {
                    StopLight stopLight = (StopLight)mapObject.mapObjectProperties;
                    if (vehicleDirection == stopLight.blockDirection)
                    { 
                        isStopLight = true;
                        if (stopLight.IsBlocked())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void IncreaseStopLightCounter()
        {
            if (mapObject != null)
            {
                if (mapObject.mapObjectProperties.mapObjectType == MapObjectType.STOPLIGHT)
                {
                    StopLight stopLight = (StopLight)mapObject.mapObjectProperties;
                    stopLight.IncreaseVehicleCounter();
                }
            }
        }

        public int GetStopLightCounter()
        {
            if (mapObject != null)
            {
                if (mapObject.mapObjectProperties.mapObjectType == MapObjectType.STOPLIGHT)
                {
                    StopLight stopLight = (StopLight)mapObject.mapObjectProperties;
                    return stopLight.vehicleCounter;
                }
            }
            return 0;
        }

        public bool IsParkedAndLockedVehicle(Vehicle currentVehicle)
        {
            if (vehicle == null)
            {
                return false;
            }
            else if (vehicle == currentVehicle)
            {
                return false;
            }
            else
            {
                return vehicle.vehicleState != VehicleState.Moving;
            }
        }

        public bool IsSlowVehicle()
        {
            if (vehicle != null)
            {
                return vehicle.GetCurrentSpeedPercent() < 0.3f;
            }
            return false;
        }

        public bool IsTileEmptyFromVehicles(Vehicle currentVehicle)
        {
            return vehicle == null || vehicle == currentVehicle;
        }


        //to save a variable we only use child object.  to find if its a parent object we just need to see
        //if mapobject is not null
        public void AddMapObject(MapObject mapObject, bool deleteBase, bool deleteLayerList, bool includeChildObject, TileLogistic tileLogistic, bool changeTileLogistic)
        {
            //We could also change this to have a bool of drawlayers or not
            this.mapObject = mapObject;

            if (includeChildObject)
            {
                childObject = mapObject;
            }

            if (deleteLayerList)
            {
                layerList = null;
            }
            if (deleteBase)
            {
                baseLayer = null;
            }

            if (changeTileLogistic)
            {
                this.tileLogistic = tileLogistic;
            }
        }

        public void SetChildObject(MapObject mapObject, bool deleteBase, bool deleteLayerList, TileLogistic tileLogistic, bool changeTileLogistic)
        {
            childObject = mapObject;

            if (deleteLayerList)
            {
                layerList = null;
            }
            if (deleteBase)
            {
                baseLayer = null;
            }

            if (changeTileLogistic)
            {
                this.tileLogistic = tileLogistic;
            }
        }

        public void SetBurntLayer()
        {
            if (fire != null)
            {
                burntLayer = GroundLayerController.GetBurntLayerByBitMask(fire.GetBurntLayerBitMaskId());
            }
        }
       
        public MapObject GetChildObject()
        {
            return childObject;
        }

        public void SetTileToFullyBurnt()
        {
            if (fire != null)
            {
                WorldController.worldMini.RecalculateMiniTile(new Point(fire.tileX, fire.tileY));
                fire = null;
            }
        }

        public bool IsFullyBurnt()
        {
            if (burntLayer != null)
            {
                return burntLayer.isFullBurnt;
            }

            return false;
        }

        public void DrawGameViewBox(SpriteBatch spriteBatch, int tileX, int tileY, float transparency)
        {
            Vector2 location = new Vector2((tileX * GroundLayerController.tileSize) + GroundLayerController.halfTileSize, (tileY * GroundLayerController.tileSize) + GroundLayerController.halfTileSize);

            baseLayer.DrawGameViewBox(spriteBatch, location, transparency);


            if (layerList != null)
            {
                for (int i = 0; i < layerList.Length; i++)
                {
                    layerList[i].DrawGameViewBox(spriteBatch, location, transparency);
                }
            }
        }



        public void Draw(SpriteBatch spriteBatch)
        {


           
       //     Vector2 location = new Vector2((tileX * GroundLayerController.tileSize) + GroundLayerController.halfTileSize, (tileY * GroundLayerController.tileSize) + GroundLayerController.halfTileSize);
            baseLayer.Draw(spriteBatch, location);

         /*   int textn = (int)baseLayer.layerType;

            SpriteFont spriteFont = GraphicsManager.GetSpriteFont(Engine.Font.Anita26);
            spriteBatch.DrawString(spriteFont, textn.ToString(), location, Color.White, 0f, Vector2.One, 1f, SpriteEffects.None, 0);*/

            if (layerList != null)
            {
                for (int i = 0; i < layerList.Length; i++)
                {
                    layerList[i].Draw(spriteBatch, location);
                }
            }                 
        }

        public void DrawBurntLayer(SpriteBatch spriteBatch)
        {
                burntLayer.Draw(spriteBatch, location);
       
        }

        public void AddObjectToDraw(MapObjectsDraw mapObjectsDraw)
        {
            if (fire != null && !fire.isCompleteBurntOut)
            {
                mapObjectsDraw.AddToFireList(fire);
            }
            if (mapObject != null)
            {
                mapObjectsDraw.AddToList(mapObject);
            }
            if (vehicle != null)
            {
                mapObjectsDraw.AddToVehicleLIst(vehicle);
            }
            if (burntLayer != null)
            {
                mapObjectsDraw.AddBurntTileToList(this);
            }

        }
    }
}
