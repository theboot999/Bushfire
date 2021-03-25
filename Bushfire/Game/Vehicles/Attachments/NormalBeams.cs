using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Game.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Vehicles.Attachments
{
    class NormalBeams : Attachment
    {

        Light lights;
        float oneLocalRotation;
        float twoLocalRotation;
        float localDistance;
        float lightsSpriteRotation;

        public NormalBeams(float distance, float angleDifference, LightType lightType)
        {
            localDistance = distance;
            oneLocalRotation = 0 - (angleDifference / 2);
            twoLocalRotation = 0 + (angleDifference / 2);
            lightsSpriteRotation = 0;
            lights = GraphicsManager.GetLight(lightType);
        }

        public override void DrawGameViewAttachment(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters, float transparency)
        {
            //This is the lightbulb
            if (vehicleParameters.lightGeneralOn)
            {
                Vector2 worldPosition = CalcWorldPosition(oneLocalRotation, lightsSpriteRotation + location.direction, localDistance, location.position);
                lights.DrawGameViewLightBulb(spriteBatch, worldPosition, location.direction, transparency);

                worldPosition = CalcWorldPosition(twoLocalRotation, lightsSpriteRotation + location.direction, localDistance, location.position);
                lights.DrawGameViewLightBulb(spriteBatch, worldPosition, location.direction, transparency);
            }
        }

        public override void DrawAttachment(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters)
        {
            //This is the lightbulb
            if (vehicleParameters.lightGeneralOn)
            {
                Vector2 worldPosition = CalcWorldPosition(oneLocalRotation, lightsSpriteRotation + location.direction, localDistance, location.position);
                lights.DrawLightBulb(spriteBatch, worldPosition, location.direction);

                worldPosition = CalcWorldPosition(twoLocalRotation, lightsSpriteRotation + location.direction, localDistance, location.position);
                lights.DrawLightBulb(spriteBatch, worldPosition, location.direction);
            }
        }


        public override void DrawLighting(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters)
        {
            if (vehicleParameters.lightGeneralOn)
            {
                Vector2 worldPosition = CalcWorldPosition(oneLocalRotation, lightsSpriteRotation + location.direction, localDistance, location.position);
                lights.DrawLighting(spriteBatch, worldPosition, location.direction);

                worldPosition = CalcWorldPosition(twoLocalRotation, lightsSpriteRotation + location.direction, localDistance, location.position);
                lights.DrawLighting(spriteBatch, worldPosition, location.direction);
            }
        }


    }
}
