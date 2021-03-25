using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.UIControls
{
    class ComboMenuTextBox : ComboTextBox
    {
        public ComboMenuTextBox(string name, string labelText, string startingText, Point location, bool isNumeric) : base(name, new Rectangle(location.X, location.Y, 750, 60))
        {
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetSpriteColour(3);
            transparency = 0.6f;
            label = new Label("label", Font.CarterOne20, Color.White, CalcVectorOffset(new Vector2(20, 10)), false, labelText);
            textBox = new TextBox("button", CalcRectangleOffset(new Rectangle(500, 6, 241, 48)), Font.OpenSans20, Color.Black, startingText, isNumeric, 15);
        }

     
    }
}
