using BushFire.Editor.Containers;
using BushFire.Editor.Controllers;
using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Game;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.MapObjects;
using BushFire.Game.Storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Editor.Tech
{
    //TODO. ELEVATION IS HARD CODED to INT 2 when creating a building.  This is because i dont want to lose my premade yet
    [Serializable]
    class CompressedBuilding
    {
        public int[,] pieceMap;
        public List<EditorShadow> shadowList;
        public int id;
        public int usingWidth;
        public int usingHeight;
        public bool isActive;
        public int elevation;
        public bool hasFence;
        public int pieceRow;
        public int baseDirectionFacing;  //0 north.  3 west//
        public int inTileShift;
        public ColorXML sampleColor;
        public List<ColorXML> availableColoursXMLList;

        [NonSerialized] EditorParams editorParams;

        public CompressedBuilding(int pieceRow, int baseDirectionFacing, EditorParams editorParams)
        {
            this.editorParams = editorParams;
            availableColoursXMLList = new List<ColorXML>();
            this.pieceRow = pieceRow;
            this.baseDirectionFacing = baseDirectionFacing;
            usingWidth = 10;
            usingHeight = 10;
            elevation = 3;
            inTileShift = 0;
            pieceMap = new int[editorParams.overallSize, editorParams.overallSize];
            for (int x = 0; x < editorParams.overallSize; x++)
            {
                for (int y = 0; y < editorParams.overallSize; y++)
                {
                    pieceMap[x, y] = -1;
                }
            }
            shadowList = new List<EditorShadow>();
        }

        //must call this on loading or creating a new building in the editor as the editor params are not serialized or saved to xml
        public void InitOld(EditorParams editorParams)
        {
            this.editorParams = editorParams;
        }

        public string GetBaseDirectionFacingString()
        {
            if (baseDirectionFacing == 0)
            {
                return "North";
            }
            else
            {
                return "West";
            }
        }

        public void InitNew(int id)
        {
            this.id = id;
        }

        public void AddBuildingPiece(Spot spot)
        {
            if (spot != null)
            {
                if (editorParams.TileBuildingLegit(spot))
                {
                    if (editorParams.samplePiece != null)
                    {
                        pieceMap[spot.x, spot.y] = editorParams.samplePiece.pieceId;
                    }
                }
            }
        }

        public void AddShadowPiece(EditorShadow editorShadow)
        {
            shadowList.Add(editorShadow);
        }

        public void RemoveBuildingPiece(Spot spot)
        {
            if (spot != null)
            {
                if (editorParams.TileBuildingLegit(spot))
                {
                    pieceMap[spot.x, spot.y] = -1;
                }
            }
        }

        public void RemoveShadowPiece(Spot spot, ShadowSide shadowSide)
        {
            if (spot != null)
            {
                for (int i = shadowList.Count - 1; i >= 0; i--)
                {
                    if (shadowList[i].tileX == spot.x && shadowList[i].tileY == spot.y && shadowList[i].shadowSide == shadowSide)
                    {
                        shadowList.RemoveAt(i);
                    }
                }
            }
        }

        public void Update(Input input)
        {
            if (input.IsKeyPressed(Keys.K))
            {
                TranslatePieces180();
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawPieceMap(spriteBatch);
            DrawShadowMap(spriteBatch);
        }

        private void DrawPieceMap(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < usingWidth; x++)
            {
                for (int y = 0; y < usingHeight; y++)
                {
                    if (pieceMap[x, y] != -1)
                    {
                        Vector2 location = new Vector2(x * GroundLayerController.tileSize + GroundLayerController.halfTileSize, y * GroundLayerController.tileSize + GroundLayerController.halfTileSize);
                        PieceController.GetPiece(pieceRow, pieceMap[x, y]).Draw(spriteBatch, location, sampleColor.ToColor(), 1f);
                    }
                }
            }
        }

        private void DrawShadowMap(SpriteBatch spriteBatch)
        {
            if (editorParams.editingMode == EditingMode.Shadows)
            {
                foreach (EditorShadow editingShadow in shadowList)
                {
                    if (editorParams.shadowSide == editingShadow.shadowSide)
                    {
                        Shadow shadow = new Shadow(ShadowSpriteController.GetShadowSprite(MapObjectShadowType.BUILDING, editingShadow.shadowSide, editingShadow.shadowId), editingShadow.tileX, editingShadow.tileY, editingShadow.shadowSide, editingShadow.shadowId, new Point(editingShadow.shadowOffsetX, editingShadow.shadowOffsetY));
                        shadow.DrawBuildingEditing(spriteBatch);
                    }
                }
            }
        }

        public List<Building> GetBuildingList()
        {
            List<Building> buildingList = new List<Building>();
            List<Color> availableColorList = new List<Color>();

            foreach (ColorXML colorXML in availableColoursXMLList)
            {
                availableColorList.Add(colorXML.ToColor());
            }

            int direction = baseDirectionFacing;

            for (int i = 0; i < 2; i++)
            {
                Piece[,] newPieceMap = new Piece[usingWidth, usingHeight];

                for (int x = 0; x < usingWidth; x++)
                {
                    for (int y = 0; y < usingHeight; y++)
                    {
                        int pieceId = pieceMap[x, y];
                        if (pieceId >= 0)
                        {
                            newPieceMap[x, y] = PieceController.GetPiece(pieceRow, pieceId);
                        }
                    }
                }

                List<Shadow> newShadowListLeft = new List<Shadow>();
                List<Shadow> newShadowListRight = new List<Shadow>();

                foreach (EditorShadow editingShadow in shadowList)
                {
                    if (editingShadow.shadowSide == ShadowSide.LEFT)
                    {
                        int tileOffset = (ShadowSpriteController.GetShadowTileSize(MapObjectShadowType.BUILDING, ShadowSide.LEFT, editingShadow.shadowId).X) / GroundLayerController.tileSize;

                        newShadowListLeft.Add(new Shadow(ShadowSpriteController.GetShadowSprite(MapObjectShadowType.BUILDING, editingShadow.shadowSide, editingShadow.shadowId), editingShadow.tileX, editingShadow.tileY, editingShadow.shadowSide, editingShadow.shadowId, new Point(editingShadow.shadowOffsetX, editingShadow.shadowOffsetY)));
                    }
                    else if (editingShadow.shadowSide == ShadowSide.RIGHT)
                    {
                        newShadowListRight.Add(new Shadow(ShadowSpriteController.GetShadowSprite(MapObjectShadowType.BUILDING, editingShadow.shadowSide, editingShadow.shadowId), editingShadow.tileX, editingShadow.tileY, editingShadow.shadowSide, editingShadow.shadowId, new Point(editingShadow.shadowOffsetX, editingShadow.shadowOffsetY)));
                    }
                }
                buildingList.Add(new Building(newPieceMap, newShadowListLeft, newShadowListRight, direction, usingWidth, usingHeight, elevation, availableColorList, inTileShift));

                if (i == 1)
                {
                    break;
                }
                TranslatePieces180();
                TranslateShadows180();

                if (direction == 0) { direction = 4; }
                if (direction == 6) { direction = 2; }
            }

            return buildingList;
        }


        private void TranslatePieces180()
        {
            int[,] newPieceMap = new int[usingWidth, usingHeight];

            //Clear all new tiles
            for (int x = 0; x < usingWidth; x++)
            {
                for (int y = 0; y < usingHeight; y++)
                {
                    newPieceMap[x, y] = -1;
                }
            }

            for (int x = 0; x < usingWidth; x++)
            {
                for (int y = 0; y < usingHeight; y++)
                {
                    if (pieceMap[x, y] > -1)
                    {
                        int newX = usingWidth - x - 1;
                        int newY = usingHeight - y - 1;
                        int newId = pieceMap[x, y];

                        newId += 2;
                        if (newId % 4 < 2)
                        {
                            newId -= 4;
                        }

                        newPieceMap[newX, newY] = newId;
                    }
                }
            }
            pieceMap = newPieceMap;
        }
      

        private void TranslateShadows180()
        {
            foreach (EditorShadow editorShadow in shadowList)
            {
                float pixelX = (editorShadow.tileX * 128) + editorShadow.shadowOffsetX;
                float pixelY = (editorShadow.tileY * 128) + editorShadow.shadowOffsetY;
                float newPixelX = (usingWidth * 128) - pixelX;
                float newPixelY = (usingHeight * 128) - pixelY;

                editorShadow.tileX = (int)newPixelX / 128;
                editorShadow.tileY = ((int)newPixelY / 128) - 1;
                editorShadow.shadowOffsetX = (int)(newPixelX % GroundLayerController.tileSize);
                editorShadow.shadowOffsetY = (int)(newPixelY % GroundLayerController.tileSize);


                if (editorShadow.shadowSide == ShadowSide.LEFT)
                {
                    editorShadow.shadowSide = ShadowSide.RIGHT;
                }
                else if (editorShadow.shadowSide == ShadowSide.RIGHT)
                {
                    editorShadow.shadowSide = ShadowSide.LEFT;

                }

            }
           
        }
    }

    enum BackingType
    {
        ROOF,
        DRIVEWAY,
    }

    enum ShadowSide
    {
        NONE,
        LEFT,
        RIGHT,
    }




}
