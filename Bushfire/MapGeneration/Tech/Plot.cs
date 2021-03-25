using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Tech
{
    class Plot
    {
        public Point topLeft;
        private Point bottomRight;
        public int plotNum { get; set; }
        public int roadFaceDirection { get; private set; }
        public int width;
        public int height;

        public Plot(Point shrunkTopLeft, Point shrunkBottomRight, int plotNum, int roadFaceDirection)
        {
            topLeft = new Point(shrunkTopLeft.X * 2, shrunkTopLeft.Y * 2);
            bottomRight = new Point((shrunkBottomRight.X * 2) + 1, (shrunkBottomRight.Y * 2) + 1);
            this.plotNum = plotNum;
            this.roadFaceDirection = roadFaceDirection;

            width = (bottomRight.X - topLeft.X) + 1;
            height = (bottomRight.Y - topLeft.Y) + 1;
        }
    }
}
