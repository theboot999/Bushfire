using BushFire.Engine.Files;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.Controllers
{
    static class DisplayController
    {
        public static float uiScale;
        
        public static GraphicsDeviceManager graphics;
        private static GameBushFire game;
        public static SpriteBatch spriteBatch;
        public static SpriteBatch spriteBatch1;
        public static List<Rectangle> resolutionList;
        public static int resolutionId;
        private static int fullscreenResolutionId;
        public static bool fullScreen;
        public static int targetFrameRate;
        public static float messageSpeed;

        private static Dictionary<int, RenderTarget2D> renderTargetList;
        private static int currentTargetId = 0;

        public static float minNightBrightness;
        public static bool showDebugWindowOne = true;
        public static bool showDebugWindowTwo = true;

        //   private bool debugDisplay = true;
        //  private Rectangle debugRes = new Rectangle(0, 0, 1920, 1080);
        //when we switch to full screen. we need to set the fullscreen res


        public static void Init(GameBushFire game)
        {
            renderTargetList = new Dictionary<int, RenderTarget2D>();
            DisplayController.game = game;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            CreateResolutionList();
            CalcFullScreenResId();
            CalcInitialRes();

            fullScreen = Data.settingsXML.fullScreen;
            targetFrameRate = Data.settingsXML.targetFrameRate;
            uiScale = Data.settingsXML.uiScale;
            messageSpeed = Data.settingsXML.messageSpeed;
            minNightBrightness = Data.settingsXML.minNightBrightness;

            spriteBatch = new SpriteBatch(ScreenController.graphicsDevice);
            spriteBatch1 = new SpriteBatch(ScreenController.graphicsDevice);

            UpdateDisplay(false);
        }

        private static void CalcFullScreenResId()
        {
            graphics.HardwareModeSwitch = false;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            graphics.HardwareModeSwitch = true;
            fullscreenResolutionId = GetResolutionId(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        }

        private static void CalcInitialRes()
        {
            if (Data.settingsXML.resolutionWidth == 0 || Data.settingsXML.resolutionHeight == 0)
            {
                resolutionId = fullscreenResolutionId;
            }
            else
            {
                resolutionId = GetResolutionId(Data.settingsXML.resolutionWidth, Data.settingsXML.resolutionHeight);
            }
        }

        public static int GetResolutionId(int resolutionWidth, int resolutionHeight)
        {
            for (int i = 0; i < resolutionList.Count; i++)
            {
                if (resolutionList[i].Width == resolutionWidth && resolutionList[i].Height == resolutionHeight)
                {
                    return i;
                }
            }

            resolutionList.Add(new Rectangle(0, 0, resolutionWidth, resolutionHeight));
            return  resolutionList.Count - 1;
        }

        private static void CreateResolutionList()
        {
            resolutionList = new List<Rectangle>();

            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (mode.Width > 1279)
                {
                    resolutionList.Add(new Rectangle(0, 0, mode.Width, mode.Height));
                }
            }
        }     

        public static void UpdateDisplay(bool recalculateUiScale)
        {
            if (fullScreen)
            {
                resolutionId = fullscreenResolutionId;
            }

            graphics.HardwareModeSwitch = false;
            
            if (targetFrameRate == -1)
            {
                //Unlimited FPS
                game.IsFixedTimeStep = false;
                graphics.SynchronizeWithVerticalRetrace = false;
            }
            else if(targetFrameRate == 0)
            {
                //Vertical Sync
                game.IsFixedTimeStep = false;
                graphics.SynchronizeWithVerticalRetrace = true;
            }
            else
            {
                game.IsFixedTimeStep = true;
                graphics.SynchronizeWithVerticalRetrace = false;
                game.TargetElapsedTime = TimeSpan.FromTicks((long)(TimeSpan.TicksPerSecond / targetFrameRate));
            }

            graphics.IsFullScreen = fullScreen;
            graphics.PreferredBackBufferWidth = resolutionList[resolutionId].Width;
            graphics.PreferredBackBufferHeight = resolutionList[resolutionId].Height;           
            graphics.ApplyChanges();
            graphics.HardwareModeSwitch = true;

            ScreenController.gameWindow = new Rectangle(0, 0, resolutionList[resolutionId].Width, resolutionList[resolutionId].Height);

            if (recalculateUiScale)
            {
                uiScale = (float)resolutionList[resolutionId].Width / 2560f;
            }
        }

        public static int AddRenderTarget(int targetWidth, int targetHeight, bool preserveContents)
        {
            int currentId = currentTargetId;
            if (preserveContents)
            {
                renderTargetList.Add(currentTargetId, new RenderTarget2D(ScreenController.graphicsDevice, targetWidth, targetHeight, false, SurfaceFormat.Bgr565, DepthFormat.None, 0, RenderTargetUsage.PreserveContents));
                
            }
            else
            {
                renderTargetList.Add(currentTargetId, new RenderTarget2D(ScreenController.graphicsDevice, targetWidth, targetHeight));
            }
            currentTargetId++;
            return currentId;
        }

        public static void DisposeRenderTarget(int id)
        {
            if (renderTargetList.ContainsKey(id))
            {
                renderTargetList[id].Dispose();
                renderTargetList.Remove(id);
            }
        }

        public static RenderTarget2D GetRenderTarget(int id)
        {
            return renderTargetList[id];
        }

        public static void Dispose()
        {
            spriteBatch.Dispose();
            spriteBatch1.Dispose();
        }
    }
}
