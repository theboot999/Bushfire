using BushFire.Editor.Tech;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Game.Controllers;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.Map.MapObjects;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map
{
    class MapObject
    {
        public MapObjectProperties mapObjectProperties;
        public int tileX { get; private set; }
        public int tileY { get; private set; }
        public int id;
        public Color color;
        public Vector2 smallOffset;
        public float scale;

        public MapObject(MapObjectProperties mapObjectProperties, int tileX, int tileY, Color color, float scale)
        {
            this.mapObjectProperties = mapObjectProperties;
            this.tileX = tileX;
            this.tileY = tileY;
            id = WorldController.GetNextMapObjectId();
            this.color = color;
            this.scale = scale;
            if (mapObjectProperties.possibleInTileShift > 0)
            {
                int x = GameController.rnd.Next(mapObjectProperties.possibleInTileShift * -1, mapObjectProperties.possibleInTileShift);
                int y = GameController.rnd.Next(mapObjectProperties.possibleInTileShift * -1, mapObjectProperties.possibleInTileShift);
                smallOffset = new Vector2(x, y);
            }
        }

        public MapObjectProperties GetMapObjectProperties()
        {
            return mapObjectProperties;
        }

        public MapObjectType GetMapObjectType()
        {
            if (mapObjectProperties != null)
            {
                return mapObjectProperties.mapObjectType;
            }
            return MapObjectType.NONE;
        }

        public void DrawGameViewBoxObject(SpriteBatch spriteBatch, float transparency)
        {
            mapObjectProperties.DrawGameViewBoxObject(spriteBatch, tileX, tileY, smallOffset, color, scale, transparency);
        }

        public void DrawObject(SpriteBatch spriteBatch)
        {
            mapObjectProperties.DrawObject(spriteBatch, tileX, tileY, smallOffset, color, scale);
        }
        public void DrawShadows(SpriteBatch spriteBatch)
        {
            mapObjectProperties.DrawShadows(spriteBatch, tileX, tileY, smallOffset, scale);
        }

        public void DrawLighting(SpriteBatch spriteBatch)
        {
            mapObjectProperties.DrawLighting(spriteBatch, tileX, tileY, smallOffset, scale);
        }

        public void DrawVisibleBlock(SpriteBatch spriteBatch)
        {
            mapObjectProperties.DrawVisibleBlock(spriteBatch, tileX, tileY, smallOffset, scale);
        }
    }

    enum MapObjectType
    {
        NONE,
        BUILDING,
        STOPLIGHT,
        STREETLIGHT,
        PARKINGSPOT,
        TREE,
        INTERSECTION
    }
}
