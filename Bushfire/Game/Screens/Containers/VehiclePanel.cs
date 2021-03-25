using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls.Internal;
using BushFire.Game.Map;
using BushFire.Game.MapObjects;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;

namespace BushFire.Game.Screens.Containers
{
    //Need to make sure
    //If our vehicle dies
    //We remove it from here
    //Also things to add
    //Vehicle Paramters to display
    //An image of it?
    //A button to zoom to it
    // A button to lock camera to it
    // However if we change our camera position via clicking mini map or pressing a key we need to make sure we unlock the camera zoom from it

    class VehiclePanel : Container
    {
        Vehicle vehicle;
        GameViewBox gameViewBox;

        public VehiclePanel(Rectangle location, DockType dockType, Vehicle vehicle) : base(location, dockType, true)
        {
            this.vehicle = vehicle;
            name = "VehiclePanel";
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetPreBuilt(Engine.ContentStorage.PrebuiltSprite.InGameMenuBack);
            transparency = 1f;
            AddBorder(2, Resizing.NONE, 40);
            AddHeading(40, vehicle.GetIdString(), GraphicsManager.GetSpriteFont(Font.CarterOne16), Color.White, true, true, false, false, true, GraphicsManager.GetPreBuilt(Engine.ContentStorage.PrebuiltSprite.InGameHeadingBar));
            AddControls();
            SetMaxTransparency(0.95f);
        }

        public void ChangeVehicle(Vehicle vehicle)
        {
            this.vehicle = vehicle;
            ModifyHeadingText(vehicle.GetIdString());
        }

        public void AddControls()
        {
            gameViewBox = new GameViewBox("GameViewBox", new Rectangle(50, 320, 400, 200), containerCamera, 0.6f, true, 2, 40);
            gameViewBox.SetFixedCameraPos(vehicle.GetPosition());
            AddUiControl(gameViewBox);


        }

        Vector2 cameraOffset = new Vector2(326, 166);

        private void UpdateDrawPoints()
        {
            Point position = vehicle.GetTilePosition();
            Point topLeftDraw = new Point(position.X - 4, position.Y - 3);
            Point botRightDraw = new Point(position.X + 5, position.Y + 3);
            gameViewBox.UpdateDrawPoints(topLeftDraw, botRightDraw, vehicle.GetPosition() - cameraOffset);
        }

        public override void Update(Input input)
        {
            base.Update(input);
            UpdateDrawPoints();
        }
    }
}
