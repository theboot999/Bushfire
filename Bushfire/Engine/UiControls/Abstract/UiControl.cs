using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BushFire.Engine;
using System;
using BushFire.Engine.Controllers;

namespace BushFire.Engine.UIControls.Abstract
{

    abstract class UiControl
    {
        public string name { get; protected set; }

        //Interact
        protected ControlClickState controlClickState { get; set; } = ControlClickState.NONE;
        public bool changed { get; protected set; }
        public bool enabled { get; set; } = true;
        public bool selected { get; set; }
        protected bool inViewport { get; set; }
        public bool retranslateDraw { get; protected set; }

        //Location
        protected Rectangle location;
        protected Vector2 locationText { get; set; }
        protected float currentUiScale { get; set; }

        //Text
        public SpriteFont spriteFont { get; set; }
        public Color fontColor { get; set; }
        private string text { get; set; }
        protected string textFlash { get; set; }
        protected Sprite spriteBack;
        protected Sprite spriteFront;

        protected bool drawSpriteBack { get; set; }
        protected bool drawSpriteFront { get; set; }
        protected bool drawText { get; set; }
        protected float activeTextureFade { get; set; } = 0f;
        public float transparency { get; set; } = 1f;
        public bool destroy { get; protected set; }

        protected UiControl()
        {
            currentUiScale = DisplayController.uiScale;
        }

        protected virtual void Rescale()
        {

        }

        protected Rectangle CalcScaleRectangle(Rectangle rectangle, float scale)
        {
            int x = Convert.ToInt32((float)rectangle.X * scale);
            int y = Convert.ToInt32((float)rectangle.Y * scale);
            int width = Convert.ToInt32((float)rectangle.Width * scale);
            int height = Convert.ToInt32((float)rectangle.Height * scale);
            return new Rectangle(x, y, width, height);
        }

        protected void MoveLocation(Point locationPoint)
        {
            location.X = locationPoint.X;
            location.Y = locationPoint.Y;
        }

        public virtual void AddSound(SoundType soundType, bool onHover, bool onPress)
        {

        }

        protected ControlClickState GetClickState()
        {
            return controlClickState;
        }

        public bool IsLeftPress()
        {
            return controlClickState == ControlClickState.LEFTPRESS;         
        }

        public bool IsRightPress()
        {
            return controlClickState == ControlClickState.RIGHTPRESS;

        }

        public void SetInViewPort(bool value)
        {
            inViewport = value;
        }

        public bool IsEitherPress()
        {
            return controlClickState == ControlClickState.LEFTPRESS || controlClickState == ControlClickState.RIGHTPRESS;
        }

        public virtual string GetText()
        {
            return text;
        }

        public virtual float GetTextFloat()
        {
            float f;

            if (float.TryParse(text, out f))
            {
                return f;
            }
            return 0f;
        }

        public virtual int GetTextInt()
        {
            float f;

            if (float.TryParse(text, out f))
            {
                return (int)f;
            }
            return 0;
        }

        public virtual void SetText(string text)
        {
            this.text = text;
            changed = true;
        }

        public virtual void SetTextColor(Color color)
        {
            fontColor = color;
            changed = true;
        }

        public void CenterText()
        {
            Vector2 p = spriteFont.MeasureString(text) * DisplayController.uiScale;
            float halfWidth = p.X / 2;
            float halfHeight = p.Y / 2;
            float centerPointX = (float)location.Left + ((float)location.Width * 0.5f);
            float centerPointY = (float)location.Top + ((float)location.Height * 0.5f);
            locationText = new Vector2(centerPointX - halfWidth, centerPointY - halfHeight);
        }

        public void LeftAlignText(float margin, bool percent)
        {
            if (percent)
            {
                float marginAmount = location.Width * margin;
                Vector2 p = spriteFont.MeasureString(text) * DisplayController.uiScale;
                float halfHeight = p.Y / 2;
                float centerPointY = (float)location.Top + ((float)location.Height * 0.5f);
                locationText = new Vector2((float)location.Left + marginAmount, centerPointY - halfHeight);
            }
            else
            {
                Vector2 p = spriteFont.MeasureString(text) * DisplayController.uiScale;
                float halfHeight = p.Y / 2;
                float centerPointY = (float)location.Top + ((float)location.Height * 0.5f);
                float left = margin * DisplayController.uiScale;
                locationText = new Vector2((float)location.Left + left, centerPointY - halfHeight);
            }
        }

        public void SetSpriteBack(Sprite sprite)
        {
            spriteBack = sprite;
            Rescale();
        }


        protected int GetIntByScale(int value)
        {
            return Convert.ToInt32((float)value * DisplayController.uiScale);
        }

        protected float GetFloatByScale(float value)
        {
            return value * DisplayController.uiScale;
        }

        #region CALLS

        public virtual void Update(Input input)
        {
            if (currentUiScale != DisplayController.uiScale)
            {
                Rescale();
            }
            changed = false;
        }

        public virtual void Draw(SpriteBatch spriteBatch, float containerFade)
        {
            if (drawSpriteBack)
            {
                spriteBatch.Draw(spriteBack.texture2D, location, spriteBack.location, spriteBack.color * transparency * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
            if (!selected && drawSpriteFront)
            {
                spriteBatch.Draw(spriteFront.texture2D, location, spriteFront.location, spriteFront.color * activeTextureFade * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
            if (selected && drawSpriteFront)
            {
                spriteBatch.Draw(spriteFront.texture2D, location, spriteFront.location, spriteFront.altColor * activeTextureFade * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
            if (drawText)
            {
                spriteBatch.DrawString(spriteFont, text + textFlash, locationText, fontColor * transparency * containerFade, 0f, Vector2.One, DisplayController.uiScale, SpriteEffects.None, 0f);
            }
        }

        #endregion
    }
}
