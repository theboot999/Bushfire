using BushFire.Editor.Controllers;
using BushFire.Editor.Tech;
using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Game.Map;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.Map.MapObjects;
using BushFire.Game.Storage;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Controllers
{

    //streetlight needs a piece and a shadow

    static class MapObjectController
    {
        private static Dictionary<MapObjectType, Dictionary<int, MapObjectProperties>> mapObjectTypeList;

        public static void Init()
        {
            mapObjectTypeList = new Dictionary<MapObjectType, Dictionary<int, MapObjectProperties>>();
            AddStreetLights();
            AddTrees();
        }

        private static void AddStreetLights()
        {
            Dictionary<int, MapObjectProperties> streetLightList = new Dictionary<int, MapObjectProperties>();
            AddToStreetLightList(streetLightList, StreetLightType.STOPLIGHTRED, LightType.STOPLIGHTRED, new Vector2(84, 103), MapObjectShadowType.STOPLIGHT, new Point(100, 14), new Point(22, 99), new Point(29, 92), true, 2);
            AddToStreetLightList(streetLightList, StreetLightType.STOPLIGHTAMBER, LightType.STOPLIGHTAMBER, new Vector2(84, 103), MapObjectShadowType.STOPLIGHT, new Point(100, 14), new Point(22, 99), new Point(29, 92), true, 2);
            AddToStreetLightList(streetLightList, StreetLightType.STOPLIGHTGREEN, LightType.STOPLIGHTGREEN, new Vector2(84, 103), MapObjectShadowType.STOPLIGHT, new Point(100, 14), new Point(22, 99), new Point(29, 92), true, 2);
            AddToStreetLightList(streetLightList, StreetLightType.STREETLIGHTCITY, LightType.STREETLIGHTWHITE, new Vector2(100, 8), MapObjectShadowType.STREETLIGHTCITY, new Point(80, 14), new Point(26, 8), new Point(31, 1), false, 3);
            AddToStreetLightList(streetLightList, StreetLightType.STREETLIGHTCOUNTRY, LightType.STREETLIGHTYELLOW, new Vector2(100, 8), MapObjectShadowType.STREETLIGHTCITY, new Point(80, 14), new Point(26, 8), new Point(31, 1), false, 3);

            mapObjectTypeList.Add(MapObjectType.STREETLIGHT, streetLightList);
        }

        private static void AddToStreetLightList(Dictionary<int, MapObjectProperties> streetLightList, StreetLightType streetLightType, LightType lightType, Vector2 lightOffset, MapObjectShadowType shadowType, Point objectSize, Point shadowPointLeft, Point shadowPointTop, bool alwaysOn, int elevation)
        {
            int rowIndex = (int)streetLightType * 4;

            for (int i = 0; i < 4; i++)
            {
                Piece piece = PieceController.GetPiece(10, rowIndex + i);
                Light light = GraphicsManager.GetLight(lightType);
                Vector2 currentLightOffset = RotateVectorRight(lightOffset, GroundLayerController.tileSize, i);
                float radian = RotateRadiansRight(0, i);
                Point shadowPointOffset = GetShadowPoint(shadowPointLeft, shadowPointTop, GroundLayerController.tileSize, i, objectSize);
                Shadow shadowLeft = new Shadow(ShadowSpriteController.GetShadowSprite(shadowType, ShadowSide.LEFT, i), 0, 0, ShadowSide.LEFT, i, shadowPointOffset);
                Shadow shadowRight = new Shadow(ShadowSpriteController.GetShadowSprite(shadowType, ShadowSide.RIGHT, i), 0, 0, ShadowSide.RIGHT, i, shadowPointOffset);
                streetLightList.Add(rowIndex + i, new StreetLight(piece, light, currentLightOffset, radian, shadowLeft, shadowRight, shadowPointOffset, alwaysOn, elevation));
            }
        }

        public static StreetLight GetStreetLight(StreetLightType streetLightType, int index)
        {
            int jump = ((int)streetLightType * 4) + index;
            return (StreetLight)mapObjectTypeList[MapObjectType.STREETLIGHT][jump];
        }

        private static void AddTrees()
        {
            Dictionary<int, MapObjectProperties> treeList = new Dictionary<int, MapObjectProperties>();
            //Todo add rotated trees and shadows etc probablyt

            for (int i = 0; i < 8; i++)
            {
                treeList.Add(i, new Tree(PieceController.GetPiece(12, i), null, null, Point.Zero, 3, 24));
            }
            mapObjectTypeList.Add(MapObjectType.TREE, treeList);
        }

        public static Tree GetTree(int index)
        {
            return (Tree)mapObjectTypeList[MapObjectType.TREE][index];
        }

        private static Point GetShadowPoint(Point shadowpointLeft, Point shadowPointTop, int tileSize, int itterations, Point objectSize)
        {
            Point newPoint = shadowPointTop;

            if (itterations == 1)
            {
                newPoint.X = tileSize - shadowpointLeft.Y;
                newPoint.Y = shadowpointLeft.X;

            }
            else if (itterations == 2)
            {
                newPoint.X = tileSize - shadowPointTop.X;
                newPoint.Y = tileSize - shadowPointTop.Y - objectSize.Y;

            }
            else if (itterations == 3)
            {
                newPoint.X = shadowpointLeft.Y;
                newPoint.Y = tileSize - shadowpointLeft.X - objectSize.X;
            }
            return newPoint;
        }

        private static Vector2 RotateVectorRight(Vector2 point, float squareSize, int itterations)
        {
            for (int i = 0; i < itterations; i++)
            {
                float halfOldWidth = squareSize / 2;
                float halfOldHeight = squareSize / 2;

                float oldX = point.X + halfOldWidth;
                float oldY = point.Y - halfOldHeight;

                point.X = (oldY * -1) + halfOldHeight - 1;
                point.Y = (oldX) - halfOldWidth;
            }

            return point;
        }

        private static float RotateRadiansRight(float radians, int itterations)
        {
            return radians += (float)itterations * 1.571f;
        }

    }

    enum StreetLightType
    {
        STOPLIGHTRED = 0,
        STOPLIGHTAMBER = 1,
        STOPLIGHTGREEN = 2,
        STREETLIGHTCITY = 3,
        STREETLIGHTCOUNTRY = 4
    }

}
