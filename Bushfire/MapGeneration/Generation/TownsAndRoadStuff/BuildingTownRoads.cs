using BushFire.Engine.Controllers;
using BushFire.Game;
using BushFire.Game.Controllers;
using BushFire.Game.Map;
using BushFire.MapGeneration.Containers;
using BushFire.MapGeneration.Tech;
using BushFire.Menu.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Generation.RoadStuff
{
    class BuildingTownRoads
    {
        //One known issue.
        //if it doesnt reach full depth it will start another plot
        //so if you have only a short plot then 


        int numberOfRoads;
        int percentageTurn = 50;
        int percentageStraight = 50;
        int minRandomPlotDepth = 3;
        int maxRandomPlotDepth = 6;
        int minRandomLength = 6;
        int maxRandomLength = 16;
        int extraToReachRoad = 6;
        int stopRoadCount;
        int plotNum = 0;
        ShrunkPlot plotLeft;
        ShrunkPlot plotRight;


        public BuildingTownRoads(List<Town> townList, ShrunkNode[,] shrunkMap, List<ShrunkPlot> shrunkPlotList, LoadingInfo loadingInfo)
        {
            float percentDone = 0;
            float percentJump = 100f / townList.Count;
            loadingInfo.UpdateLoading(LoadingType.ExpandingTowns, percentDone);
            Point workingPoint;

           //'' for (int townSiteId = 0; townSiteId < townList.Count; townSiteId++)
            foreach (Town town in townList)
            {
                //set the Id here
                List<ConnectingPoint> connectingPointsList = new List<ConnectingPoint>();
                numberOfRoads = GameController.rnd.Next(CreatingWorld.minTownRoads, CreatingWorld.maxTownRoads);
                workingPoint = town.GetShrunkPoint();               
                BuildTownCenterPoints(workingPoint, connectingPointsList, town.id, shrunkMap, shrunkPlotList);

                for (int p = 0; p < numberOfRoads; p++)
                {
                    stopRoadCount = 4;
                    if (connectingPointsList.Count > 0)
                    {
                        ConnectingPoint connectingPoint = connectingPointsList[0];
                        workingPoint = connectingPoint.connectingPoint;
                        LayRoadStretch(workingPoint, connectingPoint.directionToTravel, connectingPoint.distanceToTravel, town.id, connectingPointsList, shrunkMap, shrunkPlotList);
                        connectingPointsList.RemoveAt(0);
                    }
                }
                percentDone += percentJump;
                loadingInfo.UpdateLoading(LoadingType.ExpandingTowns, percentDone);
            }
        }



        private void LayRoadStretch(Point expansionPoint, int directionToTravel, int distanceToTravel, int townId, List<ConnectingPoint> connectingPointList, ShrunkNode[,] shrunkMap, List<ShrunkPlot> shrunkPlotList)
        {
            int depth = GameController.rnd.Next(minRandomPlotDepth, maxRandomPlotDepth);
            bool addConnectingPoint = false;
            bool forceThroughPlot = false;


            for (int roadDistance = 0; roadDistance < distanceToTravel; roadDistance++)
            {
                CalcShrunkPlots(shrunkPlotList, directionToTravel, townId);
                expansionPoint = AngleStuff.AddPointToDirection(expansionPoint, directionToTravel);
 
                if (CheckIfLandType(expansionPoint, shrunkMap, LandType.OPEN) || forceThroughPlot)
                {
                    TurnIntoRoad(expansionPoint, townId, shrunkMap, shrunkPlotList);

                    if (roadDistance != distanceToTravel - 1) //We are not at our last piece
                    {
                        if (!AddPlot(expansionPoint, AngleStuff.RotateDirection(directionToTravel, -2), plotLeft, shrunkMap))
                        {
                            plotLeft = null;
                        }
                        if (!AddPlot(expansionPoint, AngleStuff.RotateDirection(directionToTravel, 2), plotRight, shrunkMap))
                        {
                            plotRight = null;
                        }
                    }

                    if (IsMoreThanTwoNeighbours(expansionPoint, shrunkMap))
                    {
                        addConnectingPoint = false;
                        break;
                    }
                }
                else
                {
                    roadDistance = distanceToTravel - 1;
                }
                
                if (roadDistance > 4) { addConnectingPoint = true; }
                
                if (roadDistance == distanceToTravel - 1 && !forceThroughPlot)
                {
                    int amount = ExtraToReachRoad(shrunkMap, directionToTravel, expansionPoint);
                    if (amount > 0)
                    {
                        TurnIntoRoad(expansionPoint, townId, shrunkMap, shrunkPlotList);
                        distanceToTravel += amount;
                        forceThroughPlot = true;
                    }
                }              
                //Do we add to road distance
                //We also check at the end and do the smooth thing
            }
            plotLeft = null;
            plotRight = null;


            if (addConnectingPoint)
            {
                AddConnectingPoint(expansionPoint, directionToTravel, connectingPointList, shrunkMap);
            }
        }
        
        private int ExtraToReachRoad(ShrunkNode[,] shrunkMap, int directionToTravel, Point expansionPoint)
        {
            for (int i = 1; i < extraToReachRoad; i++)
            {
                expansionPoint = AngleStuff.AddPointToDirection(expansionPoint, directionToTravel);
                if (ShrunkWorldBuilder.PointLegit(expansionPoint))
                {
                    if (shrunkMap[expansionPoint.X, expansionPoint.Y].IsRoad())
                    {
                        return i;
                    }
                }
            }
            return 0;
        }

        private void CalcShrunkPlots(List<ShrunkPlot> shrunkPlotList, int roadDirection, int townId)
        {
            if (plotLeft != null && plotLeft.currentLength >= plotLeft.maxLength) //we are at the end
            {
                plotLeft = null;
            }

            if (plotRight != null && plotRight.currentLength >= plotRight.maxLength) //we are at the start
            {
                plotRight = null;
            }

            if (plotLeft == null)
            {
                plotLeft = new ShrunkPlot(plotNum, AngleStuff.RotateDirection(roadDirection, 2), townId);
                plotNum++;
                shrunkPlotList.Add(plotLeft);
            }
            if (plotRight == null)
            {
                plotRight = new ShrunkPlot(plotNum, AngleStuff.RotateDirection(roadDirection, -2), townId);
                plotNum++;
                shrunkPlotList.Add(plotRight);
            }


            plotLeft.currentLength++;
            plotRight.currentLength++;
      
        }


        private bool AddPlot(Point expansionPoint, int direction, ShrunkPlot plot, ShrunkNode[,] shrunkMap)
        {
            Point nextPoint = expansionPoint;
            for (int depth = 1; depth <= plot.maxDepth; depth++)
            {
                nextPoint = AngleStuff.AddPointToDirection(nextPoint, direction);
                if (ShrunkWorldBuilder.PointLegit(nextPoint) && CheckIfLandType(nextPoint, shrunkMap, LandType.OPEN))
                {
                    AddCornerPoints(depth, plot, nextPoint);
                    shrunkMap[nextPoint.X, nextPoint.Y].SetLandType(LandType.PLOT);
                }
                else
                {
                    if (depth < 3)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    
        private void AddCornerPoints(int depth, ShrunkPlot shrunkPlot, Point newPoint)
        {

            if (shrunkPlot.isFirst)
            {
                shrunkPlot.isFirst = false;
                shrunkPlot.AddPointOne(newPoint);
            }

            shrunkPlot.AddPointTwo(newPoint);
            

        }
   
        private bool IsMoreThanTwoNeighbours(Point expansionPoint, ShrunkNode[,] shrunkMap)
        {
            Point workingPoint;
            int count = 0;

            for (int i = 0; i < 8; i += 2)
            {
                workingPoint = AngleStuff.AddPointToDirection(expansionPoint, i);
                if (ShrunkWorldBuilder.PointLegit(workingPoint))
                {
                    if (shrunkMap[workingPoint.X, workingPoint.Y].IsRoad())
                    {
                        count++;
                    }
                }
            }
            return count > 1;
        }

        private bool IsEnoughForConnectionPoint(int distance)
        {
            if (distance > 4)
            {
                return true;
            }
            return false;
        }

   
        private void BuildTownCenterPoints(Point workingPoint, List<ConnectingPoint> connectingPointList, int townSiteId, ShrunkNode[,] shrunkMap, List<ShrunkPlot> shrunkPlotList)
        {
            for (int i = 0; i < 8; i += 2)
            {
                int amount = GameController.rnd.Next(minRandomLength, maxRandomLength);
                connectingPointList.Add(new ConnectingPoint(i, workingPoint, amount));
            }

            TurnIntoRoad(workingPoint, townSiteId, shrunkMap, shrunkPlotList);
        }


        private void AddConnectingPoint(Point workingPoint, int currentDirection, List<ConnectingPoint> connectingPointList, ShrunkNode[,] shrunkMap)
        {
            int percentage;

            if (ShrunkWorldBuilder.PointLegit(workingPoint) && stopRoadCount > 0 && shrunkMap[workingPoint.X, workingPoint.Y].IsRoad())
            {
                for (int i = 0; i < 8; i += 2)
                {
                    int chance = GameController.rnd.Next(0, 100);

                    percentage = percentageTurn;

                    if (currentDirection == i)
                    {
                        percentage = percentageStraight;
                    }

                    if (chance < percentage)
                    {
                        if (CheckIfLandType(AngleStuff.AddPointToDirection(workingPoint, i), shrunkMap, LandType.OPEN))
                        {
                            int amount = GameController.rnd.Next(minRandomLength, maxRandomLength);
                            connectingPointList.Add(new ConnectingPoint(i, workingPoint, amount));

                        }
                    }
                }
            }
        }

        private void TurnIntoRoad(Point pointToTurn, int townId, ShrunkNode[,] shrunkMap, List<ShrunkPlot> shrunkPlotList)
        {
            shrunkMap[pointToTurn.X, pointToTurn.Y].SetLandType(LandType.CITYROAD);

            if (shrunkMap[pointToTurn.X, pointToTurn.Y].townId > -1)
            {
                shrunkMap[pointToTurn.X, pointToTurn.Y].SetTownId(townId);
            }
        }
        
  

        private bool CheckIfRoad(Point checkPoint, ShrunkNode[,] shrunkMap)
        {
            bool check = false;

            if (ShrunkWorldBuilder.PointLegit(checkPoint))
            {
                if (shrunkMap[checkPoint.X, checkPoint.Y].IsRoad())
                {
                    check = true;
                }
            }
            return check;
        }

        public bool CheckIfLandType(Point checkPoint, ShrunkNode[,] shrunkMap, LandType landType)
        {
            if (ShrunkWorldBuilder.PointLegit(checkPoint))
            {
                return shrunkMap[checkPoint.X, checkPoint.Y].landType == landType;
             }
            return false;
        }
     

    }
}

