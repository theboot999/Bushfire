using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BushFire.Engine.Controllers;
using System.Diagnostics;
using BushFire.Engine.ContentStorage;
namespace BushFire.Engine.UIControls
{
    class ButtonMenuLargeGrey : Button
    {
        public ButtonMenuLargeGrey(string name, Point location, string text, Color fontColor) : base(name, location, text, true, fontColor, Font.OpenSans20Bold, GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonMenuLargeGreyBack), GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonMenuLargeGreyFront))
        {
            AddSound(SoundType.Effect3, true, false);
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
