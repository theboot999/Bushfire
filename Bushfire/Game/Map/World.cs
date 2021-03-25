using BushFire.Editor.Tech;
using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Game;
using BushFire.Game.Controllers;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.MapObjects;
using BushFire.Game.Screens;
using BushFire.Game.Storage;
using BushFire.Game.Tech;
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
    enum MouseDragUpResult
    {
        NONE,
        COLLECTEDVEHICLES,
        CLICKEDONEVEHICLE,
        CLICKEDINTERSECTION,
        CLICKEDFIRESTATION
        
    }

    class World
    {
        //Later on we will have an active an inactive vehicle list

        public int worldWidth { get; private set; }
        public int worldHeight { get; private set; }
        public Tile[,] tileGrid { get; private set; }
        private List<Intersection> intersectionList;
        public List<Town> townList { get; private set; }
        public WorldVehicles worldVehicles { get; private set; }
        bool draggingSelection = false;
        Vector2 startDragSpot;
        Vector2 endDragSpot;
        Sprite dragSprite;
        public MouseDragUpResult mouseDragUpResult;
        bool firstDownOutsideWorld;  //Hack to make sure our first down is in the world

        public World(int worldWidth, int worldHeight, Tile[,] tileGrid, List<Intersection> intersectionList, List<Town> townList)
        {
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            this.tileGrid = tileGrid;
            this.intersectionList = intersectionList;
            this.townList = townList;
            
            UpdateWorldParameters();
            UpdateShadowSide();
            dragSprite = GraphicsManager.GetSpriteColour(20);
            worldVehicles = new WorldVehicles();
            WorldController.worldFire = new WorldFire();
        }        

        public void AddVehicle()
        {
            worldVehicles.AddVehicle();
        }

        public void Dispose()
        {
            Array.Clear(tileGrid, 0, tileGrid.Length);
            intersectionList.Clear();
            townList.Clear();
            worldVehicles = null;
        }

        private void AlignVectors(Vector2 topLeftIn, Vector2 bottomRightIn, out Vector2 topLeft, out Vector2 bottomRight)
        {
            topLeft = topLeftIn;
            bottomRight = bottomRightIn;

            if (bottomRightIn.X < topLeftIn.X)
            {
                topLeft.X = bottomRightIn.X;
                bottomRight.X = topLeftIn.X;
            }
            if (bottomRightIn.Y < topLeftIn.Y)
            {
                topLeft.Y = bottomRightIn.Y;
                bottomRight.Y = topLeftIn.Y;
            }
        }

        private void AdvanceTime()
        {
            WorldController.time += 0.001f * EngineController.gameUpdateTime;

            if (WorldController.time > 24f)
            {
                WorldController.time = 0f;
                WorldController.day++;
            }

            int hour = (int)WorldController.time;
            double minutesTemp = WorldController.time - Math.Truncate(WorldController.time);
            int minutes = ((int)(MathHelper.Lerp(0, 60, (float)minutesTemp)));
            WorldController.timeString = Convert.ToString(hour) + ":" + minutes.ToString();
        }


        public void UpdateWorldParameters()
        {
            //This also can be called from the menu
            float lerpAmount = (float)(WorldController.time - Math.Truncate(WorldController.time));
            int firstIndex = (int)WorldController.time;
            int secondIndex = (int)WorldController.time + 1;
            if (firstIndex > 23) { firstIndex = 23; }
            if (secondIndex > 23) { secondIndex = 0; }

            WorldController.globalBrightness = WorldController.GetParameterValue(WorldParameters.GLOBALBRIGHTNESS, firstIndex, secondIndex, lerpAmount);
            WorldController.shadowLength = WorldController.GetParameterValue(WorldParameters.SHADOWLENGTH, firstIndex, secondIndex, lerpAmount);
            WorldController.shadowDarkness = WorldController.GetParameterValue(WorldParameters.SHADOWBRIGHTNESS, firstIndex, secondIndex, lerpAmount);
            WorldController.globalRed = WorldController.GetParameterValue(WorldParameters.GLOBALRED, firstIndex, secondIndex, lerpAmount);           
            WorldController.globalGreen = WorldController.GetParameterValue(WorldParameters.GLOBALGREEN, firstIndex, secondIndex, lerpAmount);
            WorldController.globalBlue = WorldController.GetParameterValue(WorldParameters.GLOBALBLUE, firstIndex, secondIndex, lerpAmount);
            WorldController.lightsOn = WorldController.time < WorldController.timeForLightsOff || WorldController.time > WorldController.timeForLightsOn;
            WorldController.topVehicleSpeed = 6f * EngineController.gameUpdateTime;
            WorldController.turnTopSpeed = 0.3f * EngineController.gameUpdateTime;
        }

        private void UpdateShadowSide()
        {
            float time = WorldController.time;

            if (time > 12 && time < 18)
            {
                WorldController.currentWorldShadowSide = ShadowSide.RIGHT;
            }
            else if (time > 6 && time < 12)
            {
                WorldController.currentWorldShadowSide = ShadowSide.LEFT;
            }
            else
            {
                WorldController.currentWorldShadowSide = ShadowSide.NONE;
            }
        }

        private void UpdateIntersections()
        {
            for (int i = 0; i < intersectionList.Count; i++)
            {
                intersectionList[i].Update();
            }
        }



        private void UpdateFirstDown(Input input)
        {
            if (!firstDownOutsideWorld && input.LeftButtonDown() && !WorldController.mouseInWorldFocus)
            {
                firstDownOutsideWorld = true;
            }

            if (!input.LeftButtonDown())
            {
                firstDownOutsideWorld = false;
            }
        }

        private void UpdateMouseCursor(Input input)
        {
            //Check for hand cursor because we are hovering above a vehicle
            if (input.IsKeyMapDown(KeyMap.OpenInfoWindow) && WorldController.mouseInWorldFocus)
            {
                AlignVectors(WorldController.mouseWorldHover, WorldController.mouseWorldHover, out Vector2 topLeft, out Vector2 bottomRight);
                if (worldVehicles.CheckForCursorHover(topLeft, bottomRight))
                {
                    input.ChangeMouseCursor(Engine.ContentStorage.CursorType.HANDFINGER);
                }
            }

            //Check for hand cursors
            if (input.IsKeyMapDown(KeyMap.OpenInfoWindow) && WorldController.mouseInWorldFocus)
            {
                MapObjectType mapObjectType = tileGrid[WorldController.mouseTileHover.X, WorldController.mouseTileHover.Y].GetMapObjectType();
                if (mapObjectType == MapObjectType.STOPLIGHT)
                {
                    input.ChangeMouseCursor(Engine.ContentStorage.CursorType.HANDFINGER);
                }
                //Other map objects
            }

        }

        private void UpdateMouseSelection(Input input)
        {
            mouseDragUpResult = MouseDragUpResult.NONE;

 

            if (input.LeftButtonDown() && !draggingSelection && WorldController.mouseInWorldFocus && !firstDownOutsideWorld)
            {
                draggingSelection = true;
                startDragSpot = WorldController.mouseWorldHover;
                endDragSpot = WorldController.mouseWorldHover;
            }

            if (draggingSelection)
            {
                AlignVectors(startDragSpot, endDragSpot, out Vector2 topLeft, out Vector2 bottomRight);
                worldVehicles.ModifyDraggingList(input, topLeft, bottomRight);
                endDragSpot = WorldController.mouseWorldHover;
            }

            if (input.LeftButtonClick() && draggingSelection)
            {
                //Vehicles.  Because they can be multi click drags we use this.  Single click only returns if we double clicked one vehicle
                AlignVectors(startDragSpot, endDragSpot, out Vector2 topLeft, out Vector2 bottomRight);
                mouseDragUpResult = worldVehicles.ModifySelectedList(input, topLeft, bottomRight);
                draggingSelection = false;
            }
          
   

            if (mouseDragUpResult == MouseDragUpResult.NONE && input.LeftButtonDoubleClick() || input.IsKeyMapDown(KeyMap.OpenInfoWindow) && input.LeftButtonClick())
            {
                MapObjectType mapObjectType = tileGrid[WorldController.mouseTileHover.X, WorldController.mouseTileHover.Y].GetMapObjectType();
                if (mapObjectType == MapObjectType.STOPLIGHT)
                {
                    mouseDragUpResult = MouseDragUpResult.CLICKEDINTERSECTION;
                }
            }
        }
  

        public void Update(Input input, Camera mainCamera)
        {
            AdvanceTime();
            UpdateWorldParameters();
            UpdateShadowSide();
            UpdateFirstDown(input);
            UpdateMouseCursor(input);
            UpdateMouseSelection(input);
            UpdateIntersections();
            worldVehicles.Update(input, mainCamera);

            WorldController.worldMini.RecalculateMiniTile(WorldController.mouseTileHover);
        }


        public void DrawGroundLayers(SpriteBatch spriteBatch, DrawPoints drawPoints, MapObjectsDraw mapObjectsDraw)
        {
            EngineController.debugTilesOnScreen = 0;
            for (int x = drawPoints.topLeftPoint.X; x < drawPoints.botRightPoint.X; x++)
            {
                for (int y = drawPoints.topLeftPoint.Y; y < drawPoints.botRightPoint.Y; y++)
                {
                    EngineController.debugTilesOnScreen++;
                    tileGrid[x, y].Draw(spriteBatch);
                    tileGrid[x, y].AddObjectToDraw(mapObjectsDraw);
                }
            }
            ExtraObjectCollection(mapObjectsDraw, drawPoints);
        }      

        private void ExtraObjectCollection(MapObjectsDraw mapObjectsDraw, DrawPoints drawPoints)
        {
            for (int i = 0; i < 8; i += 2)
            {
                for (int x = drawPoints.overDrawPoints[i].X; x < drawPoints.overDrawPoints[i + 1].X; x++)
                {
                    for (int y = drawPoints.overDrawPoints[i].Y; y < drawPoints.overDrawPoints[i + 1].Y; y++)
                    {
                        tileGrid[x, y].AddObjectToDraw(mapObjectsDraw);
                    }
                }
            }
        }

        public void DrawWorldUI(SpriteBatch spriteBatch)
        {
            worldVehicles.DrawSelectedVehiclesUI(spriteBatch);     

            if (draggingSelection)
            {
                AlignVectors(startDragSpot, endDragSpot, out Vector2 topLeft, out Vector2 bottomRight);
                float scaleX = bottomRight.X - topLeft.X;
                float scaleY = bottomRight.Y - topLeft.Y;
                Vector2 scaleWidth = new Vector2(scaleX, 4);
                Vector2 scaleHeight = new Vector2(4, scaleY);
              
                //Top
                spriteBatch.Draw(dragSprite.texture2D, topLeft, dragSprite.location, Color.White * 0.8f, 0, dragSprite.rotationCenter, scaleWidth, SpriteEffects.None, 0);
                //Bottom
                spriteBatch.Draw(dragSprite.texture2D, new Vector2(topLeft.X, bottomRight.Y), dragSprite.location, Color.White * 0.8f, 0, dragSprite.rotationCenter, scaleWidth, SpriteEffects.None, 0);
                //Left
                spriteBatch.Draw(dragSprite.texture2D, topLeft, dragSprite.location, Color.White * 0.8f, 0, dragSprite.rotationCenter, scaleHeight, SpriteEffects.None, 0);
                //Right
                spriteBatch.Draw(dragSprite.texture2D, new Vector2(bottomRight.X, topLeft.Y), dragSprite.location, Color.White * 0.8f, 0, dragSprite.rotationCenter, scaleHeight, SpriteEffects.None, 0);
            }
        }    
    }
}
