using BushFire.Editor.Controllers;
using BushFire.Editor.Tech;
using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
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
    class ObjectPicker : Container
    {
        EditorParams editorParams;
        public int indexShadow = 0;
        CompressedBuilding compressedBuilding;
        private bool shiftDown;

        public ObjectPicker(Rectangle location, DockType dockType, CompressedBuilding compressedBuilding, EditorParams editorParams) : base(location, dockType, true)
        {
            this.editorParams = editorParams;
            this.compressedBuilding = compressedBuilding;
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetPreBuilt(Engine.ContentStorage.PrebuiltSprite.EditorPanelBackGrey);

            AddBorder(3, Resizing.NONE, 1);
            AddHeading(40, "Object Picker", GraphicsManager.GetSpriteFont(Font.OpenSans18), Color.White, false, false, false, false, true, GraphicsManager.GetSpriteColour(6));



            AddUiControl(new Label("Index", Font.OpenSans24Bold, Color.White, new Vector2(195, 310), true, ""));
          //  AddUiControl(new PictureBox("Picture", new Rectangle(100, 80, 192, 192), null));
            AddUiControl(new Box("Box", new Rectangle(90, 70, 212, 212), 3, 0, 20, true, false));

            AddUiControl(new ButtonBlueMedium("Back", new Point(30, 360), "(E) >", Color.White));
            AddUiControl(new ButtonBlueMedium("Next", new Point(210, 360), "(R) >", Color.White));
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
    }
}
