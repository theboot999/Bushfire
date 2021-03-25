using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls.Internal;
using BushFire.Game.Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Screens.Containers
{
    class TownPanel : Container
    {
        Camera mainWorldCamera;
        //ListBox towns;
        ComboCycleGamePanel townCombo;
        Town selectedTown;

        public TownPanel(Rectangle location, DockType dockType, Camera mainWorldCamera) : base(location, dockType, true)
        {
            name = "TownPanel";
            this.mainWorldCamera = mainWorldCamera;
   
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetPreBuilt(Engine.ContentStorage.PrebuiltSprite.InGameMenuBack);
            transparency = 1f;

            AddBorder(2, Resizing.NONE, 40);
            AddHeading(40, "Towns", GraphicsManager.GetSpriteFont(Font.CarterOne16), Color.White, true, false, false, false, true, GraphicsManager.GetPreBuilt(Engine.ContentStorage.PrebuiltSprite.InGameHeadingBar));
            AddControls();
            SetMaxTransparency(0.95f);
        }

        public void AddControls()
        {
            townCombo = new ComboCycleGamePanel("Towns", "", new Point(10, 10), true, false, true);

            foreach (Town town in WorldController.world.townList)
            {
                townCombo.AddCycleObject(new CycleObject(town.name, town));
            }
            AddUiControl(townCombo);
          
            AddUiControl(new Label("NumberOfBuildings", Font.OpenSans18, Color.White, new Vector2(20, 100), false, ""));
            AddUiControl(new ButtonBlueSmall("Test", new Point(50, 200), "TEST", Color.White));
            UpdateSelectedTown();
        }

        private void UpdateSelectedTown()
        {
            selectedTown = (Town)townCombo.GetSelectedCycleObject();
            GetUiControl("NumberOfBuildings").SetText("Number Of Buildings: " + selectedTown.numberOfBuildings);
        }


        public void AddControlsTest()
        {


          /*  towns = new ListBox("ListBox", new Rectangle(10, 25, 300, 500), GraphicsManager.GetSpriteColour(7), Font.OpenSans16, containerCamera);

            foreach (Town town in WorldController.world.townList)
            {
                towns.AddItem(new ListBoxObject(town.name, town, Color.White));
            }
    

            AddUiControl(towns);*/
        }

        int debug = 0;

        public override void Update(Input input)
        {
            base.Update(input);

            if (townCombo.changed)
            {
                UpdateSelectedTown();
                mainWorldCamera.CenterOn(selectedTown.worldLocation);
            }

            if (GetButtonPress("Test"))
            {
                debug++;
                ScreenController.AddMessage("This is a test message #" + debug, Color.White);
            }
        }
    }
}
