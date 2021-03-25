using BushFire.Engine.Controllers;
using BushFire.Game.Storage;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Tech.Jobs
{
    //Perhaps on road to road we should work backwards
    //This means if we click on a spot in the middle of a bunch of treees it will very quickly realise its stuck
    //Bug TODO:
    //2nd tile in is marked as a border
    //there is land tiles around the water marked as water


    class Pathfinding : Job
    {
        public List<DrivingNode> drivingNodeList;
        SearchStyle startStyle;
        SearchStyle endStyle;
        Point startPoint;
        Point endPoint;
        Vehicle vehicle;

        public Pathfinding(Point startPoint, Point endPoint, Vehicle vehicle)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.vehicle = vehicle;
        }

        public override void Start(Reusables reusables)
        {
            drivingNodeList = new List<DrivingNode>();
            startStyle = GetSearchStyle(startPoint);
            endStyle = GetSearchStyle(endPoint);

            if (startStyle == SearchStyle.OnRoad && endStyle == SearchStyle.OnRoad)
            {
                RoadToRoad(startPoint, endPoint, reusables);     
            }
            else if (startStyle == SearchStyle.OffRoad && endStyle == SearchStyle.OnRoad)
            {
                OffRoadToOnRoad(startPoint, endPoint, reusables);
            }
            else if (startStyle == SearchStyle.OnRoad && endStyle == SearchStyle.OffRoad)
            {
                OnRoadToOffRoad(startPoint, endPoint, reusables);
            }
            else if (startStyle == SearchStyle.OffRoad && endStyle == SearchStyle.OffRoad)
            {
                OffRoadToOffRoad(startPoint, endPoint, reusables);
            }

            //If we couldnt find it we will clear the list
            if (drivingNodeList.Count > 0)
            {
                if (drivingNodeList[0].GetLocationX() != endPoint.X || drivingNodeList[0].GetLocationY() != endPoint.Y)
                {
                    drivingNodeList.Clear();
                }
            }

            CalculateCorners();

            completed = true;
        }
     
        private void RoadToRoad(Point startPoint, Point endPoint, Reusables reusables)
        {
            AStarOnRoad AStarOnRoad = new AStarOnRoad();
            drivingNodeList = AStarOnRoad.Go(startPoint, endPoint, reusables);
        }

        private void OffRoadToOnRoad(Point startPoint, Point endPoint, Reusables reusables)
        {
            SearchResult searchResult;
            Point midPoint;

            AStarOffRoad AStarOffRoad = new AStarOffRoad();
            drivingNodeList = AStarOffRoad.Go(startPoint, endPoint, false, out searchResult, out midPoint, vehicle, false, reusables);

            if (searchResult == SearchResult.RoadFound)
            {
                AStarOnRoad AStarOnRoad = new AStarOnRoad();
                List<DrivingNode> drivingNodeListTwo = AStarOnRoad.Go(midPoint, endPoint, reusables);
                drivingNodeListTwo.AddRange(drivingNodeList);
                drivingNodeList = drivingNodeListTwo;
            }
        }

        private void OnRoadToOffRoad(Point startPoint, Point endPoint, Reusables reusables)
        {
            SearchResult searchResult;
            Point midPoint;

            AStarOffRoad AStarOffRoad = new AStarOffRoad();
            //We do this backwards
            drivingNodeList = AStarOffRoad.Go(endPoint, startPoint, false, out searchResult, out midPoint, vehicle, true, reusables);

            if (searchResult == SearchResult.RoadFound)
            {
                //Lets redo our offroad to onroad search as our path is not optimal at all as we were trying to head to a different point
                drivingNodeList = AStarOffRoad.Go(endPoint, midPoint, false, out searchResult, out midPoint, vehicle, true, reusables);

                drivingNodeList.Reverse();
                AStarOnRoad AStarOnRoad = new AStarOnRoad();
                List<DrivingNode> drivingNodeListTwo = AStarOnRoad.Go(startPoint, midPoint, reusables);
                drivingNodeList.AddRange(drivingNodeListTwo);
            }
        }

        private void OffRoadToOffRoad(Point startPoint, Point endPoint, Reusables reusables)
        {
            SearchResult searchResult;
            Point midPointOne;
            Point midPointTwo;

            AStarOffRoad AStarOffRoad = new AStarOffRoad();
            drivingNodeList = AStarOffRoad.Go(startPoint, endPoint, false, out searchResult, out midPointOne, vehicle, false, reusables);          

            if (searchResult == SearchResult.RoadFound)
            {
                //We found a road, lets re optomise our search looking for that specific point
                drivingNodeList = AStarOffRoad.Go(startPoint, midPointOne, false, out searchResult, out midPointOne, vehicle, false, reusables);


                List<DrivingNode> drivingNodeListThree = AStarOffRoad.Go(endPoint, startPoint, false, out searchResult, out midPointTwo, vehicle, true, reusables);

                if (searchResult == SearchResult.RoadFound)
                {
                    //We found a road, lets re optomise our search looking for that specific point
                    drivingNodeListThree = AStarOffRoad.Go(endPoint, midPointTwo, false, out searchResult, out midPointTwo, vehicle, true, reusables);


                    AStarOnRoad AStarOnRoad = new AStarOnRoad();
                    List<DrivingNode> drivingNodeListTwo = AStarOnRoad.Go(midPointOne, midPointTwo, reusables);

                    if (drivingNodeListTwo.Count > 0)
                    {
                        drivingNodeList.RemoveAt(0);  //Need to remove the first one so theres no double up
                    }

                    drivingNodeListThree.Reverse();
                    drivingNodeListThree.AddRange(drivingNodeListTwo);
                    drivingNodeListThree.AddRange(drivingNodeList);

                    drivingNodeList = drivingNodeListThree;                 
                }
                else if (searchResult == SearchResult.EndFound)
                {
                    //We found it in list three
                    drivingNodeListThree.Reverse();
                    drivingNodeListThree.RemoveAt(drivingNodeListThree.Count - 1);
                    drivingNodeList = drivingNodeListThree;
                }
            }

         //  AddDebug(drivingNodeList);
        }

        public void AddDebug(List<DrivingNode> myList)
        {
            foreach (DrivingNode drivingNode in myList)
            {
                WorldController.world.tileGrid[drivingNode.GetLocationX(), drivingNode.GetLocationY()].AddLayer(GroundLayerController.GetLayerByIndex(LayerType.BORDER, 12));
            }
        }     

        private void CalculateCorners()
        {               
            if (drivingNodeList.Count > 2)
            {
                int currentDirection = GetDirection(drivingNodeList[drivingNodeList.Count - 2], drivingNodeList[drivingNodeList.Count - 1]);

                for (int i = drivingNodeList.Count - 1; i > 1; i--)
                {
                    int newDirection = GetDirection(drivingNodeList[i - 1], drivingNodeList[i]);

                    if (newDirection != currentDirection)
                    {
                        Direction turnDirection = AngleStuff.GetTurnDirection(currentDirection, newDirection);


                        if (currentDirection % 2 == 0 && newDirection % 2 == 0)
                        {
                            DrivingNode drivingNode = drivingNodeList[i];
                            drivingNode.SetUseByPassTurn(currentDirection, newDirection);
                            drivingNodeList[i] = drivingNode;
                        }

                        currentDirection = newDirection;

                        for (int p = i - 2; p < i + 6; p++)
                        {
                            if (p > 0 && p < drivingNodeList.Count)
                            {
                                if (drivingNodeList[p].isRoad() && drivingNodeList[p].indicatorDirection == Direction.NONE || drivingNodeList[p].isRoad() && p < i)
                                {
                                    drivingNodeList[p].SetIndicatorDirection(turnDirection);
                                }
                            }
                        }
                    }
                }
            }
        }

        private int GetDirection(DrivingNode one, DrivingNode two)
        {
            int x = one.GetLocationX() - two.GetLocationX();
            int y = one.GetLocationY() - two.GetLocationY();

            return AngleStuff.GetDirection(new Point(x, y));
        }


        private SearchStyle GetSearchStyle(Point point)
        {
            if (WorldController.GetTileLogistic(point).isRoad)
            {
                return SearchStyle.OnRoad;
            }
            else
            {
                return SearchStyle.OffRoad;
            }
        }
        
    }

    enum SearchStyle
    {
        None,
        OffRoad,
        OnRoad
    }
}