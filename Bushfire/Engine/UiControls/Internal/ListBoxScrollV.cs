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
using BushFire.Engine.UIControls.Internal;

namespace BushFire.Engine.UIControls.Abstract
{
    class ListBoxScrollV : UiControl
    {
        public int width = 16;
        private ListBoxCamera listBoxCamera;
        private Rectangle locationScrollGrip;
        private int scrollWidth;
        private Sprite spriteScroll;
        private int windowScrollAreaSize;
        private int trackScrollAreaSize;
        private int windowSize;
        private Rectangle listBoxLocation;
        private bool canGrip;
        private bool firstDownMovingIsLegit;
        private bool scrolling;
        private float tempScrollY;
        private float moveDifference;
        public bool parentHasFocus;
        public bool inListBoxViewPort;

        public ListBoxScrollV(ListBoxCamera listBoxCamera, Rectangle listBoxLocation)
        {
            this.listBoxLocation = listBoxLocation;
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetSpriteColour(6);
            spriteScroll = GraphicsManager.GetSpriteColour(20);
            this.listBoxCamera = listBoxCamera;
            currentUiScale = DisplayController.uiScale;
            SetSize();
        }

        private void SetSize()
        {
            int scaleWidth = GetIntByScale(width);
            int x = listBoxLocation.X + listBoxLocation.Width - scaleWidth;
            int y = listBoxLocation.Y;
            int height = listBoxLocation.Height;
            location = new Rectangle(x, y, scaleWidth, height);
            SetScrollBar();
        }

        
        private void SetScrollBar()
        {
            int minimalGripSize = GetIntByScale(60);         
            int scrollCentralX;
            float contentSize;
            float windowContentRatio;
            int gripSize;
            int trackSize;

            scrollWidth = GetIntByScale(width / 2);
            scrollCentralX = location.X + (location.Width - scrollWidth) / 2;
            contentSize = listBoxCamera.maxHeight;
            windowSize = listBoxCamera.viewport.Height;
            trackSize = location.Height;
            windowContentRatio = (float)windowSize / (float)contentSize;
            gripSize = (int)((float)trackSize * windowContentRatio);

            if (gripSize < minimalGripSize) { gripSize = minimalGripSize; }

            if (gripSize > trackSize) { gripSize = trackSize; }

            windowScrollAreaSize = (int)contentSize - windowSize;
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
                windowPositionRatio = (int)listBoxCamera.cameraPosition.Y / (float)windowScrollAreaSize;
                gripPositionOnTrack = (int)((float)trackScrollAreaSize * windowPositionRatio) + location.Y;
            }
            else  //We are a full track
            {
                gripPositionOnTrack = location.Y;
            }

            listBoxCamera.ClampCamera();
            locationScrollGrip.Y = gripPositionOnTrack;
        }  

        public void Rescale(Rectangle listBoxLocation)
        {
            this.listBoxLocation = listBoxLocation;
            currentUiScale = DisplayController.uiScale;
            SetSize();
        }      

        private void UpdateScrollHover(Input input)
        {
            canGrip = false;

            if (input.InViewPort(locationScrollGrip))
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

                if (!listBoxCamera.IsMaxHeight())
                {
                    moveDifference += input.GetMousePosDifference().Y;
                    locationScrollGrip.Y = (int)tempScrollY + (int)moveDifference;
                    float newGripPositionRatio = (float)(locationScrollGrip.Y - location.Y) / (float)trackScrollAreaSize;

                    listBoxCamera.cameraPosition.Y = newGripPositionRatio * (float)windowScrollAreaSize;

                    if (locationScrollGrip.Y < location.Y)
                    {
                        locationScrollGrip.Y = location.Y;
                    }

                    if (locationScrollGrip.Y > trackScrollAreaSize + location.Y)
                    {
                        locationScrollGrip.Y = trackScrollAreaSize + location.Y;
                    }
                    listBoxCamera.ClampCamera();
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
                    listBoxCamera.cameraPosition.Y -= (float)windowSize;
                    listBoxCamera.ClampCamera();
                }
                else if (input.GetMousePos().Y > locationScrollGrip.Y + locationScrollGrip.Height)
                {
                    //Going down
                    listBoxCamera.cameraPosition.Y += (float)windowSize;
                    listBoxCamera.ClampCamera();
                }
            }
        }       

        public void UpdateMouseWheelMove(Input input)
        {
            if (!scrolling && parentHasFocus && inListBoxViewPort)
            {
                if (input.scrollChangeValue > 0)
                {
                    //Going up
                    listBoxCamera.cameraPosition.Y -= ((float)windowSize * 0.1f);
                    listBoxCamera.ClampCamera();
                }
                else if (input.scrollChangeValue < 0)
                {
                    listBoxCamera.cameraPosition.Y += ((float)windowSize * 0.1f);
                    listBoxCamera.ClampCamera();
                }
            }
        }      

        public override void Update(Input input)
        {
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
