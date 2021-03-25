using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BushFire.Engine.Controllers;
using BushFire.Engine.ContentStorage;

namespace BushFire.Engine.UIControls
{
    class ButtonBlueSmall : Button
    {
        public ButtonBlueSmall(string name, Point location, string text, Color fontColor) : base(name, location, text, true, fontColor, Font.CarterOne20, GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonGreySmallBack), GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonGreySmallFront))
        {

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
