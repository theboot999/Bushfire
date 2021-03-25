using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BushFire.Engine.Controllers;

namespace BushFire.Engine.UIControls
{
    class Box : UiControl
    {
        private int borderThickness;
        private Rectangle left;
        private Rectangle top;
        private Rectangle right;
        private Rectangle bottom;     
        private Rectangle originalLocation;
        private Sprite spriteBorder;
        private bool withBorder;
        private bool withBack;

        public Box(string name, Rectangle location, int borderThickness, int backColour, int borderColour, bool withBorder, bool withBack)
        {
            this.name = name;
            this.withBorder = withBorder;
            this.withBack = withBack;
            drawSpriteBack = withBack;
            spriteBack = GraphicsManager.GetSpriteColour(backColour);
            spriteBorder = GraphicsManager.GetSpriteColour(borderColour);
            this.borderThickness = borderThickness;
            originalLocation = location;
            SetSize();
        }

        public void ChangeLocation(Rectangle location)
        {
            originalLocation = location;
            Rescale();
        }

        private void SetSize()
        {
            if (withBorder)
            {
                left = new Rectangle(GetIntByScale(originalLocation.X), GetIntByScale(originalLocation.Y) + borderThickness, borderThickness, GetIntByScale(originalLocation.Height) - borderThickness - borderThickness);
                top = new Rectangle(GetIntByScale(originalLocation.X), GetIntByScale(originalLocation.Y), GetIntByScale(originalLocation.Width), borderThickness);
                right = new Rectangle(GetIntByScale(originalLocation.X) + GetIntByScale(originalLocation.Width) - borderThickness, GetIntByScale(originalLocation.Y) + borderThickness, borderThickness, GetIntByScale(originalLocation.Height) - borderThickness - borderThickness);
                bottom = new Rectangle(GetIntByScale(originalLocation.X), GetIntByScale(originalLocation.Y) + GetIntByScale(originalLocation.Height) - borderThickness, GetIntByScale(originalLocation.Width), borderThickness);
            }

            location = CalcScaleRectangle(new Rectangle(originalLocation.X + borderThickness, originalLocation.Y + borderThickness, originalLocation.Width - borderThickness - borderThickness, originalLocation.Height - borderThickness - borderThickness), currentUiScale);

        }

        protected override void Rescale()
        {
            currentUiScale = DisplayController.uiScale;
            SetSize();
        }

        public override void Draw(SpriteBatch spriteBatch, float containerFade)
        {
            base.Draw(spriteBatch, containerFade);

            if (withBorder)
            {
                spriteBatch.Draw(spriteBorder.texture2D, left, spriteBorder.location, spriteBack.color * transparency * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                spriteBatch.Draw(spriteBorder.texture2D, top, spriteBorder.location, spriteBack.color * transparency * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                spriteBatch.Draw(spriteBorder.texture2D, right, spriteBorder.location, spriteBack.color * transparency * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                spriteBatch.Draw(spriteBorder.texture2D, bottom, spriteBorder.location, spriteBack.color * transparency * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
        }
    }
}
