using BushFire.Engine;
using BushFire.Engine.UIControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Menu.Containers;
using System.Diagnostics;
using BushFire.Engine.Controllers;
using System.Collections.Generic;
using BushFire.Editor.Screens;
using System;
using BushFire.Game.Controllers;
using BushFire.Game.Tech;
using BushFire.Game;
using BushFire.Game.Map.FireStuff;

namespace BushFire.Menu.Screens
{
    class MenuMain : Screen
    {
        Container activeContainer;
        Button activeButton;
        Rectangle centerContainerLocation = new Rectangle(800, 200, 900, 1100);

        Dictionary <int, AStarNode> myList = new Dictionary<int, AStarNode>();

        public MenuMain()
        {
            AddContainer(new BackgroundMenu(Rectangle.Empty, DockType.SCREENRESOLUTION, TextureSheet.BackGround));
            AddContainer(new MenuBar(new Rectangle(150, 200, 600, 800)));
        }
 

        private void AddActiveMenu(Container container)
        {
            if (activeContainer != null)
            {
                activeContainer.SetDestroyWithFade(); //Set the active to fade out
                AddContainer(activeContainer); //Add the active to the list so it can fade out

                if (containerList.Contains(activeContainer))
                {
                    containerList.Remove(activeContainer);
                }

            }

            containerList.Add(container);
            activeContainer = container;
        }

        private void SwitchActiveButton(string buttonName)
        {
            if (activeButton != null)
            {
                activeButton.selected = false;
            }
            activeButton = (Button)GetUiControl(buttonName);
            activeButton.selected = true;
            Engine.Files.Data.SaveSettings();
        }

        #region CALLS

        public override void Update(Input input)
        {
            base.Update(input);


            if (GetButtonPress("NewGame"))
            {
                AddActiveMenu(new NewGame(centerContainerLocation, DockType.TOPLEFTFIXEDY));
                SwitchActiveButton("NewGame");
            }
            if (GetButtonPress("Controls"))
            {
                AddActiveMenu(new Controls(centerContainerLocation, DockType.TOPLEFTFIXEDY));
                SwitchActiveButton("Controls");
            }
            if (GetButtonPress("Video"))
            {
                AddActiveMenu(new Video(centerContainerLocation, DockType.TOPLEFTFIXEDY));
                SwitchActiveButton("Video");
            }
            if (GetButtonPress("Audio"))
            {
                AddActiveMenu(new Audio(centerContainerLocation, DockType.TOPLEFTFIXEDY));
                SwitchActiveButton("Audio");
            }
            if (GetButtonPress("Quit"))
            {
                EngineController.exitProgram = true;
                SwitchActiveButton("Quit");
            }
            if (GetButtonPress("Editor"))
            {
                AddActiveMenu(new BuildingEditor(centerContainerLocation, DockType.TOPLEFTFIXEDY));
                SwitchActiveButton("Editor");
            }

 
        }
   
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            #endregion
        }

    
    }
}
