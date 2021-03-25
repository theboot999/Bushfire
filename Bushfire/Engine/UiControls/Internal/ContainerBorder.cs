using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using BushFire.Engine.Controllers;
using BushFire.Engine.ContentStorage;

namespace BushFire.Engine.UIControls.Abstract
{
    enum Resizing
    {
        NONE,
        WIDTH,
        HEIGHT,
        BOTH
    }

    class ContainerBorder : UiControl
    {
        protected ContainerCamera parentContainerCamera;

        //this is unscaled
        private int borderThickness;
        private Rectangle left;
        private Rectangle top;
        private Rectangle right;
        private Rectangle bottom;

        private Rectangle resizeRight;
        private Rectangle resizeBottom;

        Resizing allowedResize;

        public bool resizing;
        public bool justCompletedResize;
        bool canResize;
        private bool firstDownMovingIsLegit;
      //  Resizing possibleResize;


        public ContainerBorder(ContainerCamera parentContainerCamera, int borderThickness, Resizing allowedResize, int textureColour)
        {
            
            this.parentContainerCamera = parentContainerCamera;
            spriteBack = GraphicsManager.GetSpriteColour(textureColour);
            this.borderThickness = borderThickness;
            this.allowedResize = allowedResize;

            name = "ContainerBorder";
            SetSize();
            parentContainerCamera.ToggleBorder(borderThickness);

        }

        private void SetSize()
        {
            //Border thickness is fixed.  It does not scale

            left = new Rectangle(0, borderThickness, borderThickness, parentContainerCamera.worldViewport.Height - borderThickness - borderThickness);
            top = new Rectangle(0, 0, parentContainerCamera.worldViewport.Width, borderThickness);
            right = new Rectangle(parentContainerCamera.worldViewport.Width - borderThickness, borderThickness, borderThickness, parentContainerCamera.worldViewport.Height - borderThickness - borderThickness);
            bottom = new Rectangle(0, parentContainerCamera.worldViewport.Height - borderThickness, parentContainerCamera.worldViewport.Width, borderThickness);

            int resizeThickness = 8;
            if (borderThickness > 8)
            {
                resizeThickness = borderThickness;
            }
            resizeRight = new Rectangle(parentContainerCamera.worldViewport.Width - resizeThickness, resizeThickness, resizeThickness, parentContainerCamera.worldViewport.Height - resizeThickness - resizeThickness);
            resizeBottom = new Rectangle(0, parentContainerCamera.worldViewport.Height - resizeThickness, parentContainerCamera.worldViewport.Width, resizeThickness);
        }

        protected override void Rescale()
        {
            currentUiScale = DisplayController.uiScale;
            SetSize();
        }



        private void UpdateResizingHover(Input input)
        {
            canResize = false;

            if (!resizing)
            {
                if (allowedResize == Resizing.WIDTH || allowedResize == Resizing.BOTH)
                {
                    if (input.InViewPort(resizeRight))
                    {
                        canResize = true;
                        input.ChangeMouseCursor(CursorType.ARROWDIAGONAL);
                    }
                }

                if (allowedResize == Resizing.HEIGHT || allowedResize == Resizing.BOTH)
                {
                    if (input.InViewPort(resizeBottom))
                    {
                        canResize = true;
                        input.ChangeMouseCursor(CursorType.ARROWDIAGONAL);
                    }
                }
            }
        }

        private void UpdateResize(Input input)
        {
            justCompletedResize = false;
            if (!input.LeftButtonDown()) { firstDownMovingIsLegit = true; }


            if (input.LeftButtonDown() && canResize && !resizing && firstDownMovingIsLegit)
            {
                resizing = true;
                parentContainerCamera.StartResizingViewport(input);
            }

            if (input.LeftButtonDown() && !resizing) { firstDownMovingIsLegit = false; }

            if (!input.LeftButtonDown() && resizing)
            {
                resizing = false;
                justCompletedResize = true;
            }

            if (resizing)
            {
                parentContainerCamera.ResizeViewport(input, allowedResize);
                input.ChangeMouseCursor(CursorType.ARROWDIAGONAL);
            }
        }



        public override void Update(Input input)
        {
            UpdateResizingHover(input);
            UpdateResize(input);

            if (parentContainerCamera.refreshRequired)
            {
                SetSize();
            }

            base.Update(input);
        }


        public override void Draw(SpriteBatch spriteBatch, float containerFade)
        {
            spriteBatch.Draw(spriteBack.texture2D, left, spriteBack.location, spriteBack.color * transparency * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.Draw(spriteBack.texture2D, top, spriteBack.location, spriteBack.color * transparency * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.Draw(spriteBack.texture2D, right, spriteBack.location, spriteBack.color * transparency * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.Draw(spriteBack.texture2D, bottom, spriteBack.location, spriteBack.color * transparency * containerFade, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}
