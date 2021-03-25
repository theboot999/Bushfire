using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.UIControls
{
    class TextBox : UiControl
    {
        private Rectangle preScaleLocation;

        private readonly int stringLength;
        private readonly bool isNumericOnly;
        private Boolean isFocused;
        private float deleteCounter;
        private float flashCounter;
        private bool onFlash;
        private bool isHover;

        public TextBox(string name, Rectangle location, Font font, Color textColor, string startingText, bool isNumericOnly, int stringLength)
        {
            this.name = name;
            this.stringLength = stringLength;
            SetText(startingText);
            this.isNumericOnly = isNumericOnly;
            this.stringLength = stringLength;

            SetTextColor(textColor);
            drawText = true;
            spriteBack = GraphicsManager.GetSpriteColour(20);
            spriteFront = GraphicsManager.GetSpriteColour(9);
            drawSpriteBack = true;
            drawSpriteFront = false;
            spriteFont = GraphicsManager.GetSpriteFont(font);

            preScaleLocation = location;
            this.location = new Rectangle(GetIntByScale(location.X), GetIntByScale(location.Y), GetIntByScale(location.Width), GetIntByScale(location.Height));
            Rescale();
        }

        protected override void Rescale()
        {
            //need to set the text in case its empty on a rescale to get the text height
            string tempText = GetText();
            SetText("1");
            location.X = Convert.ToInt32((float)preScaleLocation.X * DisplayController.uiScale);
            location.Y = Convert.ToInt32((float)preScaleLocation.Y * DisplayController.uiScale);
            location.Width = Convert.ToInt32((float)preScaleLocation.Width * DisplayController.uiScale);
            location.Height = Convert.ToInt32((float)preScaleLocation.Height * DisplayController.uiScale);
            currentUiScale = DisplayController.uiScale;
            LeftAlignText(5f, false);
            SetText(tempText);

        }

        private void UpdateFocus(Input input)
        {
            if (location.Contains((int)input.GetMousePos().X, (int)input.GetMousePos().Y))
            {
                isHover = true;
                if (input.LeftButtonClick())
                {
                    isFocused = true;
                }
            }
            else
            {
                isHover = false;
                if (input.LeftButtonClick())
                {
                    isFocused = false;
                }
            }
        }

        private void UpdateFlashCounter()
        {
            if (isFocused)
            {
                flashCounter -= 0.1f * EngineController.drawUpdateTime;
            }
            else
            {
                onFlash = false;

            }

            if (flashCounter < 0)
            {
                flashCounter = 3;
                onFlash = !onFlash;
            }

            if (onFlash)
            {
                textFlash = "|";
            }
            else
            {
                textFlash = "";
            }

        }

        public void UpdateFade()
        {
            if (isFocused)
            {
                activeTextureFade += 0.05f * EngineController.drawUpdateTime;
            }
            else
            {
                activeTextureFade -= 0.05f * EngineController.drawUpdateTime;
            }

            activeTextureFade = MathHelper.Clamp(activeTextureFade, 0, 1);

            if (activeTextureFade == 0)
            {
                drawSpriteFront = false;
            }
            else
            {
                drawSpriteFront = true;
            }
        }

        private void UpdateInput(Input input)
        {
            if (isFocused)
            {
                string oldText = GetText();
                string newText = GetText();

                 
                //Get our input
                newText += input.GetKeyPressText(isNumericOnly);

                //Check if we are too long

                int extra = 0;
                if (newText.Contains("-"))
                {
                    extra = 1;
                }


                if (newText.Length > stringLength + extra)
                {
                    newText = newText.Remove(stringLength, newText.Length - stringLength);
                }

                if (input.IsKeyPressed(Keys.Enter)) { isFocused = false; }

                if (input.IsKeyPressed(Keys.Subtract))
                {
                    if (newText.Contains("-"))
                    {
                        newText = newText.Remove(0, 1);
                    }
                    else
                    {
                        newText = "-" + newText;
                    }
                }

                //Pressing back, if holding down use a delete delay
                if (input.IsKeyDown(Keys.Back) && deleteCounter == 0)
                {
                    if (newText.Length > 0)
                    {
                        newText = newText.Remove(newText.Length - 1, 1);
                        deleteCounter = 2;
                    }
                }

                if (input.IsKeyUp(Keys.Back))
                { 
                    deleteCounter = 0;
                }
                
                //if we let the key up we are pressing the key.  No need for a delay then

                if (newText != oldText)
                {
                    SetText(newText);
                    changed = true;
                }
            }

        }

        private void UpdateDeleteCounter()
        {
            if (deleteCounter > 0)
            {
                deleteCounter -= 0.1f * EngineController.drawUpdateTime;
            }
            else
            {
                deleteCounter = 0;
            }
        }

        #region CALLS

        public override void Update(Input input)
        {
            base.Update(input);
            UpdateFocus(input);
            UpdateFlashCounter();
            UpdateFade();
            UpdateInput(input);
            UpdateDeleteCounter();
        }

        public override void Draw(SpriteBatch spriteBatch, float containerFade)
        {
            base.Draw(spriteBatch, containerFade);
        }

        #endregion
    }
}
