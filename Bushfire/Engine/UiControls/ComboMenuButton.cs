using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls.Internal;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.UIControls
{
    class ComboMenuButton : ComboButton
    {
        public ComboMenuButton(string name, string labelText, CycleObject cycleObject, Point location, object referenceObject) : base (name, labelText, new Rectangle(location.X, location.Y, 750, 60), cycleObject, referenceObject)
        {
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetSpriteColour(3);
            transparency = 0.6f;
            label = new Label("label", Font.CarterOne20, Color.White, CalcVectorOffset(new Vector2(20, 10)), false, labelText);
            button = new ButtonBlueLarge("button", CalcPointOffset(new Point(500, 5)), cycleObject.displayName, Color.White);
        }
    }
}
