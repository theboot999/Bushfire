using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Game.Storage;
using BushFire.Game.Tech;
using BushFire.Game.Vehicles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map.UI
{
    class SelectedMovingLine
    {
        //TPDP: Make some of these sprites equal Globals
        Location location;
        Sprite spriteLineSprite;
        Sprite outerCircleSprite;
        Sprite innerCircleSprite;

        public SelectedMovingLine(Location location)
        {
            this.location = location;
            spriteLineSprite = new Sprite(new Rectangle(80, 0, 1, 12), TextureSheet.WorldUI);
            outerCircleSprite = new Sprite(new Rectangle(0, 65, 96, 96), TextureSheet.WorldUI);
            innerCircleSprite = new Sprite(new Rectangle(0, 164, 32, 32), TextureSheet.WorldUI);
          
        }

        private float switcher(bool goingUp, out bool outValue, float amount, float min, float max, float speed)
        {
            if (goingUp)
            {
                amount += speed * EngineController.drawUpdateTime;
                if (amount > max)
                {
                    goingUp = false;
                    amount = max;
                }
            }
            else
            {
                amount -= speed * EngineController.drawUpdateTime;
                if (amount < min)
                {
                    goingUp = true;
                    amount = min;
                }
            }

            outValue = goingUp;
            return amount;
            
        }

        float lineFade = 1f;
        float outerScale = 1;
        float outerFade = 1;

        bool outerScaleUp;
        bool outerFadeUp;
        bool lineFadeUp;

        public void Draw(SpriteBatch spriteBatch, List<VAction> actionList)
        {
            Vector2 start;
            Vector2 end;
            float rotation;
            Vector2 scale;
            start = location.position;

            outerScale = switcher(outerScaleUp, out outerScaleUp, outerScale, 0.8f, 1f, 0.01f);
            outerFade = switcher(outerFadeUp, out outerFadeUp, outerFade, 0.5f, 0.8f, 0.01f);
            lineFade = switcher(lineFadeUp, out lineFadeUp, lineFade, 0.5f, 0.8f, 0.01f);

            if (actionList.Count > 0)
            {
                spriteBatch.Draw(innerCircleSprite.texture2D, start, innerCircleSprite.location, Color.White * 0.8f, 0, innerCircleSprite.rotationCenter, 0.7f, SpriteEffects.None, 0);
            }

            foreach (VAction action in actionList)
            {
                if (action.actionState != ActionState.Cancelling && !action.ignoreDraw)
                {
                    TileLogistic tileLogistic = WorldController.GetTileLogistic(action.destinationPoint);
                    end = new Vector2(action.destinationPoint.X * GroundLayerController.tileSize + tileLogistic.center.X, action.destinationPoint.Y * GroundLayerController.tileSize + tileLogistic.center.Y);

                    scale = new Vector2(Vector2.Distance(start, end), 1);
                    rotation = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);

                    spriteBatch.Draw(spriteLineSprite.texture2D, start, spriteLineSprite.location, Color.White * 0.5f, rotation, spriteLineSprite.rotationCenter, scale, SpriteEffects.None, 0);
                    start = end;
                 
                    //Draw end circle
                    spriteBatch.Draw(outerCircleSprite.texture2D, end, outerCircleSprite.location, Color.White * outerFade, 0, outerCircleSprite.rotationCenter, outerScale, SpriteEffects.None, 0);
                    spriteBatch.Draw(innerCircleSprite.texture2D, end, innerCircleSprite.location, Color.White * 0.9f, 0, innerCircleSprite.rotationCenter, 1f, SpriteEffects.None, 0);
                }
            }

            
        }

    }
}
