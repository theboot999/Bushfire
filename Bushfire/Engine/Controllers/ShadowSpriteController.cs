using BushFire.Editor.Containers;
using BushFire.Editor.Tech;
using BushFire.Engine.Controllers;
using BushFire.Game.Map.MapObjectComponents;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.ContentStorage
{
    static class ShadowSpriteController
    {

        private static Dictionary<MapObjectShadowType, Dictionary<ShadowSide,Dictionary<int, Sprite>>> spriteMapobjectList;
        public readonly static int numberOfBuildingShadows = 12;

        public static Sprite whiteOut;
        public static Sprite blackOut;

        public static void Init()
        {
            whiteOut = GraphicsManager.GetSpriteColour(20);
            blackOut = GraphicsManager.GetSpriteColour(10);

            spriteMapobjectList = new Dictionary<MapObjectShadowType, Dictionary<ShadowSide, Dictionary<int, Sprite>>>();


          //  spriteMapobjectList = new Dictionary<MapObjectShadowType, Dictionary<int, Sprite>>();
            AddBuildingShadows();
            AddStreetLightShadows();
        }

        private static void AddBuildingShadows()
        {
            //In case we get confused with the X offset.
            //In shadows more than 1 tile we are includind the 2 pixel bounding pixels
            //So when rotation we should just be able to grab the same ID from the other side

            Dictionary<ShadowSide, Dictionary<int, Sprite>> shadowSideList = new Dictionary<ShadowSide, Dictionary<int, Sprite>>();

            Dictionary<int, Sprite> myList;
            //LEFT
            myList = new Dictionary<int, Sprite>();

            //Row 1.  Blank row
            myList.Add(0, new Sprite(new Rectangle(1, 1, 128, 128), TextureSheet.Shadows));
            myList.Add(1, new Sprite(new Rectangle(131, 1, 258, 128), TextureSheet.Shadows));
            myList.Add(2, new Sprite(new Rectangle(391, 1, 388, 128), TextureSheet.Shadows));
            myList.Add(3, new Sprite(new Rectangle(781, 1, 518, 128), TextureSheet.Shadows));
            //Row 2.  Left
            myList.Add(4, new Sprite(new Rectangle(1, 131, 128, 128), TextureSheet.Shadows));
            myList.Add(5, new Sprite(new Rectangle(131, 131, 258, 128), TextureSheet.Shadows));
            myList.Add(6, new Sprite(new Rectangle(391, 131, 388, 128), TextureSheet.Shadows));
            myList.Add(7, new Sprite(new Rectangle(781, 131, 518, 128), TextureSheet.Shadows));
            //Row 4. Left
            myList.Add(8, new Sprite(new Rectangle(1, 391, 128, 128), TextureSheet.Shadows));
            myList.Add(9, new Sprite(new Rectangle(131, 391, 258, 128), TextureSheet.Shadows));
            myList.Add(10, new Sprite(new Rectangle(391, 391, 388, 128), TextureSheet.Shadows));
            myList.Add(11, new Sprite(new Rectangle(781, 391, 518, 128), TextureSheet.Shadows));
            shadowSideList.Add(ShadowSide.LEFT, myList);

            //RIGHT
            myList = new Dictionary<int, Sprite>();
            //Row 1. Blank Row
            myList.Add(0, new Sprite(new Rectangle(1, 1, 128, 128), TextureSheet.Shadows));
            myList.Add(1, new Sprite(new Rectangle(131, 1, 258, 128), TextureSheet.Shadows));
            myList.Add(2, new Sprite(new Rectangle(391, 1, 388, 128), TextureSheet.Shadows));
            myList.Add(3, new Sprite(new Rectangle(781, 1, 518, 128), TextureSheet.Shadows));
            //Row 3. Right
            myList.Add(4, new Sprite(new Rectangle(1, 261, 128, 128), TextureSheet.Shadows));
            myList.Add(5, new Sprite(new Rectangle(131, 261, 258, 128), TextureSheet.Shadows));
            myList.Add(6, new Sprite(new Rectangle(391, 261, 388, 128), TextureSheet.Shadows));
            myList.Add(7, new Sprite(new Rectangle(781, 261, 518, 128), TextureSheet.Shadows));
            //Row 5. Right
            myList.Add(8, new Sprite(new Rectangle(1, 521, 128, 128), TextureSheet.Shadows));
            myList.Add(9, new Sprite(new Rectangle(131, 521, 258, 128), TextureSheet.Shadows));
            myList.Add(10, new Sprite(new Rectangle(391, 521, 388, 128), TextureSheet.Shadows));
            myList.Add(11, new Sprite(new Rectangle(781, 521, 518, 128), TextureSheet.Shadows));
            shadowSideList.Add(ShadowSide.RIGHT, myList);
          
            spriteMapobjectList.Add(MapObjectShadowType.BUILDING, shadowSideList);
        }

        private static void AddStreetLightShadows()
        {
            Dictionary<ShadowSide, Dictionary<int, Sprite>> shadowSideList;
            Dictionary<int, Sprite> myList;
            
            //Stoplights
            shadowSideList = new Dictionary<ShadowSide, Dictionary<int, Sprite>>();        
            //Stoplight ShadowsLeft
            myList = new Dictionary<int, Sprite>();
            myList.Add(0, new Sprite(new Rectangle(1, 3369, 132, 14), TextureSheet.Shadows));
            myList.Add(1, new Sprite(new Rectangle(1, 3399, 132, 100), TextureSheet.Shadows));
            myList.Add(2, new Sprite(new Rectangle(1, 3384, 220, 14), TextureSheet.Shadows));
            myList.Add(3, new Sprite(new Rectangle(1, 3505, 132, 100), TextureSheet.Shadows));
            shadowSideList.Add(ShadowSide.LEFT, myList);
            //Stoplight ShadowsRight
            myList = new Dictionary<int, Sprite>();
            myList.Add(0, new Sprite(new Rectangle(135, 3369, 220, 14), TextureSheet.Shadows));
            myList.Add(3, new Sprite(new Rectangle(135, 3505, 132, 100), TextureSheet.Shadows));
            myList.Add(2, new Sprite(new Rectangle(223, 3384, 132, 14), TextureSheet.Shadows));
            myList.Add(1, new Sprite(new Rectangle(135, 3399, 132, 100), TextureSheet.Shadows));
            shadowSideList.Add(ShadowSide.RIGHT, myList);
            spriteMapobjectList.Add(MapObjectShadowType.STOPLIGHT, shadowSideList);

            //Streetlights
            shadowSideList = new Dictionary<ShadowSide, Dictionary<int, Sprite>>();       
            //StreetLight ShadowsLeft
            myList = new Dictionary<int, Sprite>();
            myList.Add(0, new Sprite(new Rectangle(1, 3612, 138, 14), TextureSheet.Shadows));
            myList.Add(2, new Sprite(new Rectangle(1, 3628, 193, 14), TextureSheet.Shadows));
            myList.Add(1, new Sprite(new Rectangle(1, 3644, 140, 81), TextureSheet.Shadows));
            myList.Add(3, new Sprite(new Rectangle(1, 3727, 140, 81), TextureSheet.Shadows));
            shadowSideList.Add(ShadowSide.LEFT, myList);
            //StreetLight ShadowsRight
            myList = new Dictionary<int, Sprite>();
            myList.Add(0, new Sprite(new Rectangle(141, 3612, 193, 14), TextureSheet.Shadows));
            myList.Add(2, new Sprite(new Rectangle(196, 3628, 138, 14), TextureSheet.Shadows));
            myList.Add(1, new Sprite(new Rectangle(143, 3644, 140, 81), TextureSheet.Shadows));
            myList.Add(3, new Sprite(new Rectangle(143, 3727, 140, 81), TextureSheet.Shadows));

            shadowSideList.Add(ShadowSide.RIGHT, myList);
            spriteMapobjectList.Add(MapObjectShadowType.STREETLIGHTCITY, shadowSideList);
        }
    
        public static Sprite GetShadowSprite(MapObjectShadowType mapObjectShadowType, ShadowSide shadowSide, int index)
        {        
            return spriteMapobjectList[mapObjectShadowType][shadowSide][index];
        }
      
        public static Point GetShadowTileSize(MapObjectShadowType mapObjectShadowType, ShadowSide shadowSide, int index)
        {
            Sprite sprite = spriteMapobjectList[mapObjectShadowType][shadowSide][index];
            return new Point(sprite.location.Width, sprite.location.Height);
        }


    }

    enum MapObjectShadowType
    {
        BUILDING,
        STOPLIGHT,
        STREETLIGHTCITY,
        STREETLIGHTCOUNTRY
    }

}
