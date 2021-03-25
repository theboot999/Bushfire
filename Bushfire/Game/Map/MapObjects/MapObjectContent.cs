using BushFire.Game.Map.MapObjectComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map.MapObjects
{
    //idea if each mapobject has a width and ehight perhaps we could do better culling of bojects for draw. this width and hegiht could also include
    //the shadow. it would save a lot of later itterations iwth trees etc

    abstract class MapObjectProperties
    {

        public bool isSinglePiece;
        public int elevation;
        public MapObjectType mapObjectType;
        public int possibleInTileShift;

        public MapObjectProperties(int elevation, MapObjectType mapObjectType, bool isSinglePiece, int possibleInTileShift)
        {
            this.possibleInTileShift = possibleInTileShift;
            this.isSinglePiece = isSinglePiece;
            this.elevation = elevation;
            this.mapObjectType = mapObjectType;
        }

        public virtual void DrawGameViewBoxObject(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, Color color, float scale, float transparency)
        {

        }

        public virtual void DrawObject(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, Color color, float scale)
        {
         
        }
        public virtual void DrawShadows(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, float scale)
        {
           
        }

        public virtual void DrawVisibleBlock(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, float scale)
        {     

        }

        public virtual void DrawLighting(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, float scale)
        {

        }
    }
}
