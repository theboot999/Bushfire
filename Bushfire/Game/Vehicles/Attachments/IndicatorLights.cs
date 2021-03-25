using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Vehicles.Attachments
{
    class IndicatorLights : Attachment
    {
        Light leftLight;
        Light rightLight;

        float leftLightLocalRotation;
        float RightLightLocalRotation;

        float LeftLightLocalDistance;
        float RightLightLocalDistance;

        float LeftLightSpriteRotation;
        float RightLightSpriteRotation;


        public IndicatorLights(float distance, float angleDifference, LightType leftIndicator, LightType rightIndicator)
        {
            leftLightLocalRotation = 0 - (angleDifference / 2);
            RightLightLocalRotation = 0 + (angleDifference / 2);

            LeftLightSpriteRotation = 0;
            RightLightSpriteRotation = 0;

            LeftLightLocalDistance = distance;
            RightLightLocalDistance = distance;
            this.leftLight = GraphicsManager.GetLight(leftIndicator);
            this.rightLight = GraphicsManager.GetLight(rightIndicator);
        }

        public override void DrawGameViewAttachment(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters, float transparency)
        {
            //This is the lightbulb
            if (vehicleParameters.indicatorDirection == Direction.LEFT && vehicleParameters.isIndicatorFlash)
            {
                Vector2 worldPosition = CalcWorldPosition(leftLightLocalRotation, LeftLightSpriteRotation + location.direction, LeftLightLocalDistance, location.position);
                leftLight.DrawGameViewLightBulb(spriteBatch, worldPosition, location.direction, transparency);
            }
            else if (vehicleParameters.indicatorDirection == Direction.RIGHT && vehicleParameters.isIndicatorFlash)
            {
                Vector2 worldPosition = CalcWorldPosition(RightLightLocalRotation, RightLightSpriteRotation + location.direction, RightLightLocalDistance, location.position);
                rightLight.DrawGameViewLightBulb(spriteBatch, worldPosition, location.direction, transparency);
            }
        }

        public override void DrawAttachment(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters)
        {
            //This is the lightbulb
            if (vehicleParameters.indicatorDirection == Direction.LEFT && vehicleParameters.isIndicatorFlash)
            {
                Vector2 worldPosition = CalcWorldPosition(leftLightLocalRotation, LeftLightSpriteRotation + location.direction, LeftLightLocalDistance, location.position);
                leftLight.DrawLightBulb(spriteBatch, worldPosition, location.direction);
            }
            else if (vehicleParameters.indicatorDirection == Direction.RIGHT && vehicleParameters.isIndicatorFlash)
            {
                Vector2 worldPosition = CalcWorldPosition(RightLightLocalRotation, RightLightSpriteRotation + location.direction, RightLightLocalDistance, location.position);
                rightLight.DrawLightBulb(spriteBatch, worldPosition, location.direction);
            }
        }


        public override void DrawLighting(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters)
        {
            if (vehicleParameters.indicatorDirection == Direction.LEFT && vehicleParameters.isIndicatorFlash)
            {
                Vector2 worldPosition = CalcWorldPosition(leftLightLocalRotation, LeftLightSpriteRotation + location.direction, LeftLightLocalDistance, location.position);
                leftLight.DrawLighting(spriteBatch, worldPosition, location.direction);

            }
            else if (vehicleParameters.indicatorDirection == Direction.RIGHT && vehicleParameters.isIndicatorFlash)
            {
                Vector2 worldPosition = CalcWorldPosition(RightLightLocalRotation, RightLightSpriteRotation + location.direction, RightLightLocalDistance, location.position);
                rightLight.DrawLighting(spriteBatch, worldPosition, location.direction);
            }

        }
    }
}
