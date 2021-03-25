using BushFire.Engine;
using BushFire.Engine.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Tech
{
    class MapLabel
    {
        public SpriteFont spriteFont { get; set; }
        public Color fontColor { get; set; }
        private float transparency { get; set; }
        private float textScale { get; set; }
        private Vector2 locationText;
        private Vector2 scaleLocationText;
        private string text { get; set; }

        public MapLabel(string text, Vector2 locationText, Font font, Color fontColor, float transparency, float textScale)
        {
            this.text = text;
            this.locationText = locationText;
            spriteFont = GraphicsManager.GetSpriteFont(font);
            this.fontColor = fontColor;
            this.transparency = transparency;
            SetTextScale(textScale);
        }

        public void SetTextScale(float textScale)
        {
            if (this.textScale != textScale)
            {
                this.textScale = textScale;
                Vector2 p = spriteFont.MeasureString(text) * this.textScale;
                scaleLocationText = locationText - (p * 0.5f);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, float containerFade)
        {
                spriteBatch.DrawString(spriteFont, text, scaleLocationText, fontColor * transparency * containerFade, 0f, Vector2.One, textScale, SpriteEffects.None, 0f);
         }
    }
}
