using BushFire.Editor.Tech;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.Storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map.MapObjects
{
    class MapObjectPropertiesSingle : MapObjectProperties
    {
        protected Piece piece;
       // protected Point shadowPoint;
        protected Shadow shadowLeft;
        protected Shadow shadowRight;

        public MapObjectPropertiesSingle(int elevation, MapObjectType mapObjectType, bool isSinglePiece, int possibleInTileShift) : base(elevation, mapObjectType, isSinglePiece, possibleInTileShift)
        {

        }

        public virtual Piece GetPiece()
        {
            return piece;
        }

        public override void DrawGameViewBoxObject(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, Color color, float scale, float transparency)
        {
            Vector2 location = new Vector2(tileX * GroundLayerController.tileSize + GroundLayerController.halfTileSize + smallOffset.X, tileY * GroundLayerController.tileSize + GroundLayerController.halfTileSize + smallOffset.Y);
            piece.DrawGameViewBox(spriteBatch, location, color, scale, transparency);
        }

        public override void DrawObject(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, Color color, float scale)
        {
            Vector2 location = new Vector2(tileX * GroundLayerController.tileSize + GroundLayerController.halfTileSize + smallOffset.X, tileY * GroundLayerController.tileSize + GroundLayerController.halfTileSize + smallOffset.Y);
            piece.Draw(spriteBatch, location, color, scale);
        }

        public override void DrawShadows(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, float scale)
        {
            if (WorldController.currentWorldShadowSide == ShadowSide.LEFT && shadowLeft != null)
            {
                shadowLeft.Draw(spriteBatch, tileX, tileY, smallOffset, scale);
            }
            else if (WorldController.currentWorldShadowSide == ShadowSide.RIGHT && shadowRight != null)
            {
                shadowRight.Draw(spriteBatch, tileX, tileY, smallOffset, scale);
            }
        }

        public override void DrawVisibleBlock(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, float scale)
        {
            Vector2 location = new Vector2(tileX * GroundLayerController.tileSize + GroundLayerController.halfTileSize + smallOffset.X, tileY * GroundLayerController.tileSize + GroundLayerController.halfTileSize + smallOffset.Y);
            piece.DrawVisibleBlock(spriteBatch, location, scale);
        }

        public override void DrawLighting(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, float scale)
        {

        }
    }
}
