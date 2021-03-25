using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Screens.Containers
{
    class MiniViewToggle : Container
    {
        MiniView miniView;

        public MiniViewToggle(Rectangle location, Camera mainWorldCamera) : base (location, Engine.DockType.TOPLEFTFIXEDX, true)
        {
            miniView = new MiniView(new Rectangle(0, 0, 500, 500), DockType.TOPRIGHT, mainWorldCamera);

            canChangeFocusOrder = false;
            name = "MiniViewToggle";
            AddUiControl(new ButtonUIToggle("MiniMapView", GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonMinimapToggleBack), GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonMiniMapToggleFront)));
            SetMaxTransparency(0.85f);

        }

        //so pressing the x isnt triggering miniviewshow = false;

        private void ToggleMiniViewShow()
        {
            if (miniView.containerState == ContainerState.Normal)
            {
                ScreenController.RemoveContainer(miniView, true);
            }
            else
            {
                miniView.SetFadeIn();
                ScreenController.AddContainer(miniView);
            }

        }

        public override void Update(Input input)
        {
            base.Update(input);

            if (GetButtonPress("MiniMapView"))
            {
                ToggleMiniViewShow();
            }       

        }
    }
}
