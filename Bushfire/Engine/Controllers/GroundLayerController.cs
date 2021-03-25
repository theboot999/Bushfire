using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Game.Controllers;
using BushFire.Game.Map.MapObjectComponents;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Storage
{
    static class GroundLayerController
    {
        public static int debug = 0;
        private static Dictionary<LayerType, Dictionary<int, GroundLayer>> layerTypeList;
        public const int tileSize = 128;
        public const int halfTileSize = 128 / 2;
        public const int tileAndHalfTileSize = 128 + (128 / 2);

        private static Dictionary<int, GroundLayer> burntLayerList;


        public static void Init()
        {
            InitBurntLayers();
            BitMask.Init();
            layerTypeList = new Dictionary<LayerType, Dictionary<int, GroundLayer>>();
            LoadLayer(LayerType.WATER0, false);
            LoadLayer(LayerType.WATER1, false);
            LoadLayer(LayerType.WATER2, false);
            LoadLayer(LayerType.WATER3, false);
            LoadLayer(LayerType.WATER4, false);
            LoadLayer(LayerType.WATER5, false);
            LoadLayer(LayerType.WATER6, false);
            LoadLayer(LayerType.WATER7, false);
            LoadLayer(LayerType.WATER8, false);
            LoadLayer(LayerType.WATER9, false);
            LoadLayer(LayerType.GRASS10, false);
            LoadLayer(LayerType.GRASS11, false);
            LoadLayer(LayerType.GRASS12, false);
            LoadLayer(LayerType.GRASS13, false);
            LoadLayer(LayerType.GRASS14, false);
            LoadLayer(LayerType.GRASS15, false);
            LoadLayer(LayerType.GRASS16, false);
            LoadLayer(LayerType.GRASS17, false);
            LoadLayer(LayerType.GRASS18, false);
            LoadLayer(LayerType.GRASS19, false);
            LoadLayer(LayerType.GRASS20, false);
            LoadLayer(LayerType.GRASS21, false);
            LoadLayer(LayerType.GRASS22, false);
            LoadLayer(LayerType.GRASS23, false);
            LoadLayer(LayerType.GRASS24, false);
            LoadLayer(LayerType.GRASS25, false);
            LoadLayer(LayerType.GRASS26, false);
            LoadLayer(LayerType.GRASS27, false);
            LoadLayer(LayerType.GRASS28, false);
            LoadLayer(LayerType.GRASS29, false);
            LoadLayer(LayerType.GRASS30, false);
            LoadLayer(LayerType.GRASS31, false);
            LoadLayer(LayerType.GRASS32, false);
            LoadLayer(LayerType.GRASS33, false);
            LoadLayer(LayerType.GRASS34, false);
            LoadLayer(LayerType.GRASS35, false);
            LoadLayer(LayerType.GRASS36, false);
            LoadLayer(LayerType.GRASS37, false);
            LoadLayer(LayerType.GRASS38, false);
            LoadLayer(LayerType.GRASS39, false);
            LoadLayer(LayerType.GRASS40, false);
            LoadLayer(LayerType.GRASS41, false);
            LoadLayer(LayerType.GRASS42, false);
            LoadLayer(LayerType.GRASS43, false);
            LoadLayer(LayerType.GRASS44, false);
            LoadLayer(LayerType.GRASS45, false);
            LoadLayer(LayerType.GRASS46, false);
            LoadLayer(LayerType.GRASS47, false);
            LoadLayer(LayerType.GRASS48, false);
            LoadLayer(LayerType.GRASS49, false);
            LoadLayer(LayerType.GRASS50, false);
            LoadLayer(LayerType.CITYROAD, true);
            LoadLayer(LayerType.COUNTRYROAD, true);
            LoadLayer(LayerType.DRIVEWAY, true);
            LoadLayer(LayerType.PARKING, true);
            LoadLayer(LayerType.BORDER, true);
            LoadLayer(LayerType.ROADDECALS, true);
        }

        private static void InitBurntLayers()
        {
            burntLayerList = new Dictionary<int, GroundLayer>();
            int bitMaskIndex = 0;
            int tileSize = 12;




            for (int x = 0; x < 128; x++)
            {
                for (int y = 0; y < 512; y++)
                {


                    int locationX = (x * tileSize) + x + 1 + 650;
                    int locationY = (y * tileSize) + y + 1;


                    Rectangle rectangle = new Rectangle(locationX, locationY, tileSize, tileSize);
                    Sprite sprite = new Sprite(rectangle, TextureSheet.Ground);
                    GroundLayer burntLayer = new GroundLayer(LayerType.BURNT, sprite, 0, 0);
                    //       burntLayer.scale = 11.6f;
                    burntLayer.scale = 10.7f;
                    burntLayerList.Add(bitMaskIndex, burntLayer);
                    bitMaskIndex++;
                }
            }

            burntLayerList.Last().Value.isFullBurnt = true;

        }

        private static void LoadLayer(LayerType layerType, bool rotateBase)
        {
            Dictionary<int, GroundLayer> myList = new Dictionary<int, GroundLayer>();

            int textureSheetHeightIndex = (int)layerType;
            float degrees = 0;

            for (int i = 0; i < 21; i ++)
            {
                Sprite sprite = new Sprite(GetSpriteSheetRectangle(i, textureSheetHeightIndex), TextureSheet.Ground);
                GroundLayer layer = new GroundLayer(layerType, sprite, textureSheetHeightIndex, BitMask.GetBitmaskFromTileIndex(i));
                sprite.rotation = MathHelper.ToRadians(degrees);
                myList.Add(i, layer);

                if (rotateBase || i > 11) { degrees += 90; }

                if (degrees == 360)
                {
                    degrees = 0;
                }

            }
            layerTypeList.Add(layerType, myList);
        }

        private static Rectangle GetSpriteSheetRectangle(int tileIndexLeft, int tileIndexTop)
        {
            int indexLeft = tileIndexLeft / 4;
            return new Rectangle((indexLeft * tileSize) + 1 + (indexLeft * 2), (tileIndexTop * tileSize) + 1 + (tileIndexTop * 2), tileSize, tileSize);
        }



        public static GroundLayer GetLayerByIndex(LayerType layerType, int index)
        {
            Dictionary<int, GroundLayer> myList = layerTypeList[layerType];
            return myList[index];
        }

        public static GroundLayer GetLayerByBitMask(LayerType layerType, int bitmaskIndex)
        {
            int index = BitMask.GetTileIndexFromBitmask(bitmaskIndex);
            Dictionary<int, GroundLayer> myList = layerTypeList[layerType];
            return myList[index];
        }

        public static GroundLayer GetRandomLayer(LayerType layerType)
        {
                int index = GameController.rnd.Next(0, 12);
                Dictionary<int, GroundLayer> myList = layerTypeList[layerType];
                return myList[index];
        }

        public static GroundLayer GetBurntLayerByBitMask(int bitmaskIndex)
        {
            return burntLayerList[bitmaskIndex];
        }
    }


    public enum LayerType : int
    {
        WATER0 = 0,
        WATER1 = 1,
        WATER2 = 2,
        WATER3 = 3,
        WATER4 = 4,
        WATER5 = 5,
        WATER6 = 6,
        WATER7 = 7,
        WATER8 = 8,
        WATER9 = 9,
        GRASS10 = 10,
        GRASS11 = 11,
        GRASS12 = 12,
        GRASS13 = 13,
        GRASS14 = 14,
        GRASS15 = 15,
        GRASS16 = 16,
        GRASS17 = 17,
        GRASS18 = 18,
        GRASS19 = 19,
        GRASS20 = 20,
        GRASS21 = 21,
        GRASS22 = 22,
        GRASS23 = 23,
        GRASS24 = 24,
        GRASS25 = 25,
        GRASS26 = 26,
        GRASS27 = 27,
        GRASS28 = 28,
        GRASS29 = 29,
        GRASS30 = 30,
        GRASS31 = 31,
        GRASS32 = 32,
        GRASS33 = 33,
        GRASS34 = 34,
        GRASS35 = 35,
        GRASS36 = 36,
        GRASS37 = 37,
        GRASS38 = 38,
        GRASS39 = 39,
        GRASS40 = 40,
        GRASS41 = 41,
        GRASS42 = 42,
        GRASS43 = 43,
        GRASS44 = 44,
        GRASS45 = 45,
        GRASS46 = 46,
        GRASS47 = 47,
        GRASS48 = 48,
        GRASS49 = 49,
        GRASS50 = 50,
        CITYROAD = 51,
        COUNTRYROAD = 52,
        DRIVEWAY = 54,
        PARKING = 55,
        NULL1 = 53,
        NULL2 = 54,
        NULL3 = 55,
        NULL4 = 56,
        NULL5 = 57,
        NULL6 = 58,
        BORDER = 59,
        ROADDECALS = 60,
        BURNT = 70

    }
}
