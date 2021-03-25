using BushFire.Engine;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BushFire.Engine.Controllers;
using System;
using BushFire.Engine.UIControls.Internal;
using Microsoft.Xna.Framework.Input;
using BushFire.Engine.Files;
using System.Text;

namespace BushFire.Menu.Containers
{
    class Controls : Container
    {
        private ActionState state;
        private ComboMenuButton activeCombo;

        public Controls(Rectangle localLocation, DockType dockType) : base(localLocation, dockType, true)
        {
            state = ActionState.None;
            name = "Controls";
            alwaysOnTop = true;
            spriteBack = GraphicsManager.GetSpriteColour(3);
            drawSpriteBack = true;
            canChangeFocusOrder = false;
            AddBorder(2, Resizing.NONE, 3);
            AddHeading(50, "Control Settings", GraphicsManager.GetSpriteFont(Font.OpenSans20Bold), Color.White, false, false, false, false, false, GraphicsManager.GetSpriteColour(6));



            SetSizeBounds(0, 0, 900, 2000);
            AddScrollV(15, 6, 20);
            AddButtons();
        }

        private void AddButtons()
        {
            int count = 0;
            ComboMenuButton combo;

            foreach (KeyMap keyMap in (KeyMap[])Enum.GetValues(typeof(KeyMap)))
            {
                string name = keyMap.ToString();
                string displayText = AddSpacesToSentence(keyMap.ToString());
                string displayName = EngineController.keyMapList[keyMap].ToString();
                Keys key = EngineController.keyMapList[keyMap];

                combo = new ComboMenuButton(name, displayText, new CycleObject(displayName, key), new Point(50, 60 + (count * 100)), keyMap);
                AddUiControl(combo);
                count++;
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

        private void UpdateButtons()
        {
            foreach (KeyMap keyMap in (KeyMap[])Enum.GetValues(typeof(KeyMap)))
            {
                string name = keyMap.ToString();

                ComboMenuButton combo = (ComboMenuButton)GetUiControl(name);

                if (combo != null && combo.IsEitherPress())
                {
                    RemoveActiveButton();
                    AddActiveButton(combo);
                }
            }
        }

        private void RemoveActiveButton()
        {
            if (activeCombo != null)
            {
                KeyMap keyMap = (KeyMap)activeCombo.referenceObject;
                activeCombo.SetText(EngineController.keyMapList[keyMap].ToString());
                activeCombo.SetTextColor(Color.White);
                activeCombo = null;
            }
        }

        private void AddActiveButton(ComboMenuButton button)
        {
            activeCombo = button;
            activeCombo.SetText("Press New Key");
            activeCombo.SetTextColor(Color.Red);
            state = ActionState.WaitingKeyPress;
        }

        private void UpdateKeyPress(Input input)
        {
            if (state == ActionState.WaitingKeyPress && activeCombo != null)
            {
                if (input.IsKeyPressed(Keys.Escape))
                {
                    RemoveActiveButton();
                }
                else
                {
                    Keys newKey = input.GetAKeyPress();
                    if (newKey != Keys.None)  //Its a key press
                    {
                        KeyMap keyMap = (KeyMap)activeCombo.referenceObject;
                        EngineController.keyMapList[keyMap] = newKey;
                        activeCombo.SetText(EngineController.keyMapList[keyMap].ToString());
                        activeCombo.SetTextColor(Color.White);
                        RemoveActiveButton();
                        state = ActionState.None;
                        Data.SaveSettings();
                    }
                }
            }
        }

        public override void Update(Input input)
        {
            base.Update(input);
            UpdateButtons();
            UpdateKeyPress(input);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
      
            base.Draw(spriteBatch);

        }

        private enum ActionState
        {
            None,
            WaitingKeyPress
        }
    }
}
