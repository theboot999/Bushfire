using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Game.Storage;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map.UI
{
    class MoveMarker : WorldThing
    {
        float fade;
        bool fadeIn;
        Sprite sprite;
        Vector2 location;


        public MoveMarker(Point tilePoint)
        {
            sprite = new Sprite(new Rectangle(0, 0, 64, 64), GraphicsManager.GetTextureSheet(TextureSheet.WorldUI));
            TileLogistic tileLogistic = WorldController.GetTileLogistic(tilePoint);
            location = new Vector2(tilePoint.X * GroundLayerController.tileSize + tileLogistic.center.X, tilePoint.Y * GroundLayerController.tileSize + tileLogistic.center.Y);
            fadeIn = true;
            fade = 0;
        }

        public override void Update()
        {
            if (fadeIn)
            {
                fade += 0.1f * EngineController.drawUpdateTime;
                if (fade > 1.5)
                {
                    fadeIn = false;
                }
            }
            else
            {
                fade -= 0.1f * EngineController.drawUpdateTime;
                if (fade < 0)
                {
                    destroy = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
       //     spriteBatch.Draw(sprite.texture2D, location, sprite.location, Color.White * fade, sprite.rotation, sprite.rotationCenter, 1f, SpriteEffects.None, 0);
        }
    }
}
