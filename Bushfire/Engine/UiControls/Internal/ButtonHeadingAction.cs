using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BushFire.Engine.UIControls
{
    class ButtonHeadingAction : Button
    {
        // string name, Point locationPoint, string text, bool centerText, Color fontColor, Font font, Sprite spriteBack, Sprite spriteFront

        private ContainerCamera parentContainerCamera { get; set; }

        public ButtonHeadingAction(ContainerCamera parentContainerCamera) : base("", new Point(0, 0), "", false, Color.White, Font.Anita10, GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonZoomBack), GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonZoomFront))
        {
            drawText = false;
            this.parentContainerCamera = parentContainerCamera;
            SetSize();
            fadeSpeed = 0.06f;
        }

        private void SetSize()
        {
            //All unscaled
            int buttonSize = parentContainerCamera.headingHeight - 10;
            int x = GetIntByScale(10);
            int y = (GetIntByScale(parentContainerCamera.headingHeight) / 2) - (GetIntByScale(buttonSize) / 2) + parentContainerCamera.borderThickness;
            location = new Rectangle(x, y, GetIntByScale(buttonSize), GetIntByScale(buttonSize));
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
