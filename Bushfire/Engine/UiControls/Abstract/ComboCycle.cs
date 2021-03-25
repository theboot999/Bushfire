using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Internal;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.UIControls.Abstract
{
    abstract class ComboCycle : UiControl
    {
        public int listIndex = 0;
        List<CycleObject> cycleObjectList = new List<CycleObject>();
        Label labelText;
        Label labelObjectText;
        Button cycleDown;
        Button cycleUp;
        bool cycleAround;
        bool highlightAll;
        bool drawVisuals;

        //visuals
        Rectangle prescaleVisuals;
        List<Visual> visualList;


        protected Rectangle preScaleLocation;
 
        public ComboCycle(string name, Rectangle location, bool cycleAround, bool highlightAll, bool drawVisuals)
        {
            List<Visual> visualListNew = new List<Visual>();
            this.drawVisuals = drawVisuals;
            this.cycleAround = cycleAround;
            this.highlightAll = highlightAll;
            this.name = name;
            preScaleLocation = location;           
            SetSize();

            
        }

        public void Init(Label labelText, Button cycleDown, Button cycleUp, Label labelObjectText, Rectangle visuals)
        {
            this.labelText = labelText;
            this.cycleDown = cycleDown;
            this.cycleUp = cycleUp;
            this.labelObjectText = labelObjectText;
            prescaleVisuals = visuals;
            SetScaleVisuals();
        }

        protected void SetSize()
        {
            location.X = Convert.ToInt32((float)preScaleLocation.X * DisplayController.uiScale);
            location.Y = Convert.ToInt32((float)preScaleLocation.Y * DisplayController.uiScale);
            location.Width = Convert.ToInt32((float)preScaleLocation.Width * DisplayController.uiScale);
            location.Height = Convert.ToInt32((float)preScaleLocation.Height * DisplayController.uiScale);
            currentUiScale = DisplayController.uiScale;
        }

        private void SetScaleVisuals()
        {
            if (cycleObjectList.Count > 0)
            {
                visualList = new List<Visual>();
                Rectangle scaleLocation = CalcScaleRectangle(prescaleVisuals, DisplayController.uiScale);

                int count = cycleObjectList.Count();
                float gap = 6 * DisplayController.uiScale;
                
                float numberOfGaps = count - 1;
                float sizeGap = gap * numberOfGaps;
                float eachWidth = (scaleLocation.Width - sizeGap) / count;
                float halfWidth = eachWidth * 0.5f;
                float height = scaleLocation.Height;
                float startX = scaleLocation.X;

                for (int i = 0; i < count; i++)
                {
                    Vector2 location = new Vector2(startX + (i * gap) + (eachWidth * i), scaleLocation.Y + (height * 0.5f));
                    Vector2 scale = new Vector2(eachWidth, height);
                    Visual visual = new Visual(location, scale);
                    visualList.Add(visual);
                }
            }
        }


        protected Point CalcPointOffset(Point point)
        {           
            return point + new Point(preScaleLocation.X, preScaleLocation.Y);
        }

        protected Vector2 CalcVectorOffset(Vector2 vector)
        {
            return vector + new Vector2(preScaleLocation.X, preScaleLocation.Y);
        }

        protected Rectangle CalcRectangleOffset(Rectangle rectangle)
        {
            return new Rectangle(preScaleLocation.X + rectangle.X, preScaleLocation.Y + rectangle.Y, rectangle.Width, rectangle.Height);
        }

        protected override void Rescale()
        {
            SetSize();
            SetScaleVisuals();
        }

        public void AddCycleObject(CycleObject cycleObject)
        {
            cycleObjectList.Add(cycleObject);
            SetScaleVisuals();
        }


        public void SetIndex(int value)
        {
            if (listIndex < cycleObjectList.Count)
            {
                listIndex = value;              
            }
            else
            {
                listIndex = 0;
            }
            changed = true;
            UpdateObjectText();
        }

        public void SetLastIndex()
        {
            listIndex = cycleObjectList.Count - 1;
            changed = true;
            UpdateObjectText();
            return;
        }
        
        public CycleObject GetSelectedCycle()
        {
            return cycleObjectList[listIndex];
        }

        public Object GetSelectedCycleObject()
        {
            return cycleObjectList[listIndex].value;
        }

        public void SetIndexByFloat(float value)
        {
            for (int i = 0; i < cycleObjectList.Count; i++)
            {              
                float temp = (float)cycleObjectList[i].value;
                if (temp == value)
                {
                    SetIndex(i);
                    UpdateObjectText();
                    return;
                }
            }       
        }

        public void SetIndexByInt(int value)
        {
            for (int i = 0; i < cycleObjectList.Count; i++)
            {
                int temp = (int)cycleObjectList[i].value;
                if (temp == value)
                {
                    SetIndex(i);
                    UpdateObjectText();
                    return;
                }
            }
        }

        public void SetIndexByBool(bool value)
        {
            for (int i = 0; i < cycleObjectList.Count; i++)
            {
                bool temp = (bool)cycleObjectList[i].value;
                if (temp == value)
                {
                    SetIndex(i);
                    UpdateObjectText();
                    return;
                }
            }
        }

        public void UpdatePresses()
        {
            controlClickState = ControlClickState.NONE;
            if (cycleDown.IsEitherPress())
            {
                changed = true;
                listIndex--;

                if (listIndex < 0)
                {
                    if (cycleAround)
                    {
                        listIndex = cycleObjectList.Count - 1;
                    }
                    else
                    {
                        listIndex = 0;
                    }                  
                }
                controlClickState = ControlClickState.LEFTPRESS;
            }

            if (cycleUp.IsEitherPress())
            {
                changed = true;
                listIndex++;

                if (listIndex > cycleObjectList.Count - 1)
                {
                    if (cycleAround)
                    {
                        listIndex = 0;
                    }
                    else
                    {
                        listIndex = cycleObjectList.Count - 1;
                    }
                }
                controlClickState = ControlClickState.LEFTPRESS;
            }
        }

        private void UpdateObjectText()
        {
            if (GetSelectedCycle() != null)
            {
                labelObjectText.SetText(GetSelectedCycle().displayName);
            }
        }

        //controlClickState = ControlClickState.LEFTPRESS;
        public override void Update(Input input)
        {
            base.Update(input);
            cycleDown.SetInViewPort(inViewport);
            cycleDown.Update(input);
            cycleUp.SetInViewPort(inViewport);
            cycleUp.Update(input);
            labelText.Update(input);
            UpdatePresses();
            UpdateObjectText();
            labelObjectText.Update(input);
         }

        private void DrawVisuals(SpriteBatch spriteBatch, float containerFade)
        {
            if (drawVisuals)
            {
                     for (int i = 0; i < visualList.Count; i++)
                {
                    if (highlightAll && i <= listIndex)
                    {
                        visualList[i].DrawFront(spriteBatch, containerFade);
                    }
                    else if (highlightAll && i > listIndex)
                    {
                        visualList[i].DrawBack(spriteBatch, containerFade);
                    }
                    else if (!highlightAll && i != listIndex)
                    {
                        visualList[i].DrawBack(spriteBatch, containerFade);
                    }
                    else if (!highlightAll && i == listIndex)
                    {
                        visualList[i].DrawFront(spriteBatch, containerFade);
                    }

                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float containerFade)
        {
            base.Draw(spriteBatch, containerFade);
            cycleDown.Draw(spriteBatch, containerFade);
            cycleUp.Draw(spriteBatch, containerFade);
            labelText.Draw(spriteBatch, containerFade);

            labelObjectText.Draw(spriteBatch, containerFade);
            DrawVisuals(spriteBatch, containerFade);
        }
    }
}
