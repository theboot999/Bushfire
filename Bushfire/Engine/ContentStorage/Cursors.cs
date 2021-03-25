using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.ContentStorage
{
    class Cursors
    {
        private ContentManager content;
        private Dictionary<CursorType, MouseCursor> cursorList = new Dictionary<CursorType, MouseCursor>();

        public Cursors(ContentManager content)
        {
            this.content = content;
            Load();
        }

        private void Load()
        {
               cursorList.Add(CursorType.POINTER, MouseCursor.FromTexture2D(content.Load<Texture2D>(@"Cursors/Pointer"), 0, 0));
               cursorList.Add(CursorType.HANDFINGER, MouseCursor.FromTexture2D(content.Load<Texture2D>(@"Cursors/HandFinger"), 8, 0));
               cursorList.Add(CursorType.HANDCLOSE, MouseCursor.FromTexture2D(content.Load<Texture2D>(@"Cursors/HandClose"), 8, 0));
               cursorList.Add(CursorType.ARROWHORIZONTAL, MouseCursor.FromTexture2D(content.Load<Texture2D>(@"Cursors/ArrowHorizontal"), 15, 0));
               cursorList.Add(CursorType.ARROWVERTICAL, MouseCursor.FromTexture2D(content.Load<Texture2D>(@"Cursors/ArrowVertical"), 0, 15));
               cursorList.Add(CursorType.ARROWDIAGONAL, MouseCursor.FromTexture2D(content.Load<Texture2D>(@"Cursors/ArrowDiagonal"), 11, 11));
               Mouse.SetCursor(cursorList[CursorType.POINTER]);
        }

        public MouseCursor GetMouseCursor(CursorType cursorType)
        {
            return (cursorList[cursorType]);
        }   
    }

    //Cursors to add
    //HandOpen
    //Question

    public enum CursorType
    {
        POINTER,
        HANDFINGER,
        HANDCLOSE,
        ARROWHORIZONTAL,
        ARROWVERTICAL,
        ARROWDIAGONAL,
    }
}
