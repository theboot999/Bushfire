using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BushFire.Engine.Controllers;
using BushFire.Engine.ContentStorage;

namespace BushFire.Engine
{


    public class Input
    {
        //keys
        private KeyClickState[] keyStateList;

        private readonly CursorType defaultCursorType = CursorType.POINTER;
        private CursorType changeCursorTypeTo = CursorType.POINTER;
        private CursorType currentCursorType = CursorType.POINTER;

        //mouse scrollvalue
        private int previousScrollValue;
        public float scrollChangeValue;
        //mouse pos
        private Vector2 mousePos = new Vector2();
        private Vector2 previousMousePos = new Vector2();
        private Vector2 mousePosDifference = new Vector2();
        private Vector2 firstClickMousePos = new Vector2();

        //left mouse button
        private MouseClickState leftButton = MouseClickState.UP;

        //right mouse button
        private MouseClickState rightButton = MouseClickState.UP;

        //side mouse button 1
        private MouseClickState sideButtonOne = MouseClickState.UP;

        //side mouse button 2
        private MouseClickState sideButtonTwo = MouseClickState.UP;

        //Midde button
        private MouseClickState middleButton = MouseClickState.UP;

        //Scroll Wheel
        private MouseScrollState mouseScrollState = MouseScrollState.NONE;

        private bool leftButtonClick;
        private bool rightButtonClick;
        private bool middleButtonClick;

        private bool leftButtonDoubleClick;
        private float doubleClickCounter;
        private bool countingDoubleClick;

        private bool leftButtonDown;
        private bool rightButtonDown;
        private bool middleButtonDown;

        private bool sideButtonOneClick;
        private bool sideButtonOneDown;

        private bool sideButtonTwoClick;
        private bool sideButtonTwoDown;

        private bool capsLockOn { get; set; }




        public Input(MouseState mouseState)
        {
            previousScrollValue = mouseState.ScrollWheelValue;
            SetKeyStates();
        }

        

        public List<Vector2> oldMousePos = new List<Vector2>();

        #region METHODS

        private void SetKeyStates()
        {
            int maxKeys = 255;
            keyStateList = new KeyClickState[maxKeys];

            foreach (Keys key in (Keys[])Enum.GetValues(typeof(Keys)))
            {
                if ((int)key < maxKeys)
                {
                    keyStateList[(int)key] = KeyClickState.UP;
                }
            }    
        }

        #endregion

        #region keyLogic      

        private void KeyBoardInput(KeyboardState keyBoardState)
        {
            capsLockOn = keyBoardState.CapsLock;

            for (int i = 0; i < keyStateList.Length; i++)
            {
                if (keyBoardState.IsKeyDown((Keys)i))
                {
                    if (keyStateList[i] == KeyClickState.UP)
                    {
                        if (keyStateList[i] != KeyClickState.PRESS)
                        {
                            keyStateList[i] = KeyClickState.PRESS;
                        }
                    }
                    else if (keyStateList[i] == KeyClickState.PRESS)
                    {
                        keyStateList[i] = KeyClickState.DOWN;
                    }
                }
                else
                {
                    if (keyStateList[i] != KeyClickState.UP)
                    {
                        keyStateList[i] = KeyClickState.UP;
                    }
                }
            }
        }

        public bool IsKeyPressed(Keys key)
        {
            if ((int)(key) < keyStateList.Length)
            {
                return keyStateList[(int)key] == KeyClickState.PRESS;
            }
            return false;
        }

        public bool IsKeyDown(Keys key)
        {
            if ((int)(key) < keyStateList.Length)
            {
                return keyStateList[(int)key] == KeyClickState.DOWN;
            }
            return false;
        }

        public bool IsKeyUp(Keys key)
        {
            if ((int)(key) < keyStateList.Length)
            {
                return keyStateList[(int)key] == KeyClickState.UP;
            }
            return false;
        }

        public Keys GetAKeyPress()
        {
            for (int i = 0; i < keyStateList.Length; i++)
            {
                if (keyStateList[i] == KeyClickState.PRESS)
                {
                    return (Keys)i;
                }
            }
            return Keys.None;
        }

