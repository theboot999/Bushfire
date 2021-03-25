using BushFire.Editor.Tech;
using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Game;
using BushFire.Game.Controllers;
using BushFire.Game.Map;
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

namespace BushFire.Content.Game.Screens.Containers
{
    //TODO: look into adding gausian blur tuo shadows
    //TODO: gausian blur when in menu

    class GameView : Container
    {
        public Camera camera { get; private set; }
        World world;
        Label debugLabel;

        Effect EffectshadowInverter;
        Effect Effectblur;
        Effect Effectsoften;
        Effect Effectshadow;
        Effect Effectlighting;

        Sprite backingWhite;

        MapObjectsDraw mapObjectsDraw;

        int worldObjectsRender;
        int worldShadowsRender;

        int shadowMaskInvertedRender;
        int shadowMaskRender;

        int worldLightingRender;
        int lightingMaskRender;

        int blurRender;
        int blurRenderTwo;

        int burntLayerRender;
        int burntToSoftenRender;


        public static int debugItemsDrawn;
        public static int debugMapObjectsDrawn;
        public static int debugVehiclesDrawn;

        public GameView(Rectangle location, DockType dockType) : base(location, dockType, true)
        {
            canChangeFocusOrder = false;
            world = WorldController.world;
            mapObjectsDraw = new MapObjectsDraw();

            Vector2 cameraMin = new Vector2(GroundLayerController.tileSize, GroundLayerController.tileSize);
            Vector2 cameraMax = new Vector2((world.worldWidth * GroundLayerController.tileSize) - GroundLayerController.tileSize, (world.worldHeight * GroundLayerController.tileSize) - GroundLayerController.tileSize);

            camera = new Camera(containerCamera.worldCameraViewport, 4, cameraMin, cameraMax, false);
            camera.clampToMap = true;
            camera.CenterOn(new Vector2(24000, 24000));
            camera.UpdateDrawPoints(GroundLayerController.tileSize, world.worldWidth, world.worldHeight);
            debugLabel = new Label("DEBUG", Font.Anita14, Color.White, new Vector2(300, 20), false, "");
            camera.lockZoom = false;
            camera.lockKeyMove = false;
            backingWhite = GraphicsManager.GetSpriteColour(20);
            InitRendering();
            //we can not set our container camera to be scrollable as that would simply bug our input and draw out
        }

        public override void ResolutionChange()
        {
            base.ResolutionChange();
            camera.UpdateViewport(containerCamera.worldCameraViewport);
            camera.UpdateDrawPoints(GroundLayerController.tileSize, world.worldWidth, world.worldHeight);
            Dispose();
            InitRendering();
        }

        private void InitRendering()
        {
            //Render Targets
            worldObjectsRender = DisplayController.AddRenderTarget(camera.viewport.Width, camera.viewport.Height, true);
            worldShadowsRender = DisplayController.AddRenderTarget(camera.viewport.Width, camera.viewport.Height, true);
            worldLightingRender = DisplayController.AddRenderTarget(camera.viewport.Width, camera.viewport.Height, false);
            lightingMaskRender = DisplayController.AddRenderTarget(camera.viewport.Width, camera.viewport.Height, false);
            shadowMaskInvertedRender = DisplayController.AddRenderTarget(camera.viewport.Width, camera.viewport.Height, false);
            shadowMaskRender = DisplayController.AddRenderTarget(camera.viewport.Width, camera.viewport.Height, false);
            blurRender = DisplayController.AddRenderTarget(camera.viewport.Width, camera.viewport.Height, false);
            blurRenderTwo = DisplayController.AddRenderTarget(camera.viewport.Width, camera.viewport.Height, false);
            burntLayerRender = DisplayController.AddRenderTarget(camera.viewport.Width, camera.viewport.Height, false);
            burntToSoftenRender = DisplayController.AddRenderTarget(camera.viewport.Width, camera.viewport.Height, false);

            //Effects
            Effectshadow = GraphicsManager.GetEffect(Engine.ContentStorage.EffectType.Shadow);
            Effectshadow.Parameters["shadowMask"].SetValue(DisplayController.GetRenderTarget(shadowMaskRender));
            Effectlighting = GraphicsManager.GetEffect(Engine.ContentStorage.EffectType.Lighting);
            Effectlighting.Parameters["lightMask"].SetValue(DisplayController.GetRenderTarget(lightingMaskRender));

            EffectshadowInverter = GraphicsManager.GetEffect(Engine.ContentStorage.EffectType.ShadowInverter);
            EffectshadowInverter.Parameters["shadowMask"].SetValue(DisplayController.GetRenderTarget(shadowMaskInvertedRender));
            EffectshadowInverter.Parameters["shadowMask"].SetValue(DisplayController.GetRenderTarget(shadowMaskInvertedRender));

            Effectsoften = GraphicsManager.GetEffect(Engine.ContentStorage.EffectType.Soften);
            Effectsoften.Parameters["Texture"].SetValue(DisplayController.GetRenderTarget(burntLayerRender));

            Effectblur = GraphicsManager.GetEffect(Engine.ContentStorage.EffectType.Blur);
        }

