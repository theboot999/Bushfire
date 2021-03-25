using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Game.Controllers;
using BushFire.Menu.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Screens.Containers.InMenu
{
    class MenuBarInGame : Container
    {
        public MenuBarInGame() : base(new Rectangle(800, 100, 900, 1100), DockType.CENTERSCREENBOTH, true)
        {
            GameController.inGameState = InGameState.INMENU;
            alwaysOnTop = true;
            name = "MenuBar";
            spriteBack = GraphicsManager.GetPreBuilt(Engine.ContentStorage.PrebuiltSprite.InGameMenuBack);
            drawSpriteBack = true;
            canChangeFocusOrder = false;

            AddBorder(2, Resizing.BOTH, 21);
            AddHeading(50, "Main Menu", GraphicsManager.GetSpriteFont(Font.OpenSans20Bold), Color.White, false, false, false, false, false, GraphicsManager.GetSpriteColour(6));

            AddUiControl(new ButtonMenuLargeGrey("Controls", new Point(250, 150), "Controls", Color.White));
            AddUiControl(new ButtonMenuLargeGrey("Video", new Point(250, 250), "Video", Color.White));
            AddUiControl(new ButtonMenuLargeGrey("Audio", new Point(250, 350), "Audio", Color.White));
            AddUiControl(new ButtonMenuLargeGrey("MainMenu", new Point(250, 450), "Main Menu", Color.White));
            AddUiControl(new ButtonMenuLargeGrey("Back", new Point(250, 780), "Back", Color.White));
        }

        public override void Update(Input input)
        {
            base.Update(input);

            if (GetButtonPress("MainMenu"))
            {
                if (GameController.inGameState == InGameState.INMENU)
                {
                    GameController.inGameState = InGameState.WAITINGONEXIT;
                }


            }
            if (GetButtonPress("Back"))
            {
                ScreenController.RemoveContainer(this, true);
                GameController.inGameState = InGameState.RUNNING;
            }

            if (GetButtonPress("Video"))
            {
                ScreenController.RemoveContainer(this, true);
                ScreenController.AddContainer(new VideoInGame());
            }

            if (GetButtonPress("Audio"))
            {
                ScreenController.RemoveContainer(this, true);
                ScreenController.AddContainer(new AudioInGame());
            }

            if (GetButtonPress("Controls"))
            {
                ScreenController.RemoveContainer(this, true);
                ScreenController.AddContainer(new ControlsInGame());
            }

        }
    }
}
