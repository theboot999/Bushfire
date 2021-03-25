using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BushFire.Engine.UIControls.Abstract;
using System.Diagnostics;
using BushFire.Engine.Controllers;

namespace BushFire.Engine.UIControls
{
    class Label : UiControl
    {
        protected Vector2 preScaleLocation;
        private bool centerText;

        public Label(string name, Font font, Color fontColor, Vector2 locationText, bool centerText, string text)
        {
            this.centerText = centerText;
            drawText = true;
            drawSpriteFront = false;
            drawSpriteBack = false;
            Initialize(name, locationText, text, GraphicsManager.GetSpriteFont(font), fontColor);
        }

        protected void Initialize(string name, Vector2 locationText, string text, SpriteFont spriteFont, Color fontColor)
        {
            preScaleLocation = locationText;
            currentUiScale = DisplayController.uiScale;
            this.spriteFont = spriteFont;
            this.fontColor = fontColor;
            this.name = name;
            SetText(text);
            Rescale();
        }

        protected override void Rescale()
        {
            locationText = preScaleLocation * DisplayController.uiScale;
            currentUiScale = DisplayController.uiScale;

            if (centerText)
            {
                CenterLabel();
            }
        }

        private void CenterLabel()
        {
            Vector2 halfSize = spriteFont.MeasureString(GetText()) * 0.5f;
            locationText = (preScaleLocation - halfSize) * DisplayController.uiScale;
        }

        public override void SetText(string text)
        {
            base.SetText(text);
            if (centerText)
            {
                CenterLabel();
            }
        }

        public override void Update(Input input)
        {
            base.Update(input);
        }

        public override void Draw(SpriteBatch spriteBatch, float containerFade)
        {
            base.Draw(spriteBatch, containerFade);

        }
    }
}
