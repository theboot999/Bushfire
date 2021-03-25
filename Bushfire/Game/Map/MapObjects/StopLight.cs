using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Game.Controllers;
using BushFire.Game.Map.MapObjects;
using BushFire.Game.MapObjects;
using BushFire.Game.Storage;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map.MapObjectComponents
{
    class StopLight : MapObjectPropertiesSingle
    {
        //TODO: So we also should make the 4th tile on an intersection thats only 3 way to link to an empty stoplight.  this means we can click on it to advance the intersection
        //If stage is more than 0, it means its a block
        //Vehicle Counter only counts vehicles that were going with the flow of the traffic light.  To measure traffic flow

        private StreetLight streetLight;
        private int stage;
        public int blockDirection;
        private Intersection controllingIntersection;
        int spriteId;
        public int vehicleCounter { get; private set; }

        public StopLight(int blockDirection, int stage, Intersection controllingIntersection, int spriteId) : base(2, MapObjectType.STOPLIGHT, true, 0)
        {
            this.controllingIntersection = controllingIntersection;
            this.blockDirection = blockDirection;
            this.stage = stage;
            this.spriteId = spriteId;
            SetLight();
        }

        public Intersection GetControllingIntersection()
        {
            return controllingIntersection;
        }

        public void Advance()
        {
            stage++;

            if (stage > 3)
            {
                stage = 0;
            }
      
            SetLight();
        }

        public bool IsBlocked()
        {
            return stage > 0;
        }

        private void SetLight()
        {
            if (stage == 0)
            {
                streetLight = MapObjectController.GetStreetLight(StreetLightType.STOPLIGHTGREEN, spriteId);
            }
            else if (stage == 1)
            {
                streetLight = MapObjectController.GetStreetLight(StreetLightType.STOPLIGHTAMBER, spriteId);
            }
            else
            {
                streetLight = MapObjectController.GetStreetLight(StreetLightType.STOPLIGHTRED, spriteId);
            }
        }

        public void IncreaseVehicleCounter()
        {
            vehicleCounter++;
        }

        public override Piece GetPiece()
        {
            return streetLight.GetPiece();
        }

        public override void DrawGameViewBoxObject(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, Color color, float scale, float transparency)
        {        
            streetLight.DrawGameViewBoxObject(spriteBatch, tileX, tileY, smallOffset, color, scale, transparency);
        }

        public override void DrawObject(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, Color color, float scale)
        {
            streetLight.DrawObject(spriteBatch, tileX, tileY, smallOffset, color, scale);
        }

        public override void DrawShadows(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, float scale)
        {
            streetLight.DrawShadows(spriteBatch, tileX, tileY, smallOffset, scale);
        }

        public override void DrawVisibleBlock(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, float scale)
        {
            streetLight.DrawVisibleBlock(spriteBatch, tileX, tileY, smallOffset, scale);
        }

        public override void DrawLighting(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, float scale)
        {
            streetLight.DrawLighting(spriteBatch, tileX, tileY, smallOffset, scale);
        }





        //stoplights are drawn elevation 2
    }
}
