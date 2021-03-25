using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Tech
{
    class ConnectingPoint
    {
        public int directionToTravel;
        public int distanceToTravel;
        public Point connectingPoint;

        public ConnectingPoint(int directionToTravel, Point connectingPoint, int distanceToTravel)
        {
            this.directionToTravel = directionToTravel;
            this.connectingPoint = connectingPoint;
            this.distanceToTravel = distanceToTravel;
        }
    }
}
