using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.Map.MapObjects;
using BushFire.Game.Storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map
{
    //Each cell is updated once per frame from the worldMiniMap Class
    //Each cell contains a concurrent queue of redraws required
    //Later on we will add if its on fire or if its burnt or whatever needs to be added
    //but basically we can redraw the basic cell to show that its burnt
    //its a concurrent queue so we can call this from the fire burning thread

    class MiniMapCell
    {
        int tilesPerCell;
        int tilePixel;
        private int cellPixelSize;
        public Sprite sprite;
        int renderTargetTextureId;
        int xMajor;
        int yMajor;
        //List<Point> recalculateList = new List<Point>();
        private ConcurrentQueue<Point> redrawQueue;


        public MiniMapCell(int xMajor, int yMajor, int tilesPerCell, int tilePixel)
        {
            this.xMajor = xMajor;
            this.yMajor = yMajor;
            this.tilesPerCell = tilesPerCell;
            this.tilePixel = tilePixel;
            cellPixelSize = tilesPerCell * tilePixel;
            InitCreateCell();
            redrawQueue = new ConcurrentQueue<Point>();
      //      DebugFun();
        }

        private void DebugFun()
        {
            for (int y = yMajor * tilesPerCell; y < yMajor * (tilesPerCell) + tilesPerCell; y++)
            {
                for (int x = xMajor * tilesPerCell; x < xMajor * (tilesPerCell) + tilesPerCell; x++)
                {

                    AddToRecalculateList(new Point(x, y));
                }
            }
        }

        private void InitCreateCell()
        {
            //Preparing
            renderTargetTextureId = DisplayController.AddRenderTarget(cellPixelSize - 1, cellPixelSize - 1, true);

            ScreenController.graphicsDevice.SetRenderTarget(DisplayController.GetRenderTarget(renderTargetTextureId));

            DisplayController.spriteBatch1.Begin(SpriteSortMode.Deferred);
            HashSet<MapObject> mapObjectList = new HashSet<MapObject>();

            //Adding
            InitAddTiles(mapObjectList, xMajor, yMajor);

            //Output
            sprite = new Sprite(new Rectangle(0, 0, cellPixelSize, cellPixelSize), DisplayController.GetRenderTarget(renderTargetTextureId));
            DisplayController.spriteBatch1.End();
            ScreenController.graphicsDevice.SetRenderTarget(null);
            //Debug.WriteLine(next);
        }

        private void InitAddTiles(HashSet<MapObject> mapObjectList, int xMajor, int yMajor)
        {
            for (int x = 0; x < tilesPerCell; x++)
            {
                for (int y = 0; y < tilesPerCell; y++)
                {
                    Rectangle destination = new Rectangle(x * tilePixel, y * tilePixel, tilePixel, tilePixel);
                    Tile tile = WorldController.world.tileGrid[(tilesPerCell * xMajor) + x, (tilesPerCell * yMajor) + y];
                    AddLayerDraw(tile, destination, Color.White);

                    MapObject mapObject = tile.GetChildObject();
                    if (mapObject != null)
                    {
                        AddMapObjectDraw(tile, destination, (tilesPerCell * xMajor) + x, (tilesPerCell * yMajor) + y, mapObject.color);
                    }
                    
                }
            }
        }

        private void AddLayerDraw(Tile tile, Rectangle destination, Color color)
        {
            if (tile.GetBaseLayer() != null)
            {
                GroundLayer layer = tile.GetBaseLayer();
                DisplayController.spriteBatch1.Draw(layer.sprite.texture2D, destination, layer.sprite.location, color, layer.sprite.rotation, layer.sprite.rotationCenter, SpriteEffects.None, 1f);
            }

            if (tile.GetLayerList() != null)
            {
                foreach (GroundLayer extraLayer in tile.GetLayerList())
                {
                    DisplayController.spriteBatch1.Draw(extraLayer.sprite.texture2D, destination, extraLayer.sprite.location, color, extraLayer.sprite.rotation, extraLayer.sprite.rotationCenter, SpriteEffects.None, 1f);
                }
            }

            if (tile.GetBurntLayer() != null)
            {
                GroundLayer layer = tile.GetBurntLayer();
                DisplayController.spriteBatch1.Draw(layer.sprite.texture2D, destination, layer.sprite.location, color, layer.sprite.rotation, layer.sprite.rotationCenter, SpriteEffects.None, 1f);
            }

            /*  if (tile.isCompleteBurnt)
              {
                  GroundLayer extraLayer = GroundLayerController.burntLayer;
                  DisplayController.spriteBatch1.Draw(extraLayer.sprite.texture2D, destination, extraLayer.sprite.location, color, extraLayer.sprite.rotation, extraLayer.sprite.rotationCenter, SpriteEffects.None, 1f);
              }*/
        }

        private void AddMapObjectDraw(Tile tile, Rectangle destination, int worldTileX, int worldTileY, Color color)
        {
            MapObject mapObject = tile.GetChildObject();

            if (mapObject != null)
            {
                if (mapObject.mapObjectProperties.isSinglePiece)
                {
                    MapObjectPropertiesSingle single = (MapObjectPropertiesSingle)mapObject.mapObjectProperties;
                    Piece piece = single.GetPiece();
                    piece.DrawMiniMapTile(DisplayController.spriteBatch1, destination, 1f, color);
                }
                else
                {
                    MapObjectPropertiesMultiple multiple = (MapObjectPropertiesMultiple)mapObject.mapObjectProperties;
                    int arrayX = worldTileX - mapObject.tileX;   //work out which tile in the object array we are drawing
                    int arrayY = worldTileY - mapObject.tileY;
                    Piece piece = multiple.GetPiece(arrayX, arrayY);

                    if (piece != null)
                    {
                        piece.DrawMiniMapTile(DisplayController.spriteBatch1, destination, 1f, color);
                    }
                }
            }
        } 

        public void AddToRecalculateList(Point point)
        {
            redrawQueue.Enqueue(point);
        }
        //Need to fine tune this
        //the minimap is lagging on a 4000 x 4000 map
        //because all the other redraw quese are empty
        //perhaps we just need 1 major cue that everything goes in
        //will smooth it out
        //because normally a fire would only be going in a couple of quese

        public bool IsEmptyCalculateList()
        {
            return redrawQueue.IsEmpty;
        }

        public void DrawCellRecalculateList()
        {
                int i = 0;
                ScreenController.graphicsDevice.SetRenderTarget(DisplayController.GetRenderTarget(renderTargetTextureId));
                DisplayController.spriteBatch1.Begin(SpriteSortMode.Deferred);

                while (!redrawQueue.IsEmpty)
                {
                    if (redrawQueue.TryDequeue(out Point point))
                    {
                        int cellX = point.X - (xMajor * tilesPerCell);
                        int cellY = point.Y - (yMajor * tilesPerCell);

                        Tile tile = WorldController.world.tileGrid[point.X, point.Y];
                        Rectangle destination = new Rectangle(cellX * tilePixel, cellY * tilePixel, tilePixel, tilePixel);

                        Color color = GetTileColor(tile);
                        AddLayerDraw(tile, destination, color);
                        AddMapObjectDraw(tile, destination, point.X, point.Y, color);
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }

                DisplayController.spriteBatch1.End();
                ScreenController.graphicsDevice.SetRenderTarget(null);
          }

        private Color GetTileColor(Tile tile)
        {
         //   if (tile.fire == null) { return Color.White; }
       //     if (tile.fire.isCompleteBurntOut || tile.fire.isPartialBurntOut) { return Color.Black; }
        //    if (tile.fire.miniFireCurrentBurning > 0) { return Color.Black; }
            return Color.White;
        }

        public void Dispose()
        {
            sprite.Dispose();
            DisplayController.DisposeRenderTarget(renderTargetTextureId);
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y, float containerFade)
        {
            Rectangle location = new Rectangle((x * 500), y * 500, 500, 500);
            spriteBatch.Draw(sprite.texture2D, location, sprite.location, Color.White * containerFade, sprite.rotation, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}
