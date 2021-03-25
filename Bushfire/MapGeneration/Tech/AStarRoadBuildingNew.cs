using BushFire.Engine.Controllers;
using BushFire.Game;
using BushFire.Game.Map;
using BushFire.Game.Storage;
using BushFire.Game.Tech;
using BushFire.MapGeneration.Generation.RoadStuff;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Tech
{
    class AStarRoadBuildingNew
    {
        //so the basic difference in this one is
        //if it was in the list and we come across it again
        //we simply ignore it

        Point startPoint;
        Point endPoint;
        int htileCost = 5;             //Heuristic tile cost for straight
        int directionChangeCost = 24;   //Extra cost for doing a direction change
        int maximumToCheck = 1000000;
        bool found;
        ShrunkNode[,] shrunkMap;

        public AStarRoadBuildingNew()
        {

        }

        public List<Point> GetTravelList(Point startPoint, Point endPoint, ShrunkNode[,] shrunkMap, int findTownId)
        {
            this.shrunkMap = shrunkMap;
            Dictionary<int, AStarNode> checkedNodeList = new Dictionary<int, AStarNode>();
            Dictionary<int, AStarNode> toCheckList = new Dictionary<int, AStarNode>();
            this.startPoint = startPoint;
            this.endPoint = endPoint;

            Point workingPoint = Point.Zero;
            AStarNode workingNode;

            toCheckList.Add(GetOneD(startPoint), new AStarNode(GetOneD(startPoint), GetOneD(startPoint), 0, 0, 255, false));

            for (int i = 0; i < maximumToCheck; i++)
            {
                if (NextRoundChecks(toCheckList, checkedNodeList, out workingPoint, out workingNode, findTownId))
                {
                        AddNewPoints(toCheckList, checkedNodeList, workingPoint, workingNode, findTownId);
                }
                else
                {
                    break;
                }
            }


            List<Point> routeList = new List<Point>();
            if (found)
            {
                UpdateTravelList(routeList, workingPoint, checkedNodeList);
            }
            return routeList;  //we are returning an empty list
        }


   
        private bool NextRoundChecks(Dictionary<int, AStarNode> toCheckList, Dictionary<int, AStarNode> checkedNodeList, out Point workingPoint, out AStarNode workingNode, int findTownId)
        {
            if (toCheckList.Count > 0)
            {
                workingNode = toCheckList.ElementAt(0).Value;

                foreach (AStarNode nodeCheck in toCheckList.Values)
                {
                    if (nodeCheck.GetTotalScore() < workingNode.GetTotalScore())
                    {
                        workingNode = nodeCheck;
                    }
                }
                toCheckList.Remove(workingNode.currentPoint);

                if (checkedNodeList.ContainsKey(workingNode.currentPoint))
                {
                    AStarNode existing = checkedNodeList[workingNode.currentPoint];
                    if (workingNode.GetTotalScore() < existing.GetTotalScore())     //Its a better version
                    {
                        checkedNodeList.Remove(workingNode.currentPoint);
                        checkedNodeList.Add(workingNode.currentPoint, workingNode);
                    }
                    else  //Using recursive lets get the next best one
                    {
                        NextRoundChecks(toCheckList, checkedNodeList, out workingPoint, out workingNode, findTownId);
                    }

                }
                else
                {
                    checkedNodeList.Add(workingNode.currentPoint, workingNode);
                }

                workingPoint = GetTwoD(workingNode.currentPoint);

                if (shrunkMap[workingPoint.X, workingPoint.Y].townId == findTownId || workingPoint == endPoint)
                {
                    found = true;
                    return false;
                }
                else
                {
                    return true;
                }
            }

            workingPoint = Point.Zero;
            workingNode = new AStarNode(0, 0, 0, 0, 0, false);
            return false;
        }

        private void AddNewPoints(Dictionary<int, AStarNode> toCheckList, Dictionary<int, AStarNode> checkedNodeList, Point workingPoint, AStarNode workingNode, int findTownId)
        {
            for (byte i = 0; i < 8; i += 2)
            {
                Point newPoint = workingPoint + AngleStuff.directionPoint[i];

                if (ShrunkWorldBuilder.xLegitShrunkMap(newPoint.X, 0) && ShrunkWorldBuilder.yLegitShrunkMap(newPoint.Y, 0))
                {
                    if (shrunkMap[newPoint.X, newPoint.Y].townId == findTownId)
                    {
                        //we found it... lets add this cheapy to the list so it will always grab it next time
                        int id = GetOneD(newPoint);
                        toCheckList.Add(id, new AStarNode(id, GetOneD(workingPoint), 1, 1, i, false));
                        break;
                    }
                    else
                    {
                        if (!IsInLists(newPoint, toCheckList, checkedNodeList))
                        {
                            int tileScore = shrunkMap[newPoint.X, newPoint.Y].pathScore;
                            int id = GetOneD(newPoint);
                            toCheckList.Add(id, new AStarNode(id, GetOneD(workingPoint), CalcNScore(tileScore, workingNode, i != workingNode.directionFrom), CalcHScore(newPoint), i, false));
                        }
                    }
                }
            }
        }

        private bool EndOrDirectionBlock(Point newPoint, TileLogistic newLogistic, int directionBlock)
        {
            return newLogistic.IsDirectionBlock(directionBlock) || newPoint == endPoint;
        }


        private bool IsInLists(Point newPoint, Dictionary<int, AStarNode> toCheckList, Dictionary<int, AStarNode> checkedNodeList)
        {
            if (checkedNodeList.ContainsKey(GetOneD(newPoint)))
            {
                return true;
            }
            if (toCheckList.ContainsKey(GetOneD(newPoint)))
            {
                return true;
            }
            return false;
        }

        private int CalcNScore(int tileScore, AStarNode workingNode, bool isDirectionCost)
        {
            if (isDirectionCost)
            {
                return tileScore + workingNode.nScore + directionChangeCost;
            }
            else
            {
                return tileScore + workingNode.nScore;
            }
            
        }

        private int CalcHScore(Point newPoint)
        {
            int scoreX = Math.Abs(endPoint.X - newPoint.X);
            int scoreY = Math.Abs(endPoint.Y - newPoint.Y);
            return ((scoreX + scoreY) * 3) * htileCost;
        }

        private int GetOneD(Point point)
        {
            return (point.Y * ShrunkWorldBuilder.shrunkWorldWidth) + point.X;
        }

        private int GetOneD(int x, int y)
        {
            return (y * ShrunkWorldBuilder.shrunkWorldWidth) + x;
        }

        private Point GetTwoD(int index)
        {
            return new Point(index % ShrunkWorldBuilder.shrunkWorldWidth, index / ShrunkWorldBuilder.shrunkWorldWidth);
        }


        private List<Point> UpdateTravelList(List<Point> routeList, Point currentPoint, Dictionary<int, AStarNode> checkedNodeList)
        {
            Point tempPoint;

            routeList.Add(currentPoint);
            do
            {
                tempPoint = GetTwoD(checkedNodeList[GetOneD(currentPoint)].previousPoint);  // comeFrom[currentPoint.X, currentPoint.Y];
                routeList.Add(tempPoint);
                currentPoint = tempPoint;
            }
            while (!(tempPoint == startPoint));
            return routeList;
        }

    }
}
