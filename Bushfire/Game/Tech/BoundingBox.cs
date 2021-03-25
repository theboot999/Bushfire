using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Tech
{
    public struct BoundingBox
    {
        public Vector2 topLeft { get; set; }
        public Vector2 topRight { get; set; }
        public Vector2 bottomLeft { get; set; }
        public Vector2 bottomRight { get; set; }

        public BoundingBox(Vector2 topLeft, Vector2 bottomRight)
        {
            this.topLeft = topLeft;
            this.bottomRight = bottomRight;
            bottomLeft = new Vector2(topLeft.X, bottomRight.Y);
            topRight = new Vector2(bottomRight.X, topLeft.Y);
        }

        public BoundingBox(Rectangle rectangle, Vector2 worldPos, float angle)
        {
            topLeft = new Vector2(rectangle.X - (rectangle.Width / 2), rectangle.Y - (rectangle.Height / 2));
            topRight = new Vector2(rectangle.X + (rectangle.Width / 2), rectangle.Y - (rectangle.Height / 2));
            bottomRight = new Vector2(rectangle.X + (rectangle.Width / 2), rectangle.Y + (rectangle.Height / 2));
            bottomLeft = new Vector2(rectangle.X - (rectangle.Width / 2), rectangle.Y + (rectangle.Height / 2));

            topLeft = Vector2.Transform(topLeft, Matrix.CreateRotationZ(angle)) + worldPos;
            topRight = Vector2.Transform(topRight, Matrix.CreateRotationZ(angle)) + worldPos;
            bottomRight = Vector2.Transform(bottomRight, Matrix.CreateRotationZ(angle)) + worldPos;
            bottomLeft = Vector2.Transform(bottomLeft, Matrix.CreateRotationZ(angle)) + worldPos;
        }



    }
}
