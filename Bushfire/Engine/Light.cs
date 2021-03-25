using BushFire.Engine.ContentStorage;
using BushFire.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine
{
    class Light
    {
        private Sprite lightSprite;
        private Sprite lightTexture;
        float scale;
        public bool bulbOnly;

        public Light(Sprite lightSprite, Sprite lightTexture, float scale, bool bulbOnly)
        {
            this.scale = scale;
            this.lightSprite = lightSprite;
            this.lightTexture = lightTexture;
            this.bulbOnly = bulbOnly;
        }
        //need to add something in here for the shine center point
        //for directional lighting etc
        //if its not directional then center is just the center
        //use this for game lights while using the world brightness

        public void DrawGameViewLightBulb(SpriteBatch spriteBatch, Vector2 locationVector, float rotation, float transparency)
        {
            spriteBatch.Draw(lightTexture.texture2D, locationVector, lightTexture.location, Color.White * lightTexture.transparency * transparency, rotation, lightTexture.rotationCenter, 1f, lightSprite.spriteEffect, 1);
        }

        public void DrawLightBulb(SpriteBatch spriteBatch, Vector2 locationVector, float rotation)
        {
            spriteBatch.Draw(lightTexture.texture2D, locationVector, lightTexture.location, Color.White * lightTexture.transparency, rotation, lightTexture.rotationCenter, 1f, lightSprite.spriteEffect, 1);
        }

        public void DrawLighting(SpriteBatch spriteBatch, Vector2 locationVector, float rotation)
        {
            spriteBatch.Draw(lightSprite.texture2D, locationVector, lightSprite.location, Color.White, rotation, lightSprite.rotationCenter, scale, lightSprite.spriteEffect, 1);
        }
        

        //Use this for menu lights
        public void DrawLighting(SpriteBatch spriteBatch, Vector2 locationVector, float rotation, float brightness)
        {
            spriteBatch.Draw(lightSprite.texture2D, locationVector, lightSprite.location, Color.White * brightness, rotation, lightSprite.rotationCenter, scale, lightSprite.spriteEffect, 1);
        }
    }


}