        public bool IsKeyMapPressed(KeyMap keyMap)
        {
            return (IsKeyPressed(EngineController.keyMapList[keyMap]));
        }

        public bool IsKeyMapDown(KeyMap keyMap)
        {
            return (IsKeyDown(EngineController.keyMapList[keyMap]));
        }

        public bool IsKeyMapUp(KeyMap keyMap)
        {
            return (IsKeyUp(EngineController.keyMapList[keyMap]));
        }

        public string GetKeyPressText(bool isNumeric)
        {
            string text = "";

            if (IsKeyPressed(Keys.D0) || IsKeyPressed(Keys.NumPad0)) { text += "0"; }
            if (IsKeyPressed(Keys.D1) || IsKeyPressed(Keys.NumPad1)) { text += "1"; }
            if (IsKeyPressed(Keys.D2) || IsKeyPressed(Keys.NumPad2)) { text += "2"; }
            if (IsKeyPressed(Keys.D3) || IsKeyPressed(Keys.NumPad3)) { text += "3"; }
            if (IsKeyPressed(Keys.D4) || IsKeyPressed(Keys.NumPad4)) { text += "4"; }
            if (IsKeyPressed(Keys.D5) || IsKeyPressed(Keys.NumPad5)) { text += "5"; }
            if (IsKeyPressed(Keys.D6) || IsKeyPressed(Keys.NumPad6)) { text += "6"; }
            if (IsKeyPressed(Keys.D7) || IsKeyPressed(Keys.NumPad7)) { text += "7"; }
            if (IsKeyPressed(Keys.D8) || IsKeyPressed(Keys.NumPad8)) { text += "8"; }
            if (IsKeyPressed(Keys.D9) || IsKeyPressed(Keys.NumPad9)) { text += "9"; }
            if (IsKeyPressed(Keys.Decimal) || IsKeyPressed(Keys.OemPeriod)) { text += "."; }

            if (isNumeric)
            {
                text = RemoveExtraDecimals(text);
            }
            else
            {
                if (IsKeyPressed(Keys.A)) { text += "a"; }
                if (IsKeyPressed(Keys.B)) { text += "b"; }
                if (IsKeyPressed(Keys.C)) { text += "c"; }
                if (IsKeyPressed(Keys.D)) { text += "d"; }
                if (IsKeyPressed(Keys.E)) { text += "e"; }
                if (IsKeyPressed(Keys.F)) { text += "f"; }
                if (IsKeyPressed(Keys.G)) { text += "g"; }
                if (IsKeyPressed(Keys.H)) { text += "h"; }
                if (IsKeyPressed(Keys.I)) { text += "i"; }
                if (IsKeyPressed(Keys.J)) { text += "j"; }
                if (IsKeyPressed(Keys.K)) { text += "k"; }
                if (IsKeyPressed(Keys.L)) { text += "l"; }
                if (IsKeyPressed(Keys.M)) { text += "m"; }
                if (IsKeyPressed(Keys.N)) { text += "n"; }
                if (IsKeyPressed(Keys.O)) { text += "o"; }
                if (IsKeyPressed(Keys.P)) { text += "p"; }
                if (IsKeyPressed(Keys.Q)) { text += "q"; }
                if (IsKeyPressed(Keys.R)) { text += "r"; }
                if (IsKeyPressed(Keys.S)) { text += "s"; }
                if (IsKeyPressed(Keys.T)) { text += "t"; }
                if (IsKeyPressed(Keys.U)) { text += "u"; }
                if (IsKeyPressed(Keys.V)) { text += "v"; }
                if (IsKeyPressed(Keys.W)) { text += "w"; }
                if (IsKeyPressed(Keys.X)) { text += "x"; }
                if (IsKeyPressed(Keys.Y)) { text += "y"; }
                if (IsKeyPressed(Keys.Z)) { text += "z"; }
                if (IsKeyPressed(Keys.Space)) { text += " "; }

                if (IsKeyDown(Keys.LeftShift) || capsLockOn)
                {
                    text = text.ToUpper();
                }
            }
            return text;
        }

        private string RemoveExtraDecimals(string text)
        {
            int count = 0;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '.')
                {
                    count++;
                }
            }

