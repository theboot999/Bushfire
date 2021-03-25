using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Menu.Containers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Screens.Containers.InMenu
{
    class ControlsInGame : Controls
    {
        public ControlsInGame() : base(new Rectangle(800, 100, 900, 1100), DockType.CENTERSCREENBOTH)
        {
            spriteBack = GraphicsManager.GetPreBuilt(Engine.ContentStorage.PrebuiltSprite.InGameMenuBack);
            AddBorder(2, Resizing.NONE, 20);
            AddUiControl(new ButtonBlueLarge("Back", new Point(150, 660), "Back", Color.White));
        }

        public override void Update(Input input)
        {
            base.Update(input);

            if (GetButtonPress("Back"))
            {
                ScreenController.RemoveContainer(this, true);
                ScreenController.AddContainer(new MenuBarInGame());
            }
        }
    }
}
