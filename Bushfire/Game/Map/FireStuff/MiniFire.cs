using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Game.Controllers;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map.FireStuff
{
    class MiniFire
    {
        //we can use a const sprite
        public float fuel;
        Sprite sprite;
        Fire fire;
        int miniX;
        int miniY;
        public bool isBurnt { get; private set; }

        public bool releaseSmoke;

        public MiniFire(float fuel, Fire fire, int miniX, int miniY)
        {
            this.fuel = fuel;
            fuel = 2000000f;
            this.fire = fire;
            this.miniX = miniX;
            this.miniY = miniY;
            debugSpreadTimer = 20f;
            debugReleaseTimer = 40f;
            sprite = GraphicsManager.GetGameSprite(GameSprite.RedCircle);
        }

    
        private void UpdateSpread()
        {
            int spreadX = GameController.rnd.Next(-1, 2);
            int spreadY = GameController.rnd.Next(-1, 2);
            WorldController.worldFire.SpreadMiniFire(fire.tileX, fire.tileY, miniX, miniY, spreadX, spreadY);
        }

        float debugSpreadTimer;
        float debugReleaseTimer;


        //Intensity is precalculated off game update time from the tiles intensity
        public void Update()
        {    //if its still burning
            fuel -= fire.calculatedIntensity;

            debugSpreadTimer -= 1f * EngineController.gameUpdateTime;
            debugReleaseTimer -= 1f * EngineController.gameUpdateTime;

            if (debugSpreadTimer < 0)
            {
                debugSpreadTimer = 20f;
                UpdateSpread();
            }

            if (debugReleaseTimer < 0)
            {
                debugReleaseTimer = 40f;
                releaseSmoke = true;
            }
            else
            {
                releaseSmoke = false;
            }



            if (fuel < 0)
            {
                isBurnt = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            if (!isBurnt)
            {
                spriteBatch.Draw(sprite.texture2D, location, sprite.location, Color.White * 0.2f, 0f, sprite.rotationCenter, 1f, SpriteEffects.None, 0);
            }

        }
        //this is temporrary.  later on it hink we just realease particals

    }
}
