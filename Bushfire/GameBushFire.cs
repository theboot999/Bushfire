using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BushFire.Engine;
using BushFire.Menu.Screens;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using BushFire.Engine.Controllers;
using BushFire.Engine.Files;
using BushFire.Content.Game.Screens.Containers;
using BushFire.Content.Game.Screens;
using System.Reflection;
using System;
using BushFire.Game.Storage;
using BushFire.Game;
using BushFire.Editor.Controllers;
using BushFire.Engine.ContentStorage;
using BushFire.Game.Controllers;
using BushFire.Game.Tech;
using System.Linq;

namespace BushFire
{
    //BUG WE ARE GETTIN GKEYBOARD INPUT IF WINDOW NOT SELECTED

    class GameBushFire : Microsoft.Xna.Framework.Game
    {
        private Input input;

        public GameBushFire()
        {
            DisplayController.graphics = new GraphicsDeviceManager(this);
            DisplayController.graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            DisplayController.graphics.PreparingDeviceSettings += (object s, PreparingDeviceSettingsEventArgs args) =>
            {
                args.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            };
        }

        #region LOAD / UNLOAD
        protected override void Initialize()
        {
            debugDrawTimeStopwatch = new Stopwatch();
           // Window.IsBorderless = false;
            Data.SetSettings();
            ScreenController.graphicsDevice = GraphicsDevice;
            DisplayController.Init(this);           
            EngineController.Init();
            GraphicsManager.Init(Content);
            AudioManager.Init(Content);

            //Game specific init. this could probably be done in game load
            GroundLayerController.Init();
            AngleStuff.Init();
            PieceController.Init();
            ShadowSpriteController.Init();
            MapObjectController.Init();
            TileLogisticsController.Init();
            GraphicsDevice.Clear(Color.White);
            IsMouseVisible = true;
            base.Initialize();            
            ScreenController.ChangeScreen(new MenuMain());
        }




        protected override void LoadContent()
        {
     
            input = new Input(Mouse.GetState());

            if (EngineController.debugMode)
            {
                GraphicsManager.TestContent();
                AudioManager.TestContent();
            }
        }

        protected override void UnloadContent()
        {
            AudioManager.Unload();
            DisplayController.Dispose();
            Content.Dispose();
        }

        #endregion

        #region METHODS   

        #endregion

        #region CALLS



        protected override void Update(GameTime gameTime)
        {
            EngineController.UpdateTimer(gameTime);
            if (IsActive)  //if the window is active only capture input for not fullscreen
            {
                input.Update(Keyboard.GetState(), Mouse.GetState());       
            }

            ScreenController.activeScreen.Update(input);
            base.Update(gameTime);
            input.UpdateCursor();
            if (EngineController.exitProgram) { Exit(); }

        }

        Stopwatch debugDrawTimeStopwatch;

        protected override void Draw(GameTime gameTime)
        {
            debugDrawTimeStopwatch.Start();
            GraphicsDevice.Clear(Color.Black);
            ScreenController.activeScreen.Draw(DisplayController.spriteBatch);
            base.Draw(gameTime);
            EngineController.AddDrawFrame();

            debugDrawTimeStopwatch.Stop();
            EngineController.drawTime = debugDrawTimeStopwatch.ElapsedTicks;
            debugDrawTimeStopwatch.Reset();
        }
        #endregion

 

    }
}
