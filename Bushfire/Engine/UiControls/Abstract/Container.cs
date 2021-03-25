using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System;
using BushFire.Engine.Controllers;

namespace BushFire.Engine.UIControls.Abstract
{
    enum ContainerState
    {
        FadeIn,
        Normal,
        FadeOut,
        Destroy
    }

    abstract class Container : UiControl
    {
        //Bug:  Still have a bug where if the mouse is down (Eg resizing or scrolling a scroll bar and we move quickly over another window
        //      We lose focus on the window we are resizing. If we move the mouse fast it snaps out of the window viewport for a second and sets
        //      the container its now over to havefocus
        //      Still need to find a work around for it

        protected List<UiControl> controlList = new List<UiControl>();
        protected ContainerCamera containerCamera { get; set; }
        protected DockType dockType { get; set; }
        private Rectangle originalLocation;

        protected bool canChangeFocusOrder { get; set; }            //Is it fixed in the focus (Use this if you need a backing container)
        public bool hasFocus { get; private set; }                  //If the entire container has focus
        public bool hasCameraFocus { get; private set; }            //If the usable part of the container has focus (Not the headings/scrollbars etc)
        public bool SetFocusToTop { get; private set; }             //If we need to set the focus of the container to the top level on the next focus Trawl

        private bool hasBorder { get; set; }
        private bool hasHeading { get; set; }
        private bool hasScrollV { get; set; }
        private bool hasScrollH { get; set; }
        public bool alwaysOnTop { get; protected set; }
        public bool isAction { get; private set; }
        public bool ignoreInput { get; protected set; }

        public ContainerState containerState { get; private set; }
        protected float containerFade { get; private set; }

        protected ContainerBorder containerBorder { get; private set; }
        protected ContainerHeading containerHeading { get; set; }
        private ContainerScrollV containerScrollV { get; set; }
        private ContainerScrollH containerScrollH { get; set; }

        private float maxTransparency = 1f;


        protected Container(Rectangle localLocation, DockType dockType, bool fadeIn)
        {

            currentUiScale = DisplayController.uiScale;
            originalLocation = localLocation;
            this.dockType = dockType;
            canChangeFocusOrder = true;
            this.alwaysOnTop = alwaysOnTop;
            localLocation = CalcDockType();
            containerCamera = new ContainerCamera(localLocation);
            location = new Rectangle(0, 0, localLocation.Width, localLocation.Height);

            if (fadeIn)
            {
                SetFadeIn();
            }
            else
            {
                containerState = ContainerState.Normal;
                containerFade = 1f;
            }

        }

        public void SetMaxTransparency(float value)
        {
            maxTransparency = value;
            containerState = ContainerState.FadeIn;
        }


        public void SetFadeIn()
        {
            containerState = ContainerState.FadeIn;
            containerFade = 0f;
        }

        public void SetDestroyWithFade()
        {
            containerState = ContainerState.FadeOut;
        }

        public void SetDestroyWithOutFade()
        {
            containerState = ContainerState.Destroy;
        }


        protected override void Rescale()
        {
            Rectangle localLocation = CalcDockType();      
            containerCamera.Rescale(localLocation, true);      
            location = new Rectangle(0, 0, localLocation.Width, localLocation.Height);
            currentUiScale = DisplayController.uiScale;
        }

        public virtual void ResolutionChange()
        {
            if (dockType == DockType.SCREENRESOLUTION)
            {
                location = ScreenController.gameWindow;
                containerCamera = new ContainerCamera(location);

            }
        }


