using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Game.Controllers;
using BushFire.Game.Storage;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Vehicles
{
    //CANT ADD THE ACTIONS AS FAST AS WE WANT
    //WE DONT HAVE A DESTINATION YET
    //SO ACTIONS NEED AN, ACTION.STARET METHOD

    enum MovementState
    {
        Stopped,
        Straight,
        turning,
        Stopping,
    }

    class MovementAction : VAction
    {      
        
        MovementState movementState;
        bool waitingOnNextWayPoint;
        WaypointControl wayPointControl;


        public Vector2 destination; //Temporary until we get this working

        //these are our currents

        private float currentMaxSpeedPercent;
        private bool runningSlow;



        VehicleParameters vehicleParameters;
        VehicleSpecific vehicleSpecific;
        

        private Location location;

        public MovementAction(Location location, Point destinationPoint, VehicleParameters vehicleParameters, VehicleSpecific vehicleSpecific)
        {
            vehicleState = VehicleState.Moving;

            this.destinationPoint = destinationPoint;
            wayPointControl = new WaypointControl(location, vehicleParameters);
            this.vehicleParameters = vehicleParameters;
            this.vehicleSpecific = vehicleSpecific;
            this.location = location;

        }

        public override void StartAction()
        {
            waitingOnNextWayPoint = true;
            movementState = MovementState.Stopped;
            actionState = ActionState.InProgress;
            wayPointControl.AskForNewPathfinding(destinationPoint);
        }

        public override void CancelAction()
        {
            wayPointControl.ClearWayPointList();
            ignoreDraw = true;
            //Wait to next waypoint is done.
            //we can do that simply by clearing the list

        }

        public override void Update()
        {
            if (actionState == ActionState.InProgress)
            {
                if (waitingOnNextWayPoint)
                {
                    UpdateNextWayPoint();
                }

                if (movementState == MovementState.Straight || movementState == MovementState.turning)
                {
                    UpdateTurningAndSpeed();
                    UpdateFollowingVehicleSpeed();
                    UpdateMoving();
                    UpdateNearDestination();
                }
                else if (movementState == MovementState.Stopping)
                {
                    vehicleParameters.isBraking = true;
                    UpdateStopping();
                }
                else if (movementState == MovementState.Stopped)
                {
                    vehicleParameters.isBraking = true;
                    vehicleParameters.currentSpeedPercentage = 0; //Final check to fix any reversing slightly
                }
            }
        }

        private void UpdateNextWayPoint()
        {
            if (wayPointControl.ReadyNextWayPoint())
            {
                destination = wayPointControl.GetNextDestination(out runningSlow);
                movementState = MovementState.Straight;
                waitingOnNextWayPoint = false;
            }
            else
            {
                if (movementState != MovementState.Stopped)
                {
                    movementState = MovementState.Stopping;
                }
            }

            if (wayPointControl.ReachedFinalWaypoint() && movementState == MovementState.Stopped)
            {
                actionState = ActionState.Finished;
            }
        }

        private void UpdateTurningAndSpeed()
        {
            float m = ClampDirection(ClampDirection((float)Math.Atan2(location.position.Y - destination.Y, location.position.X - destination.X)) - location.direction);
            //Turning Left
            if (m < 3.12f)
            {
                movementState = MovementState.turning;
                float amount = 3.08f - m;
                if (amount > 0.105)
                {
                //    Debug.WriteLine("TightTurn" + GameController.rnd.Next(0, 100));
                    location.direction -= (vehicleSpecific.tightTurnRotation * EngineController.gameUpdateTime);
                    currentMaxSpeedPercent = vehicleSpecific.tightTurnSpeedPercent;

                }
                else if (amount > 0.5)
                {
               //     Debug.WriteLine("MidTurn" + GameController.rnd.Next(0, 100));
                    location.direction -= (vehicleSpecific.midTurnRotation * EngineController.gameUpdateTime);
                    currentMaxSpeedPercent = vehicleSpecific.midTurnSpeedPercent;
                }
                else
                {
                    if (vehicleParameters.currentSpeedPercentage > vehicleSpecific.topSpeedPercent * 0.5f)
                    {
                        //We are at a decent speed
                 //       Debug.WriteLine("WideTurn AT SPEED" + GameController.rnd.Next(0, 100));
                        location.direction -= (vehicleSpecific.wideTurnRotationAtSpeed * EngineController.gameUpdateTime);
                        currentMaxSpeedPercent = vehicleSpecific.topSpeedPercent;
                    }
                    else
                    {
                        //Were doing a slow speed
                    //    Debug.WriteLine("WideTurn SLOW" + GameController.rnd.Next(0, 100));
                        location.direction -= (vehicleSpecific.wideTurnRotationGoingSlow * EngineController.gameUpdateTime);
                        currentMaxSpeedPercent = vehicleSpecific.topSpeedPercent;
                    }
                }
                location.direction = ClampDirection(location.direction);
            }
            //Turning Right
            else if (m > 3.16f)
            {
                movementState = MovementState.turning;
                float amount = m - 3.08f;
                if (amount > 1)
                {
                 //   Debug.WriteLine("TightTurn" + GameController.rnd.Next(0, 100));
                    location.direction += (vehicleSpecific.tightTurnRotation * EngineController.gameUpdateTime);
                    currentMaxSpeedPercent = vehicleSpecific.tightTurnSpeedPercent;
                }
                else if (amount > 0.5)
                {
                //    Debug.WriteLine("MidTurn" + GameController.rnd.Next(0, 100));
                    location.direction += (vehicleSpecific.midTurnRotation * EngineController.gameUpdateTime);
                    currentMaxSpeedPercent = vehicleSpecific.midTurnSpeedPercent;
                }
                else
                {
                    if (vehicleParameters.currentSpeedPercentage > vehicleSpecific.topSpeedPercent * 0.5f)
                    {
                        //We are at a decent speed
                   //     Debug.WriteLine("WideTurn AT SPEED" + GameController.rnd.Next(0, 100));
                        location.direction += (vehicleSpecific.wideTurnRotationAtSpeed * EngineController.gameUpdateTime);
                        currentMaxSpeedPercent = vehicleSpecific.topSpeedPercent;
                    }
                    else
                    {
                  //      Debug.WriteLine("WideTurn SLOW" + GameController.rnd.Next(0, 100));
                        location.direction += (vehicleSpecific.wideTurnRotationGoingSlow * EngineController.gameUpdateTime);
                        currentMaxSpeedPercent = vehicleSpecific.topSpeedPercent;
                    }
                }
                location.direction = ClampDirection(location.direction);
            }
            else
            {
                //straight no turn
                movementState = MovementState.Straight;
                if (runningSlow)
                {
                 //   Debug.WriteLine("RunningSlow" + GameController.rnd.Next(0, 100));
                    currentMaxSpeedPercent = vehicleSpecific.topSpeedPercent * 0.4f;
                }
                else
                {
                    currentMaxSpeedPercent = vehicleSpecific.topSpeedPercent;
                }
            }
        }

        
        private void UpdateFollowingVehicleSpeed()
        {
            if (vehicleParameters.followingVehicle != null)
            {
                float otherVehicleSpeed = vehicleParameters.followingVehicle.GetCurrentSpeedPercent();
            
                if (currentMaxSpeedPercent > otherVehicleSpeed)
                {
                    currentMaxSpeedPercent = otherVehicleSpeed;
                }

                if (currentMaxSpeedPercent < 0.1f)
                {
                    currentMaxSpeedPercent = 1f;
                }
             
            }
        }



        private void UpdateMoving()
        {
            if (vehicleParameters.currentSpeedPercentage < currentMaxSpeedPercent)
            {
                vehicleParameters.currentSpeedPercentage += vehicleSpecific.acceleration * EngineController.gameUpdateTime;
                vehicleParameters.isBraking = false;
            }
            else if (vehicleParameters.currentSpeedPercentage - currentMaxSpeedPercent < 0.002f)
            {
                vehicleParameters.currentSpeedPercentage = currentMaxSpeedPercent;
                vehicleParameters.isBraking = false;
            }
            else
            {
                vehicleParameters.currentSpeedPercentage -= CalcBrakeSpeed() * EngineController.gameUpdateTime;

                vehicleParameters.isBraking = true;

            }

            float newSpeed = WorldController.topVehicleSpeed * vehicleParameters.currentSpeedPercentage;
            Vector2 moveAmount = RadianToVector(location.direction) * newSpeed;
            location.position += moveAmount;
        }

        private float CalcBrakeSpeed()
        {
            return MathHelper.Lerp(0.02f, 0.08f, vehicleParameters.currentSpeedPercentage / vehicleSpecific.topSpeedPercent);
           // return 0.03f;
        }

        private void UpdateNearDestination()
        {
            if (Vector2.Distance(location.position, destination) < 25f)
            {
                waitingOnNextWayPoint = true;
            }
        }

        private void UpdateStopping()
        {
            vehicleParameters.currentSpeedPercentage -= 0.08f * EngineController.gameUpdateTime;
            float newSpeed = WorldController.topVehicleSpeed * vehicleParameters.currentSpeedPercentage;
            Vector2 moveAmount = RadianToVector(location.direction) * newSpeed;
            location.position += moveAmount;

            if (vehicleParameters.currentSpeedPercentage < 0)
            {
                vehicleParameters.currentSpeedPercentage = 0;
                movementState = MovementState.Stopped;
            }
        }

        private Vector2 RadianToVector(float value)
        {
            Vector2 temp = new Vector2((float)Math.Cos(value), (float)Math.Sin(value));
            temp.Normalize();
            return temp;
        }

        private float ClampDirection(float direction)
        {
            if (direction < 0)
            {
                direction += 6.28f;
            }
            else if (direction > 6.28)
            {
                direction -= 6.28f;

            }
            return direction;
        }
    }
}
