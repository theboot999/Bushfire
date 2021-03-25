using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BushFire.Engine.UIControls
{
    class ButtonHeadingPin : Button
    {
        private ContainerCamera parentContainerCamera { get; set; }

        private bool isPinned { get; set; }

        public ButtonHeadingPin(ContainerCamera parentContainerCamera) : base("", new Point(0, 0), "", false, Color.White, Font.Anita10, GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonPinBack), GraphicsManager.GetPreBuilt(PrebuiltSprite.ButtonPinFront))
        {
            drawText = false;
            this.parentContainerCamera = parentContainerCamera;
            spriteFront.altColor = Color.White;
            SetSize();
            fadeSelected = 0.65f;
            fadeSpeed = 0.06f;
        }

        private void SetSize()
        {
            //All unscaled
            int buttonSize = parentContainerCamera.headingHeight - 10;
            int x = parentContainerCamera.worldViewport.Width - GetIntByScale(55) - GetIntByScale(buttonSize);
            int y = (GetIntByScale(parentContainerCamera.headingHeight) / 2) - (GetIntByScale(buttonSize) / 2) + parentContainerCamera.borderThickness;
            location = new Rectangle(x, y, GetIntByScale(buttonSize), GetIntByScale(buttonSize));
        }

        public bool TogglePin()
        {
            if (isPinned)
            {
                isPinned = false;
                selected = false;
            }
            else
            {
                isPinned = true;
                selected = true;
            }
            return isPinned;
        }

        public void SetPinned(bool value)
        {
            if (value)
            {
                isPinned = true;
                selected = true;
            }
            else
            {
                isPinned = false;
                selected = false;
            }
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
