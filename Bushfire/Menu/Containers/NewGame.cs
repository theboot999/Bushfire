using BushFire.Engine;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls;
using Microsoft.Xna.Framework;
using BushFire.Engine.Controllers;
using BushFire.Content.Game.Screens;
using BushFire.Game;
using BushFire.Engine.Files;
using BushFire.Menu.Screens;
using System;
using System.Collections.Generic;
using BushFire.Engine.UIControls.Internal;
using BushFire.Game.Controllers;

namespace BushFire.Menu.Containers
{
    class NewGame : Container
    {
        Random rnd = new Random();
        List<Point> worldSizeList;

        public NewGame(Rectangle localLocation, DockType docktype) : base(localLocation, docktype, true)
        {
            name = "NewGame";
            alwaysOnTop = true;
            spriteBack = GraphicsManager.GetSpriteColour(3);
            drawSpriteBack = true;
            canChangeFocusOrder = false;

            AddBorder(2, Resizing.NONE, 3);
            AddHeading(50, "New Game", GraphicsManager.GetSpriteFont(Font.OpenSans20Bold), Color.White, false, false, false, false, false, GraphicsManager.GetSpriteColour(6));

            Init();

            AddButtons();
        }

        private void Init()
        {
            worldSizeList = new List<Point>();
            worldSizeList.Add(new Point(250, 250));
            worldSizeList.Add(new Point(500, 500));
            worldSizeList.Add(new Point(750, 750));
            worldSizeList.Add(new Point(1000, 1000));
            worldSizeList.Add(new Point(1250, 1250));
            worldSizeList.Add(new Point(1500, 1500));
            worldSizeList.Add(new Point(1750, 1750));
            worldSizeList.Add(new Point(2000, 2000));
            worldSizeList.Add(new Point(2500, 2500));
            worldSizeList.Add(new Point(3000, 3000));
            worldSizeList.Add(new Point(3500, 3500));
            worldSizeList.Add(new Point(4000, 4000));
            worldSizeList.Add(new Point(4500, 4500));
            worldSizeList.Add(new Point(5000, 5000));
            worldSizeList.Add(new Point(6000, 6000));
        }

        private void AddButtons()
        {
            ComboMenuCycle menuCycle;

            AddUiControl(new ComboMenuTextBox("Seed", "Seed", "10", new Point(50, 60), false));
            
            menuCycle = new ComboMenuCycle("WorldSize", "World Size", new Point(50, 160), true, false, true);
            foreach (Point point in worldSizeList)
            {
                string displayName = point.X + " X " + point.Y;
                menuCycle.AddCycleObject(new CycleObject(displayName, point));
            }
            menuCycle.SetIndex(1);
            AddUiControl(menuCycle);

            menuCycle = new ComboMenuCycle("NumberOfTowns", "Number of Towns", new Point(50, 260), true, false, true);
            AddUiControl(menuCycle);
            menuCycle.AddCycleObject(new CycleObject("Maximum 3", (int)3));
            menuCycle.AddCycleObject(new CycleObject("Maximum 4", (int)4));
            menuCycle.AddCycleObject(new CycleObject("Maximum 5", (int)5));
            menuCycle.AddCycleObject(new CycleObject("Maximum 6", (int)6));
            menuCycle.AddCycleObject(new CycleObject("Maximum 8", (int)8));
            menuCycle.AddCycleObject(new CycleObject("Maximum 10", (int)10));
            menuCycle.AddCycleObject(new CycleObject("Maximum 12", (int)12));
            menuCycle.AddCycleObject(new CycleObject("Maximum 14", (int)14));
            menuCycle.AddCycleObject(new CycleObject("Maximum 16", (int)16));
            menuCycle.AddCycleObject(new CycleObject("Maximum 18", (int)18));
            menuCycle.AddCycleObject(new CycleObject("Maximum 20", (int)20));
            menuCycle.AddCycleObject(new CycleObject("Maximum 25", (int)25));
            menuCycle.AddCycleObject(new CycleObject("Maximum 30", (int)30));
            menuCycle.AddCycleObject(new CycleObject("Maximum 35", (int)35));
            menuCycle.AddCycleObject(new CycleObject("Maximum 40", (int)40));
            menuCycle.SetIndex(2);

            menuCycle = new ComboMenuCycle("MinTownRoads", "Minimum Town Streets", new Point(50, 360), true, false, true);
            AddUiControl(menuCycle);
            for (int i = 10; i < 201; i+=10)
            {
                menuCycle.AddCycleObject(new CycleObject(i.ToString(), i));
            }
            menuCycle.SetIndex(0);

            menuCycle = new ComboMenuCycle("MaxTownRoads", "Maximum Town Streets", new Point(50, 460), true, false, true);
            AddUiControl(menuCycle);
            for (int i = 40; i < 491; i += 20)
            {
                menuCycle.AddCycleObject(new CycleObject(i.ToString(), i));
            }
            menuCycle.SetIndex(0);

            AddUiControl(new ButtonBlueLarge("New", new Point(550, 600), "Create World", Color.White));
        }


        public void SetSeed()
        {
            int seed = GetUiControl("Seed").GetTextInt();
            if (seed == 0)
            {
                seed = GameController.rnd.Next(0, 1000000);
            }
            GameController.seed = seed;
            GameController.rnd = new Random(GameController.seed);
        }

        public Point GetWorldSize()
        {
            ComboMenuCycle menu = (ComboMenuCycle)GetUiControl("WorldSize");
            return (Point)menu.GetSelectedCycleObject();
        }

        public int GetNumberOfTowns()
        {
            ComboMenuCycle menu = (ComboMenuCycle)GetUiControl("NumberOfTowns");
            return (int)menu.GetSelectedCycleObject();
        }

        public int GetMinTownRoads()
        {
            ComboMenuCycle menu = (ComboMenuCycle)GetUiControl("MinTownRoads");
            return (int)menu.GetSelectedCycleObject();
        }

        public int GetMaxTownRoads()
        {
            ComboMenuCycle menu = (ComboMenuCycle)GetUiControl("MaxTownRoads");

            ComboMenuCycle menuMin = (ComboMenuCycle)GetUiControl("MinTownRoads");
            int maxRoads = (int)menu.GetSelectedCycleObject();
            int minRoads = (int)menuMin.GetSelectedCycleObject();

            if (maxRoads < minRoads)
            {
                maxRoads = minRoads + 1;
            }

            return maxRoads;
        }

        public override void Update(Input input)
        {
            base.Update(input);

            if (GetButtonPress("New"))
            {
                SetSeed();
                ScreenController.ChangeScreen(new CreatingWorld(GetWorldSize().X, GetWorldSize().Y, GetNumberOfTowns(), GetMinTownRoads(), GetMaxTownRoads()));
            }
        }
    }
}
