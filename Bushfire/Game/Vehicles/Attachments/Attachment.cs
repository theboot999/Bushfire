using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Vehicles.Attachments
{
    enum AttachmentType
    {
        Headlight,
        TailLight,
        BrakeLight,
        Indicator1,
        Indicator2,
        EmergencyLight1,
        EmergencyLight2,
        EmergencyLight3,
        EmergencyLight4
    }

    class Attachment
    {
        public virtual void Update(Vector2 location, float rotation, VehicleParameters vehicleParameters)
        {

        }

        protected Vector2 CalcWorldPosition(float localRotation, float rotation, float localDistance, Vector2 location)
        {
            Vector2 temp = new Vector2((float)Math.Cos(localRotation + rotation), (float)Math.Sin(localRotation + rotation));
            temp.Normalize();
            return location + (temp * localDistance);
        }

        public virtual void DrawGameViewAttachment(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters, float transparency)
        {

        }

        public virtual void DrawAttachment(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters)
        {
         
        }
        public virtual void DrawShadows(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters)
        {

        }

        public virtual void DrawVisibleBlock(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters)
        {

        }

        public virtual void DrawLighting(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters)
        {

        }
    }
}
