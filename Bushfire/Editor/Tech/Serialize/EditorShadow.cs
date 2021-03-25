using BushFire.Game.Map.MapObjectComponents;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Editor.Tech
{
    
    [Serializable]
    class EditorShadow
    {
        public int shadowId;
        public int tileX;
        public int tileY;
        public ShadowSide shadowSide;
        public int shadowOffsetX;
        public int shadowOffsetY;

        public EditorShadow(int shadowId, int tileX, int tileY, int shadowOffsetX, int shadowOffsetY, ShadowSide shadowSide)
        {
            this.shadowId = shadowId;
            this.tileX = tileX;
            this.tileY = tileY;
            this.shadowOffsetX = shadowOffsetX;
            this.shadowOffsetY = shadowOffsetY;
            this.shadowSide = shadowSide;
        }
    }


}
