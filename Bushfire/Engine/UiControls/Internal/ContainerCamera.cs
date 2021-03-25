using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using BushFire.Engine.Controllers;

namespace BushFire.Engine.UIControls.Abstract
{
    class ContainerCamera
    {
        public Viewport worldViewport;                      //The viewport of the overall Container including the Internal Controls
        public Viewport worldCameraViewport;                //The viewport of the useable part of the container
        public Vector2 cameraPosition;                      //Camera Position
        public Viewport tempWorldViewport;                  //Used when resizing and moving
        private Vector2 moveDifference;                     //Used when resizing and moving

        private bool refreshNextFrame { get; set; }           //Force the internal controls to do a refresh next update      
        public bool refreshRequired { get; set; }             //At the start of the next frame this gets set to true;
        public int borderThickness { get; private set; }    //Thickness of all 4 sides border.  This number never scales
        public int headingHeight { get; private set; }      //Unscaled.  Is off the top border.  This needs to be scaled to use
        public int scrollVThickness { get; private set; }   //Unscaled.  Is off the right side.  This needs to be scaled to use
        public int scrollHThickness { get; private set; }   //Unscaled.  Is off the bottom side.  This needs to be scaled to use

        public int minWindowWidth { get; private set; }
        public int minWindowHeight { get; private set; }
        public int maxWindowWidth { get; private set; }
        public int maxWindowHeight { get; private set; }

        public int preScaleMinWindowWidth { get; private set; }
        public int preScaleMinWindowHeight { get; private set; }
        public int preScaleMaxWindowWidth { get; private set; }
        public int preScaleMaxWindowHeight { get; private set; }

        public int attemptingWidth;
        public int attemptingHeight;


        public ContainerCamera(Rectangle size)
        {
            worldViewport = new Viewport(size);
            UpdateWorldCameraViewport();
            minWindowWidth = worldViewport.Width;
            minWindowHeight = worldViewport.Height;
            maxWindowWidth = worldViewport.Width;
            maxWindowHeight = worldViewport.Height;
            preScaleMinWindowWidth = worldViewport.Width;
            preScaleMinWindowHeight = worldViewport.Height;
            preScaleMaxWindowWidth = worldViewport.Width;
            preScaleMaxWindowHeight = worldViewport.Height;
            attemptingWidth = worldViewport.Width;
            attemptingHeight = worldViewport.Height;
        }

    public void Rescale(Rectangle size, bool scaleMinMax)
        {
            if (scaleMinMax)
            {
                minWindowWidth = CalcScaleInt(preScaleMinWindowWidth);
                minWindowHeight = CalcScaleInt(preScaleMinWindowHeight);
                maxWindowWidth = CalcScaleInt(preScaleMaxWindowWidth);
                maxWindowHeight = CalcScaleInt(preScaleMaxWindowHeight);
            }
            worldViewport = new Viewport(size);
            UpdateWorldCameraViewport();
        }


        private void UpdateWorldCameraViewport()
        {
            worldCameraViewport = new Viewport(CalcLeft(), CalcTop(), CalcWidth(), CalcHeight());
        }
        
        private int CalcLeft()
        {
            return worldViewport.X + borderThickness;
        }

        private int CalcTop()
        {
            return worldViewport.Y + borderThickness + CalcScaleInt(headingHeight);
        }

        private int CalcWidth()
        {
            return worldViewport.Width - borderThickness * 2 - CalcScaleInt(scrollVThickness);
        }

        private int CalcHeight()
        {
            return worldViewport.Height - borderThickness * 2 - CalcScaleInt(scrollHThickness) - CalcScaleInt(headingHeight);
        }

        public int CalcScaleInt(int value)
        {
            return Convert.ToInt32(((float)value) * DisplayController.uiScale);
        }

        private int CalcMaxCameraHeight()
        {
            return maxWindowHeight - (CalcScaleInt(scrollHThickness) + CalcScaleInt(headingHeight) + (borderThickness * 2));
        }

        private int CalcMaxCameraWidth()
        {
            return maxWindowWidth - (CalcScaleInt(scrollVThickness) + (borderThickness * 2));
        }

        public void ToggleBorder(int borderThickness)
        {
            this.borderThickness = borderThickness;
            UpdateWorldCameraViewport();
        }

        public void ToggleHeading(int headingHeight)
        {
            this.headingHeight = headingHeight;
            UpdateWorldCameraViewport();
        }

        public void ToggleHScroll(int thickness)
        {
            scrollHThickness = thickness + 1;
            UpdateWorldCameraViewport();
        }

        public void ToggleVScroll(int thickness)
        {
            scrollVThickness = thickness + 1;
            UpdateWorldCameraViewport();
        }

        public void SetSizeBounds(int minWidth, int minHeight, int maxWidth, int maxHeight)
        {
            preScaleMinWindowWidth = minWidth;
            preScaleMinWindowHeight = minHeight;
            preScaleMaxWindowWidth = maxWidth;
            preScaleMaxWindowHeight = maxHeight;

            minWindowWidth = CalcScaleInt(minWidth);
            minWindowHeight = CalcScaleInt(minHeight);
            maxWindowWidth = CalcScaleInt(maxWidth);
            maxWindowHeight = CalcScaleInt(maxHeight);

            if (worldViewport.Width > maxWidth) { worldViewport.Width = maxWidth; }
            if (worldViewport.Width < minWidth) { worldViewport.Width = minWidth; }
            if (worldViewport.Height > maxHeight) { worldViewport.Height = maxHeight; }
            if (worldViewport.Height < minHeight) { worldViewport.Height = minHeight; }
            UpdateWorldCameraViewport();
            refreshNextFrame = true;
        }

