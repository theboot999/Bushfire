using BushFire.Editor.Tech;
using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls.Internal;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Editor.Containers
{
    class General : Container
    {
        EditorParams editorParams;
        ComboEditorCycleNoLabel editStyleCombo;
        ComboEditorCycleNoLabel widthCombo;
        ComboEditorCycleNoLabel heightCombo;
        ComboEditorCycleNoLabel activeCombo;
        ComboEditorCycleNoLabel hasFenceCombo;       
        ComboEditorCycleNoLabel snapCombo;
        ComboEditorCycleNoLabel elevationCombo;
        ComboEditorCycleNoLabel inTileShiftCombo;

        CompressedBuilding compressedBuilding;
        ListBox colorGlobalList;
        ListBox colorInList;
        public List<ColorXML> availableColorsList = new List<ColorXML>();


        public General(Rectangle location, DockType dockType, CompressedBuilding compressedBuilding, EditorParams editorParams) : base(location, dockType, true)
        {
            this.editorParams = editorParams;
            this.compressedBuilding = compressedBuilding;
            InitColours();
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetPreBuilt(Engine.ContentStorage.PrebuiltSprite.EditorPanelBackGrey);
            AddBorder(3, Resizing.NONE, 1);
            AddHeading(40, "General", GraphicsManager.GetSpriteFont(Font.OpenSans18), Color.White, false, false, false, false, true, GraphicsManager.GetSpriteColour(6));
            AddControls();
        }

        private void AddControls()
        {
            AddUiControl(new ButtonBlueMedium("Save", new Point(435, 10), "SAVE", Color.White));
            AddUiControl(new Label("Building#", Font.OpenSans16, Color.White, new Vector2(50, 70), false, "Building #" + compressedBuilding.id.ToString()));
            AddUiControl(new Label("TileSet", Font.OpenSans16, Color.White, new Vector2(250, 70), false, "Tileset " + compressedBuilding.pieceRow));
            AddUiControl(new Label("Facing", Font.OpenSans16, Color.White, new Vector2(400, 70), false, "Facing " + compressedBuilding.GetBaseDirectionFacingString()));
            AddUiControl(new Label("Location", Font.OpenSans16, Color.Red, new Vector2(50, 20), false, ""));

            editStyleCombo = new ComboEditorCycleNoLabel("Edit", "", new Point(100, 130), true, false, true);
            activeCombo = new ComboEditorCycleNoLabel("Active", "", new Point(100, 195), true, false, true);
            widthCombo = new ComboEditorCycleNoLabel("Width", "", new Point(100, 260), true, false, true);
            heightCombo = new ComboEditorCycleNoLabel("Height", "", new Point(100, 325), true, false, true);
            hasFenceCombo = new ComboEditorCycleNoLabel("HasFence", "", new Point(100, 390), true, false, true);
            snapCombo = new ComboEditorCycleNoLabel("Snap", "", new Point(100, 455), true, false, true);
            elevationCombo = new ComboEditorCycleNoLabel("Elevation", "", new Point(100, 520), true, false, true);
            inTileShiftCombo = new ComboEditorCycleNoLabel("InTileShift", "", new Point(100, 585), true, false, true);

            //EditStyle
            editStyleCombo.AddCycleObject(new CycleObject("Mode Buildings", EditingMode.Building));
            editStyleCombo.AddCycleObject(new CycleObject("Mode Shadows", EditingMode.Shadows));
            editStyleCombo.AddCycleObject(new CycleObject("Mode Objects", EditingMode.Objects));
            editStyleCombo.AddCycleObject(new CycleObject("Mode Driving", EditingMode.DrivingPicker));
            AddUiControl(editStyleCombo);

            //Active
            activeCombo.AddCycleObject(new CycleObject("Not Active", false));
            activeCombo.AddCycleObject(new CycleObject("Active", true));
            activeCombo.SetIndexByBool(compressedBuilding.isActive);
            AddUiControl(activeCombo);

            //Width & Height
            for (int i = 2; i < editorParams.overallSize; i += 2)
            {
                widthCombo.AddCycleObject(new CycleObject("Width " + i, i));
                heightCombo.AddCycleObject(new CycleObject("Height " + i, i));
            }
            widthCombo.SetIndexByInt(compressedBuilding.usingWidth);
            heightCombo.SetIndexByInt(compressedBuilding.usingHeight);
            AddUiControl(widthCombo);
            AddUiControl(heightCombo);
            
            //hasFenceCombo
            hasFenceCombo.AddCycleObject(new CycleObject("No Fence", false));
            hasFenceCombo.AddCycleObject(new CycleObject("Has Fence", true));
            hasFenceCombo.SetIndexByBool(compressedBuilding.hasFence);
            AddUiControl(hasFenceCombo);

            //SnapCombo
            int value = 1;
            for (int i = 1; i < 8; i++)
            {
                snapCombo.AddCycleObject(new CycleObject("Snap " + value, (float)value));
                value += value;
            }
            AddUiControl(snapCombo);

            //ElevationCombo
            for (int i = 0; i < 6; i++)
            {
                elevationCombo.AddCycleObject(new CycleObject("Floor Height " + i, i));
            }
            elevationCombo.SetIndexByInt(compressedBuilding.elevation);
            AddUiControl(elevationCombo);

            //inTileShiftCombo
            for (int i = 0; i < 17; i++)
            {
                inTileShiftCombo.AddCycleObject(new CycleObject("In Tile Shift " + i * 4, i * 4));
            }
            inTileShiftCombo.SetIndexByInt(compressedBuilding.inTileShift);
            AddUiControl(inTileShiftCombo);

            //ListBoxes
            AddUiControl(new Label("Label", Font.OpenSans18, Color.White, new Vector2(133, 720), true, "Available Colours"));
            AddUiControl(new Label("Label", Font.OpenSans18, Color.White, new Vector2(455, 720), true, "Using Colours"));
            AddUiControl(new ButtonCycleMenu("AddColor", new Point(270, 850), ">", Font.CarterOne13, Color.White));
            AddUiControl(new ButtonCycleMenu("RemoveColor", new Point(270, 950), "<", Font.CarterOne13, Color.White));
            colorGlobalList = new ListBox("ListBox", new Rectangle(10, 750, 250, 400), GraphicsManager.GetSpriteColour(7), Font.CarterOne14, containerCamera);
            foreach (ColorXML colorXML in availableColorsList)
            {
                AddAvailableColor(colorXML);
            }
            colorGlobalList.SetIndex(0);
            compressedBuilding.sampleColor = (ColorXML)colorGlobalList.GetSelectedObjectValue();
            AddUiControl(colorGlobalList);
            colorInList = new ListBox("ListBox", new Rectangle(330, 750, 250, 400), GraphicsManager.GetSpriteColour(7), Font.CarterOne14, containerCamera);
            foreach (ColorXML colorXML in compressedBuilding.availableColoursXMLList)
            {
                AddInColour(colorXML);
            }
            AddUiControl(colorInList);
        }

        public void UpdateDisplayLocations(Spot activeSpot,Point offSet)
        {
            SetControlText("Location", "Piece:X" + activeSpot.x + " :Y" + activeSpot.y + " Offset:X" + offSet.X + " :Y" + offSet.Y);
        }

        private void AddAvailableColor(ColorXML colorXML)
        {
            foreach (ColorXML usedColor in compressedBuilding.availableColoursXMLList)
            {
                if (colorXML.name == usedColor.name)
                {
                    return;
                }
            }
            colorGlobalList.AddItem(new ListBoxObject(colorXML.name, colorXML, colorXML.ToColor()));
        }

        private void AddInColour(ColorXML colorXML)
        {
            colorInList.AddItem(new ListBoxObject(colorXML.name, colorXML, colorXML.ToColor()));
        }



        private void InitColours()
        {
            Dictionary<string, Color> dictionary = typeof(Color).GetProperties(BindingFlags.Public |
                BindingFlags.Static)
                     .Where(prop => prop.PropertyType == typeof(Color))
                     .ToDictionary(prop => prop.Name,
                                   prop => (Color)prop.GetValue(null, null));
            int skip = 0;
            foreach (KeyValuePair<string, Color> color in dictionary)         
            {
                skip++;

                if (skip > 2)
                {
                    availableColorsList.Add(new ColorXML(color.Value, AddSpacesToSentence(color.Key)));
                }
            }
        }

        public string AddSpacesToSentence(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        private void UpdateCombos()
        {
            compressedBuilding.usingWidth = (int)widthCombo.GetSelectedCycleObject();
            compressedBuilding.usingHeight = (int)heightCombo.GetSelectedCycleObject();
            compressedBuilding.isActive = (bool)activeCombo.GetSelectedCycleObject();
            compressedBuilding.hasFence = (bool)hasFenceCombo.GetSelectedCycleObject();
            compressedBuilding.elevation = (int)elevationCombo.GetSelectedCycleObject();
            compressedBuilding.inTileShift = (int)inTileShiftCombo.GetSelectedCycleObject();
        }

        private void UpdateListBox()
        {
            if (colorGlobalList.changed)
            {
                compressedBuilding.sampleColor = (ColorXML)colorGlobalList.GetSelectedObjectValue();
            }
            if (colorInList.changed)
            {
                compressedBuilding.sampleColor = (ColorXML)colorInList.GetSelectedObjectValue();
            }
        }

        private void UpdateColourAdders()
        {
            if (GetButtonPress("AddColor"))
            {
                ColorXML colorXML = (ColorXML)colorGlobalList.GetSelectedObjectValue();
                if (colorXML != null)
                {
                    if (colorInList.AddItemUniqueDisplay(new ListBoxObject(colorXML.name, colorXML, colorXML.ToColor())))
                    {
                        compressedBuilding.availableColoursXMLList.Add(colorXML);
                    }
                    colorGlobalList.RemoveSelectedItem();
                }
            }

            if (GetButtonPress("RemoveColor"))
            {
                ColorXML colorXML = (ColorXML)colorInList.GetSelectedObjectValue();
                if (colorXML != null)
                {
                    colorGlobalList.AddItemUniqueDisplay(new ListBoxObject(colorXML.name, colorXML, colorXML.ToColor()));
                    colorInList.RemoveSelectedItem();
                    compressedBuilding.availableColoursXMLList.Remove(colorXML);
                }
            }
        }

        public void UpdateEditingMode()
        {
            if (editStyleCombo.changed)
            {
                editorParams.ChangeEditingMode((EditingMode)editStyleCombo.GetSelectedCycleObject());
            }
        }

        private void UpdateSnapPress()
        {
            if (snapCombo.changed)
            {
                editorParams.snap = (float)snapCombo.GetSelectedCycleObject();
            }
        }


        public override void Update(Input input)
        {
            base.Update(input);

            UpdateCombos();
            UpdateColourAdders();
            UpdateListBox();
            UpdateEditingMode();
            UpdateSnapPress();
            
        }
    }
}
