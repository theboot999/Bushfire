using BushFire.Content.Game.Screens.Containers;
using BushFire.Editor.Containers;
using BushFire.Editor.Tech;
using BushFire.Engine;
using BushFire.Game;
using BushFire.Game.Storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map.MapObjectComponents
{
    class Shadow
    {
        public Sprite sprite;
        public int tileX;
        public int tileY;
        public int shadowId;
        public ShadowSide shadowSide;
        public Point shadowOffset;

        public Shadow(Sprite sprite, int tileX, int tileY, ShadowSide shadowSide, int shadowId, Point shadowOffset)
        {
            this.sprite = sprite;
            this.tileX = tileX;
            this.tileY = tileY;
            this.shadowId = shadowId;
            this.shadowSide = shadowSide;
            this.shadowOffset = shadowOffset;
        }

        //EditingMode

        public void DrawSampleShadowEditing(SpriteBatch spriteBatch)
        {
            int locationX = tileX * GroundLayerController.tileSize + shadowOffset.X;
            int locationY = tileY * GroundLayerController.tileSize + shadowOffset.Y;
            Vector2 location = new Vector2(locationX + (sprite.rotationCenter.X * WorldController.shadowLength), locationY + sprite.rotationCenter.Y - GroundLayerController.halfTileSize);
            spriteBatch.Draw(sprite.texture2D, location, sprite.location, Color.Black * 0.4f, 0, sprite.rotationCenter, 0.4f, sprite.spriteEffect, 0);
        }

        public void DrawBuildingEditing(SpriteBatch spriteBatch)
        {
            if (shadowSide == ShadowSide.LEFT)
            {
                int locationX = tileX * GroundLayerController.tileSize + shadowOffset.X;
                int locationY = tileY * GroundLayerController.tileSize + shadowOffset.Y;
                Vector2 location = new Vector2(locationX - (sprite.rotationCenter.X), locationY + sprite.rotationCenter.Y);
                spriteBatch.Draw(sprite.texture2D, location, sprite.location, Color.Black * 0.4f, 0, sprite.rotationCenter, 1, sprite.spriteEffect, 0);

    
            }
            else if (shadowSide == ShadowSide.RIGHT)
            {
                int locationX = tileX * GroundLayerController.tileSize + shadowOffset.X;
                int locationY = tileY * GroundLayerController.tileSize + shadowOffset.Y;
                Vector2 location = new Vector2(locationX + (sprite.rotationCenter.X), locationY + sprite.rotationCenter.Y);
                spriteBatch.Draw(sprite.texture2D, location, sprite.location, Color.Black * 0.4f, 0, sprite.rotationCenter, 1, sprite.spriteEffect, 0);
            }
        }

        public EditorShadow GetEditorShadow()
        {
            return new EditorShadow(shadowId, tileX, tileY, shadowOffset.X, shadowOffset.Y, shadowSide);
        }

        public void SetTileAndOffset(int tileX, int tileY, Point shadowOffset)
        {
            this.tileX = tileX;
            this.tileY = tileY;
            this.shadowOffset = shadowOffset;
        }



        public int GetHeight()
        {
            return sprite.location.Height;
        }

        public void Draw(SpriteBatch spriteBatch, int worldObjectX, int worldObjectY, Vector2 smallOffset, float inScale)
        {
            if (shadowSide == ShadowSide.LEFT)
            {
                GameView.debugItemsDrawn++;
                int locationX = (worldObjectX + tileX) * GroundLayerController.tileSize + shadowOffset.X;
                int locationY = (worldObjectY + tileY) * GroundLayerController.tileSize + shadowOffset.Y;
                Vector2 location = new Vector2(locationX - (sprite.rotationCenter.X * WorldController.shadowLength) + smallOffset.X, locationY + sprite.rotationCenter.Y + smallOffset.Y);
                Vector2 scale = new Vector2(WorldController.shadowLength, 1);
                spriteBatch.Draw(sprite.texture2D, location, sprite.location, Color.White, 0, sprite.rotationCenter, scale, sprite.spriteEffect, 0);
            }
            else if (shadowSide == ShadowSide.RIGHT)
            {
                GameView.debugItemsDrawn++;
                int locationX = (worldObjectX + tileX) * GroundLayerController.tileSize + shadowOffset.X;
                int locationY = (worldObjectY + tileY) * GroundLayerController.tileSize + shadowOffset.Y;
                Vector2 location = new Vector2(locationX + (sprite.rotationCenter.X * WorldController.shadowLength) + smallOffset.X, locationY + sprite.rotationCenter.Y + smallOffset.Y);
                Vector2 scale = new Vector2(WorldController.shadowLength, 1);
                spriteBatch.Draw(sprite.texture2D, location, sprite.location, Color.White, 0, sprite.rotationCenter, scale, sprite.spriteEffect, 0);
            }
        }
    }
}
