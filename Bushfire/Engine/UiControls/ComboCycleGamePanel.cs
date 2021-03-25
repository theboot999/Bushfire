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
    class ComboCycleGamePanel : ComboCycle
    {
            public ComboCycleGamePanel(string name, string labelText, Point location, bool cycleAround, bool highlightAll, bool drawVisuals) : base(name, new Rectangle(location.X, location.Y, 475, 52), cycleAround, highlightAll, drawVisuals)
            {
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetSpriteColour(3);
            transparency = 0.6f;

            Label text = new Label("", Font.OpenSans22Bold, Color.White, CalcVectorOffset(new Vector2(250, 10)), true, labelText);
            ButtonCycleGame down = new ButtonCycleGame("CycleDown", CalcPointOffset(new Point(10, 8)), "<", Font.CarterOne24, Color.White);
            ButtonCycleGame up = new ButtonCycleGame("CycleUp", CalcPointOffset(new Point(425, 8)), ">", Font.CarterOne24, Color.White);
            Label labelObject = new Label("object", Font.OpenSans18Bold, Color.White, CalcVectorOffset(new Vector2(250, 20)), true, "");
            Rectangle visuals = CalcRectangleOffset(new Rectangle(58, 37, 357, 6));
            Init(text, down, up, labelObject, visuals);
        }

            public override void Update(Input input)
            {
                base.Update(input);

            }

    }
}
