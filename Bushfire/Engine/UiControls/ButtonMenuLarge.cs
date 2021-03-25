using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BushFire.Engine.Controllers;
using System.Diagnostics;
using BushFire.Engine.ContentStorage;

namespace BushFire.Engine.UIControls
{
    class ButtonMenuLarge : Button
    {
        public ButtonMenuLarge(string name, Point location, string text, Color fontColor) : base (name, location, text, true, fontColor, Font.OpenSans20Bold, GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonMenuLargeBlueBack), GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonMenuLargeBlueFront))
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
