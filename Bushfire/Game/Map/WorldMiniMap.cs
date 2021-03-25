using BushFire.Editor.Tech;
using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.Map.MapObjects;
using BushFire.Game.MapObjects;
using BushFire.Game.Storage;
using BushFire.Game.Tech;
using BushFire.MapGeneration.Containers;
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
    class WorldMini
    {
        //Careful if we put the update minimap textures in the fire thread
        //We could clash with the renderer if we do it wrong

        const int tilePixel = 2;
        const int tilesPerCell = 250;  //cellSize divided by tilePixel


        private MiniMapCell[,] miniMapCellGrid;
        public int cellsWidth { get; private set; }
        public int cellsHeight { get; private set; }
        private List<MapLabel> townLabels = new List<MapLabel>();
        private float[] uiScale;

        List<Vehicle> miniMapVehicleList;
        Sprite vehicleCircle;
        float vehicleScale = 0.5f;
        bool countUp;
        bool drawVehicleCircles = true;

        int updateCellX = 0;
        int updateCellY = 0;
        int cellsCount = 0;

        public WorldMini()
        {
            AddLabels();
            cellsWidth = WorldController.world.worldWidth / tilesPerCell;
            cellsHeight = WorldController.world.worldHeight / tilesPerCell;
            miniMapCellGrid = new MiniMapCell[cellsWidth, cellsHeight];
            cellsCount = cellsWidth * cellsHeight;
            vehicleCircle = new Sprite(new Rectangle(0, 200, 64, 64), TextureSheet.WorldUI);
            miniMapVehicleList = WorldController.world.worldVehicles.miniMapVehicleList;
        }

        private void AddLabels()
        {
            foreach (Town town in WorldController.world.townList)
            {
                MapLabel label = new MapLabel(town.name, town.miniMapLocation, Font.OpenSans30, Color.Goldenrod, 1f, 1f);
                townLabels.Add(label);
            }
            uiScale = new float[4];
            uiScale[0] = 1.5f;
            uiScale[1] = 1.1f;
            uiScale[2] = 0.8f;
            uiScale[3] = 0.6f;
        }


        public bool Create(LoadingInfo loadingInfo)
        {
            miniMapCellGrid[updateCellX, updateCellY] = new MiniMapCell(updateCellX, updateCellY, tilesPerCell, tilePixel);
            updateCellX++;

            if (updateCellX >= cellsWidth)
            {
                updateCellX = 0;
                updateCellY++;

                if (updateCellY >= cellsHeight)
                {
                    updateCellY = 0;
                    updateCellX = 0;
                    return true;
                }
            }

            UpdateCreatingLabel(loadingInfo);
            return false;
        }

        private void UpdateCreatingLabel(LoadingInfo loadingInfo)
        {
            int total = cellsWidth * cellsHeight;
            int done = (updateCellY * cellsWidth) + updateCellX + 1;

            float percentDone = (float)done / (float)total * 100;

            loadingInfo.UpdateLoading(LoadingType.CreatingMiniMap, percentDone);
        }

        public void RecalculateMiniTile(Point point)
        {
            int cellX = point.X / tilesPerCell;
            int cellY = point.Y / tilesPerCell;

       //     Debug.WriteLine("CELLX: " + cellX + " point " + point.X + " / " + tilesPerCell);
            miniMapCellGrid[cellX, cellY].AddToRecalculateList(point);
        }


        public void Dispose()
        {
            for (int x = 0; x < cellsWidth; x++)
            {
                for (int y = 0; y < cellsHeight; y++)
                {
                    miniMapCellGrid[x, y].Dispose();
                    miniMapCellGrid[x, y] = null;
                }
            }
        }


        public Vector2 GetWorldSize()
        {
            return new Vector2(cellsWidth * tilesPerCell * 2, cellsHeight * tilesPerCell * 2);
        }

        private void UpdateDrawRecalculateList()
        {
            for (int i = 0; i < cellsCount;  i++)
            {

                updateCellX++;
                if (updateCellX >= cellsWidth)
                {
                    updateCellX = 0;
                    updateCellY++;

                    if (updateCellY >= cellsHeight)
                    {
                        updateCellY = 0;
                    }
                }

                MiniMapCell cell = miniMapCellGrid[updateCellX, updateCellY];

                if (cell.IsEmptyCalculateList())
                {
                    break;
                }
                cell.DrawCellRecalculateList();
 
            }

        }

        public void Update()
        {
            UpdateDrawRecalculateList();
        }

        public void Draw(SpriteBatch spriteBatch, DrawPoints drawPoints, Camera camera, bool drawLabels, float containerFade)
        {
            //Draw Cells
            for (int x = drawPoints.topLeftPoint.X; x < drawPoints.botRightPoint.X; x++)
            {
                for (int y = drawPoints.topLeftPoint.Y; y < drawPoints.botRightPoint.Y; y++)
                {
                    miniMapCellGrid[x, y].Draw(spriteBatch, x, y, containerFade);
                }
            }

            //Draw Vehicle Circles
            if (drawVehicleCircles)
            {

                if (countUp)
                {
                    vehicleScale += 0.005f * EngineController.drawUpdateTime;
                    if (vehicleScale > 0.5)
                    {
                        countUp = false;
                    }
                }
                else
                {
                    vehicleScale -= 0.005f * EngineController.drawUpdateTime;
                    if (vehicleScale < 0.35)
                    {
                        countUp = true;
                    }
                }

                float scale = uiScale[camera.zoomCurrentIndex] * vehicleScale;

                foreach (Vehicle vehicle in miniMapVehicleList)
                {
                    Vector2 location = vehicle.GetMiniMapPosition();
                    spriteBatch.Draw(vehicleCircle.texture2D, location, vehicleCircle.location, Color.White * containerFade, 0, vehicleCircle.rotationCenter, scale, SpriteEffects.None, 0);
                }
            }

            //Draw Labels
            if (drawLabels)
            {
                float labelScale = uiScale[camera.zoomCurrentIndex];
                for (int i = 0; i < townLabels.Count; i++)
                {
                    townLabels[i].SetTextScale(labelScale);
                    townLabels[i].Draw(spriteBatch, containerFade);
                }
            }

        }


    }
}
