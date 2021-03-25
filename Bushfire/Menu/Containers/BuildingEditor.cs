using BushFire.Engine;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls;
using Microsoft.Xna.Framework;
using BushFire.Engine.Controllers;
using BushFire.Content.Game.Screens;
using BushFire.Editor.Screens;
using BushFire.Editor.Tech;
using BushFire.Engine.Files;
using BushFire.Engine.UIControls.Internal;

namespace BushFire.Menu.Containers
{
    class BuildingEditor : Container
    {
        BuildingsBinary buildingsBinary;
        ListBox listBoxBuildings;

        public BuildingEditor(Rectangle localLocation, DockType docktype) : base(localLocation, docktype, true)
        {
            name = "Editor";

            alwaysOnTop = true;
            spriteBack = GraphicsManager.GetSpriteColour(3);
            drawSpriteBack = true;
            canChangeFocusOrder = false;

            AddBorder(2, Resizing.NONE, 3);
            AddHeading(50, "Building Editor", GraphicsManager.GetSpriteFont(Font.OpenSans20Bold), Color.White, false, false, false, false, false, GraphicsManager.GetSpriteColour(6));



            buildingsBinary = Data.LoadBuildingsBinary();
            AddControls();


        }

        private void AddControls()
        {
            ComboMenuCycle menuCycle = new ComboMenuCycle("DirectionFacing", "Direction Facing", new Point(50, 50), true, false, true);
            menuCycle.AddCycleObject(new CycleObject("West", 6));
            menuCycle.AddCycleObject(new CycleObject("North", 0));
            AddUiControl(menuCycle);

            menuCycle = new ComboMenuCycle("HousePieceRow", "House Piece Row", new Point(50, 150), true, false, true);
            for (int i = 0; i < 4; i++)
            {
                menuCycle.AddCycleObject(new CycleObject(i.ToString(), i));
            }
            AddUiControl(menuCycle);

            AddUiControl(new ButtonBlueLarge("New", new Point(80, 250), "New Building", Color.White));
            AddUiControl(new ButtonBlueLarge("Load", new Point(80, 350), "Load Building", Color.White));
            listBoxBuildings = new ListBox("ListBox", new Rectangle(380, 250, 400, 600), GraphicsManager.GetSpriteColour(7), Font.OpenSans16, containerCamera);

            foreach (CompressedBuilding building in buildingsBinary.buildingList)
            {
                string displayName = "Building " + building.id.ToString();

                if (building.isActive)
                {
                    listBoxBuildings.AddItem(new ListBoxObject(displayName, building, Color.White));
                }
                else
                {
                    listBoxBuildings.AddItem(new ListBoxObject(displayName, building, Color.Red));
                }


            }

            AddUiControl(listBoxBuildings);
        }


        public int GetDirectionFacing()
        {
            ComboMenuCycle menu = (ComboMenuCycle)GetUiControl("DirectionFacing");
            return (int)menu.GetSelectedCycleObject();
        }

        public int GetHousePieceRow()
        {
            ComboMenuCycle menu = (ComboMenuCycle)GetUiControl("HousePieceRow");
            return (int)menu.GetSelectedCycleObject();
        }


        public override void Update(Input input)
        {
            base.Update(input);

            if (GetButtonPress("New"))
            {
                EditorParams editorParams = new EditorParams();
                CompressedBuilding building = new CompressedBuilding(GetHousePieceRow(), GetDirectionFacing(), editorParams);
                building.InitNew(buildingsBinary.GetNextId());
                ScreenController.ChangeScreen(new BuildingScreen(building, buildingsBinary, editorParams));
            }

            if (GetButtonPress("Load"))
            {
                CompressedBuilding building = (CompressedBuilding)listBoxBuildings.GetSelectedObjectValue();
                if (building != null)
                {
                    EditorParams editorParams = new EditorParams();
                    building.InitOld(editorParams);
                    ScreenController.ChangeScreen(new BuildingScreen(building, buildingsBinary, editorParams));
                }


            }

        }
    }
}