            if (count > 1)
            {
                for (int i = text.Length - 1; i >= 0; i--)
                {
                    if (text[i] == '.')
                    {
                        text = text.Remove(i, 1);
                        count--;
                        if (count == 1)
                        {
                            break;
                        }
                    }
                }
            }

            return text;
        }


        #endregion

        #region Mouselogic

        public void ChangeMouseCursor(CursorType cursorType)
        {
            changeCursorTypeTo = cursorType;
        }

        public CursorType GetCurrentCursorChange()
        {
            return changeCursorTypeTo;
        }

        public void MouseScrollInput(MouseState mouseState)
        {
            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;

            mouseScrollState = MouseScrollState.NONE;

            if (mouseState.ScrollWheelValue < previousScrollValue)
            {
                mouseScrollState = MouseScrollState.SCROLLUP;
            }
            else if (mouseState.ScrollWheelValue > previousScrollValue)
            {
                mouseScrollState = MouseScrollState.SCROLLDOWN;
            }
            scrollChangeValue = mouseState.ScrollWheelValue - previousScrollValue;
            previousScrollValue = mouseState.ScrollWheelValue;
        }

        private void UpdateMouseMoveDifference()
        {
            mousePosDifference = mousePos - previousMousePos;
            previousMousePos = mousePos;
        }

        public bool InViewPort(Viewport viewPort)
        {
            if (viewPort.Bounds.Contains(new Point((int)mousePos.X, (int)mousePos.Y)))
            {
                return true;
            }

            return false;
        }

        public bool InViewPort(Rectangle viewPort)
        {
            if (viewPort.Contains(new Point((int)mousePos.X, (int)mousePos.Y)))
            {
                return true;
            }

            return false;
        }



        public void TranslateMousePos(Vector2 translatedMouse)
        {
            oldMousePos.Add(mousePos);
            mousePos = translatedMouse;
        }

        public void ReturnMousePos()
        {
            mousePos = oldMousePos[oldMousePos.Count - 1];
            oldMousePos.RemoveAt(oldMousePos.Count - 1);
        }

        public Vector2 GetMousePos()
        {
            return mousePos;
        }

        public Vector2 GetMousePosGlobal()
        {
            if (oldMousePos.Count > 0)
            {
                return oldMousePos[0];
            }
            return mousePos;
        }

        public Vector2 GetMousePosDifference()
        {
            return mousePosDifference;
        }

        public bool LeftButtonClick()
        {
            return leftButtonClick;
        }

        public bool RightButtonClick()
        {
            return rightButtonClick;
        }

        public bool LeftButtonDoubleClick()
        {
            return leftButtonDoubleClick;
        }


        public bool LeftButtonDown()
        {
            return leftButtonDown;
        }

        public bool RightButtonDown()
        {
            return rightButtonDown;
        }

        public bool MiddleButtonDown()
        {
            return middleButtonDown;
        }

        public bool MiddleButtonClick()
        {
            return middleButtonClick;
        }

        public bool SideButtonOneClick()
        {
            return sideButtonOneClick;
        }

        public bool SideButtonTwoClick()
        {
            return sideButtonTwoClick;
        }

        public void MouseClickInput(MouseState mouseState)
        {
            leftButtonDoubleClick = false;
            leftButtonClick = false;
            rightButtonClick = false;
            sideButtonOneClick = false;
            sideButtonTwoClick = false;
            middleButtonClick = false;
            leftButtonDown = false;
            rightButtonDown = false;
            sideButtonOneDown = false;
            sideButtonTwoDown = false;
            middleButtonDown = false;

            //left button

            if (!(mouseState.LeftButton == ButtonState.Pressed))
            {
                if (leftButton == MouseClickState.DRAG)
                {
                    leftButtonClick = true;
                }
                leftButton = MouseClickState.UP;
            }
            else
            {
                if (leftButton == MouseClickState.UP)
                {
                    leftButton = MouseClickState.CLICK;
                }
                else if (leftButton == MouseClickState.CLICK)
                {
                    leftButton = MouseClickState.DRAG;
                }

                if (leftButton == MouseClickState.DRAG)
                {
                    leftButtonDown = true;
                }
            }

            //right button
            if (!(mouseState.RightButton == ButtonState.Pressed))
            {
                if (rightButton == MouseClickState.DRAG)
                {
                    rightButtonClick = true;
                }
                rightButton = MouseClickState.UP;
            }
            else
            {
                if (rightButton == MouseClickState.UP)
                {
                    rightButton = MouseClickState.CLICK;
                }
                else if (rightButton == MouseClickState.CLICK)
                {
                    rightButton = MouseClickState.DRAG;
                }

                if (rightButton == MouseClickState.DRAG)
                {
                    rightButtonDown = true;
                }
            }

            //sideButton 1

            if (!(mouseState.XButton1 == ButtonState.Pressed))
            {
                if (sideButtonOne == MouseClickState.DRAG)
                {
                    sideButtonOneClick = true;
                }
                sideButtonOne = MouseClickState.UP;
            }
            else
            {
                if (sideButtonOne == MouseClickState.UP)
                {
                    sideButtonOne = MouseClickState.CLICK;
                }
                else if (sideButtonOne == MouseClickState.CLICK)
                {
                    sideButtonOne = MouseClickState.DRAG;
                }

                if (sideButtonOne == MouseClickState.DRAG)
                {
                    sideButtonOneDown = true;
                }
            }

            //sideButton 2

            if (!(mouseState.XButton2 == ButtonState.Pressed))
            {
                if (sideButtonTwo == MouseClickState.DRAG)
                {
                    sideButtonTwoClick = true;
                }
                sideButtonTwo = MouseClickState.UP;
            }
            else
            {
                if (sideButtonTwo == MouseClickState.UP)
                {
                    sideButtonTwo = MouseClickState.CLICK;
                }
                else if (sideButtonTwo == MouseClickState.CLICK)
                {
                    sideButtonTwo = MouseClickState.DRAG;
                }

                if (sideButtonTwo == MouseClickState.DRAG)
                {
                    sideButtonTwoDown = true;
                }
            }

            //middleButton2

            if (!(mouseState.MiddleButton == ButtonState.Pressed))
            {
                if (middleButton == MouseClickState.DRAG)
                {
                    middleButtonClick = true;
                }
                middleButton = MouseClickState.UP;
            }
            else
            {
                if (middleButton == MouseClickState.UP)
                {
                    middleButton = MouseClickState.CLICK;
                }
                else if (middleButton == MouseClickState.CLICK)
                {
                    middleButton = MouseClickState.DRAG;
                }

                if (middleButton == MouseClickState.DRAG)
                {
                    middleButtonDown = true;
                }
            }


            if (!countingDoubleClick && leftButtonClick)
            {
                countingDoubleClick = true;
                firstClickMousePos = mousePos;
            }
            else if (countingDoubleClick)
            {
                doubleClickCounter += 0.015f * EngineController.drawUpdateTime;

                if (doubleClickCounter > 1)
                {
                    doubleClickCounter = 0;
                    countingDoubleClick = false;
                }
                if (leftButtonClick)
                {
                    if (Vector2.Distance(firstClickMousePos, mousePos) < 100f)
                    {
                        leftButtonDoubleClick = true;
                    }
                    doubleClickCounter = 0;
                    countingDoubleClick = false;
                }
            }


        }
       

        public MouseScrollState getMouseScrollState()
        {
            return mouseScrollState;
        }

        #endregion

        #region CALLS

        public void Update(KeyboardState keyBoardState, MouseState mouseState)
        {
            changeCursorTypeTo = defaultCursorType; //At the start of a frame we always set cursor to default
            KeyBoardInput(keyBoardState);
            MouseScrollInput(mouseState);
            MouseClickInput(mouseState);
            UpdateMouseMoveDifference();
        }

        public void UpdateCursor()
        {
            //Changing the cursor is expensive.  We only want to change it if there was a change from the previous cursor
            if (changeCursorTypeTo != currentCursorType)
            {
                currentCursorType = changeCursorTypeTo;

                Mouse.SetCursor(GraphicsManager.GetMouseCursor(currentCursorType));
            }
        }

        #endregion
    }
}

