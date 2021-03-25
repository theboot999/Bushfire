using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Game.Controllers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Screens.Containers
{
    class HelpKeyPressInfo : Container
    {
        HelpInformation helpInformation;

        public HelpKeyPressInfo() : base(new Rectangle(0, 50, 500, 54), Engine.DockType.CENTERSCREENX, true)
        {
            canChangeFocusOrder = false;
            ignoreInput = true;
            name = "HelpKeyPressInfo";
            spriteBack = GraphicsManager.GetPreBuilt(PrebuiltSprite.TopBar);
            AddUiControl(new Label("Help", Font.OpenSans20, Color.White, new Vector2(250, 18), true, GetHelpString()));
            helpInformation = new HelpInformation();
        }

        private string GetHelpString()
        {
            return "Press " + EngineController.keyMapList[KeyMap.ShowHelp].ToString() + " For Help";
        }

        private void ToggleHelpInformation()
        {
            if (helpInformation.containerState == ContainerState.Normal)
            {
                ScreenController.RemoveContainer(helpInformation, true);
            }
            else
            {
                helpInformation.SetFadeIn();
                ScreenController.AddContainer(helpInformation);
            }

        }

        public override void Update(Input input)
        {
            base.Update(input);

            if (GameController.inGameState == InGameState.INMENU)
            {
                //We only update the text in the menu because we will only change the keymapping then
                SetControlText("Help", GetHelpString());
            }

            if (input.IsKeyMapPressed(KeyMap.ShowHelp))
            {
                ToggleHelpInformation();
            }
        }


    }
}
