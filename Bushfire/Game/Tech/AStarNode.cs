using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Tech
{
    struct AStarNode
    {
        public int currentPoint;
        public int previousPoint;
        public int nScore;
        public int hScore;
        public byte directionFrom;
        public bool isRoad;

        public AStarNode(int currentPoint, int previousPoint, int nScore, int hScore, byte directionFrom, bool isRoad)
        {
            this.currentPoint = currentPoint;
            this.previousPoint = previousPoint;
            this.nScore = nScore;
            this.hScore = hScore;
            this.directionFrom = directionFrom;
            this.isRoad = isRoad;
        }

        public int GetTotalScore()
        {
            return nScore + hScore;
        }

        public Point GetCurrent2D()
        {
            return new Point(currentPoint % WorldController.world.worldWidth, currentPoint / WorldController.world.worldWidth);
        }

        public Point GetPrevious2D()
        {
            return new Point(previousPoint % WorldController.world.worldWidth, previousPoint / WorldController.world.worldWidth);
        }

        public int GetMirrorDirection()
        {
            return AngleStuff.RotateDirection(directionFrom, 4);
        }
    }
}
