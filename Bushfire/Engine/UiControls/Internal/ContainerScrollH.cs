using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BushFire.Engine.Controllers;
using BushFire.Engine.ContentStorage;

namespace BushFire.Engine.UIControls.Abstract
{
    class ContainerScrollH : UiControl
    {
        private int thickness;
        private ContainerCamera parentContainerCamera;
        private Rectangle locationScrollGrip;
        private int scrollThickness;
        private Sprite spriteScroll;
        private int windowScrollAreaSize;
        private int trackScrollAreaSize;
        private int windowSize;
        private bool firstDownMovingIsLegit;
        private bool scrolling;
        private bool canGrip;
        private float tempScrollX;
        private float moveDifference;

        public ContainerScrollH(ContainerCamera parentContainerCamera, int thickness, int textureColourBack, int textureColorScroll)
        {
            this.thickness = thickness;
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetSpriteColour(textureColourBack);
            spriteScroll = GraphicsManager.GetSpriteColour(textureColorScroll);
            this.parentContainerCamera = parentContainerCamera;
            name = "ContainerScrollH";
            currentUiScale = DisplayController.uiScale;
            SetSize();
        }

        private void SetSize()
        {
            int borderThickness = parentContainerCamera.borderThickness + 1;
            int scaleThickness = GetIntByScale(thickness);
            int x = 0 + borderThickness;
            int width = parentContainerCamera.worldViewport.Width - borderThickness - borderThickness - parentContainerCamera.scrollVThickness;
            int y = parentContainerCamera.worldViewport.Height - borderThickness - scaleThickness;
            int height = scaleThickness;
            location = new Rectangle(x, y, width, height);
            parentContainerCamera.ToggleHScroll(thickness);
            parentContainerCamera.SetRefreshNextFrame();
            SetScrollBar();
        }

        private void SetScrollBar()
        {
            int minimalGripSize = GetIntByScale(60);
            int scrollCentralY;
            int contentSize;
            float windowContentRatio;
            int gripSize;
            int trackSize;

            scrollThickness = GetIntByScale(thickness / 2);
            scrollCentralY = location.Y + (location.Height - scrollThickness) / 2;
            contentSize = parentContainerCamera.maxWindowWidth -  GetIntByScale(parentContainerCamera.scrollVThickness) - (parentContainerCamera.borderThickness * 2);
            windowSize = parentContainerCamera.worldCameraViewport.Width;
            trackSize = location.Width;
            windowContentRatio = (float)windowSize / (float)contentSize;

            gripSize = (int)((float)trackSize * windowContentRatio);

            if (gripSize < minimalGripSize) { gripSize = minimalGripSize; }

            if (gripSize > trackSize) { gripSize = trackSize; }

            windowScrollAreaSize = contentSize - windowSize;
            trackScrollAreaSize = trackSize - gripSize;
            locationScrollGrip = new Rectangle(0, scrollCentralY, gripSize, scrollThickness);
            SetGripPosition();
        }

        private void SetGripPosition()
        {
            float windowPositionRatio;
            int gripPositionOnTrack;

            if (windowScrollAreaSize > 0)
            {
                windowPositionRatio = (int)parentContainerCamera.cameraPosition.X / (float)windowScrollAreaSize;
                gripPositionOnTrack = (int)((float)trackScrollAreaSize * windowPositionRatio) + location.X;
            }
            else  //We are a full track
            {
                gripPositionOnTrack = location.X;
            }
            parentContainerCamera.ClampCamera();
            locationScrollGrip.X = gripPositionOnTrack;
        }

        protected override void Rescale()
        {
            currentUiScale = DisplayController.uiScale;
            SetSize();
        }     

        private void UpdateScrollHover(Input input)
        {
            canGrip = false;

            if (inViewport && input.InViewPort(locationScrollGrip))
            {
                canGrip = true;
                if (!scrolling)
                {
                    input.ChangeMouseCursor(CursorType.HANDFINGER);
                }
            }
        }

        private void UpdateScrollCheck(Input input)
        {
            if (!input.LeftButtonDown()) { firstDownMovingIsLegit = true; }

            if (input.LeftButtonDown() && canGrip && !scrolling && firstDownMovingIsLegit)
            {
                scrolling = true;
                StartScrolling();
            }

            if (input.LeftButtonDown() && !scrolling) { firstDownMovingIsLegit = false; }
            if (!input.LeftButtonDown() && scrolling) { scrolling = false; }
        }

        private void StartScrolling()
        {
            tempScrollX = (int)locationScrollGrip.X;
            moveDifference = 0;
        }

        private void UpdateScroll(Input input)
        {
            if (scrolling)
            {
                input.ChangeMouseCursor(CursorType.HANDCLOSE);

                if (!parentContainerCamera.IsMaxWidth())
                {
                    moveDifference += input.GetMousePosDifference().X;
                    locationScrollGrip.X = (int)tempScrollX + (int)moveDifference;
                    float newGripPositionRatio = (float)(locationScrollGrip.X - location.X) / (float)trackScrollAreaSize;
                    parentContainerCamera.cameraPosition.X = newGripPositionRatio * (float)windowScrollAreaSize;

                    if (locationScrollGrip.X < location.X)
                    {
                        locationScrollGrip.X = location.X;
                    }

                    if (locationScrollGrip.X > trackScrollAreaSize + location.X)
                    {
                        locationScrollGrip.X = trackScrollAreaSize + location.X;
                    }
                    parentContainerCamera.ClampCamera();
                }
            }
        }

        private void UpdateScrollClick(Input input)
        {

            if (!scrolling && input.LeftButtonClick() && input.InViewPort(location) && !input.InViewPort(locationScrollGrip))
            {
                if (input.GetMousePos().X < locationScrollGrip.X)
                {
                    //Going up
                    parentContainerCamera.cameraPosition.X -= (float)windowSize;
                    parentContainerCamera.ClampCamera();
                }
                else if (input.GetMousePos().X > locationScrollGrip.X + locationScrollGrip.Width)
                {
                    //Going down
                    parentContainerCamera.cameraPosition.X += (float)windowSize;
                    parentContainerCamera.ClampCamera();
                }
            }
        }

        private void UpdateMouseWheelMove(Input input)
        {
            if (!scrolling && input.InViewPort(location))
            {
                if (input.scrollChangeValue > 0)
                {
                    //Going up
                    parentContainerCamera.cameraPosition.X -= ((float)windowSize * 0.1f);
                    parentContainerCamera.ClampCamera();
                }
                else if (input.scrollChangeValue < 0)
                {
                    parentContainerCamera.cameraPosition.X += ((float)windowSize * 0.1f);
                    parentContainerCamera.ClampCamera();
                }
            }
        }

        public override void Update(Input input)
        {
            if (parentContainerCamera.refreshRequired)
            {
                SetSize();
            }

            SetGripPosition();
            UpdateScrollHover(input);
            UpdateScrollCheck(input);
            UpdateScroll(input);
            UpdateScrollClick(input);
            UpdateMouseWheelMove(input);
            base.Update(input);
        }

        public override void Draw(SpriteBatch spriteBatch, float containerFade)
        {
            base.Draw(spriteBatch, containerFade);

            spriteBatch.Draw(spriteScroll.texture2D, locationScrollGrip, spriteScroll.location, spriteBack.color * transparency * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}
