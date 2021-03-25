using BushFire.Content.Game.Screens.Containers;
using BushFire.Engine;
using BushFire.Game.Storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map.MapObjectComponents
{
    class GroundLayer
    {
        public LayerType layerType { get; private set; }
        public Sprite sprite { get; private set; }         
        public int smoothingLayerIndex { get; private set; }     
        public int bitMaskId { get; private set; }
        public float scale = 1;
        public bool isFullBurnt = false;

        public GroundLayer(LayerType layerType, Sprite sprite, int smoothingLayerIndex, int bitMaskId)
        {
            this.layerType = layerType;
            this.sprite = sprite;
            this.smoothingLayerIndex = smoothingLayerIndex;
            this.bitMaskId = bitMaskId;
        }

        public int GetSmoothingLayer()
        {
            return (int)layerType;
        }

        public void DrawGameViewBox(SpriteBatch spriteBatch, Vector2 location, float transparency)
        {
            GameView.debugItemsDrawn++;
            spriteBatch.Draw(sprite.texture2D, location, sprite.location, Color.White * transparency, sprite.rotation, sprite.rotationCenter, 1f, SpriteEffects.None, 0);
        }

        public void DrawMiniMapTile(SpriteBatch spriteBatch, Rectangle destinationRectangle)
        {
            GameView.debugItemsDrawn++;
            spriteBatch.Draw(sprite.texture2D, destinationRectangle, sprite.location, Color.White, sprite.rotation, sprite.rotationCenter, SpriteEffects.None, 1f);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            GameView.debugItemsDrawn++;
            spriteBatch.Draw(sprite.texture2D, location, sprite.location, Color.White, sprite.rotation, sprite.rotationCenter, scale, SpriteEffects.None, 0);
        }
    }


    

 
}
