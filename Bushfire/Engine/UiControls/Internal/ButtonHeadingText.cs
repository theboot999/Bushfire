using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BushFire.Engine.UIControls
{
    class ButtonHeadingText : Button
    {
        // string name, Point locationPoint, string text, bool centerText, Color fontColor, Font font, Sprite spriteBack, Sprite spriteFront

        private ContainerCamera parentContainerCamera { get; set; }
        private bool isPressed { get; set; }

        public ButtonHeadingText(ContainerCamera parentContainerCamera) : base("", new Point(0, 0), "", false, Color.White, Font.Anita10, GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonTextBack), GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonTextFront))
        {
            selected = true;
            drawText = false;
            this.parentContainerCamera = parentContainerCamera;
            SetSize();
            fadeSelected = 0.65f;
            fadeSpeed = 0.06f;
        }

        private void SetSize()
        {
            //All unscaled
            int buttonSize = parentContainerCamera.headingHeight - 10;
            int x = GetIntByScale(60);
            int y = (GetIntByScale(parentContainerCamera.headingHeight) / 2) - (GetIntByScale(buttonSize) / 2) + parentContainerCamera.borderThickness;
            location = new Rectangle(x, y, GetIntByScale(buttonSize), GetIntByScale(buttonSize));
        }

        public bool TogglePressed()
        {
            if (isPressed)
            {
                isPressed = false;
                selected = false;
            }
            else
            {
                isPressed = true;
                selected = true;
            }
            return isPressed;
        }

        protected override void Rescale()
        {
            SetSize();
        }

        #region CALLS

        public override void Update(Input input)
        {
            base.Update(input);

            if (parentContainerCamera.refreshRequired)
            {
                SetSize();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float containerFade)
        {
            base.Draw(spriteBatch, containerFade);
        }

        #endregion
    }
}
