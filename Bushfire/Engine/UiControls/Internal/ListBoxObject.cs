using BushFire.Engine.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.UIControls.Internal
{
    class ListBoxObject
    {
        public object value;
        public string displayName;
        public Vector2 textLocation;
        public Rectangle location;
        public bool selected;
        public Sprite selectedSprite;
        public Color fontColor;
        public int index;

        public ListBoxObject(string displayName, object value, Color fontColor)
        {
            selectedSprite = GraphicsManager.GetSpriteColour(14);
            this.displayName = displayName;
            this.value = value;
            this.fontColor = fontColor;
        }
        
        public void SetLocation(Rectangle location, int fontHeight, int index)
        {
            this.index = index;
            this.location = location;
            int difference = (location.Height - fontHeight) / 2;
            textLocation = new Vector2(GetIntByScale(20), location.Y + difference);
        }

        

        protected int GetIntByScale(int value)
        {
            return Convert.ToInt32((float)value * DisplayController.uiScale);
        }

        public void Update(Input input, bool inViewport)
        {        
            if (location.Contains((int)input.GetMousePos().X, (int)input.GetMousePos().Y) && inViewport  && input.LeftButtonClick())
            {
                selected = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, float containerFade)
        {
            if (selected)
            {
                spriteBatch.Draw(selectedSprite.texture2D, location, selectedSprite.location, selectedSprite.color * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }          
                spriteBatch.DrawString(font, displayName, textLocation, fontColor * containerFade, 0f, Vector2.One, DisplayController.uiScale, SpriteEffects.None, 0f);
        }

    }
}
