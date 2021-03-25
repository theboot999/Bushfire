using BushFire.Editor.Tech;
using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.Storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Editor.Containers
{
    //So create another settings view, where its load save
    //also cycle throguh the actual different looks off the tilesets
    //also ont here can have hide shadows, hide borders with buttons
    //however when switching views to shadows the shadows get turned back on

    class EditorView : Container
    {
        EditorParams editorParams;
        private Camera camera { get; set; }
        public CompressedBuilding compressedBuilding;
        private BorderMap borderMap;
        public Spot activeSpot;
        public Point offSet;
        private bool inGrid;
        public float snap = 1f;


        public EditorView(Rectangle location, DockType dockType, CompressedBuilding compressedBuilding, EditorParams editorParams) : base(location, dockType, true)
        {
            this.editorParams = editorParams;
            camera = new Camera(containerCamera.worldCameraViewport, 4, Vector2.Zero, new Vector2(1000, 1000), false);
            camera.CenterOn(new Vector2(500, 500));
            camera.setZoomIndex(5);
            camera.lockZoom = false;
            camera.lockKeyMove = false;
            canChangeFocusOrder = false;
            borderMap = new BorderMap(compressedBuilding, editorParams);
            this.compressedBuilding = compressedBuilding;
            inGrid = false;
        }

        

        private void UpdateActiveSpot(Input input)
        {
            Vector2 pos = camera.ScreenToWorld(input.GetMousePos());


            activeSpot = new Spot((int)pos.X / GroundLayerController.tileSize, (int)pos.Y / GroundLayerController.tileSize);

            if (pos.X < 0) { activeSpot.x -= 1; }
            if (pos.Y < 0) { activeSpot.y -= 1; }

            float x = pos.X % GroundLayerController.tileSize;
            float y = pos.Y % GroundLayerController.tileSize;

            if (x < 0) { x += 128; }
            if (y < 0) { y += 128; }

            x = (float)Math.Round(x / snap) * snap;
            y = (float)Math.Round(y / snap) * snap;

            offSet = new Point((int)x, (int)y);

            if (activeSpot.x > -1 && activeSpot.x < editorParams.overallSize && activeSpot.y > -1 && activeSpot.y < editorParams.overallSize)
            {
                inGrid = true;
            }
            else
            {
                inGrid = false;
            }
        }

  

        private void UpdateHover()
        {
            if (editorParams.editingMode == EditingMode.Shadows)
            {
                editorParams.hoverShadow.SetTileAndOffset(activeSpot.x, activeSpot.y, offSet);
            }
        }


        private void UpdateClickBuilding(Input input)
        {
            if (inGrid && editorParams.editingMode == EditingMode.Building && hasCameraFocus)
            {
                if (input.LeftButtonDown())
                {
                      compressedBuilding.AddBuildingPiece(activeSpot);    
                }

                if (input.RightButtonDown())
                {
                        compressedBuilding.RemoveBuildingPiece(activeSpot);
                }
            }
        }

        private void UpdateClickShadow(Input input)
        {
            if (editorParams.editingMode == EditingMode.Shadows && hasCameraFocus)
            {
                if (input.LeftButtonClick())
                {
                    compressedBuilding.AddShadowPiece(editorParams.hoverShadow.GetEditorShadow());
                }

                if (input.RightButtonClick())
                {
                    compressedBuilding.RemoveShadowPiece(activeSpot, editorParams.shadowSide);
                }
            }
        }
     


        public override void Update(Input input)
        {

            base.Update(input);
            camera.Update(input, hasCameraFocus);
            UpdateActiveSpot(input);
            UpdateHover();
            UpdateClickBuilding(input);
            UpdateClickShadow(input);
            borderMap.UpdateHighlight(activeSpot);
            borderMap.Update(input);
            compressedBuilding.Update(input);         
        }

        private void DrawSamples(SpriteBatch spriteBatch)
        {
            if (editorParams.editingMode == EditingMode.Shadows)
            {
                if (editorParams.hoverShadow != null)
                {
                    editorParams.hoverShadow.DrawBuildingEditing(spriteBatch);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.End();
            ScreenController.graphicsDevice.Viewport = camera.viewport;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TranslationMatrix(1f, 1f));
            borderMap.Draw(spriteBatch);
            compressedBuilding.Draw(spriteBatch);
            DrawSamples(spriteBatch);
        }
    }



}
