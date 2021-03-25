using BushFire.Engine.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.UIControls.Internal
{
    class ListBoxCamera
    {
        //Dont adjust the zoom if this is being used for a listbox
        //Only if its a gameviewboxcamera
        public Viewport viewport { get; set; }
        public Vector2 cameraPosition;
        private Vector2 cameraMovement = Vector2.Zero;
        public float maxHeight;
        public float zoom = 1f;

        public ListBoxCamera()
        {

        }


        public void UpdateViewport(Viewport viewport)
        {
            this.viewport = viewport;
        }

        public Matrix TranslationMatrix()
        {
            return Matrix.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, 0) *
            Matrix.CreateRotationZ(0f) *
            Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
            Matrix.CreateTranslation(new Vector3(0, 0, 0));
        }

        public void ClampCamera()
        {
            if (cameraPosition.Y < 0)
            {
                cameraPosition.Y = 0;
            }
            if (cameraPosition.Y + viewport.Height > maxHeight)
            {
                cameraPosition.Y = maxHeight - viewport.Height;
            }
        }

        public bool IsMaxHeight()
        {
            return viewport.Height == maxHeight;
        }

        public void TranslateDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            ScreenController.graphicsDevice.Viewport = viewport;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, TranslationMatrix());
        }
    }
}
