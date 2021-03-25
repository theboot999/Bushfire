using BushFire.Engine;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls;
using Microsoft.Xna.Framework;
using BushFire.Engine.Controllers;

namespace BushFire.Menu.Containers
{
    class MenuBar : Container
    {
        public MenuBar(Rectangle localLocation) : base(localLocation, DockType.TOPLEFTFIXEDY, true)
        {
            name = "MenuBar";
            spriteBack = GraphicsManager.GetSpriteColour(0);
            canChangeFocusOrder = false;
            AddUiControl(new ButtonMenuLargeGrey("NewGame", new Point(50, 80), "New Game", Color.White));
            AddUiControl(new ButtonMenuLargeGrey("Controls", new Point(50, 180), "Controls", Color.White));
            AddUiControl(new ButtonMenuLargeGrey("Video", new Point(50, 280), "Video", Color.White));
            AddUiControl(new ButtonMenuLargeGrey("Audio", new Point(50, 380), "Audio", Color.White));           
            AddUiControl(new ButtonMenuLargeGrey("Editor", new Point(50, 480), "Building Editor", Color.White));
            AddUiControl(new ButtonMenuLargeGrey("Quit", new Point(50, 580), "Quit", Color.White));
        }


        public override void Update(Input input)
        {
            base.Update(input);
        }
    }
}
