using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.UIControls
{
    class ButtonUIToggle : Button
    {
        public ButtonUIToggle(string name, Sprite spriteBack, Sprite spriteFront) : base(name, Point.Zero, "", false, Color.White, Font.Anita10, spriteBack, spriteFront)
        {

        }
    }
}
