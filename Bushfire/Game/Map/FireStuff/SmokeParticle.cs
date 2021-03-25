using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Game.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map.FireStuff
{
    class SmokeParticle
    {
        //we can store a global winddirection vector normalized then * windspeed that gets calculated each game tick so we dont have to recalculate them all the time
        //So if its starting with 0 life
        //then we need to fade it in
        //otherwise its an already burning one and we have max fade


        public bool destroy;

        private Vector2 increaseSize;

        private Vector2 location;
        private Vector2 drawSize;
        private Sprite sprite;

 
        private Vector2 velocity;
        private float fade;
        private Color drawColor = Color.White;
        private float drawRotation = 0f;

        private float life;  //number between 0 and 1.  1 is end of life
                             //if life is more than 0.9 begin fade


        private const float lifeIncrease = 0.0001f;
        private const float windDirection = MathHelper.Pi;
        private const float windSpeed = 300f; //20 km

        public SmokeParticle(Vector2 location, Vector2 startDrawSize, Color drawColor, float life)
        {
            sprite = GraphicsManager.GetGameSprite(GetRandomSmoke());

            drawRotation = AngleStuff.GetRandomRadian();

            this.drawColor = drawColor;
            this.location = location;
            this.life = life;
            //this.life = 0.5f;
            fade = 0.5f;
            SetIncreaseSizes();
            SetStartLocation();
            SetDrawSize(startDrawSize);
        }

        private void SetStartLocation()
        {
            if (life > 0)
            {
                Vector2 endVelocity = AngleStuff.RadianToVector(windDirection) * windSpeed * (life);
                location += endVelocity;
            }
        }

        private void SetIncreaseSizes()
        {
          //  float x = (float)(GameController.rnd.NextDouble() * (70f - 20f) + 20f);
          //  float y = (float)(GameController.rnd.NextDouble() * (70f - 20f) + 20f);

            float x = (float)(GameController.rnd.NextDouble() * (70f - 20f) + 20f);
            float y = (float)(GameController.rnd.NextDouble() * (70f - 20f) + 20f);
            increaseSize = new Vector2(x, y);
        }

        private void SetDrawSize(Vector2 startDrawSize)
        {
            //randomizing the draw size slightly
            float sizeDif = 0.5f;
            float minX = startDrawSize.X - (startDrawSize.X * sizeDif);
            float maxX = startDrawSize.X + (startDrawSize.X * sizeDif);
            float minY = startDrawSize.Y - (startDrawSize.Y * sizeDif);
            float maxY = startDrawSize.Y + (startDrawSize.Y * sizeDif);
            drawSize = new Vector2(RandomController.GetRandomFloat(minX, maxX), RandomController.GetRandomFloat(minY, maxY));

            if (life > 0)
            {
                drawSize = drawSize + (increaseSize * life);
            }
        }


        private void UpdateVelocity()
        {
            velocity = AngleStuff.RadianToVector(windDirection) * windSpeed * (lifeIncrease * EngineController.gameUpdateTime);
        }



        private void UpdateDrawSize()
        {
            drawSize += increaseSize * (lifeIncrease * EngineController.gameUpdateTime);
        }



        private void UpdateLife()
        {
            life += lifeIncrease * EngineController.gameUpdateTime;
            if (life > 1)
            {
                destroy = true;
            }
        }


        private void UpdateMove()
        {
            location += velocity;
        }


    

        private GameSprite GetRandomSmoke()
        {
            int c = GameController.rnd.Next(0, 9);
            switch (c)
            {
                case 0:
                    return GameSprite.Smoke1;
                case 1:
                    return GameSprite.Smoke2;
                case 2:
                    return GameSprite.Smoke3;
                case 3:
                    return GameSprite.Smoke4;
                case 4:
                    return GameSprite.Smoke5;
                case 5:
                    return GameSprite.Smoke6;
                case 6:
                    return GameSprite.Smoke7;
                case 7:
                    return GameSprite.Smoke8;
            }
            return GameSprite.Smoke8;
        }

        public void DrawUpdate()
        {
                UpdateDrawSize();
                UpdateVelocity();
                UpdateMove();
                UpdateLife();   
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 drawScale = new Vector2(drawSize.X / sprite.location.Width, drawSize.Y / sprite.location.Height);
            spriteBatch.Draw(sprite.texture2D, location, sprite.location, drawColor * fade, drawRotation, sprite.rotationCenter, drawScale, sprite.spriteEffect, 0);
        }

    }

 

}
