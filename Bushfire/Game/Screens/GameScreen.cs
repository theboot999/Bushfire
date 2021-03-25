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
using BushFire.Game.Storage;
using BushFire.Game.Controllers;
using BushFire.Game.Map;
using BushFire.Game.MapObjects;
using BushFire.Game.Map.FireStuff;

namespace BushFire.Content.Game.Screens
{
    //So logic for intersection panel
    //If we click on an intersection
    //we first look for any inteserction panels in our list
    //if we find one that is not pinned.
    //we grab that panel
    //we remove it from the list
    //we add it to the list again(makes it on top)
    //and we update the intersection on it

    class GameScreen : Screen
    {
        Camera mainWorldCamera;
        Thread secondThread;

        Stopwatch debugMainThreadStopWatch;
        Stopwatch debugSecondThreadStopwatch;
        //Threading
        private volatile UpdateState secondThreadState;
        private volatile bool secondThreadRunning;
        private static readonly object lockObject = new object();

        public GameScreen()
        {       
            GameView gameView = new GameView(new Rectangle(100, 100, 1500, 1500), DockType.SCREENRESOLUTION);
            mainWorldCamera = gameView.camera;
            AddContainer(gameView);
            AddContainer(new MiniViewToggle(new Rectangle(15, 340, 110, 110), mainWorldCamera));
            AddContainer(new TownPanelViewToggle(new Rectangle(15, 490, 110, 110), mainWorldCamera));
            AddContainer(new TopBar());
            AddContainer(new HelpKeyPressInfo());
            GameController.rnd = new Random();
            GameController.inGameState = InGameState.RUNNING;
            debugMainThreadStopWatch = new Stopwatch();
            debugSecondThreadStopwatch = new Stopwatch();
            SetThreading();


        }

        private void SetThreading()
        {
            //Second Thread - Fire thread runs at same frame time as main thread
            secondThread = new Thread(UpdateSecondThread);
            secondThread.IsBackground = true;
            secondThread.Priority = ThreadPriority.Highest;
            secondThreadRunning = true;
            secondThreadState = UpdateState.IDLE;
            secondThread.Start();

            //job Worker - Runs continously looking for pathfinding jobs
            GameController.jobWorker = new JobWorker();
            GameController.jobWorker.StartWorker();
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
            base.Dispose();
        }

        private void UpdateKeys(Input input)
        {
            if (input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape) && GameController.inGameState != InGameState.INMENU)
            {
                AddContainer(new MenuBarInGame());
            }

            if (input.IsKeyMapPressed(KeyMap.IncreaseGameSpeed))
            {
                if (input.IsKeyMapDown(KeyMap.FastMovingCamera))
                {
                    EngineController.timeMultiply += 10f;

                }
                else
                {
                    EngineController.timeMultiply += 0.1f;

                }
                EngineController.timeMultiply = MathHelper.Clamp(EngineController.timeMultiply, 0, 101);

            }
            if (input.IsKeyMapPressed(KeyMap.DecreaseGameSpeed))
            {
                if (input.IsKeyMapDown(KeyMap.FastMovingCamera))
                {
                    EngineController.timeMultiply -= 10f;
                }
                else
                {
                    EngineController.timeMultiply -= 0.1f;
                }
                    
                EngineController.timeMultiply = MathHelper.Clamp(EngineController.timeMultiply, 0, 101);
            }
            if (input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.H))
            {
                WorldController.AddDebugMessage(WorldController.mouseTileHover);
            }
        }
        int debugx;

        private void UpdateInGameMouseClicks()
        {
            //This is for single selection clicks.  Vehicle Selections are done through world Vehicles
            //For vehicle popups we may have to modify something slighty
            if (WorldController.world.mouseDragUpResult == MouseDragUpResult.CLICKEDINTERSECTION)
            {
                IntersectionPanel intersectionPanel = (IntersectionPanel)GetNonPinnedContainer("IntersectionPanel");
                Intersection intersection = WorldController.world.tileGrid[WorldController.mouseTileHover.X, WorldController.mouseTileHover.Y].GetIntersection();
                if (intersection != null)
                {
                    if (intersectionPanel == null)
                    {
                        debugx += 50;
                        AddContainer(new IntersectionPanel(new Rectangle(debugx, 500, 500, 600), DockType.BOTTOMRIGHT, intersection));
                    }
                    else
                    {
                        MoveContainerToTop(intersectionPanel);
                        intersectionPanel.ChangeIntersection(intersection);
                    }
                }            
            }
            else if (WorldController.world.mouseDragUpResult == MouseDragUpResult.CLICKEDONEVEHICLE)
            {
                VehiclePanel vehiclePanel = (VehiclePanel)GetNonPinnedContainer("VehiclePanel");
                Vehicle vehicle = WorldController.world.worldVehicles.GetFirstSelectedVehicle();

                if (vehicle != null)
                {
                    if (vehiclePanel == null)
                    {
                        debugx += 50;
                        AddContainer(new VehiclePanel(new Rectangle(debugx, 500, 500, 600), DockType.BOTTOMRIGHT, vehicle));
                    }
                    else
                    {
                        MoveContainerToTop(vehiclePanel);
                        vehiclePanel.ChangeVehicle(vehicle);
                    }
                }
            }       
        }

        private void UpdateSecondThread()
        {
            while (secondThreadRunning)
            {
                if (GameController.inGameState == InGameState.RUNNING)
                {
                    if (secondThreadState == UpdateState.START)
                    {
                        debugSecondThreadStopwatch.Start();
                        WorldController.worldFire.Update();

                        SetSecondThreadUpdateState(UpdateState.FINISHED);
                        debugSecondThreadStopwatch.Stop();
                        EngineController.secondThreadTime = debugSecondThreadStopwatch.ElapsedTicks;
                        debugSecondThreadStopwatch.Reset();
                    }

                }
            }
        }



        public override void Update(Input input)
        {
            debugMainThreadStopWatch.Start();

            base.Update(input);
            SetSecondThreadUpdateState(UpdateState.START);

            UpdateKeys(input);
         
            if (GameController.inGameState == InGameState.RUNNING)
            {
                //Run main Update
                WorldController.world.Update(input, mainWorldCamera);
                WorldController.worldMini.Update();
                UpdateInGameMouseClicks();

                debugMainThreadStopWatch.Stop();
                while (secondThreadState != UpdateState.FINISHED)
                {
                    //Wait for other thread
                }
                //Other thread is finished we can add any fires on the main thread
                WorldController.worldFire.UpdateThreadedFireSpreading();
            }
            else if (GameController.inGameState == InGameState.WAITINGONEXIT)
            {
                secondThreadRunning = false;
                GameController.jobWorker.SetToDestroy();
                GameController.inGameState = InGameState.SETTINGEXIT;
            }
            else if (GameController.inGameState == InGameState.SETTINGEXIT)
            {
                if (GameController.jobWorker.IsDestroyed())
                {
                    GameController.jobWorker = null;
                    GameController.inGameState = InGameState.EXIT;
                }
            }
            else if (GameController.inGameState == InGameState.EXIT)
            {
                WorldController.DisposeWorld();
                ScreenController.ChangeScreen(new MenuMain());
            }


            EngineController.mainThreadTime = debugMainThreadStopWatch.ElapsedTicks;
            debugMainThreadStopWatch.Reset();
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
