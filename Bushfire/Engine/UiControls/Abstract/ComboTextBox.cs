using BushFire.Engine.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.UIControls.Abstract
{
    class ComboTextBox : UiControl
    {
        protected Label label;
        protected TextBox textBox;
        protected Rectangle preScaleLocation;

        public ComboTextBox(string name, Rectangle location)
        {
            this.name = name;
            preScaleLocation = location;
            SetSize();
        }

        protected void SetSize()
        {
            location.X = Convert.ToInt32((float)preScaleLocation.X * DisplayController.uiScale);   // = CalcUnscaleAndScaleRectangle(location, currentUiScale, Settings.uiScale);
            location.Y = Convert.ToInt32((float)preScaleLocation.Y * DisplayController.uiScale);
            location.Width = Convert.ToInt32((float)preScaleLocation.Width * DisplayController.uiScale);
            location.Height = Convert.ToInt32((float)preScaleLocation.Height * DisplayController.uiScale);
            currentUiScale = DisplayController.uiScale;
        }

        protected override void Rescale()
        {
            SetSize();
        }

        protected Point CalcPointOffset(Point point)
        {
            return point + new Point(preScaleLocation.X, preScaleLocation.Y);
        }

        protected Vector2 CalcVectorOffset(Vector2 vector)
        {
            return vector + new Vector2(preScaleLocation.X, preScaleLocation.Y);
        }

        protected Rectangle CalcRectangleOffset(Rectangle rectangle)
        {
            return new Rectangle(preScaleLocation.X + rectangle.X, preScaleLocation.Y + rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public override string GetText()
        {
            return textBox.GetText();
        }

        public override float GetTextFloat()
        {
            return textBox.GetTextFloat();
        }

        public override int GetTextInt()
        {
            return textBox.GetTextInt();
        }

        public override void Update(Input input)
        {
            textBox.SetInViewPort(inViewport);
            textBox.Update(input);
            label.Update(input);
            base.Update(input);
        }

        public override void Draw(SpriteBatch spriteBatch, float containerFade)
        {
            base.Draw(spriteBatch, containerFade);
            textBox.Draw(spriteBatch, containerFade);
            label.Draw(spriteBatch, containerFade);
        }
    }
}
