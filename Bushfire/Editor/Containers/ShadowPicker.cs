using BushFire.Editor.Controllers;
using BushFire.Editor.Tech;
using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls.Internal;
using BushFire.Game.Map.MapObjectComponents;
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
    class ShadowPicker : Container
    {
        EditorParams editorParams;
        public Shadow sampleShadow;
        public int indexShadow = 0;
        CompressedBuilding compressedBuilding;
        private bool shiftDown;
        ComboEditorCycleNoLabel editStyleCombo;

        public ShadowPicker(Rectangle location, DockType dockType, CompressedBuilding compressedBuilding, EditorParams editorParams) : base(location, dockType, true)
        {
            this.editorParams = editorParams;
            this.compressedBuilding = compressedBuilding;
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetPreBuilt(Engine.ContentStorage.PrebuiltSprite.EditorPanelBackGrey);
            AddBorder(3, Resizing.NONE, 1);
            AddHeading(40, "Shadow Picker", GraphicsManager.GetSpriteFont(Font.OpenSans18), Color.White, false, false, false, false, true, GraphicsManager.GetSpriteColour(6));

            editStyleCombo = new ComboEditorCycleNoLabel("Side", "", new Point(5, 10), true, false, true);
            editStyleCombo.AddCycleObject(new CycleObject("Left Shadows", ShadowSide.LEFT));
            editStyleCombo.AddCycleObject(new CycleObject("Right Shadows", ShadowSide.RIGHT));
            AddUiControl(editStyleCombo);

            AddUiControl(new Label("Index", Font.OpenSans24Bold, Color.White, new Vector2(195, 320), true, ""));

            AddUiControl(new ButtonBlueMedium("Back", new Point(30, 360), "(Q) >", Color.White));
            AddUiControl(new ButtonBlueMedium("Next", new Point(210, 360), "(E) >", Color.White));
            SetSampleShadow();

        }



        private void AddIndex(int value)
        {
            if (shiftDown)
            {
                value *= 4;
            }

            indexShadow += value;

            if (indexShadow < 0)
            {
                indexShadow = ShadowSpriteController.numberOfBuildingShadows - 1;
            }
            if (indexShadow > ShadowSpriteController.numberOfBuildingShadows - 1)
            {
                indexShadow = 0;
            }

            SetSampleShadow();
        }

        private void UpdateCyclePiece(Input input)
        {
            shiftDown = input.IsKeyDown(Keys.LeftShift);

            if (input.IsKeyPressed(Keys.Q) || GetButtonPress("Back"))
            {
                AddIndex(-1);
                Button button = (Button)GetUiControl("Back");
                button.SetPress();
            }
            if (input.IsKeyPressed(Keys.E) || GetButtonPress("Next"))
            {
                AddIndex(1);
                Button button = (Button)GetUiControl("Next");
                button.SetPress();
            }
        }



        private void UpdateLabel()
        {
            GetUiControl("Index").SetText(indexShadow.ToString());
        }
        
        private void UpdateShadowSidePress()
        {
            if (editStyleCombo.changed)
            {
                editorParams.shadowSide = (ShadowSide)editStyleCombo.GetSelectedCycleObject();
                SetSampleShadow();
            }
        }

     

        private void SetSampleShadow()
        {
            //Switch this to a box eventualy
            float x = 200 * DisplayController.uiScale;
            float y = 200 * DisplayController.uiScale;
            sampleShadow = new Shadow(ShadowSpriteController.GetShadowSprite(MapObjectShadowType.BUILDING, editorParams.shadowSide, indexShadow), 0, 0, editorParams.shadowSide, indexShadow, new Point((int)x,(int)y));
            editorParams.hoverShadow = new Shadow(ShadowSpriteController.GetShadowSprite(MapObjectShadowType.BUILDING, editorParams.shadowSide, indexShadow), 0, 0, editorParams.shadowSide, indexShadow, new Point(0, 0));                
        }

        public override void Update(Input input)
        {

            base.Update(input);
            UpdateShadowSidePress();
            UpdateCyclePiece(input);
            UpdateLabel();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            sampleShadow.DrawSampleShadowEditing(spriteBatch);
        }

   
    }
}