        public override void Dispose()
        {
            DisplayController.DisposeRenderTarget(worldObjectsRender);
            DisplayController.DisposeRenderTarget(worldShadowsRender);
            DisplayController.DisposeRenderTarget(worldLightingRender);
            DisplayController.DisposeRenderTarget(lightingMaskRender);
            DisplayController.DisposeRenderTarget(shadowMaskInvertedRender);
            DisplayController.DisposeRenderTarget(shadowMaskRender);
            DisplayController.DisposeRenderTarget(blurRender);
            DisplayController.DisposeRenderTarget(blurRenderTwo);
            DisplayController.DisposeRenderTarget(burntLayerRender);
            DisplayController.DisposeRenderTarget(burntToSoftenRender);
        }

        StringBuilder stringBuilder = new StringBuilder();

        private void UpdateDebug(Input input)
        {
            stringBuilder.Clear();
            stringBuilder.Append("Camera + " + camera.cameraPosition + System.Environment.NewLine + "Mouse + " + WorldController.mouseWorldHover
            + System.Environment.NewLine + "Tile + " + WorldController.mouseTileHover
            + System.Environment.NewLine + "World Brightness " + WorldController.globalBrightness
            + System.Environment.NewLine + "ShadowSide " + WorldController.currentWorldShadowSide
            + System.Environment.NewLine + "ShadowLength " + WorldController.shadowLength
            + System.Environment.NewLine + "ShadowDarkness " + WorldController.shadowDarkness
            + System.Environment.NewLine + "CameraZoom " + camera.zoom
            + System.Environment.NewLine + "Layers Drawn " + debugItemsDrawn
            + System.Environment.NewLine + "Map Objects Drawn " + debugMapObjectsDrawn
            + System.Environment.NewLine + "Vehicles Drawn " + debugVehiclesDrawn);

            debugLabel.SetText(stringBuilder.ToString());
        }

        private void UpdateMouseClicks(Input input)
        {
            WorldController.mouseWorldHover = camera.ScreenToWorld(input.GetMousePos());
            WorldController.mouseTileHover.X = (int)WorldController.mouseWorldHover.X / 128;
            WorldController.mouseTileHover.Y = (int)WorldController.mouseWorldHover.Y / 128;
            WorldController.mouseInWorldFocus = hasCameraFocus;
        }

        public override void Update(Input input)
        {
            if (GameController.inGameState == InGameState.RUNNING)
            {
                base.Update(input);
                camera.viewport = containerCamera.worldCameraViewport;  //in case there as a resize etc
                camera.Update(input, hasCameraFocus);
                camera.UpdateDrawPoints(GroundLayerController.tileSize, world.worldWidth, world.worldHeight);

                if (DisplayController.showDebugWindowTwo)
                {
                    UpdateDebug(input);
                }

                if (input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Y))
                {
                    WorldController.time = 10f;
                }
                if (input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.U))
                {
                    WorldController.time = 20f;
                }

                if (input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.C))
                {
                    WorldController.AddDebugMessage(new Point(WorldController.mouseTileHover.X, WorldController.mouseTileHover.Y));

                }

