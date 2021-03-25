using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Menu.Containers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BushFire.Engine
{
    //BUG.  In editor removing a container through the fade and remove method is destroying hte container.
    //However in the game the minimap etc is not being destroyed.  Why is that (We dont wwant it to be destroyed)

    abstract class Screen
    {
        protected List<Container> containerList = new List<Container>();
        private MessageControl messageControl = new MessageControl();
        DebugThing debugThing;

        public Screen()
        {
            GC.Collect();
            debugThing = new DebugThing();
        }

        public void ResolutionChange()
        {
            foreach (Container container in containerList)
            {
                container.ResolutionChange();
            }
        }

        public void AddContainer(Container container)
        {
            List<Container> tempList = new List<Container>();
            containerList.Add(container);

            foreach (Container sortContainer in containerList)
            {
                if (sortContainer.alwaysOnTop)
                {
                    tempList.Add(sortContainer);
                }
            }

            foreach (Container sortContainer in tempList)
            {
                containerList.Remove(sortContainer);
                containerList.Add(sortContainer);
            }
        }

        public void MoveContainerToTop(Container container)
        {
            if (containerList.Contains(container))
            {
                containerList.Remove(container);
                containerList.Add(container);
            }
        }

        public void RemoveContainer(Container container, bool fade)
        {
            if (container != null && containerList.Contains(container))
            {
                if (fade)
                {
                    container.SetDestroyWithFade();
                }
                else
                {
                    container.SetDestroyWithOutFade();
                }

            }
        }      

        protected Container GetContainer(string name)
        {
            foreach (Container container in containerList)
            {
                if (container.name == name)
                {
                    return container;
                }
            }
            return null;
        }

        protected Container GetNonPinnedContainer(string name)
        {
            foreach (Container container in containerList)
            {
                if (container.name == name && !container.IsPinned())
                {
                    return container;
                }
            }
            return null;
        }



        protected bool IsContainer(string name)
        {
            foreach (Container container in containerList)
            {
                if (container.name == name)
                {
                    return true;
                }
            }
            return false;
        }

        protected int GetIntByScale(int value)
        {
            return Convert.ToInt32((float)value * DisplayController.uiScale);
        }

        protected void UpdateContainerFocus(Input input)
        {
            bool foundFocus = false;
            Container moveToTop = null;

            for (int i = containerList.Count - 1; i > -1; i--)
            {
                containerList[i].UpdateFocus(input, foundFocus);

                if (containerList[i].hasFocus)
                {
                    foundFocus = true;
                }

                if (containerList[i].SetFocusToTop)
                {
                    moveToTop = containerList[i];
                }      
            }

            if (moveToTop != null)
            {
                containerList.Remove(moveToTop);
                containerList.Add(moveToTop);

            }

        }

        public UiControl GetUiControl(string name)
        {
            foreach (Container container in containerList)
            {
                UiControl uiControl= container.GetUiControl(name);
                if (uiControl != null)
                {
                    return uiControl;
                }                
            }
            return null;
        }


        public bool GetButtonPress(string buttonName)
        {
            UiControl uiControl = GetUiControl(buttonName);

            if (uiControl != null)
            {
                return uiControl.IsEitherPress();
            }
            return false;
        }

        public void AddMessage(string text, Color color)
        {
            messageControl.AddMessage(text, color);
        }

        public virtual void Dispose()
        {
            foreach (Container container in containerList)
            {
                container.Dispose();
            }
        }

        public virtual void Update(Input input)
        {
            UpdateContainerFocus(input);

            for (int i = containerList.Count - 1; i > -1; i--)
            {
                containerList[i].Update(input);

                if (i < containerList.Count)  //In case we have removed it via screen controller we can check if its still legit
                {
                    if (containerList[i].containerState == ContainerState.Destroy)
                    {
                        containerList[i].Dispose();
                        containerList.RemoveAt(i);
                    }
                }
            }

            messageControl.Update(input);

            if (input.IsKeyMapPressed(KeyMap.ToggleDebug))
            {
                if (!DisplayController.showDebugWindowOne && !DisplayController.showDebugWindowTwo)
                {
                    DisplayController.showDebugWindowOne = true;
                }
                else if (DisplayController.showDebugWindowOne && !DisplayController.showDebugWindowTwo)
                {
                    DisplayController.showDebugWindowTwo = true;
                }
                else
                {
                    DisplayController.showDebugWindowOne = false;
                    DisplayController.showDebugWindowTwo = false;
                }

            }
            //DEBUG ONLY
            if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.N))
            {
                DisplayController.uiScale -= 0.005f * EngineController.drawUpdateTime;
            }

            if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.M))
            {
                DisplayController.uiScale += 0.005f * EngineController.drawUpdateTime;
            }

            if (DisplayController.showDebugWindowOne)
            {
                debugThing.Update(input);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();


            foreach (Container container in containerList)
            {
                container.Draw(spriteBatch);
            }

            if (DisplayController.showDebugWindowOne)
            {
                debugThing.Draw(spriteBatch);
            }

            spriteBatch.End();
            ScreenController.graphicsDevice.Viewport = new Viewport(ScreenController.gameWindow);

            spriteBatch.Begin();
            messageControl.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
