using BushFire.Engine.Controllers;
using BushFire.Game;
using BushFire.Game.Controllers;
using BushFire.Game.Map;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.MapObjects;
using BushFire.Game.Storage;
using BushFire.Game.Tech;
using BushFire.MapGeneration.Containers;
using BushFire.MapGeneration.Generation.RoadStuff;
using BushFire.MapGeneration.Tech;
using BushFire.Menu.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Generation.TownsAndRoadStuff
{
    class CreatingIntersections
    {
        //So we count the neighbour roads on the shrunk map
        //then we can count the neighbourroads on the expanded map
        //so basicaly we
        private int chanceToKeepThreeWay = 60;

        public CreatingIntersections(Tile[,] tileGrid, ShrunkNode[,] shrunkMap, List<Intersection> intersectionList, LoadingInfo loadingInfo)
        {
            Random rnd = GameController.GetRandomWithSeed();
            int id = 1;
            float percentDone = 0;
            float percentJump = 100f / CreatingWorld.worldWidth;

            for (int x = 0; x < ShrunkWorldBuilder.shrunkWorldWidth; x++)
            {
                percentDone += percentJump;
                loadingInfo.UpdateLoading(LoadingType.AddingIntersections, percentDone);

                for (int y = 0; y < ShrunkWorldBuilder.shrunkWorldHeight; y++)
                {
                    int amount = CountNeighbourRoadsShrunkMap(shrunkMap, new Point(x, y));
                    Point expanded = new Point(x * 2, y * 2);

                    if (amount > 2 && rnd.Next(0, 100) < chanceToKeepThreeWay || amount > 3)
                    {
                        if (CountNeighbourRoadsNormalMap(tileGrid, expanded) > 2)
                        {
                            AddIntersection(intersectionList, tileGrid, shrunkMap, x, y, expanded, rnd, id);
                            id++;
                        }
                    }
                    else if (amount > 2)
                    {
                        if (CountNeighbourRoadsNormalMap(tileGrid, expanded) > 2)
                        {
                            AddTurningArrows(tileGrid, expanded, 1);
                        }
                    }
                }
            }
        }

        private int CountNeighbourRoadsShrunkMap(ShrunkNode[,] shrunkMap, Point point)
        {
            int amount = 0;
            Point nextPoint;

            for (int i = 0; i < 8; i += 2)
            {
                nextPoint = AngleStuff.AddPointToDirection(point, i);
                if (ShrunkWorldBuilder.PointLegit(nextPoint))
                {
                    if (shrunkMap[nextPoint.X, nextPoint.Y].landType == LandType.CITYROAD || shrunkMap[nextPoint.X, nextPoint.Y].landType == LandType.COUNTRYROAD)
                    {
                        amount++;
                    }
                }
            }
            return amount;
        }



        private int CountNeighbourRoadsNormalMap(Tile[,] tileGrid, Point point)
        {
            int amount = 0;

            Point nextPoint;
            //only checking in four directions;
            for (int i = 2; i < 5; i += 1)
            {
                nextPoint = AngleStuff.AddPointToDirection(point, i);
                if (CreatingWorld.IsRoad(tileGrid, nextPoint))
                {
                    amount++;
                }
            }
            return amount;
        }



        //should each stoplight be its own mapobject
        //its extra things to add to our draw list then
        private void AddIntersection(List<Intersection> intersectionList, Tile[,] tileGrid, ShrunkNode[,] shrunkMap, int shrunkX, int shrunkY, Point expandedPoint, Random rndExtras, int id)
        {
            Intersection intersection = new Intersection(rndExtras, expandedPoint.X, expandedPoint.Y, id);

            MapObject mapObject;

            Dictionary<Direction, StopLight> stopLightList = new Dictionary<Direction, StopLight>();
            int tileX;
            int tileY;
            StopLight stopLight;

            int stageLeftRight = rndExtras.Next(0, 1) * 2;  //should always be 0 or 2
            int stageUpDown;
            if (stageLeftRight == 0) { stageUpDown = 2; } else { stageUpDown = 0; }


            //Road Down
            if (ShrunkWorldBuilder.IsRoad(new Point(shrunkX, shrunkY + 1), shrunkMap))
            {
                tileX = expandedPoint.X;
                tileY = expandedPoint.Y + 1;
                stopLight = new StopLight(0, stageUpDown, intersection, 0);
                stopLightList.Add(Direction.DOWN, stopLight);
                mapObject = new MapObject(stopLight, tileX, tileY, Color.White, 1f);
                tileGrid[tileX, tileY].AddMapObject(mapObject, false, false, true, null, false);
                AddWhiteLine(tileGrid, tileX, tileY, 3);
            }

            // Road Right
            if (ShrunkWorldBuilder.IsRoad(new Point(shrunkX + 1, shrunkY), shrunkMap))
            {
                tileX = expandedPoint.X + 1;
                tileY = expandedPoint.Y + 1;
                stopLight = new StopLight(6, stageLeftRight, intersection, 3);
                stopLightList.Add(Direction.RIGHT, stopLight);
                mapObject = new MapObject(stopLight, tileX, tileY, Color.White, 1f);
                tileGrid[tileX, tileY].AddMapObject(mapObject, false, false, true, null, false);
                AddWhiteLine(tileGrid, tileX, tileY, 2);
            }

            // Road Up
            if (ShrunkWorldBuilder.IsRoad(new Point(shrunkX, shrunkY - 1), shrunkMap))
            {
                tileX = expandedPoint.X + 1;
                tileY = expandedPoint.Y;

                stopLight = new StopLight(4, stageUpDown, intersection, 2);
                stopLightList.Add(Direction.UP, stopLight);
                mapObject = new MapObject(stopLight, tileX, tileY, Color.White, 1f);
                tileGrid[tileX, tileY].AddMapObject(mapObject, false, false, true, null, false);
                AddWhiteLine(tileGrid, tileX, tileY, 1);
            }

            //Road Left
            if (ShrunkWorldBuilder.IsRoad(new Point(shrunkX - 1, shrunkY), shrunkMap))
            {
                tileX = expandedPoint.X;
                tileY = expandedPoint.Y;

                stopLight = new StopLight(2, stageLeftRight, intersection, 1);
                stopLightList.Add(Direction.LEFT, stopLight);
                mapObject = new MapObject(stopLight, tileX, tileY, Color.White, 1f);
                tileGrid[tileX, tileY].AddMapObject(mapObject, false, false, true, null, false);
                AddWhiteLine(tileGrid, tileX, tileY, 0);
            }

            AddTurningArrows(tileGrid, expandedPoint, 0);

            intersection.SetStopLightList(stopLightList);
            intersectionList.Add(intersection);
        }
    
        private void AddWhiteLine(Tile[,] tileGrid, int tileX, int tileY, int tileIndex)
        {
            tileGrid[tileX, tileY].AddLayer(GroundLayerController.GetLayerByIndex(LayerType.ROADDECALS, tileIndex));
        }

        enum ArrowType
        {
            INTERLINES = 0,
            ARROWLFR = 4,
            ARROWLF = 8,
            ARROWRF = 12,
            ARROWLR = 16

        }

        private void AddTurningArrows(Tile[,] tileGrid, Point expandedPoint, int offset)
        {
            Point leftP = new Point(expandedPoint.X - 1 - offset, expandedPoint.Y);
            Point upP = new Point(expandedPoint.X + 1, expandedPoint.Y - 1 - offset);
            Point rightP = new Point(expandedPoint.X + 2 + offset, expandedPoint.Y + 1);
            Point downP = new Point(expandedPoint.X, expandedPoint.Y + 2 + offset);

            bool left;
            bool up;
            bool right;
            bool down;

            left = CreatingWorld.IsRoad(tileGrid, leftP);
            up = CreatingWorld.IsRoad(tileGrid, upP);
            right = CreatingWorld.IsRoad(tileGrid, rightP);
            down = CreatingWorld.IsRoad(tileGrid, downP);

            if (left && up && right && down)
            {
                AddArrow(tileGrid, leftP, ArrowType.ARROWLFR, 0);
                AddArrow(tileGrid, upP, ArrowType.ARROWLFR, 1);
                AddArrow(tileGrid, rightP, ArrowType.ARROWLFR, 2);
                AddArrow(tileGrid, downP, ArrowType.ARROWLFR, 3);      
            }

            if (left && up && right && (!down))
            {
                AddArrow(tileGrid, leftP, ArrowType.ARROWLF, 0);
                AddArrow(tileGrid, upP, ArrowType.ARROWLR, 1);
                AddArrow(tileGrid, rightP, ArrowType.ARROWRF, 2);

            }

            if (up && right && down && (!left))
            {
                AddArrow(tileGrid, upP, ArrowType.ARROWLF, 1);
                AddArrow(tileGrid, rightP, ArrowType.ARROWLR, 2);
                AddArrow(tileGrid, downP, ArrowType.ARROWRF, 3);
            }

            if (right && down && left && (!up))
            {
                AddArrow(tileGrid, leftP, ArrowType.ARROWRF, 0);
                AddArrow(tileGrid, rightP, ArrowType.ARROWLF, 2);
                AddArrow(tileGrid, downP, ArrowType.ARROWLR, 3);
            }
            if (down && left && up && (!right))
            {
                AddArrow(tileGrid, leftP, ArrowType.ARROWLR, 0);
                AddArrow(tileGrid, upP, ArrowType.ARROWRF, 1);
                AddArrow(tileGrid, downP, ArrowType.ARROWLF, 3);
            }
        }

        private void AddArrow(Tile[,] tileGrid, Point point, ArrowType layerType, int indexOffset)
        {
            if (CreatingWorld.IsStraightRoad(tileGrid, point))
            {
                tileGrid[point.X, point.Y].AddLayer(GroundLayerController.GetLayerByIndex(LayerType.ROADDECALS, (int)layerType + indexOffset));
            }
        }
    }
}