                UpdateMouseClicks(input);
            }
        }


        private void DrawGroundLayers(SpriteBatch spriteBatch)
        {
            //Draw Ground Layers
            ScreenController.graphicsDevice.Viewport = camera.viewport;
            ScreenController.graphicsDevice.SetRenderTarget(DisplayController.GetRenderTarget(worldObjectsRender));
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TranslationMatrix(1f, 1f));
            world.DrawGroundLayers(spriteBatch, camera.drawPoints, mapObjectsDraw);
            spriteBatch.End();

            //Draw burnt layer
            if (mapObjectsDraw.IsBurntTiles())
            {
                ScreenController.graphicsDevice.SetRenderTarget(DisplayController.GetRenderTarget(burntLayerRender));
                ScreenController.graphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.TranslationMatrix(1f, 1f));
                mapObjectsDraw.DrawBurntLayer(spriteBatch);
                spriteBatch.End();

              /*  //Soften the burnt
                ScreenController.graphicsDevice.SetRenderTarget(DisplayController.GetRenderTarget(burntToSoftenRender));
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null);
                spriteBatch.Draw(DisplayController.GetRenderTarget(burntLayerRender), Vector2.Zero, Color.White * 0.5f);
                spriteBatch.End();*/

                //Back to world render
                ScreenController.graphicsDevice.SetRenderTarget(DisplayController.GetRenderTarget(worldObjectsRender));
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null);
                spriteBatch.Draw(DisplayController.GetRenderTarget(burntLayerRender), Vector2.Zero, Color.White * 0.85f);
                spriteBatch.End();
            }      
        }

        private void DrawMapObjects(SpriteBatch spriteBatch)
        {
            //Draw Map Objects
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TranslationMatrix(1f, 1f));
            mapObjectsDraw.DrawObjects(spriteBatch);
            spriteBatch.End();
        }

        //Render order
        //Draw Ground Layers
        //Draw Burnt Layers
        //Draw Objects
        //Draw Shadows
        //Draw Fire and Smoke
        //Draw Lighting


        private void DrawShadows(SpriteBatch spriteBatch)
        {
            //Switch RenderTarget to ShadowTarget
            ScreenController.graphicsDevice.SetRenderTarget(DisplayController.GetRenderTarget(shadowMaskInvertedRender));
            ScreenController.graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TranslationMatrix(1f, 1f));
            mapObjectsDraw.DrawShadows(spriteBatch);
            spriteBatch.End();

            ScreenController.graphicsDevice.SetRenderTarget(DisplayController.GetRenderTarget(shadowMaskRender));
            ScreenController.graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TranslationMatrix(1f, 1f));
            mapObjectsDraw.DrawLightsOverShadows(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, EffectshadowInverter);
            EffectshadowInverter.Parameters["darkness"].SetValue(WorldController.shadowDarkness);
            spriteBatch.Draw(DisplayController.GetRenderTarget(shadowMaskInvertedRender), Vector2.Zero, Color.White);
            spriteBatch.End();

            ScreenController.graphicsDevice.SetRenderTarget(DisplayController.GetRenderTarget(worldShadowsRender));
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, Effectshadow);
            spriteBatch.Draw(DisplayController.GetRenderTarget(worldObjectsRender), Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        private void DrawFires(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TranslationMatrix(1f, 1f));
            mapObjectsDraw.DrawFires(spriteBatch);
            spriteBatch.End();
        }


        private void DrawLighting(SpriteBatch spriteBatch)
        {
            //Building the lighting Map
            ScreenController.graphicsDevice.SetRenderTarget(DisplayController.GetRenderTarget(lightingMaskRender));
            ScreenController.graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TranslationMatrix(1f, 1f));
            mapObjectsDraw.DrawLighting(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(backingWhite.texture2D, new Rectangle(0, 0, camera.viewport.Width, camera.viewport.Height), backingWhite.location, Color.White * WorldController.globalBrightness, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.End();

            //Outputting the lighting map to our final worldRender
            ScreenController.graphicsDevice.SetRenderTarget(DisplayController.GetRenderTarget(worldLightingRender));
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, Effectlighting);
            Effectlighting.Parameters["red"].SetValue(WorldController.globalRed);
            Effectlighting.Parameters["green"].SetValue(WorldController.globalGreen);
            Effectlighting.Parameters["blue"].SetValue(WorldController.globalBlue);
            spriteBatch.Draw(DisplayController.GetRenderTarget(worldShadowsRender), Vector2.Zero, Color.White);
            spriteBatch.End();

        }

        private void DrawFinalToScreen(SpriteBatch spriteBatch)
        {
            if (GameController.inGameState != InGameState.INMENU)  //Standard draw
            {
                ScreenController.graphicsDevice.SetRenderTarget(null);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null);
                spriteBatch.Draw(DisplayController.GetRenderTarget(worldLightingRender), Vector2.Zero, Color.White);
                spriteBatch.End();
            }
            else   //In menu so draw Blur
            {
                ScreenController.graphicsDevice.SetRenderTarget(DisplayController.GetRenderTarget(blurRender));
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, Effectlighting);
                Effectlighting.Parameters["red"].SetValue(WorldController.globalRed);
                Effectlighting.Parameters["green"].SetValue(WorldController.globalGreen);
                Effectlighting.Parameters["blue"].SetValue(WorldController.globalBlue);
                spriteBatch.Draw(DisplayController.GetRenderTarget(worldLightingRender), Vector2.Zero, Color.White);
                spriteBatch.End();

                ScreenController.graphicsDevice.SetRenderTarget(DisplayController.GetRenderTarget(blurRenderTwo));
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null);
                Effectblur.Parameters["Texture"].SetValue(DisplayController.GetRenderTarget(blurRender));
                Effectblur.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(DisplayController.GetRenderTarget(blurRender), Vector2.Zero, Color.White);
                spriteBatch.End();

                ScreenController.graphicsDevice.SetRenderTarget(null);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null);
                Effectblur.Parameters["Texture"].SetValue(DisplayController.GetRenderTarget(blurRenderTwo));
                Effectblur.CurrentTechnique.Passes[1].Apply();
                spriteBatch.Draw(DisplayController.GetRenderTarget(blurRenderTwo), Vector2.Zero, Color.White);
                spriteBatch.End();
            }
        }

        private void DrawWorldUI(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TranslationMatrix(1f, 1f));
            world.DrawWorldUI(spriteBatch);
            spriteBatch.End();
        }

        private void ResetDraw()
        {
            mapObjectsDraw.ResetLists();
            debugItemsDrawn = 0;
            debugMapObjectsDrawn = 0;
            debugVehiclesDrawn = 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.End();

            ResetDraw();
            DrawGroundLayers(spriteBatch);
            DrawMapObjects(spriteBatch);
            DrawShadows(spriteBatch);
            DrawFires(spriteBatch);
            DrawLighting(spriteBatch);
            DrawFinalToScreen(spriteBatch);
            DrawWorldUI(spriteBatch);

            spriteBatch.Begin();
            if (DisplayController.showDebugWindowTwo) { debugLabel.Draw(spriteBatch, containerFade); }
        }
    }
}
