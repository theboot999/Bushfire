using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Tech
{
    class TileLogistic
    {
        //If were not bypassing for turn we need a special turning vector
        //lets just center it now

        public Vector2 center;
        public Vector2 turnOverrideAmount;
        public bool isRoad;
        public int roadId;
        public int defaultDirection;
        public int directionBlockOne;
        public int directionBlockTwo;
        public int pathFindingScore;
        public LandType landType;

        public TileLogistic(LandType landType, Vector2 center, Vector2 turnOverrideAmount, bool isRoad, int roadId, int directionBlockOne, int directionBlockTwo, int pathFindingScore, int defaultDirection)
        {
            this.center = center;
            this.isRoad = isRoad;
            this.roadId = roadId;
            this.directionBlockOne = directionBlockOne;
            this.directionBlockTwo = directionBlockTwo;
            this.turnOverrideAmount = turnOverrideAmount;
            this.pathFindingScore = pathFindingScore;
            this.landType = landType;
            this.defaultDirection = defaultDirection;
        }

        public bool IsDirectionBlock(int direction)
        {
            return direction == directionBlockOne || direction == directionBlockTwo;
        }

        public bool IsRoad()
        {
            return landType == LandType.CITYROAD || landType == LandType.COUNTRYROAD;
        }

        public bool IsOpen()
        {
            return landType == LandType.OPEN;
        }

        public float GetDefaultDirectionRadian()
        {
            return AngleStuff.DirectionToRadian(defaultDirection);
        }

        public int GetDefaultDirectionMirror()
        {
            return AngleStuff.RotateDirection(defaultDirection, 4);
        }
    }
}
