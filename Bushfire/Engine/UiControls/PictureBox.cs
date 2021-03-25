using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.UIControls
{
    class PictureBox : UiControl
    {
        public Rectangle preScaleLocation;

        public PictureBox(string name, Rectangle location, Sprite sprite)
        {
            drawSpriteBack = true;
            this.spriteBack = sprite;
            this.name = name;
            this.preScaleLocation = location;
            Rescale();
        }

        protected override void Rescale()
        {
            location = CalcScaleRectangle(preScaleLocation, DisplayController.uiScale);
            location.X += (location.Width / 2);
            location.Y += (location.Height / 2);
            currentUiScale = DisplayController.uiScale;
        }

        public void ChangeLocation(Rectangle location)
        {
            preScaleLocation = location;
            Rescale();
        }

        public override void Draw(SpriteBatch spriteBatch, float containerFade)
        {

            spriteBatch.Draw(spriteBack.texture2D, location, spriteBack.location, spriteBack.color * transparency * containerFade, spriteBack.rotation, spriteBack.rotationCenter, spriteBack.spriteEffect, 0f);


        }

    }
}
