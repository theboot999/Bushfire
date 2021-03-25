using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.UIControls
{
    class ComboEditorCycleNextTo : ComboCycle
    {
        public ComboEditorCycleNextTo(string name, string labelText, Point location, bool cycleAround, bool highlightAll, bool drawVisuals) : base(name, new Rectangle(location.X, location.Y, 750, 60), cycleAround, highlightAll, drawVisuals)
        {
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetSpriteColour(3);
            transparency = 0.6f;

            Label text = new Label("label", Font.OpenSans22Bold, Color.White, CalcVectorOffset(new Vector2(30, 10)), false, labelText);
            ButtonCycleMenu down = new ButtonCycleMenu("CycleDown", CalcPointOffset(new Point(270, 5)), "<", Font.CarterOne24, Color.White);
            ButtonCycleMenu up = new ButtonCycleMenu("CycleUp", CalcPointOffset(new Point(590, 5)), ">", Font.CarterOne24, Color.White);
            Label labelObject = new Label("object", Font.OpenSans20, Color.White, CalcVectorOffset(new Vector2(450, 25)), true, "");
            Rectangle visuals = CalcRectangleOffset(new Rectangle(330, 45, 250, 8));
            Init(text, down, up, labelObject, visuals);
        }

        public override void Update(Input input)
        {
            base.Update(input);

        }
    }
}
