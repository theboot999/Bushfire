using BushFire.Engine.Controllers;
using BushFire.Game.Map;
using BushFire.Game.Storage;
using BushFire.Game.Tech.Jobs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Tech
{
    enum SearchResult
    {
        EndFound,
        RoadFound,
        None
    }

    class AStarOffRoad
    {
        int width = WorldController.world.worldWidth;
        int height = WorldController.world.worldHeight;
        Point startPoint;
        Point endPoint;
        int htileCost = 25;             //Heuristic tile cost for straight
        int directionChangeCost = 5;
        int diagonalCost = 12;
        int maximumToCheck = 400000;
        bool foundTarget;
        bool foundRoad;
        Vehicle currentVehicle;
 
        public AStarOffRoad()
        {

        }
      
        
        public List<DrivingNode> Go(Point startPoint, Point endPointZ, bool reversed, out SearchResult searchResult, out Point midPoint, Vehicle currentVehicle, bool keepDestination, Reusables reusables)
        {
            reusables.Clear();
            //We were not using this before.  Does it affect anything.  Will it block vehilces in?
            this.currentVehicle = currentVehicle;
            this.startPoint = startPoint;
            endPoint = endPointZ;
            foundTarget = false;
            foundRoad = false;
            Point workingPoint;
            AStarNode workingNode;

            reusables.toCheckList.Add(GetOneD(startPoint), new AStarNode(GetOneD(startPoint), GetOneD(startPoint), 0, 0, 0, false));

            for (int i = 0; i < maximumToCheck; i++)
            {
                if (NextRoundChecks(reusables, out workingPoint, out workingNode))
                {
                    AddNewPoints(reusables, workingPoint, workingNode);
                }
                else
                {
                    break;
                }
            }

   //         Debugging(checkedNodeList, toCheckList);
            if (foundTarget)
            {
                searchResult = SearchResult.EndFound;
                midPoint = GetMidPoint(reversed);
                //Debugging(checkedNodeList, toCheckList);
                return GetDrivingNodeList(reusables, reversed, startPoint, endPoint, keepDestination);
            }
            else if (foundRoad)
            {
                searchResult = SearchResult.RoadFound;
                midPoint = GetMidPoint(reversed);
                //Debugging(checkedNodeList, toCheckList);
                return GetDrivingNodeList(reusables, reversed, startPoint, endPoint, keepDestination);
            }
            else
            {
                searchResult = SearchResult.None;
                midPoint = GetMidPoint(reversed);
                //Debugging(checkedNodeList, toCheckList);
                return new List<DrivingNode>();
            }
        }

        private void Debugging(Dictionary<int, AStarNode> checkedNodeList, Dictionary<int, AStarNode> toCheckList)
        {
            foreach (AStarNode node in toCheckList.Values)  //Yellow to check
            {
                Point point = node.GetCurrent2D();
                WorldController.world.tileGrid[point.X, point.Y].AddLayer(GroundLayerController.GetLayerByIndex(LayerType.BORDER, 4));
            }

            foreach (AStarNode node in checkedNodeList.Values)
            {
                Point point = node.GetCurrent2D();
                WorldController.world.tileGrid[point.X, point.Y].AddLayer(GroundLayerController.GetLayerByIndex(LayerType.BORDER, 8));
            }          
        }

        private Point GetMidPoint(bool reversed)
        {
            if (reversed)
            {
                return startPoint;
            }
            else
            {
                return endPoint;
            }
        }

        private bool NextRoundChecks(Reusables reusables, out Point workingPoint, out AStarNode workingNode)
        {
            if (reusables.toCheckList.Count > 0)
            {
                workingNode = reusables.toCheckList.ElementAt(0).Value;

                foreach (AStarNode nodeCheck in reusables.toCheckList.Values)
                {
                    if (nodeCheck.GetTotalScore() < workingNode.GetTotalScore())
                    {
                        workingNode = nodeCheck;
                    }
                }
                reusables.toCheckList.Remove(workingNode.currentPoint);

                if (reusables.checkedNodeList.ContainsKey(workingNode.currentPoint))
                {
                    AStarNode existing = reusables.checkedNodeList[workingNode.currentPoint];
                    if (workingNode.GetTotalScore() < existing.GetTotalScore())     //Its a better version
                    {
                        reusables.checkedNodeList.Remove(workingNode.currentPoint);
                        reusables.checkedNodeList.Add(workingNode.currentPoint, workingNode);
                    }
                    else  //Using recursive lets get the next best one
                    {
                        NextRoundChecks(reusables, out workingPoint, out workingNode);
                    }

                }
                else
                {
                    reusables.checkedNodeList.Add(workingNode.currentPoint, workingNode);
                }

                workingPoint = GetTwoD(workingNode.currentPoint);
       
                if (workingNode.isRoad)
                {
                    endPoint = workingNode.GetCurrent2D();
                    foundRoad = true;
                    return false;
                }
                else if (workingPoint == endPoint)
                {
                    foundTarget = true;
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

        private void AddNewPoints(Reusables reusables, Point workingPoint, AStarNode workingNode)
        {
              //Diagonals
            for (byte i = 1; i < 8; i += 2)
            {
                Point newPoint = workingPoint + AngleStuff.directionPoint[i];

                if (WorldController.world.tileGrid[newPoint.X, newPoint.Y].IsRoad() || WorldController.world.tileGrid[newPoint.X, newPoint.Y].IsOpen())
                {
                    Point diagonal1 = workingPoint + AngleStuff.directionPoint[i - 1];
                    Point diagonal2 = workingPoint + AngleStuff.directionPoint[i + 1];

                    if (WorldController.world.tileGrid[diagonal1.X, diagonal1.Y].IsDrivable() && WorldController.world.tileGrid[diagonal2.X, diagonal2.Y].IsDrivable())
                    {
                        if (!WorldController.world.tileGrid[newPoint.X, newPoint.Y].IsParkedAndLockedVehicle(currentVehicle))
                        {
                            if (!IsInLists(newPoint, reusables))
                            {
                                int tileScore = WorldController.world.tileGrid[newPoint.X, newPoint.Y].tileLogistic.pathFindingScore;
                                bool isRoad = WorldController.world.tileGrid[newPoint.X, newPoint.Y].tileLogistic.IsRoad();
                                int id = GetOneD(newPoint);
                                bool directionChange = workingNode.directionFrom != i;

                                reusables.toCheckList.Add(id, new AStarNode(id, GetOneD(workingPoint), CalcNScoreWithDiagonal(tileScore, workingNode, directionChange), CalcHScore(newPoint), i, isRoad));
                            }
                        }
                    }

                }
            }


            //Straights
            for (byte i = 0; i < 8; i += 2)
            {
                Point newPoint = workingPoint + AngleStuff.directionPoint[i];

                if (WorldController.world.tileGrid[newPoint.X, newPoint.Y].IsDrivable())
                {
                    if (!WorldController.world.tileGrid[newPoint.X, newPoint.Y].IsParkedAndLockedVehicle(currentVehicle))
                    {
                        if (!IsInLists(newPoint, reusables))
                        {
                            int tileScore = WorldController.world.tileGrid[newPoint.X, newPoint.Y].tileLogistic.pathFindingScore;
                            bool isRoad = WorldController.world.tileGrid[newPoint.X, newPoint.Y].tileLogistic.IsRoad();
                            int id = GetOneD(newPoint);
                            bool directionChange = workingNode.directionFrom != i;
                            //int directionChange = directionChangeCost * (Directions.GetDirectionDifference(workingNode.directionFrom, i));
                            reusables.toCheckList.Add(id, new AStarNode(id, GetOneD(workingPoint), CalcNScore(tileScore, workingNode, directionChange), CalcHScore(newPoint), i, isRoad));
                        }
                    }
                }
            }
        }

        private bool EndOrDirectionBlockOrStop(Point newPoint, TileLogistic newLogistic, int directionBlock)
        {
            return newLogistic.IsDirectionBlock(directionBlock) || newPoint == endPoint || newPoint.X == endPoint.X || newPoint.Y == endPoint.Y;
        }


        private bool IsInLists(Point newPoint, Reusables reusables)
        {
            if (reusables.checkedNodeList.ContainsKey(GetOneD(newPoint)))
            {
                return true;
            }
            if (reusables.toCheckList.ContainsKey(GetOneD(newPoint)))
            {
                return true;
            }
            return false;
        }

        private int CalcNScore(int tileScore, AStarNode workingNode, bool directionChange)
        {
            if (!directionChange)
            {
                return tileScore + workingNode.nScore;
            }
            else
            {
                return tileScore + workingNode.nScore + directionChangeCost;
            }
        }


        private int CalcNScoreWithDiagonal(int tileScore, AStarNode workingNode, bool directionChange)
        {
            if (!directionChange)
            {
                return tileScore + diagonalCost + workingNode.nScore;
            }
            else
            {
                return tileScore + diagonalCost + workingNode.nScore + directionChangeCost;
            }
        }

        private int CalcHScore(Point newPoint)
        {
            int scoreX = Math.Abs(endPoint.X - newPoint.X);
            int scoreY = Math.Abs(endPoint.Y - newPoint.Y);
            return ((scoreX + scoreY) * htileCost);
        }

        private int GetOneD(Point point)
        {
            return (point.Y * width) + point.X;
        }

        private int GetOneD(int x, int y)
        {
            return (y * width) + x;
        }

        private Point GetTwoD(int index)
        {
            return new Point(index % width, index / width);
        }


        //we need to order it our next tile to drive to is the end off the list
        //as we want to pull them off the end
        public List<DrivingNode> GetDrivingNodeList(Reusables reusables, bool reversed, Point startPoint, Point endPoint, bool keepDestination)
        {
            List<DrivingNode> drivingNodeList = new List<DrivingNode>();

            AStarNode currentNode = reusables.checkedNodeList[GetOneD(endPoint)];
            Point currentPoint = currentNode.GetCurrent2D();


            while (currentPoint != startPoint)
            {
                currentPoint = currentNode.GetCurrent2D();
                drivingNodeList.Add(new DrivingNode(currentPoint.X, currentPoint.Y));
                currentPoint = currentNode.GetPrevious2D();
                currentNode = reusables.checkedNodeList[GetOneD(currentPoint)];
            }

            drivingNodeList.Add(new DrivingNode(currentPoint.X, currentPoint.Y));

            if (drivingNodeList.Count > 0)
            {
                if (reversed)
                {
                    //We need to pop the first from the list
                    drivingNodeList.RemoveAt(0);
                }
                else
                {
                    drivingNodeList.RemoveAt(drivingNodeList.Count - 1);
                }
            }

            if (keepDestination)
            {
                drivingNodeList.Add(new DrivingNode(startPoint.X, startPoint.Y));
            }

            return AddDirectionTurns(drivingNodeList, startPoint);
        }

        private List<DrivingNode> AddDirectionTurns(List<DrivingNode> drivingNodeList, Point startPoint)
        {
            if (drivingNodeList.Count > 0)
            {
                Point point = new Point(drivingNodeList[drivingNodeList.Count - 1].GetLocationX() - startPoint.X, drivingNodeList[drivingNodeList.Count - 1].GetLocationY() - startPoint.Y);
                int direction = AngleStuff.GetDirection(point);

                DrivingNode node = drivingNodeList[drivingNodeList.Count - 1];
                node.SetDrivingDirection((byte)direction);
                drivingNodeList[drivingNodeList.Count - 1] = node;

                for (int i = drivingNodeList.Count - 2; i > -1; i--)
                {
                    point = new Point(drivingNodeList[i].GetLocationX() - drivingNodeList[i + 1].GetLocationX(), drivingNodeList[i].GetLocationY() - drivingNodeList[i + 1].GetLocationY());
                    direction = AngleStuff.GetDirection(point);

                    DrivingNode node1 = drivingNodeList[i];
                    node1.SetDrivingDirection((byte)direction);
                    drivingNodeList[i] = node1;
                }
            }
            return drivingNodeList;
        }
    }
}

