using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Game.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Vehicles.Attachments
{
    //SO WE NEED TO MAKE
    //EACH VEHICLE HAVE THE SAME KIND OF PARAMETERS
    //LIKE EVERYTHING ELSE
    //AND ONLY STORE A LOCAL COPY OF WHAT THEY NEED
    //LIKE INDIVIDUAL PARAMETERS
    //SO WE NEED A VEHICLE FACTORY


    class EmergencyLights : Attachment
    {
        Light redLight;
        Light blueLight;

        float redLightLocalRotation;
        float blueLightLocalRotation;

        float redLightLocalDistance;
        float blueLightLocalDistance;

        float redLightSpriteRotation;
        float blueLightSpriteRotation;

        int timingIndex;

        public EmergencyLights(float distance, float angleDifference, LightType redLightType, LightType blueLightType, int timingIndex)
        {
            redLightLocalRotation = 0 - (angleDifference / 2);
            blueLightLocalRotation = 0 + (angleDifference / 2);

            redLightSpriteRotation = 0;
            blueLightSpriteRotation = 0;

            redLightLocalDistance = distance;
            blueLightLocalDistance = distance;
            this.redLight = GraphicsManager.GetLight(redLightType);
            this.blueLight = GraphicsManager.GetLight(blueLightType);
            this.timingIndex = timingIndex;
        }

        public override void DrawGameViewAttachment(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters, float transparency)
        {
            if (vehicleParameters.lightEmergencyOn)
            {
                if (vehicleParameters.isRedLightEmergency[timingIndex])
                {
                    Vector2 worldPosition = CalcWorldPosition(redLightLocalRotation, redLightSpriteRotation + location.direction, redLightLocalDistance, location.position);
                    redLight.DrawGameViewLightBulb(spriteBatch, worldPosition, location.direction, transparency);
                }
                if (vehicleParameters.isBlueLightEmergency[timingIndex])
                {
                    Vector2 worldPosition = CalcWorldPosition(blueLightLocalRotation, blueLightSpriteRotation + location.direction, blueLightLocalDistance, location.position);
                    blueLight.DrawGameViewLightBulb(spriteBatch, worldPosition, location.direction, transparency);
                }
            }
        }

        public override void DrawAttachment(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters)
        {
            //This is the lightbulb
            if (vehicleParameters.lightEmergencyOn)
            {
                if (vehicleParameters.isRedLightEmergency[timingIndex])
                {
                    Vector2 worldPosition = CalcWorldPosition(redLightLocalRotation, redLightSpriteRotation + location.direction, redLightLocalDistance, location.position);
                    redLight.DrawLightBulb(spriteBatch, worldPosition, location.direction);
                }
                if (vehicleParameters.isBlueLightEmergency[timingIndex])
                {
                    Vector2 worldPosition = CalcWorldPosition(blueLightLocalRotation, blueLightSpriteRotation + location.direction, blueLightLocalDistance, location.position);
                    blueLight.DrawLightBulb(spriteBatch, worldPosition, location.direction);
                }
            }
        }


        public override void DrawLighting(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters)
        {
            if (vehicleParameters.lightEmergencyOn)
            {
                if (vehicleParameters.isRedLightEmergency[timingIndex])
                {
                    Vector2 worldPosition = CalcWorldPosition(redLightLocalRotation, redLightSpriteRotation + location.direction, redLightLocalDistance, location.position);
                    redLight.DrawLighting(spriteBatch, worldPosition, location.direction + redLightLocalRotation);

                }
                if (vehicleParameters.isBlueLightEmergency[timingIndex])
                {
                    Vector2 worldPosition = CalcWorldPosition(blueLightLocalRotation, blueLightSpriteRotation + location.direction, blueLightLocalDistance, location.position);
                    blueLight.DrawLighting(spriteBatch, worldPosition, location.direction + blueLightLocalRotation);
                }
            }
        }

    }
}
