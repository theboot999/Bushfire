using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Game.Storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Editor.Tech
{
    class Border
    {
        int borderThickness;
        int size = GroundLayerController.tileSize;
        Sprite sprite;
        int x;
        int y;
        public bool visible = true;

        public Border(int spriteColour, int x, int y, int borderThickness)
        {
            this.borderThickness = borderThickness;
            this.x = x * size;
            this.y = y * size;
            sprite = GraphicsManager.GetSpriteColour(spriteColour);

        }

        public void SetNewSpot(Spot spot)
        {
            x = spot.x * size;
            y = spot.y * size;

        }



        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                Rectangle left = new Rectangle(x, y, borderThickness, size);
                Rectangle right = new Rectangle(x + size - borderThickness, y, borderThickness, size);
                Rectangle top = new Rectangle(x, y, size, borderThickness);
                Rectangle bottom = new Rectangle(x, y + size - borderThickness, size, borderThickness);
                spriteBatch.Draw(sprite.texture2D, left, sprite.location, sprite.color * 0.3f, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                spriteBatch.Draw(sprite.texture2D, right, sprite.location, sprite.color * 0.3f, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                spriteBatch.Draw(sprite.texture2D, top, sprite.location, sprite.color * 0.3f, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                spriteBatch.Draw(sprite.texture2D, bottom, sprite.location, sprite.color * 0.3f, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
        }
    }
}
