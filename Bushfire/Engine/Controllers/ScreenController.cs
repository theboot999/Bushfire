using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.Controllers
{
    static class ScreenController
    {
        public static Screen activeScreen { get; private set; }
        public static GraphicsDevice graphicsDevice;        //Used for changing the viewport
        public static Rectangle gameWindow;                 //Used for getting the current screen resolution and the viewport

        public static void ChangeScreen(Screen screen)
        {
            if (activeScreen != null)
            {
                activeScreen.Dispose();
            }
            activeScreen = screen;
        }

        public static void ResolutionChange()
        {
            activeScreen.ResolutionChange();
        }

        public static void SetViewport(Viewport viewport)
        {
            graphicsDevice.Viewport = viewport;
        }

        public static void AddContainer(Container container)
        {
            activeScreen.AddContainer(container);
        }

        public static void RemoveContainer(Container container, bool fade)
        {           
            activeScreen.RemoveContainer(container, fade);
        }

        public static void AddMessage(string text, Color color)
        {
            activeScreen.AddMessage(text, color);
        }
    }
}
