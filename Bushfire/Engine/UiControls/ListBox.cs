using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls.Internal;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace BushFire.Engine.UIControls
{
    class ListBox : UiControl
    {
        //Good listbox for a game panel new ListBox("ListBox", new Rectangle(10, 25, 300, 500), GraphicsManager.GetSpriteColour(7), Font.OpenSans16, containerCamera);

        private Rectangle preScaleLocation;
        private List<ListBoxObject> itemList;
        private ListBoxObject selectedItem;
        private ListBoxCamera listBoxCamera;
        private ContainerCamera parentContainerCamera;
        private ListBoxScrollV listBoxScrollV;
        private int currentCount;

        public ListBox(string name, Rectangle location, Sprite spriteBack, Font font, ContainerCamera parentContainerCamera)
        {
            currentCount = 0;
            retranslateDraw = true;
            listBoxCamera = new ListBoxCamera();
            listBoxScrollV = new ListBoxScrollV(listBoxCamera, location);
            this.parentContainerCamera = parentContainerCamera;
            itemList = new List<ListBoxObject>();
            drawSpriteBack = true;
            this.spriteBack = spriteBack;
            this.name = name;
            currentUiScale = DisplayController.uiScale;
            preScaleLocation = location;
            spriteFont = GraphicsManager.GetSpriteFont(font);
            Rescale();           
            UpdateViewport();
        }

        private void UpdateViewport()
        {
            listBoxCamera.UpdateViewport(new Viewport(parentContainerCamera.worldCameraViewport.X + location.X, parentContainerCamera.worldCameraViewport.Y + location.Y, location.Width, location.Height));
        }

        protected override void Rescale()
        {          
            location.X = Convert.ToInt32((float)preScaleLocation.X * DisplayController.uiScale);
            location.Y = Convert.ToInt32((float)preScaleLocation.Y * DisplayController.uiScale);
            location.Width = Convert.ToInt32((float)preScaleLocation.Width * DisplayController.uiScale);
            location.Height = Convert.ToInt32((float)preScaleLocation.Height * DisplayController.uiScale);
            currentUiScale = DisplayController.uiScale;
            UpdateViewport();

            int fontHeight = (int)(spriteFont.MeasureString("D").Y * DisplayController.uiScale);

            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].SetLocation(GetItemSize(i), fontHeight, itemList[i].index);
            }
            listBoxScrollV.Rescale(location);
            SetMaxHeight();
        }


        public Rectangle GetItemSize(int itemNumber)
        {
            int fontHeight = (int)(spriteFont.MeasureString("D").Y * DisplayController.uiScale) + GetIntByScale(10);
            int width = location.Width - GetIntByScale(listBoxScrollV.width);
            return new Rectangle(0, fontHeight * itemNumber, width, fontHeight);
        }

        public void AddItem(ListBoxObject item)
        {
            if (!itemList.Contains(item))
            {
                
                Rectangle location = GetItemSize(currentCount);
                int fontHeight = (int)(spriteFont.MeasureString("D").Y * DisplayController.uiScale);

                item.SetLocation(location, fontHeight, currentCount);               
                itemList.Add(item);
                currentCount++;
            }
            SetMaxHeight();
        }

        public bool AddItemUniqueDisplay(ListBoxObject item)
        {
            if (!itemList.Contains(item))
            {
                foreach (ListBoxObject itemIn in itemList)
                {
                    if (itemIn.displayName == item.displayName)
                    {
                        return false;
                    }
                }
                
                Rectangle location = GetItemSize(currentCount);
                int fontHeight = (int)(spriteFont.MeasureString("D").Y * DisplayController.uiScale);

                item.SetLocation(location, fontHeight, currentCount);
                itemList.Add(item);
                currentCount++;
                SetMaxHeight();
                return true;
            }
            return false;
        }

        public void RemoveSelectedItem()
        {
            if (selectedItem != null)
            {
                int index = selectedItem.index;


                itemList.Remove(selectedItem);
                currentCount--;
                for (int i = index; i < currentCount; i++)
                {
                    Rectangle location = GetItemSize(i);
                    int fontHeight = (int)(spriteFont.MeasureString("D").Y * DisplayController.uiScale);
                    itemList[i].SetLocation(location, fontHeight, i);
                }

                selectedItem = null;

                if (index > itemList.Count - 1)
                {
                    index = itemList.Count - 1;
                }

                if (index > -1)
                {
                    itemList[index].selected = true;
                    selectedItem = itemList[index];
                }

                SetMaxHeight();
            }
        }

        public object GetSelectedObjectValue()
        {
            if (selectedItem != null)
            {
                return selectedItem.value;
            }
            return null;
        }

        private void SetMaxHeight()
        {
            float maxSize = 0;
            if (itemList.Count > 0)
            {
                ListBoxObject item = itemList.Last();               
                if (item != null)
                {
                    maxSize = (float)(item.location.Y + item.location.Height);
                }
            }

            if (maxSize < location.Height)
            {
                maxSize = location.Height;
            }
            listBoxCamera.maxHeight = maxSize;
            listBoxScrollV.Rescale(location);
            //else max size just equals the height;
        }

        private void TranslateInputToCamera(Input input)
        {
            input.TranslateMousePos(input.GetMousePos() - new Vector2(location.X - listBoxCamera.cameraPosition.X, location.Y - listBoxCamera.cameraPosition.Y));
        }

        private void TranslateInputBack(Input input)
        {
            input.ReturnMousePos();
        }

        public void SetIndex(int index)
        {
            if (index < itemList.Count)
            {
                selectedItem = itemList[index];
                itemList[index].selected = true;
                changed = true;
                foreach (ListBoxObject item in itemList)
                {
                    if (item != selectedItem)
                    {
                        item.selected = false;
                    }
                }
            }
        }

        private void UpdateItems(Input input)
        {
            changed = false;
            foreach (ListBoxObject item in itemList)
            {
                item.Update(input, inViewport);

                if (item.selected && item != selectedItem)
                {
                    if (selectedItem != null)
                    {
                        selectedItem.selected = false;
                    }
                    selectedItem = item;
                    changed = true;
                }
            }
        }


        public override void Update(Input input)
        {
            UpdateViewport();
            base.Update(input);
            listBoxScrollV.parentHasFocus = inViewport;
            listBoxScrollV.inListBoxViewPort = location.Contains((int)input.GetMousePos().X, (int)input.GetMousePos().Y);
            listBoxScrollV.Update(input);

            TranslateInputToCamera(input);
            UpdateItems(input);

            TranslateInputBack(input);
        }

        public override void Draw(SpriteBatch spriteBatch, float containerFade)
        {
            base.Draw(spriteBatch, containerFade);

            listBoxScrollV.Draw(spriteBatch, containerFade);

            listBoxCamera.TranslateDraw(spriteBatch);

            foreach (ListBoxObject item in itemList)
            {
                item.Draw(spriteBatch, spriteFont, containerFade);
            }

        }
    }
}
