using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BushFire.Engine.Controllers;
using BushFire.Engine.ContentStorage;

namespace BushFire.Engine.UIControls.Abstract
{
    abstract class Button : UiControl
    {
        public bool isHover { get; protected set; }

        //These are readonly
        protected readonly float fadeNoHover = 0;
        protected float fadeHover = 0.8f;
        protected float fadeSelected = 0.5f;
        protected float fadeButtonDown = 1;
        protected float fadeSpeed = 0.03f;

        protected bool centerText;
        private Point preScaleLocation;

        //SoundEffects
        private bool soundOnHover;
        private bool soundOnPress;
        private bool enterHover;
        private bool firstHover;
        private Sound sound;

        protected Button(string name, Point locationPoint, string text, bool centerText, Color fontColor, Font font, Sprite spriteBack, Sprite spriteFront)
        {
            drawSpriteBack = true;
            drawSpriteFront = true;
            drawText = true;
            this.spriteBack = spriteBack;
            this.spriteFront = spriteFront;
            this.name = name;
            currentUiScale = DisplayController.uiScale;
            preScaleLocation = locationPoint;
            SetText(text);
            spriteFont = GraphicsManager.GetSpriteFont(font);
            this.fontColor = fontColor;
            this.centerText = centerText;

            location = new Rectangle(GetIntByScale(locationPoint.X), GetIntByScale(locationPoint.Y), GetIntByScale(spriteBack.location.Width), GetIntByScale(spriteBack.location.Height));

            if (centerText)
            {
                CenterText();
            }
            else
            {
                LeftAlignText(0.1f, true);
            }
        }

        protected override void Rescale()
        {
            location.X = Convert.ToInt32((float)preScaleLocation.X * DisplayController.uiScale);
            location.Y = Convert.ToInt32((float)preScaleLocation.Y * DisplayController.uiScale);
            location.Width = Convert.ToInt32((float)spriteBack.location.Width * DisplayController.uiScale);
            location.Height = Convert.ToInt32((float)spriteBack.location.Height * DisplayController.uiScale);
            currentUiScale = DisplayController.uiScale;

            if (centerText)
            {
                CenterText();
            }
            else
            {
                LeftAlignText(0.1f, true);
            }
        }

        #region METHODS

        public override void AddSound(SoundType soundType, bool onHover, bool onPress)
        {
            soundOnHover = onHover;
            soundOnPress = onPress;
            sound = new Sound(SoundType.Effect3);
        }

        protected void RemoveSound()
        {
            soundOnHover = false;
            sound = null;
        }

        private void ShowHover()
        {
            if (activeTextureFade < fadeHover)
            {
                activeTextureFade += fadeSpeed * EngineController.drawUpdateTime;
            }

            if (activeTextureFade > fadeHover)
            {
                activeTextureFade -= fadeSpeed * EngineController.drawUpdateTime;
            }
        }

        private void HideHover()
        {
            if (activeTextureFade > fadeNoHover)
            {
                activeTextureFade -= fadeSpeed * EngineController.drawUpdateTime;
            }

            if (activeTextureFade < fadeNoHover)
            {
                activeTextureFade = fadeNoHover;
            }
        }

        private void ShowSelected()
        {
            if (activeTextureFade < fadeSelected)
            {
                activeTextureFade += fadeSpeed * EngineController.drawUpdateTime;
            }

            if (activeTextureFade > fadeSelected)
            {
                activeTextureFade -= fadeSpeed * EngineController.drawUpdateTime;
            }
        }

        private void ShowDown(Input input)
        {
            if (activeTextureFade < fadeButtonDown)
            {
                activeTextureFade += fadeSpeed * EngineController.drawUpdateTime;
            }

            if (activeTextureFade > fadeButtonDown)
            {
                activeTextureFade = fadeButtonDown;
            }
            input.ChangeMouseCursor(CursorType.HANDCLOSE);
        }

        public void SetPress()
        {
            activeTextureFade = 1f;
        }
        #endregion

        private void UpdatePress(Input input)
        {
            if (controlClickState != ControlClickState.NONE) { controlClickState = ControlClickState.NONE; }

            if (location.Contains((int)input.GetMousePos().X, (int)input.GetMousePos().Y) && inViewport && enabled)
            {
                isHover = true;

                input.ChangeMouseCursor(CursorType.HANDFINGER);


                if (input.LeftButtonDown())
                {
                    ShowDown(input);
                }
                else
                {
                    ShowHover();
                }

                if (input.LeftButtonClick())
                {
                    controlClickState = ControlClickState.LEFTPRESS;
                }
                else if (input.RightButtonClick())
                {
                    controlClickState = ControlClickState.RIGHTPRESS;
                }
                else
                {
                    controlClickState = ControlClickState.NONE;
                }
            }
            else
            {
                isHover = false;
                HideHover();

                if (selected)
                {
                    ShowSelected();
                }
            }
        }

        private void UpdateSound()
        {
            if (soundOnHover && sound != null)
            {
                firstHover = false;
                if (!enterHover & isHover)
                {
                    enterHover = true;
                    firstHover = true;
                }

                if (!isHover && enterHover)
                {
                    enterHover = false;
                }

                if (firstHover)
                {
                    sound.PlayOneOff(1f);
                }
            }

            if (soundOnPress && IsEitherPress())
            {
                sound.PlayOneOff(1f);
            }
        }
        
        #region CALLS

        public override void Update(Input input)
        {

            base.Update(input);
            UpdatePress(input);
            UpdateSound();
        
        }

        public override void Draw(SpriteBatch spriteBatch, float containerFade)
        {
            base.Draw(spriteBatch, containerFade);
        }

        #endregion

    }
}
