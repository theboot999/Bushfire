using BushFire.Editor.Containers;
using BushFire.Editor.Tech;
using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Game.Controllers;
using BushFire.Game.Screens.Containers.InMenu;
using BushFire.Menu.Containers;
using BushFire.Menu.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BushFire.Editor.Screens
{
    class BuildingScreen : Screen
    {
        EditorParams editorParams;
        EditorView editorView;
        BuildingPicker buildingPicker;
        ShadowPicker shadowPicker;
        ObjectPicker objectPicker;
        DrivingPicker drivingPicker;
        General general;
        BuildingsBinary buildingsBinary;
        CompressedBuilding compressedBuilding;


        public BuildingScreen(CompressedBuilding compressedBuilding, BuildingsBinary buildingsBinary, EditorParams editorParams)
        {
            GameController.inGameState = InGameState.RUNNING;
            this.editorParams = editorParams;
            this.compressedBuilding = compressedBuilding;
            GameController.editorInMenu = false;
            this.buildingsBinary = buildingsBinary;
            AddContainer(new BackgroundMenu(new Rectangle(0, 0, 0, 0), DockType.SCREENRESOLUTION, TextureSheet.Editor));
            editorView = new EditorView(new Rectangle(100, 100, 2000, 1500), DockType.SCREENRESOLUTION, compressedBuilding, editorParams);
            AddContainer(editorView);
            buildingPicker = new BuildingPicker(new Rectangle(200, 150, 400, 500), DockType.TOPRIGHTFIXEDY, compressedBuilding, editorParams);
            shadowPicker = new ShadowPicker(new Rectangle(200, 150, 400, 500), DockType.TOPRIGHTFIXEDY, compressedBuilding, editorParams);
            objectPicker = new ObjectPicker(new Rectangle(200, 150, 400, 500), DockType.TOPRIGHTFIXEDY, compressedBuilding, editorParams);
            drivingPicker = new DrivingPicker(new Rectangle(200, 150, 400, 500), DockType.TOPRIGHTFIXEDY, compressedBuilding, editorParams);
            AddContainer(buildingPicker);

            general = new General(new Rectangle(200, 150, 600, 1250), DockType.TOPLEFTFIXEDY, compressedBuilding, editorParams);
            AddContainer(general);

        }


        private void UpdateEditingMode()
        {
            if (editorParams.editingModeChanged)
            {
                 containerList.Remove(buildingPicker);
                 containerList.Remove(shadowPicker);
                 containerList.Remove(objectPicker);
                containerList.Remove(drivingPicker);
                //  ScreenController.RemoveContainer(buildingPicker, true);
                // ScreenController.RemoveContainer(shadowPicker, true);
                // ScreenController.RemoveContainer(objectPicker, true);

                if (editorParams.editingMode == EditingMode.Building)
                {
                  //  buildingPicker.SetFadeIn();
                    AddContainer(buildingPicker);
                }
                else if (editorParams.editingMode == EditingMode.Shadows)
                {
              //      shadowPicker.SetFadeIn();
                    AddContainer(shadowPicker);
                }
                else if (editorParams.editingMode == EditingMode.Objects)
                {
              //      objectPicker.SetFadeIn();
                    AddContainer(objectPicker);
                }
                else if (editorParams.editingMode == EditingMode.DrivingPicker)
                {
                    //      objectPicker.SetFadeIn();
                    AddContainer(drivingPicker);
                }

            }
        }

        public override void Update(Input input)
        {

            //Debug.WriteLine(compressedBuilding.shadowList.Count);
            base.Update(input);

            UpdateEditingMode();
            general.UpdateDisplayLocations(editorView.activeSpot, editorView.offSet);

            if (GetButtonPress("Save"))
            {
                buildingsBinary.AddBuilding(editorView.compressedBuilding);
            }

            if (input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape) && !GameController.editorInMenu)
            {
                AddContainer(new MenuBarInGame());
            }

            if (GameController.inGameState == InGameState.WAITINGONEXIT)
            {
                ScreenController.ChangeScreen(new MenuMain());
            }
        }

    }

}
