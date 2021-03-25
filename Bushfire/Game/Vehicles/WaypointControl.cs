using BushFire.Engine.Controllers;
using BushFire.Game.Controllers;
using BushFire.Game.Map;
using BushFire.Game.Tech.Jobs;
using BushFire.Game.Vehicles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Tech
{
    //SO i want this class to exclusively control waypoints and the tiles for the vehicles
    //the main one will control the vector driving positions

    class WaypointControl
    {
        Pathfinding pathfindingJob;
        public List<DrivingNode> drivingNodeList = new List<DrivingNode>();
        private bool waitingOnNewPathfinding;
        Location location;
        VehicleParameters vehicleParameters;
        float waypointCheckCounter;


        public WaypointControl(Location location, VehicleParameters vehicleParameters)
        {
            this.location = location;
            this.vehicleParameters = vehicleParameters;
        }

        public bool ReadyNextWayPoint()
        {
            if (waitingOnNewPathfinding)
            {
                //Check if our pathfinding job is done.  If it was in progress and is done we mark it as completed
                UpdateWaitingOnNewPathing();
                return false;
            }

            //We are not waiting on new pathfinding an
            //Now we work out if the next one is clean.  Be warned we have modified waiting on new pathing in the above method
            if (!waitingOnNewPathfinding && drivingNodeList.Count > 0)
            {
                if (waypointCheckCounter < 0.0001)
                {
                    waypointCheckCounter = 1;

                    Tile nextTile = GetTile(1);
                    bool isStopLight;

                    if (IsTileEmptyFromVehicles(GetTile(1)))
                    {
                        if (!IsStoplightBlock(nextTile, GetDirection(1), out isStopLight))
                        {

                            Tile lastTile = GetTile(2);

                            if (IsTileEmptyFromVehicles(lastTile))
                            {
                                vehicleParameters.followingVehicle = null;

                            }
                            else if (IsTileWithVehicleMovingSameDirection(lastTile))
                            {
                                vehicleParameters.followingVehicle = lastTile.vehicle;
                                //match speed
                            }
                            if (isStopLight)
                            {
                                nextTile.IncreaseStopLightCounter();
                            }

                            waypointCheckCounter = 0;
                            return true;
                        }


                    }
                    else
                    {
                        //To fix the edge case of were we have 2 vehicles trying to turn right into an intersection
                        //The fix is, we remove them from the tail tile of the map
                        //First we need to determine if were in an intersection
                        //Second if we are stopped
                        //third if we are turning

                        //Tile is our next tile
                        if (nextTile.IsRoad())
                        {
                            if (AngleStuff.GetDirectionDifference(vehicleParameters.directionTravelling, nextTile.vehicle.GetTravellingDirectionInt()) == 4)
                            {
                                location.RemoveTailFromTile();
                            }
                        }
                    }
                }
                else
                {
                    waypointCheckCounter -= 0.03f * EngineController.gameUpdateTime;
                }

            }



            return false;
        }


        private Tile GetTile(int nodeFromEnd)
        {
            if (drivingNodeList.Count >= nodeFromEnd)
            {
                int index = drivingNodeList.Count - nodeFromEnd;
                return WorldController.world.tileGrid[drivingNodeList[index].GetLocationX(), drivingNodeList[index].GetLocationY()];
            }

            return null;
        }

        private int GetDirection(int nodeFromEnd)
        {
            if (drivingNodeList.Count >= nodeFromEnd)
            {
                return drivingNodeList[drivingNodeList.Count - nodeFromEnd].GetDrivingDirection();
            }
            return -1;
        }


        private bool IsTileWithVehicleMovingSameDirection(Tile tile)
        {
            if (tile.vehicle.vehicleState == VehicleState.Moving)
            {
                if (vehicleParameters.directionTravelling == tile.vehicle.GetTravellingDirectionInt())
                {
                    vehicleParameters.followingVehicle = tile.vehicle;
                    return true;
                }
            }
            return false;
        }

        private bool IsTileEmptyFromVehicles(Tile tile)
        {
            if (tile != null)
            {
                return tile.IsTileEmptyFromVehicles(vehicleParameters.vehicle);
            }
            return true;
        }

        private bool IsStoplightBlock(Tile tile, int drivingDirection, out bool wasStopLight)
        {
            return tile.IsStopLightBlockDirection(drivingDirection, out wasStopLight);
        }




        public bool ReachedFinalWaypoint()
        {
            return drivingNodeList.Count < 1 && !waitingOnNewPathfinding;
        }

        public void ClearWayPointList()
        {
            if (drivingNodeList.Count > 0)
            {
                drivingNodeList.Clear();
            }
        }

        public Vector2 GetNextDestination(out bool incomingCornerOrStopLight)  
        {
            
            //Clear any previous destination
            // last one on the list
            DrivingNode next = drivingNodeList[drivingNodeList.Count - 1];


            location.AdvanceNode(next);
            drivingNodeList.RemoveAt(drivingNodeList.Count - 1);
            incomingCornerOrStopLight = CalcIncomingCornerOrParkedVehicle(1);
            vehicleParameters.directionTravelling = next.GetDrivingDirection();
            //If this is wrong we were meant to get the direction of the last node

            if (!incomingCornerOrStopLight)
            {
                incomingCornerOrStopLight = CalcIncomingCornerOrParkedVehicle(2);
            }


            //find next destination.  //Mark the map
            return next.GetLocationVector();
        }

        private bool CalcIncomingCornerOrParkedVehicle(int distance)
        {
            //This could also include stoplights etc
            if (drivingNodeList.Count - distance > 0)
            {
                DrivingNode next = drivingNodeList[drivingNodeList.Count - distance];

                Tile nextTile = GetTile(distance);

                //  bool isComing = next.useTurnOverride || nextTile.IsStopLightBlockDirection(vehicleParameters.directionTravelling, out bool none);

                bool isComing = next.GetDrivingDirection() != vehicleParameters.directionTravelling || nextTile.IsStopLightBlockDirection(vehicleParameters.directionTravelling, out bool none);

                if (isComing)
                {
                    return true;
                }
                else
                {
                    //We have a slow vehicle in front of us so we should slow down
                    Tile tile = WorldController.world.tileGrid[next.GetLocationX(), next.GetLocationY()];
                    isComing = tile.IsSlowVehicle();
                    return tile.IsSlowVehicle();

                }
            }

            if (drivingNodeList.Count == 1) //Last node
            {
                return true;
            }

            return false;
        }

        public void AskForNewPathfinding(Point destinationPoint)
        {
            //clear drivingNodeList
            drivingNodeList.Clear();

            if(pathfindingJob != null)
            {
                pathfindingJob.CancelJob();
            }

            waitingOnNewPathfinding = true;
            //if we are driving it will have to be the next destination tilex?
            pathfindingJob = new Pathfinding(new Point(location.tileX, location.tileY), destinationPoint, vehicleParameters.vehicle);
            GameController.AddJob(pathfindingJob);
            //DoJobworker
        }
            
        private void UpdateWaitingOnNewPathing()
        {
            if (pathfindingJob.completed)
            {
                drivingNodeList = pathfindingJob.drivingNodeList;
                pathfindingJob = null;
                waitingOnNewPathfinding = false;
            }
        }
        

    }
}
