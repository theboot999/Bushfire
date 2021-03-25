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
    class ContainerScrollV : UiControl
    {
        private int thickness;
        protected ContainerCamera parentContainerCamera;
        private Rectangle locationScrollGrip;
        private int scrollWidth;
        private Sprite spriteScroll;
        private int windowScrollAreaSize;
        private int trackScrollAreaSize;
        private int windowSize;
        private bool canGrip;
        private bool firstDownMovingIsLegit;
        private bool scrolling;
        private float tempScrollY;
        private float moveDifference;

        public ContainerScrollV(ContainerCamera parentContainerCamera, int thickness, int textureColourBack, int textureColorScroll)
        {
            this.thickness = thickness;
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetSpriteColour(textureColourBack);
            spriteScroll = GraphicsManager.GetSpriteColour(textureColorScroll);
            this.parentContainerCamera = parentContainerCamera;
            name = "ContainerScrollV";
            currentUiScale = DisplayController.uiScale;
            SetSize();
        }

        private void SetSize()
        {
            int borderThickness = parentContainerCamera.borderThickness + 1;
            int headingThickness = GetIntByScale(parentContainerCamera.headingHeight);
            int scaleThickness = GetIntByScale(thickness);
            int x = parentContainerCamera.worldViewport.Width - borderThickness - scaleThickness;
            int width = scaleThickness;
            int y = borderThickness + headingThickness;
            int height = parentContainerCamera.worldViewport.Height - headingThickness - borderThickness - borderThickness;
            location = new Rectangle(x, y, width, height);
            parentContainerCamera.ToggleVScroll(thickness);
            parentContainerCamera.SetRefreshNextFrame();
            SetScrollBar();           
        }     

        private void SetScrollBar()
        {
            int minimalGripSize = GetIntByScale(60);
            int scrollCentralX;
            int contentSize;         
            float windowContentRatio;
            int gripSize;
            int trackSize;

            scrollWidth = GetIntByScale(thickness / 2);
            scrollCentralX = location.X + (location.Width - scrollWidth) / 2;
            contentSize = parentContainerCamera.maxWindowHeight - GetIntByScale(parentContainerCamera.headingHeight) - GetIntByScale(parentContainerCamera.scrollHThickness) - (parentContainerCamera.borderThickness * 2);
            windowSize = parentContainerCamera.worldCameraViewport.Height;
            trackSize = location.Height;
            windowContentRatio = (float)windowSize / (float)contentSize;
            
            gripSize = (int)((float)trackSize * windowContentRatio);
            
            if (gripSize < minimalGripSize) { gripSize = minimalGripSize; }

            if (gripSize > trackSize) { gripSize = trackSize; }

            windowScrollAreaSize = contentSize - windowSize;
            trackScrollAreaSize = trackSize - gripSize;
            locationScrollGrip = new Rectangle(scrollCentralX, 0, scrollWidth, gripSize);            
            SetGripPosition();         
        }
      
        private void SetGripPosition()
        {
            float windowPositionRatio;
            int gripPositionOnTrack;

            if (windowScrollAreaSize > 0)
            {
                windowPositionRatio = (int)parentContainerCamera.cameraPosition.Y / (float)windowScrollAreaSize;
                gripPositionOnTrack = (int)((float)trackScrollAreaSize * windowPositionRatio) + location.Y;
            }
            else  //We are a full track
            {
                gripPositionOnTrack = location.Y;
            }

            parentContainerCamera.ClampCamera();
            locationScrollGrip.Y = gripPositionOnTrack;
        }

        protected override void Rescale()
        {
            currentUiScale = DisplayController.uiScale;
            SetSize();
        }

        private void UpdateScrollHover(Input input)
        {
            canGrip = false;

            if (inViewport  && input.InViewPort(locationScrollGrip))
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
            tempScrollY = (int)locationScrollGrip.Y;
            moveDifference = 0;
        }

        private void UpdateScroll(Input input)
        {
            if (scrolling)
            {
                input.ChangeMouseCursor(CursorType.HANDCLOSE);

                if (!parentContainerCamera.IsMaxHeight())
                {
                    moveDifference += input.GetMousePosDifference().Y;
                    locationScrollGrip.Y = (int)tempScrollY + (int)moveDifference;
                    float newGripPositionRatio = (float)(locationScrollGrip.Y - location.Y) / (float)trackScrollAreaSize;
                    parentContainerCamera.cameraPosition.Y = newGripPositionRatio * (float)windowScrollAreaSize;

                    if (locationScrollGrip.Y < location.Y)
                    {
                        locationScrollGrip.Y = location.Y;
                    }

                    if (locationScrollGrip.Y > trackScrollAreaSize + location.Y)
                    {
                        locationScrollGrip.Y = trackScrollAreaSize + location.Y;
                    }
                    parentContainerCamera.ClampCamera();
                }
            }
        }

        private void UpdateScrollClick(Input input)
        {

            if (!scrolling && input.LeftButtonClick() && input.InViewPort(location) && !input.InViewPort(locationScrollGrip))
            {
                if (input.GetMousePos().Y < locationScrollGrip.Y)
                {
                    //Going up
                    parentContainerCamera.cameraPosition.Y -= (float)windowSize;
                    parentContainerCamera.ClampCamera();
                }
                else if (input.GetMousePos().Y > locationScrollGrip.Y + locationScrollGrip.Height)
                {
                    //Going down
                    parentContainerCamera.cameraPosition.Y += (float)windowSize;
                    parentContainerCamera.ClampCamera();
                }
            }
        }

        public void UpdateMouseWheelMove(Input input, bool hasFocus)
        {
            if (!scrolling && hasFocus)
            {
                if (input.scrollChangeValue > 0)
                {
                    //Going up
                    parentContainerCamera.cameraPosition.Y -= ((float)windowSize * 0.1f);
                    parentContainerCamera.ClampCamera();
                }
                else if (input.scrollChangeValue < 0)
                {
                    parentContainerCamera.cameraPosition.Y += ((float)windowSize * 0.1f);
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
            UpdateMouseWheelMove(input, input.InViewPort(location));
            base.Update(input);

        }


        public override void Draw(SpriteBatch spriteBatch, float containerFade)
        {
            base.Draw(spriteBatch, containerFade);
            spriteBatch.Draw(spriteScroll.texture2D, locationScrollGrip, spriteScroll.location, spriteBack.color * transparency * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}
