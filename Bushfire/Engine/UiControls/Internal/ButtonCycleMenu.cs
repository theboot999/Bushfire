using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls.Internal;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.UIControls
{   
    class ButtonCycleMenu : Button
    {
        public ButtonCycleMenu(string name, Point location, string text, Font font, Color fontColor) : base(name, location, text, true, fontColor, Font.CarterOne20, GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonGreySmallBack), GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonGreySmallFront))
        {
            AddSound(SoundType.Effect3, false, true);
        }


        #region CALLS

        public override void Update(Input input)
        {
            base.Update(input);
        }

        public override void Draw(SpriteBatch spriteBatch, float containerFade)
        {
            base.Draw(spriteBatch, containerFade);
        }

        #endregion
    }
}
