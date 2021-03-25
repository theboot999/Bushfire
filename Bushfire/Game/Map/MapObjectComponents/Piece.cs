using BushFire.Content.Game.Screens.Containers;
using BushFire.Editor.Controllers;
using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map.MapObjectComponents
{
    class Piece
    {
        public Sprite sprite;
        public PieceStyle pieceStyle;
        public int pieceId;

        public Piece(PieceStyle pieceStyle, Sprite sprite, int pieceId)
        {
            this.pieceStyle = pieceStyle;
            this.sprite = sprite;
            this.pieceId = pieceId;
 
        }

        public void DrawMiniMapTile(SpriteBatch spriteBatch, Rectangle location, float transparency, Color color)
        {
            GameView.debugItemsDrawn++;

            spriteBatch.Draw(sprite.texture2D, location, sprite.location, color * transparency, sprite.rotation, sprite.rotationCenter, sprite.spriteEffect, 0);
        }

        public void DrawGameViewBox(SpriteBatch spriteBatch, Vector2 location, Color color, float scale, float transparency)
        {
            spriteBatch.Draw(sprite.texture2D, location, sprite.location, color * transparency, sprite.rotation, sprite.rotationCenter, scale, sprite.spriteEffect, 0);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, Color color, float scale)
        {
            GameView.debugItemsDrawn++;
            spriteBatch.Draw(sprite.texture2D, location, sprite.location, color, sprite.rotation, sprite.rotationCenter, scale, sprite.spriteEffect, 0);
        }

        public void DrawVisibleBlock(SpriteBatch spriteBatch, Vector2 location, float scale)
        {
            GameView.debugItemsDrawn++;
            spriteBatch.Draw(sprite.texture2D, location, sprite.location, Color.Black, sprite.rotation, sprite.rotationCenter, scale, sprite.spriteEffect, 0);
        }
    }
}
