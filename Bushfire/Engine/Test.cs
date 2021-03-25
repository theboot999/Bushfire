using System;
using BushFire.Engine.Controllers;
using BushFire.Game;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BushFire.Engine
{
    class Cameraa
    {
        private float rotation;
        private Vector2 cameraMax;
        private Viewport viewport;
        private Vector2 cameraPosition;
        private float zoom;

        public Cameraa(Viewport viewport)
        {
            viewport = new Viewport(0, 0, 1000, 1000);
            cameraMax = new Vector2(3000, 3000);
            this.viewport = viewport;
            zoom = 1f;
            rotation = 0f;
        }

        public Vector2 GetViewportCenter()
        {
            return new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
        }

        public Matrix TranslationMatrix(float scrollSpeedX, float scrollSpeedY)
        {
            return Matrix.CreateTranslation(-cameraPosition.X * scrollSpeedX, -cameraPosition.Y * scrollSpeedY, 0) *
            Matrix.CreateRotationZ(rotation) *
            Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
            Matrix.CreateTranslation(new Vector3(GetViewportCenter(), 0));
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TranslationMatrix(1f, 1f));
        }

        public Vector2 ScreenToWorld(Vector2 screenClick)
        {
            return Vector2.Transform(screenClick - new Vector2(viewport.X, viewport.Y), Matrix.Invert(TranslationMatrix(1f, 1f)));
        }

        public void MoveCamera(Vector2 cameraMovement)
        {
            cameraPosition += cameraMovement;
            ClampCamera();
        }

        public void CenterOn(Vector2 position)
        {
            cameraPosition = position;
            ClampCamera();
        }

        private void ClampCamera()
        {
            Vector2 cameraWorldMin = Vector2.Transform(Vector2.Zero, Matrix.Invert(TranslationMatrix(1f, 1f)));
            Vector2 cameraSize = new Vector2(viewport.Width, viewport.Height) / zoom;
            Vector2 positionOffset = cameraPosition - cameraWorldMin;
            cameraPosition = Vector2.Clamp(cameraWorldMin, Vector2.Zero, cameraMax - cameraSize) + positionOffset;
        }
    }
}