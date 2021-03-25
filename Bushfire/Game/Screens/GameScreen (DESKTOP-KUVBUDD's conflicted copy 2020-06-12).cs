using BushFire.Engine;
using BushFire.Engine.UIControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Menu.Containers;
using System.Diagnostics;
using BushFire.Engine.Controllers;
using System.Collections.Generic;
using BushFire.Menu.Screens;
using BushFire.Content.Game.Screens.Containers;
using BushFire.Game.Screens.Containers;
using System;
using System.Threading;
using BushFire.Game;
using BushFire.Game.Screens.Containers.InMenu;
using BushFire.Game.Tech;
using BushFire.Game.Tech.Jobs;

namespace BushFire.Content.Game.Screens
{
    class GameScreen : Screen
    {
        Camera mainWorldCamera;
        Thread secondThread;
        JobWorker jobWorker;

        //Threading
        private volatile UpdateState secondThreadState;
        private volatile bool secondThreadRunning;
        private static readonly object lockObject = new object();

        public GameScreen()
        {       
            GC.Collect();  //Seems to do a better job of cleaning map generation garbage
            GameView gameView = new GameView(new Rectangle(100, 100, 1500, 1500), DockType.SCREENRESOLUTION);
            mainWorldCamera = gameView.camera;
            AddContainer(gameView);
            AddContainer(new MiniViewToggle(new Rectangle(15, 340, 100, 100), mainWorldCamera));
            AddContainer(new TownPanelViewToggle(new Rectangle(15, 540, 100, 100), mainWorldCamera));
            AddContainer(new TopBar());
            InitNewGame();
        }

        private void InitNewGame()
        {
            EngineController.rnd = new Random();
            EngineController.gameRunning = true;
            EngineController.inMenu = false;

            //Second Thread - Fire thread runs at same frame time as main thread
            secondThread = new Thread(UpdateSecondThread);
            secondThread.IsBackground = true;
            secondThread.Priority = ThreadPriority.Highest;
            secondThreadRunning = true;
            secondThreadState = UpdateState.IDLE;
            secondThread.Start();

            //job Worker - Runs continously looking for pathfinding jobs
            jobWorker = new JobWorker();
            jobWorker.StartWorker();
        }
     
        private void CreateSecondThread()
        {

        }

        private void SetSecondThreadUpdateState(UpdateState updateState)
        {
            lock (lockObject)
            {
                secondThreadState = updateState;
            }
        }

        public override void Dispose()
        {
            secondThreadRunning = false;
            jobWorker.Dispose();
            base.Dispose();
        }

        private void UpdateKeys(Input input)
        {
            if (input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape) && !EngineController.inMenu)
            {
                AddContainer(new MenuBarInGame());
            }

            if (input.IsKeyMapPressed(KeyMap.IncreaseGameSpeed))
            {
                EngineController.timeMultiply += 1f;
                EngineController.timeMultiply = MathHelper.Clamp(EngineController.timeMultiply, 1, 101);
            }
            if (input.IsKeyMapPressed(KeyMap.DecreaseGameSpeed))
            {
                EngineController.timeMultiply -= 1f;
                EngineController.timeMultiply = MathHelper.Clamp(EngineController.timeMultiply, 1, 101);
            }
        }  

        private void UpdateSecondThread()
        {
            while (secondThreadRunning)
            {
                if (EngineController.gameRunning)
                {
                    if (secondThreadState == UpdateState.START)
                    {
                        SetSecondThreadUpdateState(UpdateState.FINISHED);
                    }
                }         
            }
        }

        public override void Update(Input input)
        {
            base.Update(input);
            SetSecondThreadUpdateState(UpdateState.START);
            UpdateKeys(input);
          

            if (EngineController.gameRunning)
            {
                WorldController.world.Update(input);
                WorldController.worldMini.Update();
                //do game logic here
                while (secondThreadState != UpdateState.FINISHED)
                {

                }
                //Here we can have a list for each thread we are running of things that may need to be updated that require syncing
                //for example, if a fire has claimed a tile

          //  if (input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.B))
                    if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.B))
                    {

                    jobWorker.AddJob(new Pathfinding(new Point(10,10), new Point(10,100)));

                 /*   EngineController.StartDebugTimer();
                    AStarRoadOnly a = new AStarRoadOnly();
                   // List<DrivingNode> list = a.GetDrivingNodeList(new Point(170, 203), WorldController.mouseTileHover);
                    List<DrivingNode> list = a.GetDrivingNodeList(new Point(226, 268), WorldController.mouseTileHover);
                    //  List<DrivingNode> list = a.GetDrivingNodeList(new Point(162, 202), new Point(173, 233));
                    EngineController.StopDebugTimer();*/
                }

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }

    enum UpdateState
    {
        IDLE,
        START,
        RUNNING,
        FINISHED
    }

}
