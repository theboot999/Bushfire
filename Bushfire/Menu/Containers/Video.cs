using BushFire.Engine;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls;
using Microsoft.Xna.Framework;
using BushFire.Engine.Controllers;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.UIControls.Internal;
using BushFire.Engine.Files;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using BushFire.Game;
using BushFire.Game.Controllers;

namespace BushFire.Menu.Containers
{
    class Video : Container
    {
        public Video(Rectangle localLocation, DockType dockType) : base(localLocation, dockType, true)
        {         
            name = "Video";
            alwaysOnTop = true;

            spriteBack = GraphicsManager.GetSpriteColour(3);
            AddBorder(2, Resizing.NONE, 3);
            AddHeading(50, "Video Settings", GraphicsManager.GetSpriteFont(Font.OpenSans20Bold), Color.White, false, false, false, false, false, GraphicsManager.GetSpriteColour(6));

            drawSpriteBack = true;
            canChangeFocusOrder = false;

            AddButtons();
        }



        private void AddButtons()
        {
            ComboMenuCycle menu;

            //Uiscale
            menu = new ComboMenuCycle("Scale", "Interface Scale", new Point(50, 60), false, false, true);
            float startAmount = 0.5f;
            for (int i = 0; i < 31; i++)
            {
                float amount = startAmount + (float)i * 0.025f;
                float displayValue = (amount * 100f);
                string displayName = string.Format("{0:0.0}", displayValue) + "%";
                menu.AddCycleObject(new CycleObject(displayName, amount));
            }
            menu.SetIndexByFloat(DisplayController.uiScale);
            AddUiControl(menu);

            //Resolution
            menu = new ComboMenuCycle("Resolution", "Resolution", new Point(50, 160), true, false, true);
            foreach (Rectangle rectangle in DisplayController.resolutionList)
            {
                string displayName = rectangle.Width + " X " + rectangle.Height;
                menu.AddCycleObject(new CycleObject(displayName, rectangle));
            }
            menu.SetIndex(DisplayController.resolutionId);
            AddUiControl(menu);

            //Target Frame Rate
            menu = new ComboMenuCycle("TargetFrameRate", "Target Frame Rate", new Point(50, 260), true, false, true);
            menu.AddCycleObject(new CycleObject("Vertical Sync", (int)0));
            menu.AddCycleObject(new CycleObject("60", (int)60));
            menu.AddCycleObject(new CycleObject("75", (int)75));
            menu.AddCycleObject(new CycleObject("100", (int)100));
            menu.AddCycleObject(new CycleObject("120", (int)120));
            menu.AddCycleObject(new CycleObject("144", (int)144));
            menu.AddCycleObject(new CycleObject("165", (int)165));
            menu.AddCycleObject(new CycleObject("240", (int)240));
            menu.AddCycleObject(new CycleObject("Unlimited", (int)-1));
            menu.SetIndexByInt(DisplayController.targetFrameRate);
            AddUiControl(menu);

            //Vsync
            menu = new ComboMenuCycle("FullScreen", "Full Screen", new Point(50, 360), true, false, true);
            menu.AddCycleObject(new CycleObject("No", false));
            menu.AddCycleObject(new CycleObject("Yes", true));
            menu.SetIndexByBool(DisplayController.fullScreen);
            AddUiControl(menu);


            //Night Brightness
            menu = new ComboMenuCycle("NightBright", "Night Brightness", new Point(50, 460), true, false, true);

            menu.AddCycleObject(new CycleObject("0", 0f));
            menu.AddCycleObject(new CycleObject("1", 0.025f));
            menu.AddCycleObject(new CycleObject("2", 0.05f));
            menu.AddCycleObject(new CycleObject("3", 0.075f));
            menu.AddCycleObject(new CycleObject("4", 0.1f));
            menu.AddCycleObject(new CycleObject("5", 0.125f));
            menu.AddCycleObject(new CycleObject("6", 0.15f));
            menu.AddCycleObject(new CycleObject("7", 0.175f));
            menu.AddCycleObject(new CycleObject("8", 0.2f));
            menu.AddCycleObject(new CycleObject("9", 0.225f));
            menu.AddCycleObject(new CycleObject("10", 0.25f));
            menu.AddCycleObject(new CycleObject("11", 0.275f));
            menu.AddCycleObject(new CycleObject("12", 0.3f));

            menu.SetIndexByFloat(DisplayController.minNightBrightness);
            AddUiControl(menu);

            //Message Speed
            menu = new ComboMenuCycle("MessageSpeed", "Message Speed", new Point(50, 560), true, false, true);
            menu.AddCycleObject(new CycleObject("Super Slow", 0.4f));
            menu.AddCycleObject(new CycleObject("Slow", 0.7f));
            menu.AddCycleObject(new CycleObject("Normal", 1f));
            menu.AddCycleObject(new CycleObject("Fast", 1.3f));
            menu.AddCycleObject(new CycleObject("Very Fast", 1.7f));
            menu.AddCycleObject(new CycleObject("Super Fast", 2f));
            menu.AddCycleObject(new CycleObject("Crazy", 2.5f));
            menu.SetIndexByFloat(DisplayController.messageSpeed);
            AddUiControl(menu);
        }

