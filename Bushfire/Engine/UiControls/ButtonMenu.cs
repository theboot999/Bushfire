using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BushFire.Engine.Controllers;
using System.Diagnostics;

namespace BushFire.Engine.UIControls
{
    class ButtonMenu : Button
    {
        public ButtonMenu(string name, Point locationPoint, string text, bool centerText, Color fontColor, Font font, Sprite spriteBack, Sprite spriteFront) : base(name, locationPoint, text, centerText, fontColor, font, spriteBack, spriteFront)
        {

        }

        public ButtonMenu(string name, Point locationPoint, string text, bool centerText, Color fontColor, Font font, Sprite spriteBack, Sprite spriteFront, SoundType soundtype) : base(name, locationPoint, text, centerText, fontColor, font, spriteBack, spriteFront)
        {
                 AddSound(soundtype, true, false);
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
