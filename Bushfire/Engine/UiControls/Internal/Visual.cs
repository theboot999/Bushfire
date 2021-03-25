using BushFire.Engine.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.UIControls.Internal
{
    class Visual
    {
        Vector2 location;
        Vector2 scale;  //scaled of 1 pixel
        Sprite spriteVisualFront;
        Sprite spriteVisualBack;

        public Visual(Vector2 location, Vector2 scale)
        {
            this.location = location;
            this.scale = scale;
            spriteVisualFront = GraphicsManager.GetSpriteColour(20);
            spriteVisualBack = GraphicsManager.GetSpriteColour(70);
        }

        public void DrawBack(SpriteBatch spriteBatch, float containerFade)
        {
            spriteBatch.Draw(spriteVisualBack.texture2D, location, spriteVisualBack.location, Color.White * containerFade, 0f, spriteVisualBack.rotationCenter, scale, SpriteEffects.None, 0);
        }

        public void DrawFront(SpriteBatch spriteBatch, float containerFade)
        {
            spriteBatch.Draw(spriteVisualFront.texture2D, location, spriteVisualFront.location, Color.White * containerFade, 0f, spriteVisualBack.rotationCenter, scale, SpriteEffects.None, 0);
        }

    }
}
