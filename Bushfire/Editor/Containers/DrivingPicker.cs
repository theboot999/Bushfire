using BushFire.Editor.Tech;
using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Editor.Containers
{
    class DrivingPicker : Container
    {
        EditorParams editorParams;
        CompressedBuilding compressedBuilding;

        public DrivingPicker(Rectangle location, DockType dockType, CompressedBuilding compressedBuilding, EditorParams editorParams) : base(location, dockType, true)
        {
            this.compressedBuilding = compressedBuilding;
            this.editorParams = editorParams;
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetPreBuilt(Engine.ContentStorage.PrebuiltSprite.EditorPanelBackGrey);
            AddBorder(3, Resizing.NONE, 1);
            AddHeading(40, "Driving Picker", GraphicsManager.GetSpriteFont(Font.OpenSans18), Color.White, false, false, false, false, true, GraphicsManager.GetSpriteColour(6));

     

        }
    }
}