        public void SetSizeBounds(int maxWidth, int maxHeight)
        {
            maxWindowWidth = maxWidth;
            maxWindowHeight = maxHeight;
            UpdateWorldCameraViewport();
            refreshNextFrame = true;
        }

        public bool IsMaxHeight()
        {
            return worldViewport.Height == maxWindowHeight;      
        }

        public bool IsMaxWidth()
        {
            return worldViewport.Width == maxWindowWidth;
        }

        public Vector2 GetViewportCenter()
        {
            return new Vector2(worldViewport.Width * 0.5f, worldViewport.Height * 0.5f);
        }

        public Matrix TranslationMatrix()
        {
            return Matrix.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, 0) *
            Matrix.CreateRotationZ(0f) *
            Matrix.CreateScale(new Vector3(1, 1, 1)) *
            Matrix.CreateTranslation(new Vector3(0,0,0));
        }

        public void MoveCamera(Vector2 cameraMovement)
        {
            cameraPosition += cameraMovement;
            ClampCamera();
         }

        public void ClampCamera()
        {
            if (cameraPosition.Y < 0)
            {
                cameraPosition.Y = 0;
            }
            if (cameraPosition.X < 0)
            {
                cameraPosition.X = 0;
            }
            if (cameraPosition.Y + worldCameraViewport.Height > CalcMaxCameraHeight())
            {
                cameraPosition.Y = CalcMaxCameraHeight() - worldCameraViewport.Height;
            }
            if (cameraPosition.X + worldCameraViewport.Width > CalcMaxCameraWidth())
            {
                cameraPosition.X = CalcMaxCameraWidth() - worldCameraViewport.Width;
            }
        } 
      
        public void TranslateDrawToContainer(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            ScreenController.graphicsDevice.Viewport = worldViewport;
            spriteBatch.Begin();
        }

        public void TranslateDrawToCamera(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            ScreenController.graphicsDevice.Viewport = worldCameraViewport;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, TranslationMatrix());
        }

        public void StartMovingViewport(Input input)
        {
            tempWorldViewport = worldViewport;
            moveDifference = Vector2.Zero;
        }

        public void MoveViewport(Input input)
        {
            moveDifference += input.GetMousePosDifference();
            worldViewport = new Viewport(tempWorldViewport.X + (int)moveDifference.X, tempWorldViewport.Y + (int)moveDifference.Y, tempWorldViewport.Width, tempWorldViewport.Height);
            UpdateWorldCameraViewport();           
        }

        public void StartResizingViewport(Input input)
        {
            tempWorldViewport = worldViewport;
            moveDifference = Vector2.Zero;
        }

        public void ResizeViewport(Input input, Resizing possibleResize)
        {
            moveDifference += input.GetMousePosDifference();

            if (possibleResize == Resizing.WIDTH)
            {
                moveDifference.Y = 0;
            }
            else if(possibleResize == Resizing.HEIGHT)
            {
                moveDifference.X = 0;
            }

            worldViewport = new Viewport(tempWorldViewport.X, tempWorldViewport.Y, tempWorldViewport.Width + (int)moveDifference.X, tempWorldViewport.Height + (int)moveDifference.Y);

            ClampWorldViewport();

            UpdateWorldCameraViewport();
            refreshNextFrame = true;

            attemptingWidth = worldViewport.Width;
            attemptingHeight = worldViewport.Height;
        }

        private void ClampWorldViewport()
        {
            //Need to also clamp screen bounds

            if (worldViewport.Width > maxWindowWidth) { worldViewport.Width = maxWindowWidth; }
            if (worldViewport.Width < minWindowWidth) { worldViewport.Width = minWindowWidth; }
            if (worldViewport.Height > maxWindowHeight) { worldViewport.Height = maxWindowHeight; }
            if (worldViewport.Height < minWindowHeight) { worldViewport.Height = minWindowHeight; }
        }

        public void SnapResizeViewport(Point size)
        {
            worldViewport.Width = size.X + borderThickness;
            worldViewport.Height = size.Y + borderThickness + CalcScaleInt(headingHeight);

            if (worldViewport.Width > attemptingWidth)
            {
                worldViewport.Width = attemptingWidth;
            }
            if (worldViewport.Height > attemptingHeight)
            {
                worldViewport.Height = attemptingHeight;
            }

            ClampWorldViewport();

            UpdateWorldCameraViewport();
            refreshNextFrame = true;
            Debug.WriteLine(worldViewport);

        }

        public void SetRefreshNextFrame()
        {
            //If we are currently in a refresh we dont want to refresh next frame
            //or it will be an endless loop
            if (!refreshRequired)
            {
                refreshNextFrame = true;
            }
        }

        public void Update()
        {
            if (refreshNextFrame)
            {
                refreshRequired = true;
                refreshNextFrame = false;
            }
            else
            {
                refreshRequired = false;
            }

        }

           

    }
}