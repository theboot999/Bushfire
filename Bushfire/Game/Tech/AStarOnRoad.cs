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
    class AStarOnRoad
    {
        int width = WorldController.world.worldWidth;
        int height = WorldController.world.worldHeight;
        Point startPoint;
        Point endPoint;
        int htileCost = 5;             //Heuristic tile cost for straight    
        int maximumToCheck = 200000;
        bool foundTarget;

        public AStarOnRoad()
        {

        }

        public List<DrivingNode> Go(Point startPoint, Point endPoint, Reusables reusables)
        {
            reusables.Clear();
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            foundTarget = false;
            Point workingPoint;
            AStarNode workingNode;

            reusables.toCheckList.Add(GetOneD(startPoint), new AStarNode(GetOneD(startPoint), GetOneD(startPoint), 0, 0, 255, true));

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

            if (foundTarget)
            {
                //Debugging(checkedNodeList, toCheckList);
                return GetDrivingNodeList(reusables, startPoint);
            }
            else
            {
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

                if (workingPoint != endPoint)
                {
                    return true;
                }
                else
                {
                    foundTarget = true;
                    return false;
                }
            }
            workingPoint = Point.Zero;
            workingNode = new AStarNode(0, 0, 0, 0, 0, false);  //can return an empty one
            return false;
        }

        private void AddNewPoints(Reusables reusables, Point workingPoint, AStarNode workingNode)
        {
            for (byte i = 0; i < 8; i += 2)
            {
                Point newPoint = workingPoint + AngleStuff.directionPoint[i];

                if (workingPoint == Point.Zero)
                {
                    string BUG = "";
                    Debug.WriteLine("BUGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG We hit an empty point on an astar");
                }
                else
                {
                    if (WorldController.world.tileGrid[newPoint.X, newPoint.Y].IsRoad() && !WorldController.world.tileGrid[newPoint.X, newPoint.Y].tileLogistic.IsDirectionBlock(i))
                    {
                        if (!IsInLists(newPoint, reusables))
                        {
                            int count = 1;
                            int tileScore = WorldController.world.tileGrid[newPoint.X, newPoint.Y].tileLogistic.pathFindingScore;
                            int roadId = WorldController.world.tileGrid[newPoint.X, newPoint.Y].tileLogistic.roadId;

                            TileLogistic workingLogistic = WorldController.world.tileGrid[workingPoint.X, workingPoint.Y].tileLogistic;
                            TileLogistic newLogistic = WorldController.world.tileGrid[newPoint.X, newPoint.Y].tileLogistic;


                            while (newLogistic.roadId == workingLogistic.roadId && newLogistic.roadId < 12 && !EndOrDirectionBlockOrStop(newPoint, newLogistic, i))
                            {
                                if (!IsInLists(newPoint, reusables))
                                {
                                    count++;
                                    newPoint = newPoint + AngleStuff.directionPoint[i];
                                    newLogistic = WorldController.world.tileGrid[newPoint.X, newPoint.Y].tileLogistic;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            while (count > -1 && reusables.toCheckList.ContainsKey(GetOneD(newPoint)))
                            {
                                newPoint = newPoint + AngleStuff.directionPoint[AngleStuff.RotateDirection(i, 4)];
                                count--;
                            }

                            if (count > 0 && !reusables.toCheckList.ContainsKey(GetOneD(newPoint)))
                            {
                                int id = GetOneD(newPoint);
                                reusables.toCheckList.Add(id, new AStarNode(id, GetOneD(workingPoint), CalcNScore(tileScore * count, workingNode), CalcHScore(newPoint), i, true));
                            }
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

        private int CalcNScore(int tileScore, AStarNode workingNode)
        {
            return tileScore + workingNode.nScore;
        }

        private int CalcHScore(Point newPoint)
        {
            int scoreX = Math.Abs(endPoint.X - newPoint.X);
            int scoreY = Math.Abs(endPoint.Y - newPoint.Y);
            return (scoreX + scoreY) * htileCost;
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
        public List<DrivingNode> GetDrivingNodeList(Reusables reusables, Point startPoint)
        {
            List<DrivingNode> drivingNodeList = new List<DrivingNode>();

            AStarNode currentNode = reusables.checkedNodeList[GetOneD(endPoint)];
            Point currentPoint = currentNode.GetCurrent2D();

            while (currentPoint != startPoint)
            {
                currentPoint = currentNode.GetCurrent2D();
                Point reachPoint = currentNode.GetPrevious2D();

                int direction = currentNode.GetMirrorDirection();

                do
                {
                    drivingNodeList.Add(new DrivingNode(currentPoint.X, currentPoint.Y));
                    currentPoint = AngleStuff.AddPointToDirection(currentPoint, direction);
                }
                while (currentPoint != reachPoint);
                //probably a better way to do this, but if we reached our reach point we do need to add it one more

                //   drivingNodeList.Add(new DrivingNode(currentPoint.X, currentPoint.Y));



                currentNode = reusables.checkedNodeList[GetOneD(currentPoint)];
            }

            if (drivingNodeList.Count > 0)
            {
                //We need to pop the last from the list
                //   drivingNodeList.RemoveAt(drivingNodeList.Count - 1);
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
