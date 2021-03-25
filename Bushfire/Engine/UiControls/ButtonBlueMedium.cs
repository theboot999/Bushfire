using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BushFire.Engine.Controllers;
using BushFire.Engine.ContentStorage;

namespace BushFire.Engine.UIControls
{
    class ButtonBlueMedium : Button
    {
        public ButtonBlueMedium(string name, Point location, string text, Color fontColor) : base(name, location, text, true, fontColor, Font.CarterOne18, GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonGreyMediumBack), GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonGreyMediumFront))
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
