using BushFire.Engine;
using BushFire.Game.Vehicles.Attachments;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Vehicles
{
    class VehicleSpecific
    {
        //Note: the faster acceleration the tighter turns it needs to be able to do
        //Or the slower tight turn speed
        //So it doesnt get stuck going in circles

        Dictionary<AttachmentType, Attachment> attachmentList;
        public Sprite sprite;
        Sprite selectedSprite;
        Vector2 selectionScale;
        string name;
        float scale;
        public Rectangle size;


        //These are percentages
        public readonly float tightTurnSpeedPercent = 0.12f;
        public readonly float midTurnSpeedPercent = 0.2f;
        //public readonly float wideTurnSlowPercent = 0.5f;
        public readonly float topSpeedPercent = 1.5f;

        public float acceleration = 0.005f;

        //These are multipled by gametime
        public readonly float tightTurnRotation = 0.03f;
        public readonly float midTurnRotation = 0.02f;
        public readonly float wideTurnRotationGoingSlow = 0.01f;
        public readonly float wideTurnRotationAtSpeed = 0.003f;


        public VehicleSpecific(Dictionary<AttachmentType, Attachment> attachmentList, float scale, Sprite sprite, Sprite selectedSprite, string name, float topSpeed, float acceleration)
        {
            this.acceleration = acceleration;
            this.topSpeedPercent = topSpeed;
            this.attachmentList = attachmentList;
            this.scale = scale;
            this.sprite = sprite;
            this.selectedSprite = selectedSprite;
            this.name = name;
            size = new Rectangle(0, 0, (int)((float)sprite.location.Width * scale), (int)((float)sprite.location.Height * scale));
            selectionScale = new Vector2((float)size.Width / (float)selectedSprite.location.Width, (float)size.Height / (float)selectedSprite.location.Height) * 1.2f;
            
        }

        public void DrawGameViewBox(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters, float transparency)
        {
            spriteBatch.Draw(sprite.texture2D, location.position, sprite.location, sprite.color * transparency, location.direction, sprite.rotationCenter, scale, SpriteEffects.None, 0);

            foreach (Attachment attachment in attachmentList.Values)
            {
                attachment.DrawGameViewAttachment(spriteBatch, location, vehicleParameters, transparency);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters)
        {
            spriteBatch.Draw(sprite.texture2D, location.position, sprite.location, sprite.color, location.direction, sprite.rotationCenter, scale, SpriteEffects.None, 0);

            foreach (Attachment attachment in attachmentList.Values)
            {
                attachment.DrawAttachment(spriteBatch, location, vehicleParameters);
            }
        }

        public void DrawLighting(SpriteBatch spriteBatch, Location location, VehicleParameters vehicleParameters)
        {
            foreach (Attachment attachment in attachmentList.Values)
            {
                attachment.DrawLighting(spriteBatch, location, vehicleParameters);
            }
        }

        public void DrawSelected(SpriteBatch spriteBatch, Location location)
        {
            spriteBatch.Draw(selectedSprite.texture2D, location.position, selectedSprite.location, Color.White * 0.8f, location.direction, selectedSprite.rotationCenter, selectionScale, SpriteEffects.None, 0);
 
        }

        public void DrawDragging(SpriteBatch spriteBatch, Location location)
        {
            spriteBatch.Draw(selectedSprite.texture2D, location.position, selectedSprite.location, Color.Gray * 0.4f, location.direction, selectedSprite.rotationCenter, selectionScale, SpriteEffects.None, 0);
        }   

    }
}
