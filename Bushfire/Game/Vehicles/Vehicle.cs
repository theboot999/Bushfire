using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Game.Controllers;
using BushFire.Game.Map;
using BushFire.Game.Map.UI;
using BushFire.Game.Storage;
using BushFire.Game.Tech.Jobs;
using BushFire.Game.Vehicles;
using BushFire.Game.Vehicles.Attachments;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Tech
{
 
    //I dont even know if we have these?
    enum VehicleState
    {
        Chilling,
        Moving,
        FillingWater,
        SprayingWater,
        ChillingAtHome
    }

    class Vehicle
    {
        public Location location;
        private List<VAction> actionList;
        VehicleSpecific vehicleSpecific;
        SelectedMovingLine selectedMovingLine;
        public VehicleParameters vehicleParameters { get; private set; }
        public VehicleState vehicleState { get; private set; }
        public bool drawUpdates;

        public Vehicle(VehicleSpecific vehicleSpecific, int tileX, int tileY, bool hasEmergencyLights, int debugNum)
        {
            vehicleState = VehicleState.Chilling;
            this.vehicleSpecific = vehicleSpecific;

            vehicleParameters = new VehicleParameters(hasEmergencyLights, this);
            actionList = new List<VAction>();
            location = new Location(tileX, tileY, vehicleParameters, vehicleSpecific.sprite);
            selectedMovingLine = new SelectedMovingLine(location);
            vehicleParameters.debugNum = debugNum;
        
        }
     
        public string GetIdString()
        {
            return "Vehicle" + vehicleParameters.debugNum.ToString();
        }

        public float GetCurrentSpeedPercent()
        {
            return vehicleParameters.currentSpeedPercentage;
        }

        public Vector2 GetMiniMapPosition()
        {
            return location.position / 64;
           // return new Vector2(location.tileX * 2, location.tileY * 2);
        }

        public Vector2 GetPosition()
        {
            return location.position;
        }

        public Point GetTilePosition()
        {
            return new Point(location.tileX, location.tileY);
        }

        public Rectangle GetSize()
        {
            return vehicleSpecific.size;
        }

        public float GetDirectionRadian()
        {
            return location.direction;
        }

        public int GetTravellingDirectionInt()
        {
            return vehicleParameters.directionTravelling;
        }

        private void AddAction(VAction action, bool clearAll)
        {
            if (clearAll)
            {
                for (int i = actionList.Count - 1; i > 0; i--)
                {
                    actionList.RemoveAt(i);
                }
                if (actionList.Count > 0)
                {
                    actionList[0].CancelAction();  //Just cancel the last one
                }
                actionList.Add(action);
            }
            else
            {
                actionList.Add(action);
            }
        }

        public void NewMoveAction(Input input, Point tile)
        {
            bool addWayPointHold = !input.IsKeyMapDown(KeyMap.AddWayPointHold);
            MovementAction move = new MovementAction(location, tile, vehicleParameters, vehicleSpecific);
            AddAction(move, addWayPointHold);
        }

        public void NewMoveAction(Point tile, bool addWayPointHold)
        {
            MovementAction move = new MovementAction(location, tile, vehicleParameters, vehicleSpecific);
            AddAction(move, addWayPointHold);
        }



        public void Update()
        {
            drawUpdates = false;

            if (actionList.Count > 0)
            {
                if (actionList[0].actionState == ActionState.None)
                {
                    actionList[0].StartAction();
                    vehicleState = actionList[0].vehicleState;
                }

                else if (actionList[0].actionState == ActionState.InProgress)
                {
                    actionList[0].Update();
                }
                else if (actionList[0].actionState == ActionState.Finished)
                {
                    actionList.RemoveAt(0);
                    vehicleState = VehicleState.Chilling;
                }
            }
        }

        public void UpdateIfSelected(Input input)
        {

            if (input.RightButtonClick() && WorldController.mouseInWorldFocus)
            {
                NewMoveAction(input, WorldController.mouseTileHover);
            }

            if (input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.O))
            {
                vehicleParameters.lightEmergencyOn = !vehicleParameters.lightEmergencyOn;
            }
            if (input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.P))
            {
                vehicleParameters.lightGeneralOn = !vehicleParameters.lightGeneralOn;
            }
            if (input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.L))
            {
                vehicleParameters.lightBrakingOn = !vehicleParameters.lightBrakingOn;
            }
     
        }


        public void DrawGameViewBox(SpriteBatch spriteBatch, float transparency)
        {
            //Only update the emergency lighting if we are drawing the vehicle

            vehicleSpecific.DrawGameViewBox(spriteBatch, location, vehicleParameters, transparency);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Only update the emergency lighting if we are drawing the vehicle
            if (!drawUpdates)
            {
                drawUpdates = true;
                vehicleParameters.UpdateDraw();
            }

            vehicleSpecific.Draw(spriteBatch, location, vehicleParameters);
        }

        public void DrawLighting(SpriteBatch spriteBatch)
        {
            vehicleSpecific.DrawLighting(spriteBatch, location, vehicleParameters);
        }

        public void DrawSelected(SpriteBatch spriteBatch)
        {
            vehicleSpecific.DrawSelected(spriteBatch, location);
            selectedMovingLine.Draw(spriteBatch, actionList);          
        }

        public void DrawDragging(SpriteBatch spriteBatch)
        {
            vehicleSpecific.DrawDragging(spriteBatch, location);
        }


    }
}