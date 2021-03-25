using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls.Internal;
using BushFire.Game.Map;
using BushFire.Game.MapObjects;
using BushFire.Game.Storage;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Screens.Containers
{
    class IntersectionPanel : Container
    {
        Intersection intersection;
        GameViewBox gameViewBox;

        public IntersectionPanel(Rectangle location, DockType dockType, Intersection intersection) : base(location, dockType, true)
        {
            this.intersection = intersection;
            name = "IntersectionPanel";

            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetPreBuilt(Engine.ContentStorage.PrebuiltSprite.InGameMenuBack);
            transparency = 1f;
            AddBorder(2, Resizing.NONE, 40);
            AddHeading(40, intersection.GetIdString(), GraphicsManager.GetSpriteFont(Font.CarterOne16), Color.White, true, true, false, false, true, GraphicsManager.GetPreBuilt(Engine.ContentStorage.PrebuiltSprite.InGameHeadingBar));
            AddControls();
            SetMaxTransparency(0.95f);

        }

        public void ChangeIntersection(Intersection intersection)
        {
            this.intersection = intersection;
            ModifyHeadingText(intersection.GetIdString());
            gameViewBox.SetFixedCameraPos(new Vector2(intersection.tileX * GroundLayerController.tileSize - 72f, intersection.tileY * GroundLayerController.tileSize - 72f));
        }

        public void AddControls()
        {
            gameViewBox = new GameViewBox("GameViewBox", new Rectangle(100, 100, 300, 300), containerCamera, 0.75f, true, 2, 40);
            gameViewBox.SetFixedCameraPos(new Vector2(intersection.tileX * GroundLayerController.tileSize - 72f, intersection.tileY * GroundLayerController.tileSize - 72f));
            AddUiControl(gameViewBox);

            AddUiControl(new Label("Up", Font.OpenSans38, Color.Yellow, new Vector2(278, 139), true, ""));
            AddUiControl(new Label("Right", Font.OpenSans38, Color.Yellow, new Vector2(362, 278), true, ""));
            AddUiControl(new Label("Down", Font.OpenSans38, Color.Yellow, new Vector2(225, 360), true, ""));
            AddUiControl(new Label("Left", Font.OpenSans38, Color.Yellow, new Vector2(140, 219), true, ""));

            //This needs to be added last

        }

        private void UpdateDirectionCounts()
        {
            if (intersection.stopLightList.ContainsKey(Direction.UP))
            {
                SetControlText("Up", intersection.stopLightList[Direction.UP].vehicleCounter.ToString());
            }
            if (intersection.stopLightList.ContainsKey(Direction.RIGHT))
            {
                SetControlText("Right", intersection.stopLightList[Direction.RIGHT].vehicleCounter.ToString());
            }
            if (intersection.stopLightList.ContainsKey(Direction.DOWN))
            {
                SetControlText("Down", intersection.stopLightList[Direction.DOWN].vehicleCounter.ToString());
            }
            if (intersection.stopLightList.ContainsKey(Direction.LEFT))
            {
                SetControlText("Left", intersection.stopLightList[Direction.LEFT].vehicleCounter.ToString());
            }
        }

        private void UpdateDrawPoints()
        {
            Point topLeftDraw = new Point(intersection.tileX - 1, intersection.tileY - 1);
            Point botRightDraw = new Point(intersection.tileX + 3, intersection.tileY + 3);
            gameViewBox.UpdateDrawPoints(topLeftDraw, botRightDraw);
        }

        public override void Update(Input input)
        {
            base.Update(input);
            UpdateDrawPoints();
            UpdateDirectionCounts();
        }
    }
}
