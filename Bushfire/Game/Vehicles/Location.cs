using BushFire.Engine;
using BushFire.Game.Storage;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Vehicles
{
    class Location
    {
        public Vector2 position;
        public float direction;
        public int tileX;
        public int tileY;
        public int backTileX;
        public int backTileY;

        private VehicleParameters vehicleParameters;


        public Location(int tileX, int tileY, VehicleParameters vehicleParameters, Sprite sprite)
        {
            this.vehicleParameters = vehicleParameters;
            this.tileX = tileX;
            this.tileY = tileY;
            TileLogistic tileLogistic = WorldController.world.tileGrid[tileX, tileY].tileLogistic;
            position = new Vector2((tileX * GroundLayerController.tileSize), (tileY * GroundLayerController.tileSize)) + tileLogistic.center;
            direction = tileLogistic.GetDefaultDirectionRadian();

            Point newPoint = AngleStuff.AddPointToDirection(new Point(tileX, tileY), tileLogistic.GetDefaultDirectionMirror());
            backTileX = newPoint.X;
            backTileY = newPoint.Y;

            WorldController.world.tileGrid[backTileX, backTileY].vehicle = vehicleParameters.vehicle;
            WorldController.world.tileGrid[tileX, tileY].vehicle = vehicleParameters.vehicle;


        }

        public void AdvanceNode(DrivingNode nextNode)
        {
            if (WorldController.world.tileGrid[backTileX, backTileY].vehicle == vehicleParameters.vehicle)
            {
                WorldController.world.tileGrid[backTileX, backTileY].vehicle = null;
            }

            backTileX = tileX;
            backTileY = tileY;
            tileX = nextNode.GetLocationX();
            tileY = nextNode.GetLocationY();
            WorldController.world.tileGrid[tileX, tileY].vehicle = vehicleParameters.vehicle;

            vehicleParameters.indicatorDirection = nextNode.indicatorDirection;
        }


        public void AddToTile()
        {

        }

        

        //Use this sparingly.  At the moment we are using it only for getting stuck in intersections trying to turn the same time as the opposite driver
        public void RemoveTailFromTile()
        {
            if (WorldController.world.tileGrid[backTileX, backTileY].vehicle == vehicleParameters.vehicle)
            {             
                WorldController.world.tileGrid[backTileX, backTileY].vehicle = null;
            }
        }

        
    }
}
