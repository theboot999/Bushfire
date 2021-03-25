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
    class LightPicker : Container
    {
        EditorParams editorParams;
        public int indexShadow = 0;
        CompressedBuilding compressedBuilding;
        private bool shiftDown;
        ComboEditorCycleNoLabel editStyleCombo;
        ComboEditorCycleNoLabel snapCombo;
        public float snap = 1f;

        public LightPicker(Rectangle location, DockType dockType, CompressedBuilding compressedBuilding, EditorParams editorParams) : base(location, dockType, true)
        {
            this.editorParams = editorParams;
            this.compressedBuilding = compressedBuilding;
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetPreBuilt(Engine.ContentStorage.PrebuiltSprite.EditorPanelBackGrey);
            AddBorder(3, Resizing.NONE, 1);
            AddHeading(40, "Shadow Picker", GraphicsManager.GetSpriteFont(Font.OpenSans18), Color.White, false, false, false, false, true, GraphicsManager.GetSpriteColour(6));

          


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
                indexShadow = PieceController.GetRowLength(compressedBuilding.pieceRow);
            }
            if (indexShadow > PieceController.GetRowLength(compressedBuilding.pieceRow))
            {
                indexShadow = 0;
            }

            SetSampleShadow();
        }

        private void UpdateCyclePiece(Input input)
        {
            shiftDown = input.IsKeyDown(Keys.LeftShift);

            if (input.IsKeyPressed(Keys.E) || GetButtonPress("Back"))
            {
                AddIndex(-1);
                Button button = (Button)GetUiControl("Back");
                button.SetPress();
            }
            if (input.IsKeyPressed(Keys.R) || GetButtonPress("Next"))
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

    

        private void SetSampleShadow()
        {
          
        }


        public override void Update(Input input)
        {

            base.Update(input);
            
            UpdateCyclePiece(input);
            UpdateLabel();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        //enum ModeSide
        // {
        //     Left,
        //     Right
        // }
    }
}
