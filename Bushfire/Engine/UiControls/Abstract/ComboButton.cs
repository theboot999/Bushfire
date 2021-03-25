using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Internal;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.UIControls.Abstract
{
    //probaly could use a cycle object


    abstract class ComboButton : UiControl
    {
        protected Label label;
        public CycleObject cycleObject;
        protected Button button;
        public object referenceObject;
        protected Rectangle preScaleLocation;

        public ComboButton(string name, string labelText, Rectangle location, CycleObject cycleObject, object referenceObject)
        {
            this.name = name;
            this.cycleObject = cycleObject;
            this.referenceObject = referenceObject;
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

        protected override void Rescale()
        {
            SetSize();
        }

        public void UpdatePresses()
        {
            controlClickState = ControlClickState.NONE;
            if (button.IsEitherPress())
            {
                controlClickState = ControlClickState.LEFTPRESS;
            }      
        }

        public override void SetText(string text)
        {
            button.SetText(text);
            button.CenterText();        
        }

        public override void SetTextColor(Color color)
        {
            button.SetTextColor(color);
        }

        private void UpdateObjectText(string value)
        {
            cycleObject.displayName = value;
        }

        public override void Update(Input input)
        {
            button.SetInViewPort(inViewport);
            button.Update(input);
            label.Update(input);
            base.Update(input);        
            UpdatePresses();
        }
       
        public override void Draw(SpriteBatch spriteBatch, float containerFade)
        {
            base.Draw(spriteBatch, containerFade);
            button.Draw(spriteBatch, containerFade);
            label.Draw(spriteBatch, containerFade);           
        }
    }
}