        private void AddApplyButton()
        {
            if (GetUiControl("Apply") == null)
            {
                AddUiControl(new ButtonBlueLarge("Apply", new Point(550, 660), "Apply", Color.White));
            }
        }

        private void RemoveApplyButton()
        {
            RemoveUiControl("Apply");
        }

        private void UpdateApply(Input input)
        {
            bool recalculateUiScale;

            if (GetButtonPress("Apply") || input.IsKeyPressed(Keys.Enter))
            {
                RemoveApplyButton();
                ComboMenuCycle menu;
                //Resolution Change
                menu = (ComboMenuCycle)GetUiControl("Resolution");
                Rectangle rect = (Rectangle)menu.GetSelectedCycleObject();
                int newResolutionId = DisplayController.GetResolutionId(rect.Width, rect.Height);
                recalculateUiScale = newResolutionId != DisplayController.resolutionId;
                DisplayController.resolutionId = newResolutionId;
                

                //Vsync
                menu = (ComboMenuCycle)GetUiControl("TargetFrameRate");
                DisplayController.targetFrameRate = (int)menu.GetSelectedCycleObject();

                //Scale
                menu = (ComboMenuCycle)GetUiControl("Scale");
                DisplayController.uiScale = (float)menu.GetSelectedCycleObject();

                //Full Screen
                menu = (ComboMenuCycle)GetUiControl("FullScreen");
                bool isFullScreen = (bool)menu.GetSelectedCycleObject();

                if (isFullScreen != DisplayController.fullScreen)
                {
                    DisplayController.fullScreen = (bool)menu.GetSelectedCycleObject();
                   // if (isFullScreen)
                  //  {
                        recalculateUiScale = true;
                 //   }
                }
        
                //Night Brightness
                menu = (ComboMenuCycle)GetUiControl("NightBright");
                DisplayController.minNightBrightness = (float)menu.GetSelectedCycleObject();

                //MessageSpeed
                menu = (ComboMenuCycle)GetUiControl("MessageSpeed");
                DisplayController.messageSpeed = (float)menu.GetSelectedCycleObject();

                DisplayController.UpdateDisplay(recalculateUiScale);
                

                //In case we did a forced update of the ui scale size
                menu = (ComboMenuCycle)GetUiControl("Scale");
                menu.SetIndexByFloat(DisplayController.uiScale);

                //in case we forced it full screen we have changed our full screen res
                menu = (ComboMenuCycle)GetUiControl("Resolution");
                menu.SetIndex(DisplayController.resolutionId);

                Data.SaveSettings();
                ScreenController.ResolutionChange();
                WorldController.UpdateMinNightBrightness();
            }
        }

        public override void Update(Input input)
        {
            base.Update(input);

            if (GetButtonPress("Resolution") || GetButtonPress("TargetFrameRate") || GetButtonPress("Scale") || GetButtonPress("FullScreen") || GetButtonPress("Scale") || GetButtonPress("Shadows") || GetButtonPress("Lighting") || GetButtonPress("MessageSpeed"))
            {
                AddApplyButton();
            }

            if (GetButtonPress("NightBright"))
            {
                ComboMenuCycle menu = (ComboMenuCycle)GetUiControl("NightBright");
                DisplayController.minNightBrightness = (float)menu.GetSelectedCycleObject();
                WorldController.UpdateMinNightBrightness();
                Data.SaveSettings();
            }


            UpdateApply(input);
        }
    }
}
