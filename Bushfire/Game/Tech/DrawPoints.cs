using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Tech
{
    class DrawPoints
    {
        //Todo re add in there right and bottom sides
        public Point topLeftPoint;
        public Point botRightPoint;
        public Vector2 topLeft;
        public Vector2 botRight;

        public Point[] overDrawPoints = new Point[8];
        int overDrawSides = 8;
        int overDrawTops = 5;

        public void UpdateDrawPoints(float tileSize, int worldWidth, int worldHeight, Vector2 topLeft, Vector2 botRight)
        {
            this.topLeft = topLeft;
            this.botRight = botRight;

            topLeftPoint = new Point((int)(topLeft.X / tileSize), (int)(topLeft.Y / tileSize));
            topLeftPoint.X = MathHelper.Clamp(topLeftPoint.X, 0, worldWidth);
            topLeftPoint.Y = MathHelper.Clamp(topLeftPoint.Y, 0, worldHeight);

            botRightPoint = new Point((int)(botRight.X / tileSize) + 2, (int)(botRight.Y / tileSize) + 2);
            botRightPoint.X = MathHelper.Clamp(botRightPoint.X, 0, worldWidth);
            botRightPoint.Y = MathHelper.Clamp(botRightPoint.Y, 0, worldHeight);

            int width = botRightPoint.X - topLeftPoint.X;
            int height = botRightPoint.Y - topLeftPoint.Y;

            //Top
            overDrawPoints[0] = new Point(topLeftPoint.X - overDrawTops, topLeftPoint.Y - overDrawTops);
            overDrawPoints[1] = new Point(botRightPoint.X + overDrawTops, topLeftPoint.Y);
            //MidLeft
            overDrawPoints[2] = new Point(topLeftPoint.X - overDrawSides, topLeftPoint.Y);
            overDrawPoints[3] = new Point(topLeftPoint.X, botRightPoint.Y);
            //MidRight
            overDrawPoints[4] = new Point(botRightPoint.X, topLeftPoint.Y);
            overDrawPoints[5] = new Point(botRightPoint.X + overDrawSides, botRightPoint.Y);
            //Bottom
            overDrawPoints[6] = new Point(topLeftPoint.X - overDrawTops, botRightPoint.Y);
            overDrawPoints[7] = new Point(botRightPoint.X + overDrawTops, botRightPoint.Y + overDrawTops);


            for (int i = 0; i < 8; i++)
            {
                overDrawPoints[i].X = MathHelper.Clamp(overDrawPoints[i].X, 0, worldWidth);
                overDrawPoints[i].Y = MathHelper.Clamp(overDrawPoints[i].Y, 0, worldHeight);
            }

        }

        public void DebugDuplicateCheck()
        {
            

        }
    }
}
