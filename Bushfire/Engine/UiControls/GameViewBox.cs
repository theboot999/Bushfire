using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls.Internal;
using BushFire.Game;
using BushFire.Game.Map;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.UIControls
{
    class GameViewBox : UiControl
    {
        private Rectangle preScaleLocation;
        private ListBoxCamera listBoxCamera;
        private ContainerCamera parentContainerCamera;
        private Point topLeftDraw;
        private Point botRightDraw;
        private float drawScale;

        List<MapObject> mapObjectList;
        List<Vehicle> vehicleList;

        Rectangle borderLeft;
        Rectangle borderTop;
        Rectangle borderRight;
        Rectangle borderBottom;
        int borderThickness;
        bool hasBorder;
        Sprite spriteBorder;

        public GameViewBox(string name, Rectangle location, ContainerCamera parentContainerCamera, float drawScale, bool hasBorder, int borderThickness, int borderColor)
        {
            mapObjectList = new List<MapObject>();
            vehicleList = new List<Vehicle>();
            this.drawScale = drawScale;
            listBoxCamera = new ListBoxCamera();
            preScaleLocation = location;
            this.parentContainerCamera = parentContainerCamera;
            this.name = name;
            this.hasBorder = hasBorder;
            this.borderThickness = borderThickness;
            spriteBorder = GraphicsManager.GetSpriteColour(borderColor);
            Rescale();
            UpdateViewport();
        }

        private void UpdateViewport()
        {
            listBoxCamera.UpdateViewport(new Viewport(parentContainerCamera.worldCameraViewport.X + location.X, parentContainerCamera.worldCameraViewport.Y + location.Y, location.Width, location.Height));
        }

        protected override void Rescale()
        {
            location.X = Convert.ToInt32((float)preScaleLocation.X * DisplayController.uiScale);
            location.Y = Convert.ToInt32((float)preScaleLocation.Y * DisplayController.uiScale);
            location.Width = Convert.ToInt32((float)preScaleLocation.Width * DisplayController.uiScale);
            location.Height = Convert.ToInt32((float)preScaleLocation.Height * DisplayController.uiScale);
            currentUiScale = DisplayController.uiScale;
            listBoxCamera.zoom = currentUiScale * drawScale;
            UpdateViewport();

            borderLeft = new Rectangle(GetIntByScale(preScaleLocation.X), GetIntByScale(preScaleLocation.Y) + borderThickness, borderThickness, GetIntByScale(preScaleLocation.Height) - borderThickness - borderThickness);
            borderTop = new Rectangle(GetIntByScale(preScaleLocation.X), GetIntByScale(preScaleLocation.Y), GetIntByScale(preScaleLocation.Width), borderThickness);
            borderRight = new Rectangle(GetIntByScale(preScaleLocation.X) + GetIntByScale(preScaleLocation.Width) - borderThickness, GetIntByScale(preScaleLocation.Y) + borderThickness, borderThickness, GetIntByScale(preScaleLocation.Height) - borderThickness - borderThickness);
            borderBottom = new Rectangle(GetIntByScale(preScaleLocation.X), GetIntByScale(preScaleLocation.Y) + GetIntByScale(preScaleLocation.Height) - borderThickness, GetIntByScale(preScaleLocation.Width), borderThickness);
        }
     


        public void SetFixedCameraPos(Vector2 cameraPos)
        {
            listBoxCamera.cameraPosition = cameraPos;
        }

        public void UpdateDrawPoints(Point topLeftDraw, Point botRightDraw)
        {
            this.topLeftDraw = WorldController.ClampPointToWorld(topLeftDraw);
            this.botRightDraw = WorldController.ClampPointToWorld(botRightDraw);
        }

        public void UpdateDrawPoints(Point topLeftDraw, Point botRightDraw, Vector2 cameraPos)
        {
            // I think we need to clamp these somehow so we dont go off the map
            this.topLeftDraw = WorldController.ClampPointToWorld(topLeftDraw);
            this.botRightDraw = WorldController.ClampPointToWorld(botRightDraw);
            listBoxCamera.cameraPosition = cameraPos;
        }

        public override void Update(Input input)
        {
            UpdateViewport();
            base.Update(input);
        }

        public void DrawContents(SpriteBatch spriteBatch, float containerFade)
        {
            mapObjectList.Clear();
            vehicleList.Clear();

            for (int x = topLeftDraw.X; x < botRightDraw.X; x++)
            {
                for (int y = topLeftDraw.Y; y < botRightDraw.Y; y++)
                {
                    Tile tile = WorldController.world.tileGrid[x, y];
                    tile.DrawGameViewBox(spriteBatch, x, y, containerFade);

                    if (tile.vehicle != null)
                    {
                        if (!vehicleList.Contains(tile.vehicle))
                        {
                            vehicleList.Add(tile.vehicle);
                        }
                    }

                    if (tile.childObject != null)
                    {
                        mapObjectList.Add(tile.childObject);
                    }
                }
            }

            foreach (Vehicle vehicle in vehicleList)
            {
                vehicle.DrawGameViewBox(spriteBatch, containerFade);
            }

            foreach (MapObject mapObject in mapObjectList)
            {

                mapObject.DrawGameViewBoxObject(spriteBatch, containerFade);
            }


        }

        public override void Draw(SpriteBatch spriteBatch, float containerFade)
        {
            base.Draw(spriteBatch, containerFade);

            //Do i need to translate draw back?  I dont recall
            //Sides



            listBoxCamera.TranslateDraw(spriteBatch);
            DrawContents(spriteBatch, containerFade);
            parentContainerCamera.TranslateDrawToCamera(spriteBatch);

            if (hasBorder)
            {
                spriteBatch.Draw(spriteBorder.texture2D, borderLeft, spriteBorder.location, Color.White * transparency * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                spriteBatch.Draw(spriteBorder.texture2D, borderTop, spriteBorder.location, Color.White * transparency * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                spriteBatch.Draw(spriteBorder.texture2D, borderRight, spriteBorder.location, Color.White * transparency * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                spriteBatch.Draw(spriteBorder.texture2D, borderBottom, spriteBorder.location, Color.White * transparency * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }


        }
    }
}
