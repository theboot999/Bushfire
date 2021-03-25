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
    class TownPanelViewToggle : Container
    {
        TownPanel townPanel;

        public TownPanelViewToggle(Rectangle location, Camera mainWorldCamera) : base(location, Engine.DockType.TOPLEFTFIXEDX, true)
        {
            townPanel = new TownPanel(new Rectangle(500, 500, 500, 600), DockType.TOPLEFT, mainWorldCamera);

            canChangeFocusOrder = false;
            name = "MiniViewToggle";
            AddUiControl(new ButtonUIToggle("TownPanelView", GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonTownToggleBack), GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonTownToggleFront)));
            SetMaxTransparency(0.95f);
        }

        private void ToggleMiniViewShow()
        {

            if (townPanel.containerState == ContainerState.Normal)
            {
                ScreenController.RemoveContainer(townPanel, true);
            }
            else
            {
                townPanel.SetFadeIn();
                ScreenController.AddContainer(townPanel);
            }

        }



        public override void Update(Input input)
        {
            base.Update(input);

            if (GetButtonPress("TownPanelView"))
            {
                ToggleMiniViewShow();
            }



        }
    }
}
