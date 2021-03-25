using System;
using BushFire.Engine.Controllers;
using BushFire.Game;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BushFire.Engine
{
    class Camera
    {
        private float Rotation = 0f;
        private float[] zoomList;
        public int zoomCurrentIndex { get; private set; }
        private int zoomNewIndex;
        private Vector2 cameraMax;
        private Vector2 cameraMin;
        public Viewport viewport { get; set; }
        private readonly Vector2 viewportLocation;
        public Vector2 cameraPosition { get; protected set; }
        //protected Vector2 cameraMovement = Vector2.Zero;
        public float zoom;
        protected Boolean zoomCameraIn;
        protected Boolean zoomCameraOut;

        public bool lockZoom;
        public bool lockMove;
        public bool lockKeyMove;
        public bool clampToMap;
        public DrawPoints drawPoints;

        public Camera(Viewport viewport, int cameraZoomIndex, Vector2 minSize, Vector2 worldSize, bool isMiniCam)
        {
            this.viewport = viewport;
            viewportLocation = new Vector2(viewport.X, viewport.Y);
            zoomCurrentIndex = cameraZoomIndex;
            cameraMin = minSize;
            cameraMax = worldSize;
            InitZoom(isMiniCam, worldSize);
            drawPoints = new DrawPoints();
        }

        public void UpdateViewport(Viewport viewport)
        {
            this.viewport = viewport;
            CenterOn(cameraPosition);
        }

        private void InitZoom(bool isMiniCam, Vector2 worldSize)
        {
            if (isMiniCam)
            {
                //TODO: find some good values
             //   if (worldSize.X < 1001)
              //  {
                    zoomList = new float[4];
                    zoomList[0] = 0.3f;
                    zoomList[1] = 0.5f;
                    zoomList[2] = 0.8f;
                    zoomList[3] = 1f;
             /*   }
                else if (worldSize.X < 2001)
                {
                    zoomList = new float[4];
                    zoomList[0] = 0.25f;
                    zoomList[1] = 0.45f;
                    zoomList[2] = 0.75f;
                    zoomList[3] = 0.95f;
                }
                else if (worldSize.X < 3001)
                {
                    zoomList = new float[4];
                    zoomList[0] = 0.2f;
                    zoomList[1] = 0.4f;
                    zoomList[2] = 0.7f;
                    zoomList[3] = 0.9f;
                }
                else
                {
                    zoomList = new float[4];
                    zoomList[0] = 0.15f;
                    zoomList[1] = 0.35f;
                    zoomList[2] = 0.6f;
                    zoomList[3] = 0.8f;
                }*/



            }
            else
            {
                zoomList = new float[10];
                zoomList[0] = 0.15f;
                zoomList[1] = 0.2f;
                zoomList[2] = 0.3f;
                zoomList[3] = 0.4f;
                zoomList[4] = 0.5f;
                zoomList[5] = 0.8f;
                zoomList[6] = 1f;
                zoomList[7] = 1.25f;
                zoomList[8] = 1.5f;
                zoomList[9] = 2f;
            }
     
            zoom = zoomList[zoomCurrentIndex];
            zoomNewIndex = zoomCurrentIndex;   
        }



        #region GET
        public Vector2 GetViewportCenter()
        {
            return new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
        }

        public Matrix TranslationMatrix(float scrollSpeedX, float scrollSpeedY)
        {
            return Matrix.CreateTranslation(-cameraPosition.X * scrollSpeedX, -cameraPosition.Y * scrollSpeedY, 0) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
            Matrix.CreateTranslation(new Vector3(GetViewportCenter(), 0));
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TranslationMatrix(1f, 1f));
        }

        public void setZoom(float value)
        {
            zoom = value;
        }

        public void setZoomIndex(int value)
        {
            zoomCurrentIndex = value;
            zoom = zoomList[zoomCurrentIndex];
        }

        public Vector2 ScreenToWorld(Vector2 screenClick)
        {
            return Vector2.Transform(screenClick - new Vector2(viewport.X, viewport.Y), Matrix.Invert(TranslationMatrix(1f, 1f)));
        }    



        #endregion

        #region METHODS - MOVEMENT

        public void MoveCamera(Vector2 cameraMovement)
        {
            if (!lockMove)
            {
                cameraPosition += cameraMovement;
                Clamp();

            }
        }

        public void CenterOn(Vector2 position)
        {
            if (!lockMove)
            {
                cameraPosition = position;
                Clamp();
 
            }
        }

        private void Clamp()
        {
            if (clampToMap)
            {
                Vector2 cameraWorldMin = Vector2.Transform(Vector2.Zero, Matrix.Invert(TranslationMatrix(1f, 1f)));
                Vector2 cameraSize = new Vector2(viewport.Width, viewport.Height) / zoom;
                Vector2 positionOffset = cameraPosition - cameraWorldMin;
                cameraPosition = Vector2.Clamp(cameraWorldMin, cameraMin, cameraMax - cameraSize) + positionOffset;
            }
        }

        #endregion

        #region METHODS - UPDATE

        public void UpdateDrawPoints(float tileSize, int worldWidth, int worldHeight)
        {
            Vector2 topLeft = ScreenToWorld(Vector2.Zero);
            Vector2 botRight = ScreenToWorld(new Vector2(viewport.X + viewport.Width, viewport.Y + viewport.Height));
            drawPoints.UpdateDrawPoints(tileSize, worldWidth, worldHeight, topLeft, botRight);
        }



        protected void SetZoom()
        {
            if (zoomNewIndex == zoomCurrentIndex)  //only zoom if we are not at our index
            {
                if (zoomCameraIn)
                {
                    if (zoomCurrentIndex < zoomList.Length - 1)  //we may need to increase this by one.
                    {
                        zoomNewIndex = zoomCurrentIndex + 1;
                    }
                }
                else if (zoomCameraOut)
                {
                    if (zoomCurrentIndex > 0)
                    {
                        zoomNewIndex = zoomCurrentIndex - 1;
                    }
                }
            }

            if (zoomNewIndex > zoomCurrentIndex)
            {
                //  zoom += zoomSpeeds[zoomNewIndex];
                zoom += CalcZoomSpeed();
                if (zoom > zoomList[zoomNewIndex])
                {
                    zoom = zoomList[zoomNewIndex];
                    zoomCameraIn = false;
                    zoomCameraOut = false;
                    zoomCurrentIndex = zoomNewIndex;
                }
                Clamp();
            }

            if (zoomNewIndex < zoomCurrentIndex) //we are zooming out
            {
                //  zoom -= zoomSpeeds[zoomNewIndex];
                zoom -= CalcZoomSpeed();

                if (zoom < zoomList[zoomNewIndex])
                {
                    zoom = zoomList[zoomNewIndex];
                    zoomCameraIn = false;
                    zoomCameraOut = false;
                    zoomCurrentIndex = zoomNewIndex;
                }
                Clamp();
            }
        }
        
        private float CalcZoomSpeed()
        {
            return 0.02f * zoom * EngineController.drawUpdateTime;
        }



        private void UpdateMouseZoom(Input input, bool hasCameraFocus)
        {
            if (!lockZoom && hasCameraFocus)
            {
                if (input.getMouseScrollState() == MouseScrollState.SCROLLDOWN)
                {
                    zoomCameraIn = true;
                }
                else
                {
                    zoomCameraIn = false;
                }

                if (input.getMouseScrollState() == MouseScrollState.SCROLLUP)
                {
                    zoomCameraOut = true;
                }
                else
                {
                    zoomCameraOut = false;
                }
                SetZoom();
            }
        }

        //    Vector2 keyCameraMoveAmount;

        public void UpdateKeyMove(Input input)
        {
            if (!lockKeyMove && !lockMove)
            {
                /*
                float maxAmount = (20.2f - (zoom * 0.5f)) * GameController.gameTime;

                if (input.IsKeyMapDown(KeyMap.FastMovingCamera))
                {
                    maxAmount *= 2.5f;
                }

                

                if (input.IsKeyMapDown(KeyMap.MoveCameraRight))
                {
                    keyCameraMoveAmount.X += 0.01f * GameController.gameTime + (zoom * 0.5f);
                }
                else if (input.IsKeyMapDown(KeyMap.MoveCameraLeft))
                {
                    keyCameraMoveAmount.X -= 0.01f * GameController.gameTime + (zoom * 0.5f);
                }
                else
                {
                    if (keyCameraMoveAmount.X > 0)
                    {
                        keyCameraMoveAmount.X -= 0.02f * GameController.gameTime + (zoom * 0.5f);
                    }
                    else if (keyCameraMoveAmount.X < 0)
                    {
                        keyCameraMoveAmount.X += 0.02f * GameController.gameTime + (zoom * 0.5f);
                    }
                }

                if (input.IsKeyMapDown(KeyMap.MoveCameraUp))
                {
                    keyCameraMoveAmount.Y -= 0.01f * GameController.gameTime;
                }
                else if (input.IsKeyMapDown(KeyMap.MoveCameraDown))
                {
                    keyCameraMoveAmount.Y += 0.01f * GameController.gameTime;
                }
                else
                {
                    if (keyCameraMoveAmount.Y > 0)
                    {
                        keyCameraMoveAmount.Y -= 0.02f * GameController.gameTime;
                    }
                    else if (keyCameraMoveAmount.Y < 0)
                    {
                        keyCameraMoveAmount.Y += 0.02f * GameController.gameTime;
                    }
                }

                if (keyCameraMoveAmount != Vector2.Zero)
                {
                    if (!input.IsKeyMapDown(KeyMap.MoveCameraRight) && !input.IsKeyMapDown(KeyMap.MoveCameraLeft) && keyCameraMoveAmount.X < 0.3f * GameController.gameTime && keyCameraMoveAmount.X > -0.3f * GameController.gameTime)  //Setting the minimum value to zero so we dont get camera shake
                    {
                        keyCameraMoveAmount.X = 0;
                    }
                    if(!input.IsKeyMapDown(KeyMap.MoveCameraUp) && !input.IsKeyMapDown(KeyMap.MoveCameraDown) && keyCameraMoveAmount.Y < 0.3f * GameController.gameTime && keyCameraMoveAmount.Y > -0.3f * GameController.gameTime)
                    {
                        keyCameraMoveAmount.Y = 0;
                    }

                    keyCameraMoveAmount.X = MathHelper.Clamp(keyCameraMoveAmount.X, maxAmount * -1, maxAmount);
                    keyCameraMoveAmount.Y = MathHelper.Clamp(keyCameraMoveAmount.Y, maxAmount * -1, maxAmount);


                    MoveCamera(keyCameraMoveAmount);

                  

                }

                */
                float amount = 22f - (zoom * 0.5f);
                if (input.IsKeyMapDown(KeyMap.FastMovingCamera))
                {
                    amount *= 2.5f;
                }

                if (input.IsKeyMapDown(KeyMap.MoveCameraRight))
                {
                    MoveCamera(new Vector2(1, 0) * EngineController.drawUpdateTime * amount);
                }
                if (input.IsKeyMapDown(KeyMap.MoveCameraLeft))
                {
                    MoveCamera(new Vector2(-1, 0) * EngineController.drawUpdateTime * amount);
                }
                if (input.IsKeyMapDown(KeyMap.MoveCameraUp))
                {
                    MoveCamera(new Vector2(0, -1) * EngineController.drawUpdateTime * amount);
                }
                if (input.IsKeyMapDown(KeyMap.MoveCameraDown))
                {
                    MoveCamera(new Vector2(0, 1) * EngineController.drawUpdateTime * amount);
                }










            }
        }

        #endregion

        #region CALLS

        bool centerOn;

        public virtual void Update(Input input, bool hasCameraFocus)
        {
            UpdateMouseZoom(input, hasCameraFocus);
            UpdateKeyMove(input);

            if (input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.V))
            {
                centerOn = !centerOn;
            }

            if (centerOn)
            {
              //  CenterOn(WorldController.world.GetVehiclePosition());
            }
        }

        #endregion
    }
}