using BushFire.Editor.Tech;
using BushFire.Engine;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.Storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Editor.Controllers
{
    static class PieceController
    {
        //We add the shadows in the same row. they will be a longer id

        private static Dictionary<int, Dictionary<int, Piece>> pieceRowsList;

        public static void Init()
        {
            pieceRowsList = new Dictionary<int, Dictionary<int, Piece>>();
            AddStandardRow(0, PieceStyle.HOUSE, 15, true);
            AddStandardRow(1, PieceStyle.HOUSE, 2, false);
            AddStandardRow(2, PieceStyle.HOUSE, 2, false);
            AddStandardRow(10, PieceStyle.STREETLIGHT, 5, false);



            AddPieceToRow(12, PieceStyle.TREE, new Rectangle(1, 1561, 256, 256));
            AddPieceToRow(12, PieceStyle.TREE, new Rectangle(250, 1561, 256, 256));
            

            //do we do trees with rotation? with 4 cardinal directions?
            // i dont know
            // to do the shadows would be tricky. but we will need to work out shadows on vehicles
        }

        private static void AddStandardRow(int row, PieceStyle pieceStyle, int numberOfPieces, bool addFlip)
        {
            int pieceIndex = 0;
            int indexLeft = 0;

            Dictionary<int, Piece> styleList = new Dictionary<int, Piece>();
            float degrees = 0;
            SpriteEffects spriteEffect = SpriteEffects.None;

            for (int i = 0; i < numberOfPieces; i++)
            {

                for (int p = 0; p < 4; p++)
                {
                    Rectangle location = new Rectangle((indexLeft * GroundLayerController.tileSize) + 1 + (indexLeft * 2), (row * GroundLayerController.tileSize) + 1 + (row * 2), GroundLayerController.tileSize, GroundLayerController.tileSize);
                    Sprite sprite = new Sprite(location, TextureSheet.MapObjects);
                    sprite.spriteEffect = spriteEffect;
                    sprite.rotation = MathHelper.ToRadians(degrees);
                    styleList.Add(pieceIndex, new Piece(pieceStyle, sprite, pieceIndex));
                    degrees += 90;
                    if (degrees == 360) { degrees = 0; }
                    pieceIndex++;
                }

                if (addFlip)
                {
                    if (spriteEffect != SpriteEffects.FlipHorizontally)
                    {
                        spriteEffect = SpriteEffects.FlipHorizontally;
                        i--;
                        indexLeft--;
                    }
                    else
                    {
                        spriteEffect = SpriteEffects.None;
                    }
                }

                indexLeft++;
            }
            pieceRowsList.Add(row, styleList);
        }

        private static void AddPieceToRow(int row, PieceStyle pieceStyle, Rectangle location)
        {
            Dictionary<int, Piece> styleList;
            int nextPiece = 0;

            if (pieceRowsList.ContainsKey(row))
            {
                styleList = pieceRowsList[row];
                nextPiece = GetRowLength(row) + 1;
            }
            else
            {
                styleList = new Dictionary<int, Piece>();
                pieceRowsList.Add(row, styleList);
            }
            float degrees = 0;

            for (int p = 0; p < 4; p++)
            {
                Sprite sprite = new Sprite(location, TextureSheet.MapObjects);
                sprite.rotation = MathHelper.ToRadians(degrees);
                sprite.scale = 0.7f;
                styleList.Add(nextPiece, new Piece(pieceStyle, sprite, nextPiece));
                degrees += 90;
                if (degrees == 360) { degrees = 0; }
                nextPiece++;
            }
        }

        public static int GetRowLength(int row)
        {
            return pieceRowsList[row].Count - 1;
        }





        public static Piece GetPiece(int row, int pieceId)
        {
            Dictionary<int, Piece> rowList = pieceRowsList[row];
            return pieceRowsList[row][pieceId];
        }

    }

    enum PieceStyle
    {
        SAMPLE,
        HOUSE,
        HIGHRISE,
        STREETLIGHT,
        TREE
    }

}
