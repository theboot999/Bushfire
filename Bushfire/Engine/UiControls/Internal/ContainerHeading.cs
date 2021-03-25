
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.Controllers;
using BushFire.Engine.ContentStorage;

namespace BushFire.Engine.UIControls.Abstract
{


    class ContainerHeading : UiControl
    {
        protected ContainerCamera parentContainerCamera;
        public bool isKill { get; private set; }
        public bool isPinned { get; private set; }
        public bool isAction { get; private set; }
        public bool isText { get; private set; }
        private bool buttonClose { get; set; }
        private bool buttonPin { get; set; }
        private bool buttonAction { get; set; }
        private bool buttonText { get; set; }
        private bool moveable { get; set; }
        private bool firstDownMovingIsLegit { get; set; }
        public bool moving { get; private set; }
        private int height { get; set; }

        private ButtonHeadingClose buttonHeadingClose;
        private ButtonHeadingPin buttonHeadingPin;
        private ButtonHeadingAction buttonHeadingAction;
        private ButtonHeadingText buttonHeadingText;

        public ContainerHeading(ContainerCamera parentContainerCamera, int height, string text, SpriteFont spriteFont, Color fontColor, bool buttonClose, bool buttonPin, bool buttonAction, bool buttonText, bool moveable, Sprite spriteBack)
        {
            name = "ContainerHeading";
            drawSpriteBack = true;
            drawText = true;
            this.spriteBack = spriteBack;
            currentUiScale = DisplayController.uiScale;
            this.parentContainerCamera = parentContainerCamera;
            this.height = height;
            SetText(text);
            this.spriteFont = spriteFont;
            this.fontColor = fontColor;
            SetSize();
            parentContainerCamera.ToggleHeading(height);

            this.buttonClose = buttonClose;
            this.buttonPin = buttonPin;
            this.buttonAction = buttonAction;
            this.moveable = moveable;
            this.buttonText = buttonText;

            if (buttonClose) { buttonHeadingClose = new ButtonHeadingClose(parentContainerCamera); }
            if (buttonPin) { buttonHeadingPin = new ButtonHeadingPin(parentContainerCamera); }
            if (buttonAction) { buttonHeadingAction = new ButtonHeadingAction(parentContainerCamera); }
            if (buttonText)
            {
                buttonHeadingText = new ButtonHeadingText(parentContainerCamera);
                isText = buttonHeadingText.TogglePressed();
                buttonHeadingText.SetPress();
            }
        }

        public void ModifyHeading(string text)
        {
            SetText(text);
            CenterText();
        }

        private void SetSize()
        {
            int borderThickness = parentContainerCamera.borderThickness;
            location = new Rectangle(borderThickness, borderThickness, parentContainerCamera.worldViewport.Width - borderThickness - borderThickness, GetIntByScale(height));
            CenterText();
        }    

        protected override void Rescale()
        {
            SetSize();
        }    

        private void UpdateButtons(Input input)
        {
            isAction = false;
            isKill = false;

            if (buttonClose)
            {
                buttonHeadingClose.SetInViewPort(inViewport);
                buttonHeadingClose.Update(input);

                if (buttonHeadingClose.IsLeftPress())
                {
                    isKill = true;
                }
            }
            if (buttonPin)
            {
                buttonHeadingPin.SetInViewPort(inViewport);
                buttonHeadingPin.Update(input);

                if (buttonHeadingPin.IsLeftPress())
                {
                    isPinned = buttonHeadingPin.TogglePin();
                }
            }

            if (buttonAction)
            {
                buttonHeadingAction.SetInViewPort(inViewport);
                buttonHeadingAction.Update(input);

                if (buttonHeadingAction.IsLeftPress())
                {
                    isAction = true;
                }
            }

            if (buttonText)
            {
                buttonHeadingText.SetInViewPort(inViewport);
                buttonHeadingText.Update(input);

                if (buttonHeadingText.IsLeftPress())
                {
                    isText = buttonHeadingText.TogglePressed();
                }
            }
        }

        private void UpdateMouseMoving(Input input)
        {
            if (location.Contains((int)input.GetMousePos().X, (int)input.GetMousePos().Y) && inViewport)
            {
                if (input.GetCurrentCursorChange() != CursorType.HANDCLOSE)
                {
                    input.ChangeMouseCursor(CursorType.HANDFINGER);
                }
            }

            if (!input.LeftButtonDown()) { firstDownMovingIsLegit = true; }

            if (input.LeftButtonDown() && location.Contains((int)input.GetMousePos().X, (int)input.GetMousePos().Y) && inViewport && firstDownMovingIsLegit)
            {
                if (buttonClose && buttonHeadingClose.isHover)
                {
                }
                else
                {
                    if (!moving)
                    {
                        moving = true;
                        parentContainerCamera.StartMovingViewport(input);
                    }
                }

                if (buttonPin && buttonHeadingPin.isHover)
                {
                }
                else
                {
                    if (!moving)
                    {
                        moving = true;
                        parentContainerCamera.StartMovingViewport(input);
                    }
                }
            }

            if (input.LeftButtonDown() && !moving) { firstDownMovingIsLegit = false; }

            if (!input.LeftButtonDown() && moving) { moving = false; }

            if (moving)
            {
                parentContainerCamera.MoveViewport(input);
                input.ChangeMouseCursor(CursorType.HANDCLOSE);
            }
        }

        #region CALLS

        public override void Update(Input input)
        {
             base.Update(input);

            UpdateButtons(input);

            if (moveable)
            {
                UpdateMouseMoving(input);
            }

            if (parentContainerCamera.refreshRequired)
            {
                SetSize();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float containerFade)
        {
            base.Draw(spriteBatch, containerFade);
            if (buttonClose)
            {
                buttonHeadingClose.Draw(spriteBatch, containerFade);
            }
            if (buttonPin)
            {
                buttonHeadingPin.Draw(spriteBatch, containerFade);
            }
            if (buttonAction)
            {
                buttonHeadingAction.Draw(spriteBatch, containerFade);
            }
            if (buttonText)
            {
                buttonHeadingText.Draw(spriteBatch, containerFade);
            }

        }

        #endregion
    }
}
