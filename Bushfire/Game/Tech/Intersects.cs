using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Tech
{
    static class Intersects
    {
        /// <summary>
        /// This static class contains the more technical collision logic that is called by the Collision class
        /// Standard rectangle.intersects collisions are in the collision class
        /// This class contains
        /// Bound box (rotated rectangles) collisions
        /// Boolean rotated rectangle and circle collisions (Yes or no intersects)
        /// Opposing velocity calculations for rotated rectangle and circle collisions
        /// Circle and circle collisions
        /// Only the rotated Rectangle to Circle collision can calculate opposing velocity
        /// Currently only opposing velocity is needed to be calculated against a planes wheels
        /// Rotated Rectangle to circle collision does not use bounding boxes.  It only uses a circle and a rotated rectangle
        /// </summary>

        public static bool IsBoundingBoxCollision(BoundingBox boundingBox1, BoundingBox boundingBox2)
        {
            Vector2[] boundingBox1Points = new Vector2[] { boundingBox1.topLeft, boundingBox1.topRight, boundingBox1.bottomRight, boundingBox1.bottomLeft };
            Vector2[] boundingBox2Points = new Vector2[] { boundingBox2.topLeft, boundingBox2.topRight, boundingBox2.bottomRight, boundingBox2.bottomLeft };

            if (DoAxisSeparationTest(boundingBox1.topLeft, boundingBox1.topRight, boundingBox1.bottomRight, boundingBox2Points)) { return false; }
            if (DoAxisSeparationTest(boundingBox1.topLeft, boundingBox1.bottomLeft, boundingBox1.bottomRight, boundingBox2Points)) { return false; }
            if (DoAxisSeparationTest(boundingBox1.bottomLeft, boundingBox1.bottomRight, boundingBox1.topLeft, boundingBox2Points)) { return false; }
            if (DoAxisSeparationTest(boundingBox1.bottomRight, boundingBox1.topRight, boundingBox1.topLeft, boundingBox2Points)) { return false; }
            if (DoAxisSeparationTest(boundingBox2.topLeft, boundingBox2.topRight, boundingBox2.bottomRight, boundingBox1Points)) { return false; }
            if (DoAxisSeparationTest(boundingBox2.topLeft, boundingBox2.bottomLeft, boundingBox2.bottomRight, boundingBox1Points)) { return false; }
            if (DoAxisSeparationTest(boundingBox2.bottomLeft, boundingBox2.bottomRight, boundingBox2.topLeft, boundingBox1Points)) { return false; }
            if (DoAxisSeparationTest(boundingBox2.bottomRight, boundingBox2.topRight, boundingBox2.topLeft, boundingBox1Points)) { return false; }
            return true;
        }

        private static bool DoAxisSeparationTest(Vector2 x1, Vector2 x2, Vector2 x3, Vector2[] otherBoundingBoxPoints)
        {
            Vector2 vec = x2 - x1;
            Vector2 rotated = new Vector2(-vec.Y, vec.X);

            bool refSide = (rotated.X * (x3.X - x1.X)
                          + rotated.Y * (x3.Y - x1.Y)) >= 0;

            foreach (Vector2 pt in otherBoundingBoxPoints)
            {
                bool side = (rotated.X * (pt.X - x1.X)
                           + rotated.Y * (pt.Y - x1.Y)) >= 0;
                if (side == refSide)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