        private Rectangle CalcDockType()
        {
            switch (dockType)
            {
                case DockType.SCREENRESOLUTION:
                    {
                        return ScreenController.gameWindow;
                    }
                case DockType.TOPLEFT:
                    {
                        return new Rectangle(CalcScaleLeftX(), CalcScaleTopY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.TOPLEFTFIXEDX:
                    {
                        return new Rectangle(CalcFixedLeftX(), CalcScaleTopY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.TOPLEFTFIXEDY:
                    {
                        return new Rectangle(CalcScaleLeftX(), CalcFixedTopY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.TOPLEFTFIXEDBOTH:
                    {
                        return new Rectangle(CalcFixedLeftX(), CalcFixedTopY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.TOPRIGHT:
                    {
                        return new Rectangle(CalcScaleRightX(), CalcScaleTopY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.TOPRIGHTFIXEDX:
                    {
                        return new Rectangle(CalcFixedRightX(), CalcScaleTopY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.TOPRIGHTFIXEDY:
                    {
                        return new Rectangle(CalcScaleRightX(), CalcFixedTopY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.TOPRIGHTFIXEDBOTH:
                    {
                        return new Rectangle(CalcFixedRightX(), CalcFixedTopY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.BOTTOMRIGHT:
                    {
                        return new Rectangle(CalcScaleRightX(), CalcScaleBottomY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.BOTTOMRIGHTFIXEDX:
                    {
                        return new Rectangle(CalcFixedRightX(), CalcScaleBottomY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.BOTTOMRIGHTFIXEDY:
                    {
                        return new Rectangle(CalcScaleRightX(), CalcFixedBottomY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.BOTTOMRIGHTFIXEDBOTH:
                    {
                        return new Rectangle(CalcFixedRightX(), CalcFixedBottomY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.BOTTOMLEFT:
                    {
                        return new Rectangle(CalcScaleLeftX(), CalcScaleBottomY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.BOTTOMLEFTFIXEDX:
                    {
                        return new Rectangle(CalcFixedLeftX(), CalcScaleBottomY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.BOTTOMLEFTFIXEDY:
                    {
                        return new Rectangle(CalcScaleLeftX(), CalcFixedBottomY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.BOTTOMLEFTFIXEDBOTH:
                    {
                        return new Rectangle(CalcFixedLeftX(), CalcFixedBottomY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.CENTERSCREENX:
                    {
                        return new Rectangle(CalcScaleCenterWidthX(), CalcFixedTopY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.CENTERSCREENY:
                    {
                        return new Rectangle(CalcFixedLeftX(), CalcScaleCenterHeightY(), CalcScaleWidth(), CalcScaleHeight());
                    }
                case DockType.CENTERSCREENBOTH:
                    {
                        return new Rectangle(CalcScaleCenterWidthX(), CalcScaleCenterHeightY(), CalcScaleWidth(), CalcScaleHeight());
                    }
            }
            return Rectangle.Empty;
        }

        private int CalcScaleLeftX()
        {
            return GetIntByScale(originalLocation.X);
        }


        private int CalcFixedLeftX()
        {
            return (originalLocation.X);
        }

        //Remember our X is now the right side of the container
        private int CalcScaleRightX()
        {
            return ScreenController.gameWindow.Width - CalcScaleWidth() - GetIntByScale(originalLocation.X);
        }

        private int CalcFixedRightX()
        {
            return ScreenController.gameWindow.Width - CalcScaleWidth() - originalLocation.X;
        }


        private int CalcScaleTopY()
        {
            return GetIntByScale(originalLocation.Y);
        }

        private int CalcFixedTopY()
        {
            return (originalLocation.Y);
        }

        //Remember our Y is now the right side of the container
        private int CalcScaleBottomY()
        {
            return ScreenController.gameWindow.Height - CalcScaleHeight() - GetIntByScale(originalLocation.Y);
        }
               
        private int CalcFixedBottomY()
        {
            return ScreenController.gameWindow.Height - CalcScaleHeight() - originalLocation.Y;
        }

        private int CalcScaleWidth()
        {
            return GetIntByScale(originalLocation.Width);
        }

        private int CalcScaleHeight()
        {
            return GetIntByScale(originalLocation.Height);
        }

        private int CalcScaleCenterWidthX()
        {
            int scaleWidth = GetIntByScale(originalLocation.Width) / 2;
            int screenCenter = ScreenController.gameWindow.Width / 2;
            return screenCenter - scaleWidth;
        }

        private int CalcScaleCenterHeightY()
        {
            int scaleHeight = GetIntByScale(originalLocation.Height) / 2;
            int screenCenter = ScreenController.gameWindow.Height / 2;
            return screenCenter - scaleHeight;
        }

        protected void AddBorder(int borderThickness, Resizing resizableType, int textureColour)
        {
            containerBorder = new ContainerBorder(containerCamera, borderThickness, resizableType, textureColour);
            hasBorder = true;
        }

        protected void SetSizeBounds(int minWidth, int minHeight, int maxWidth, int maxHeight)
        {
            containerCamera.SetSizeBounds(minWidth, minHeight, maxWidth, maxHeight);

            if (originalLocation.Width < minWidth)
            {
                originalLocation.Width = minWidth;
            }
            if (originalLocation.Width > maxWidth)
            {
                originalLocation.Width = maxWidth;
            }
            if (originalLocation.Height < minHeight)
            {
                originalLocation.Height = minHeight;
            }
            if (originalLocation.Height > maxHeight)
            {
                originalLocation.Height = maxHeight;
            }

            Rescale();
            containerCamera.SetRefreshNextFrame();
        }

        protected void RemoveBorder()
        {
            containerBorder = null;
            hasBorder = false;
            containerCamera.ToggleBorder(0);
        }

        protected float GetCenterWidth()
        {
            return (float)originalLocation.Width / 2;
        }

        protected bool IsHeadingTextActive()
        {
            if (containerHeading != null)
            {
                return containerHeading.isText;
            }
            else
            {
                return false;
            }
        }

        public bool IsPinned()
        {
            if (containerHeading != null)
            {
                return containerHeading.isPinned;
            }
            return false;
        }

        protected void AddHeading(int height, string text, SpriteFont spriteFont, Color fontColor, bool buttonClose, bool buttonPin, bool buttonZoom, bool buttonText, bool moveAble, Sprite spriteBack)
        {
            hasHeading = true;
            containerHeading = new ContainerHeading(containerCamera, height, text, spriteFont, fontColor, buttonClose, buttonPin, buttonZoom, buttonText, moveAble, spriteBack);
        }

        protected void ModifyHeadingText(string text)
        {
            if (containerHeading != null)
            {
                containerHeading.ModifyHeading(text);
            }
        }


        protected void RemoveHeading()
        {
            hasHeading = false;
            containerHeading = null;
            containerCamera.ToggleHeading(0);
        }

        protected void AddScrollV(int thickness, int textureColourBack, int textureColorScroll)
        {
            hasScrollV = true;
            containerScrollV = new ContainerScrollV(containerCamera, thickness, textureColourBack, textureColorScroll);
        }

        protected void RemoveScrollV()
        {
            hasScrollV = false;
            containerScrollV = null;
            containerCamera.ToggleVScroll(0);
        }

        protected void AddScrollH(int thickness, int textureColourBack, int textureColorScroll)
        {
            hasScrollH = true;
            containerScrollH = new ContainerScrollH(containerCamera, thickness, textureColourBack, textureColorScroll);
        }

        protected void RemoveScrollH()
        {
            hasScrollH = false;
            containerScrollH = null;
            containerCamera.ToggleHScroll(0);
        }

        protected void AddUiControl(UiControl uiControl)
        {
            controlList.Add(uiControl);
        }

        protected void RemoveUiControl(UiControl uiControl)
        {
            if (uiControl != null && controlList.Contains(uiControl))
            {
                controlList.Remove(uiControl);
            }
        }

        protected void RemoveUiControl(string name)
        {
            UiControl uiControl = GetUiControl(name);

            if (uiControl != null && controlList.Contains(uiControl))
            {
                controlList.Remove(uiControl);
            }
        }    

        public UiControl GetUiControl(string name)
        {
            foreach (UiControl uiControl in controlList)
            {
                if (uiControl.name == name)
                {
                    return uiControl;
                }
            }
            return null;
        }

        public bool GetButtonPress(string buttonName)
        {
            if (containerState == ContainerState.Normal)
            {
                UiControl uiControl = GetUiControl(buttonName);

                if (uiControl != null)
                {
                    return uiControl.IsEitherPress();
                }
            }
            return false;
        }   

        public void SetButtonPress(string buttonName)
        {
            UiControl uiControl = GetUiControl(buttonName);

            if (uiControl != null)
            {
                if (uiControl.GetType().Equals(typeof(Button)) || uiControl.GetType().IsSubclassOf(typeof(Button)))
                {
                    Button button = (Button)uiControl;
                    button.SetPress();
                }
            }
        }

        public bool GetChanged(string controlName)
        {
            if (containerState == ContainerState.Normal)
            {
                UiControl uiControl = GetUiControl(controlName);

                if (uiControl != null)
                {
                    return uiControl.changed;
                }
            }
            return false;
        }


        public string GetTextBoxText(string textBoxName)
        {
            UiControl uiControl = GetUiControl(textBoxName);

            if (uiControl != null)
            {
                return uiControl.GetText();
            }
            return "";
        }

        public float GetTextBoxFloat(string textBoxName)
        {
            UiControl uiControl = GetUiControl(textBoxName);

            if (uiControl != null)
            {
                return uiControl.GetTextFloat();
            }
            return 0f;
        }

        public int GetTextBoxInt(string textBoxName)
        {
            UiControl uiControl = GetUiControl(textBoxName);

            if (uiControl != null)
            {
                return uiControl.GetTextInt();
            }
            return 0;
        }


        public void SetControlText(string textBoxName, string text)
        {
            UiControl uiControl = GetUiControl(textBoxName);

            if (uiControl != null)
            {
                uiControl.SetText(text);
            }
        }

        public void SetControlTextColour(string controlName, Color color)
        {
            UiControl uiControl = GetUiControl(controlName);

            if (uiControl != null)
            {
                uiControl.SetTextColor(color);
            }
        }


        private void TranslateInputToContainer(Input input)
        {
            input.TranslateMousePos(input.GetMousePos() - new Vector2(containerCamera.worldViewport.X, containerCamera.worldViewport.Y));
        }

        private void TranslateInputToCamera(Input input)
        {
            input.TranslateMousePos(input.GetMousePos() - new Vector2(containerCamera.worldCameraViewport.X - containerCamera.cameraPosition.X, containerCamera.worldCameraViewport.Y - containerCamera.cameraPosition.Y));
        }

        private void TranslateInputBack(Input input)
        {
            input.ReturnMousePos();
        }

        public virtual void Dispose()
        {

        }

        public void UpdateFocus(Input input, bool screenHasFocus)
        {
            hasFocus = false;
            hasCameraFocus = false;
            SetFocusToTop = false;
            if (!screenHasFocus)
            {
                if (input.InViewPort(containerCamera.worldViewport) && !ignoreInput)
                {
                    hasFocus = true;
                    if (canChangeFocusOrder)
                    {
                        if (input.LeftButtonClick() || input.RightButtonClick() || input.LeftButtonDown() || input.RightButtonDown())
                        {
                            SetFocusToTop = true;
                        }
                    }
                }

                if (input.InViewPort(containerCamera.worldCameraViewport))
                {
                    hasCameraFocus = true;
                }
            }

            if (containerHeading != null)
            {
                if (containerHeading.moving)
                {
                    SetFocusToTop = true;
                    hasFocus = true;
                }
            }
            if (containerBorder != null)
            {
                if (containerBorder.resizing)
                {
                    SetFocusToTop = true;
                    hasFocus = true;
                }
            }
        }

        private void UpdateInternalControls(Input input)
        {
            TranslateInputToContainer(input);
            if (hasBorder)
            {
                containerBorder.SetInViewPort(hasFocus && !hasCameraFocus);
                containerBorder.Update(input);

                if (containerBorder.justCompletedResize)
                {
                    hasCameraFocus = false;
                    hasFocus = false;
                }
            }
            if (hasHeading)
            {
                containerHeading.SetInViewPort(hasFocus && !hasCameraFocus);
                containerHeading.Update(input);

                if (containerHeading.isKill)
                {
                    SetDestroyWithFade();
                }

                isAction = containerHeading.isAction;
            }
            if (hasScrollV)
            {
                containerScrollV.SetInViewPort(hasFocus && !hasCameraFocus);
                containerScrollV.Update(input);
                containerScrollV.UpdateMouseWheelMove(input, hasCameraFocus);
            }
            if (hasScrollH)
            {
                containerScrollH.SetInViewPort(hasFocus && !hasCameraFocus);
                containerScrollH.Update(input);

            }
            TranslateInputBack(input);
        }

     
        private void UpdateUiControls(Input input)
        {
            TranslateInputToCamera(input);

            foreach (UiControl uiControl in controlList)
            {
                uiControl.SetInViewPort(hasCameraFocus);
                uiControl.Update(input);
            }
            TranslateInputBack(input);
        }

        private void UpdateLocation()
        {
            location.Width = containerCamera.worldViewport.Width;
            location.Height = containerCamera.worldViewport.Height;
        }

        private void UpdateState()
        {
            if (containerState == ContainerState.FadeIn)
            {
                containerFade += 0.1f * EngineController.drawUpdateTime;

                if (containerFade > maxTransparency)
                {
                    containerFade = maxTransparency;
                    containerState = ContainerState.Normal;
                }
            }

            else if(containerState == ContainerState.FadeOut)
            {
                containerFade -= 0.1f * EngineController.drawUpdateTime;

                if (containerFade < 0f)
                {
                    containerFade = 0f;
                    containerState = ContainerState.Destroy;
                }
            }

        }


        public override void Update(Input input)
        {
            //We dont call rescale from here
            UpdateState();
            base.Update(input);
            containerCamera.Update();

            UpdateInternalControls(input);

            if (containerState == ContainerState.Normal)
            {
            
                UpdateUiControls(input);
            }

            UpdateLocation();          
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            containerCamera.TranslateDrawToContainer(spriteBatch);

            base.Draw(spriteBatch, containerFade);

            if (hasBorder) { containerBorder.Draw(spriteBatch, containerFade); }
            if (hasHeading) { containerHeading.Draw(spriteBatch, containerFade); }
            if (hasScrollV) { containerScrollV.Draw(spriteBatch, containerFade); }
            if (hasScrollH) { containerScrollH.Draw(spriteBatch, containerFade); }

            containerCamera.TranslateDrawToCamera(spriteBatch);

            foreach (UiControl uiControls in controlList)
            {
                uiControls.Draw(spriteBatch, containerFade);
                if (uiControls.retranslateDraw)
                {
                    containerCamera.TranslateDrawToCamera(spriteBatch);
                }
            }
        //    containerCamera.refreshRequired = false;
        }

    }
}
