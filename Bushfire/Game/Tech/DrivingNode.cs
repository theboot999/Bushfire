using BushFire.Game.Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Tech
{
    //Class has been heavily optomised for performance and minimal memory usage
    //Using a struct as memory usage is heaps less
    //If modifying any of the nodes in a list need to make sure we assign a new node to it
    //Not modify directly or end up with an empty struct

    struct DrivingNode
    {
        private int location;            
        public Direction indicatorDirection; 
        public byte turnOverRideDirection;
        private byte drivingDirection;

        public DrivingNode(int locationX, int locationY)
        {
            drivingDirection = 255;
            location = (locationY * WorldController.world.worldWidth) + locationX;
            turnOverRideDirection = 255; //We dont use it
            indicatorDirection = Direction.NONE;
        }
      
        public void SetDrivingDirection(byte drivingDirection)
        {
            this.drivingDirection = drivingDirection;
        }

        public void SetIndicatorDirection(Direction indicatorDirection)
        {
            this.indicatorDirection = indicatorDirection;
        }

        public int GetDrivingDirection()
        {
            return (int)drivingDirection;
        }

        public int GetLocationX()
        {
            return location % WorldController.world.worldWidth;
        }

        public int GetLocationY()
        {
            return location / WorldController.world.worldWidth;
        }

        public void SetUseByPassTurn(int directionOne, int directionTwo)
        {
            if (directionOne == 2 && directionTwo == 4 || directionOne == 0 && directionTwo == 6) //Bottom Left Corner
            {
                turnOverRideDirection = 5;
            }
            else if (directionOne == 2 && directionTwo == 0 || directionOne == 4 && directionTwo == 6) //Top Left Corner
            {
                turnOverRideDirection = 7;
            }
            else if (directionOne == 4 && directionTwo == 2 || directionOne == 6 && directionTwo == 0) //Top Right Corner
            {
                turnOverRideDirection = 1;
            }
            else if (directionOne == 0 && directionTwo == 2 || directionOne == 6 && directionTwo == 4) //Bottom Right Corner
            {
                turnOverRideDirection = 3;
            }
        }

        public Vector2 GetLocationVector()
        {
            int x = GetLocationX();
            int y = GetLocationY();

            TileLogistic tileLogistic = WorldController.world.tileGrid[x, y].tileLogistic;

            if (turnOverRideDirection == 255)
            {
                return new Vector2(x * 128, y * 128) + tileLogistic.center;
            }

            else if (turnOverRideDirection == 5)
            {
                return new Vector2((x * 128) + tileLogistic.center.X - tileLogistic.turnOverrideAmount.X, (y * 128) + tileLogistic.center.Y + tileLogistic.turnOverrideAmount.Y);
            }
            else if (turnOverRideDirection == 7)
            {
                return new Vector2((x * 128) + tileLogistic.center.X - tileLogistic.turnOverrideAmount.X, (y * 128) + tileLogistic.center.Y - tileLogistic.turnOverrideAmount.Y);
            }
            else if (turnOverRideDirection == 1)
            {
                return new Vector2((x * 128) + tileLogistic.center.X + tileLogistic.turnOverrideAmount.X, (y * 128) + tileLogistic.center.Y - tileLogistic.turnOverrideAmount.Y);
            }
            else if (turnOverRideDirection == 3)
            {
                return new Vector2((x * 128) + tileLogistic.center.X + tileLogistic.turnOverrideAmount.X, (y * 128) + tileLogistic.center.Y + tileLogistic.turnOverrideAmount.Y);
            }
            return Vector2.Zero;
        }

        public bool isRoad()
        {
            return WorldController.world.tileGrid[GetLocationX(), GetLocationY()].tileLogistic.isRoad;
        }

    }
}
