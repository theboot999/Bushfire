using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BushFire.Engine.UIControls
{
    class ButtonHeadingClose : Button
    {
        private ContainerCamera parentContainerCamera { get; set; }

        public ButtonHeadingClose(ContainerCamera parentContainerCamera) : base ("", new Point(0,0), "", false, Color.White, Font.Anita10, GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonCloseBack), GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonCloseFront))
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
            int x = parentContainerCamera.worldViewport.Width - GetIntByScale(10) - GetIntByScale(buttonSize);
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
