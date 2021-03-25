using BushFire.Editor.Tech;
using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Game.Map.MapObjects;
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
    class StreetLight : MapObjectPropertiesSingle
    {

        Light light;
        Vector2 localLightVector;
        float lightRotation;
        bool globeAlwaysOn;

        public StreetLight(Piece piece, Light light, Vector2 localLightVector, float lightRotation, Shadow shadowLeft, Shadow shadowRight, Point shadowPoint, bool globeAlwaysOn, int elevation) : base(elevation, MapObjectType.STREETLIGHT, true, 0)
        {          
            this.piece = piece;
            this.light = light;
            this.localLightVector = localLightVector;
            this.lightRotation = lightRotation;       
            this.shadowLeft = shadowLeft;
            this.shadowRight = shadowRight;
          //  this.shadowPoint = shadowPoint;
            this.globeAlwaysOn = globeAlwaysOn;
            this.elevation = elevation;
        }

        public override void DrawGameViewBoxObject(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, Color color, float scale, float transparency)
        {
            Vector2 location = new Vector2(tileX * GroundLayerController.tileSize + GroundLayerController.halfTileSize + smallOffset.X, tileY * GroundLayerController.tileSize + GroundLayerController.halfTileSize + smallOffset.Y);
            piece.DrawGameViewBox(spriteBatch, location, color, scale, transparency);

            if (globeAlwaysOn || WorldController.lightsOn)
            {
                light.DrawGameViewLightBulb(spriteBatch, new Vector2((tileX) * GroundLayerController.tileSize, (tileY) * GroundLayerController.tileSize) + localLightVector, lightRotation, transparency);
            }
        }

        public override void DrawObject(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, Color color, float scale)
        {
            base.DrawObject(spriteBatch, tileX, tileY, smallOffset, color, scale);

            if (globeAlwaysOn || WorldController.lightsOn)
            {
                light.DrawLightBulb(spriteBatch, new Vector2((tileX) * GroundLayerController.tileSize, (tileY) * GroundLayerController.tileSize) + localLightVector, lightRotation);
            }
        }



        public override void DrawLighting(SpriteBatch spriteBatch, int tileX, int tileY, Vector2 smallOffset, float scale)
        {
            if (globeAlwaysOn || WorldController.lightsOn)
            {
                light.DrawLighting(spriteBatch, new Vector2((tileX) * GroundLayerController.tileSize, (tileY) * GroundLayerController.tileSize) + localLightVector, lightRotation);
            }
        }

  
    }
}
