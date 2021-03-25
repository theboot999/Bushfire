using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Tech
{
    struct CheckNode
    {
        public Point currentPoint { get; private set; }
        public Point previousPoint { get; private set; }
        public int score { get; private set; }

        public CheckNode(Point currentPoint, Point previousPoint, int score)
        {
            this.currentPoint = currentPoint;
            this.previousPoint = previousPoint;
            this.score = score;
        }

        public void Update(Point currentPoint, Point previousPoint, int score)
        {
            this.currentPoint = currentPoint;
            this.previousPoint = previousPoint;
            this.score = score;
        }
    }
}
