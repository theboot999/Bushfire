using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Game;
using BushFire.Game.Map;
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

namespace BushFire.Game.Screens.Containers
{
    class MiniView : Container
    {
        Camera camera;
        Camera mainWorldCamera;
        WorldMini worldMini;
        private int zoomIndex = 3;
        Label debugLabel;
        Sprite spriteMainScreenBox;
        WorldVehicles worldVehicles;

        public MiniView(Rectangle location, DockType dockType, Camera mainWorldCamera) : base(location, dockType, true)
        {
            name = "MiniView";
            this.mainWorldCamera = mainWorldCamera;
            worldMini = WorldController.worldMini;
            worldVehicles = WorldController.world.worldVehicles;
            camera = new Camera(containerCamera.worldCameraViewport, 1, Vector2.Zero, new Vector2((worldMini.cellsWidth * 500) - 2, (worldMini.cellsHeight * 500) - 2), true);
            camera.clampToMap = true;
            camera.setZoomIndex(zoomIndex);
            debugLabel = new Label("DEBUG", Font.Anita12, Color.White, new Vector2(5, 20), false, "");
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetSpriteColour(6);

            transparency = 0.9f;
            camera.lockKeyMove = true;
            camera.lockZoom = true;
            spriteMainScreenBox = GraphicsManager.GetSpriteColour(8);
            AddBorder(2, Resizing.BOTH, 40);
            AddHeading(40, "Mini Map", GraphicsManager.GetSpriteFont(Font.CarterOne16), Color.White, true, false, true, true, true, GraphicsManager.GetPreBuilt(Engine.ContentStorage.PrebuiltSprite.InGameHeadingBar));
            SetSizeBounds(300, 300, 2500, 1400);



        }

        protected override void Rescale()
        {
            base.Rescale();
            Point maxSize = GetMaxSizeContainerMapSize();
            containerCamera.SnapResizeViewport(GetMaxSizeContainerMapSize());
            containerCamera.SetSizeBounds(maxSize.X, maxSize.Y);
        }

        private Point GetTile(Input input)
        {
            //need to do cull checking if outside the map
            Vector2 pos = camera.ScreenToWorld(input.GetMousePos()) / 2f;
            return new Point((int)Math.Ceiling(pos.X), (int)Math.Ceiling(pos.Y));
        }

        //ZOO
        private Point GetMaxSizeContainerMapSize()
        {
            Vector2 worldSize = worldMini.GetWorldSize();
            Vector2 topLeft = camera.WorldToScreen(Vector2.Zero);
            Vector2 bottomRight = camera.WorldToScreen(worldSize);
            Vector2 final = bottomRight - topLeft;

            return new Point((int)final.X, (int)final.Y);
        }

        private void UpdateDebug(Input input)
        {
            string text = "Camera + " + camera.cameraPosition + System.Environment.NewLine + "Mouse + " + camera.ScreenToWorld(input.GetMousePos())
            + System.Environment.NewLine + "Tile + " + GetTile(input)
            + System.Environment.NewLine + "CameraZoom " + camera.zoom;
            debugLabel.SetText(text);
        }

        private void UpdateZoomIndex()
        {
            if (isAction)
            {
                zoomIndex+=1;
                if (zoomIndex > 3)
                {
                    zoomIndex = 0;
                }
                camera.setZoomIndex(zoomIndex);
                containerCamera.SnapResizeViewport(GetMaxSizeContainerMapSize());
            }

            if (containerBorder != null && containerBorder.resizing)
            {
                Point maxSize = GetMaxSizeContainerMapSize();
                containerCamera.SetSizeBounds(maxSize.X + containerCamera.borderThickness, maxSize.Y + containerCamera.borderThickness + containerCamera.CalcScaleInt(containerCamera.headingHeight));
            }
        }



        private void UpdateMouseClick(Input input)
        {
            if (hasCameraFocus && hasFocus)
            {
                if (input.LeftButtonClick())
                {
                    Point tile = GetTile(input);
                    Vector2 position = new Vector2(tile.X * GroundLayerController.tileSize, tile.Y * GroundLayerController.tileSize);
                    mainWorldCamera.CenterOn(position);
                }
                if (input.RightButtonClick())
                {
                    Point tile = GetTile(input);
                    worldVehicles.AddActionToSelectedVehicles(input, tile);
                }
            }
        }

        public override void Update(Input input)
        {
            base.Update(input);

            camera.viewport = containerCamera.worldCameraViewport;
            camera.Update(input, hasCameraFocus);
            UpdateZoomIndex();
            UpdateMouseClick(input);
            camera.CenterOn(mainWorldCamera.cameraPosition / 64);
            camera.UpdateDrawPoints(500, worldMini.cellsWidth, worldMini.cellsHeight);
            if (DisplayController.showDebugWindowTwo) { UpdateDebug(input); }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.End();
            ScreenController.graphicsDevice.Viewport = camera.viewport;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TranslationMatrix(1f, 1f));
            worldMini.Draw(spriteBatch, camera.drawPoints, camera, IsHeadingTextActive(), containerFade);
            spriteBatch.End();

            spriteBatch.Begin();
            DrawMainScreenBox(spriteBatch);
           


            if (hasCameraFocus && DisplayController.showDebugWindowTwo)
            {
                debugLabel.Draw(spriteBatch, containerFade);
            }
        }


        private void DrawMainScreenBox(SpriteBatch spriteBatch)
        {
            float hBorder = GetFloatByScale(3);
            Vector2 topLeft = new Vector2((mainWorldCamera.drawPoints.topLeft.X * 2f) / 128f, (mainWorldCamera.drawPoints.topLeft.Y * 2f) / 128f);
            Vector2 botRight = new Vector2((mainWorldCamera.drawPoints.botRight.X * 2f) / 128f, (mainWorldCamera.drawPoints.botRight.Y * 2f) / 128f);

            topLeft = camera.WorldToScreen(topLeft);
            botRight = camera.WorldToScreen(botRight);


            float scaleWidth = botRight.X - topLeft.X;
            float scaleHeight = botRight.Y - topLeft.Y;

            Vector2 top = new Vector2(topLeft.X - hBorder, topLeft.Y - hBorder);
            Vector2 right = new Vector2(botRight.X - hBorder, topLeft.Y - hBorder);
            Vector2 bottom = new Vector2(topLeft.X, botRight.Y - hBorder);
            Vector2 left = new Vector2(topLeft.X - hBorder, topLeft.Y);

            Vector2 topScale = new Vector2(scaleWidth, hBorder);
            Vector2 sideScale = new Vector2(hBorder, scaleHeight);

            spriteBatch.Draw(spriteMainScreenBox.texture2D, top, spriteMainScreenBox.location, Color.White * 0.7f * containerFade, 0, spriteMainScreenBox.rotationCenter, topScale, SpriteEffects.None, 0);
            spriteBatch.Draw(spriteMainScreenBox.texture2D, right, spriteMainScreenBox.location, Color.White * 0.7f * containerFade, 0, spriteMainScreenBox.rotationCenter, sideScale, SpriteEffects.None, 0);
            spriteBatch.Draw(spriteMainScreenBox.texture2D, bottom, spriteMainScreenBox.location, Color.White * 0.7f * containerFade, 0, spriteMainScreenBox.rotationCenter, topScale, SpriteEffects.None, 0);
            spriteBatch.Draw(spriteMainScreenBox.texture2D, left, spriteMainScreenBox.location, Color.White * 0.7f * containerFade, 0, spriteMainScreenBox.rotationCenter, sideScale, SpriteEffects.None, 0);
        }
    }
}
