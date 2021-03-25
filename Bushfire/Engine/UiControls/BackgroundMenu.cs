using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BushFire.Engine.Controllers;

namespace BushFire.Engine.UIControls.Abstract
{
    class BackgroundMenu : Container
    {
        public BackgroundMenu(Rectangle localLocation, DockType scaleType, TextureSheet textureSheet) :base (localLocation, scaleType, true)
        {
            canChangeFocusOrder = false;
            drawSpriteBack = true;
            spriteBack = new Sprite(GraphicsManager.GetTextureSheetSize(textureSheet), textureSheet);
        }


        public override void Update(Input input)
        {

            base.Update(input);
          
        }
    }
}
