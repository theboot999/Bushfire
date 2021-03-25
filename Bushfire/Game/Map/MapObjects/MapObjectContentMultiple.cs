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
    class MapObjectPropertiesMultiple : MapObjectProperties
    {
        protected Piece[,] pieceMap;
        protected List<Shadow> shadowListLeft;
        protected List<Shadow> shadowListRight;
        public int width;
        public int height;

        public MapObjectPropertiesMultiple(Piece[,] pieceMap, List<Shadow> shadowListLeft, List<Shadow> shadowListRight, int width, int height, int elevation, MapObjectType mapObjectType, bool isSinglePiece, int possibleInTileShift) : base(elevation, mapObjectType, isSinglePiece, possibleInTileShift)
        {
            this.pieceMap = pieceMap;
            this.shadowListLeft = shadowListLeft;
            this.shadowListRight = shadowListRight;
            this.width = width;
            this.height = height;
        }

        public bool isPiece(int x, int y)
        {
            return pieceMap[x, y] != null;
        }

        public Piece GetPiece(int x, int y)
        {
            return pieceMap[x, y];
        }
    
        public override void DrawGameViewBoxObject(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, Color color, float scale, float transparency)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (pieceMap[x, y] != null)
                    {
                        Vector2 location = new Vector2((x + tileX) * GroundLayerController.tileSize + GroundLayerController.halfTileSize + smallOffset.X, (y + tileY) * GroundLayerController.tileSize + GroundLayerController.halfTileSize + smallOffset.Y);

                        pieceMap[x, y].DrawGameViewBox(spriteBatch, location, color, scale, transparency);
                    }
                }
            }
        }


        public override void DrawObject(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, Color color, float scale)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (pieceMap[x, y] != null)
                    {
                        Vector2 location = new Vector2((x + tileX) * GroundLayerController.tileSize + GroundLayerController.halfTileSize + smallOffset.X, (y + tileY) * GroundLayerController.tileSize + GroundLayerController.halfTileSize + smallOffset.Y);
                    
                        pieceMap[x, y].Draw(spriteBatch, location, color, scale);
                    }
                }
            }
        }

        public override void DrawShadows(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, float scale)
        {

            if (WorldController.currentWorldShadowSide == ShadowSide.LEFT)
            {
                for (int i = 0; i < shadowListLeft.Count; i++)
                {
                    shadowListLeft[i].Draw(spriteBatch, tileX, tileY, smallOffset, scale);
                }
            }
            else if (WorldController.currentWorldShadowSide == ShadowSide.RIGHT)
            {
                for (int i = 0; i < shadowListRight.Count; i++)
                {
                    shadowListRight[i].Draw(spriteBatch, tileX, tileY, smallOffset, scale);
                }
            }
        }

        public override void DrawVisibleBlock(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, float scale)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (pieceMap[x, y] != null)
                    {
                        Vector2 location = new Vector2((x + tileX) * GroundLayerController.tileSize + GroundLayerController.halfTileSize + smallOffset.X, (y + tileY) * GroundLayerController.tileSize + GroundLayerController.halfTileSize + smallOffset.Y);
                        pieceMap[x, y].DrawVisibleBlock(spriteBatch, location, scale);

                    }
                }
            }
        }

        public override void DrawLighting(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, float scale)
        {

        }


    }
}
