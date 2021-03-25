using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Screens.Containers
{
    class TopBar : Container
    {
        public TopBar() : base (new Rectangle(0,0,1100,54), Engine.DockType.CENTERSCREENX, true)
        {
            canChangeFocusOrder = false;
            name = "TopBar";
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetPreBuilt(PrebuiltSprite.TopBar);
            AddUiControl(new Label("Day", Font.CarterOne16, Color.White, new Vector2(130, 10), false, ""));
            AddUiControl(new Label("Time", Font.CarterOne16, Color.White, new Vector2(320, 10), false, ""));
            AddUiControl(new Label("Temp", Font.CarterOne16, Color.White, new Vector2(550, 10), false, ""));
            AddUiControl(new Label("GSpeed", Font.CarterOne16, Color.White, new Vector2(800, 10), false, ""));
        }

        public override void Update(Input input)
        {
            base.Update(input);
            SetControlText("Day", "Day: " + WorldController.day.ToString());
            SetControlText("Time", "Time: " + WorldController.timeString);
            SetControlText("Temp", "26 Degrees");
            SetControlText("GSpeed", "Game Speed: " + EngineController.timeMultiply);
        }
    }
}
