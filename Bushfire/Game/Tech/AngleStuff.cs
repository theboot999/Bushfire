using BushFire.Engine.Controllers;
using BushFire.Game.Controllers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game
{
    static class AngleStuff
    {
        public static Point[] directionPoint { get; private set; }
        public static Dictionary<Point, int> pointDirection { get; private set; }

        public static float pieZero = 0f;
        public static float pieQuarter = MathHelper.Pi * 0.25f;
        public static float pieHalf = MathHelper.Pi * 0.50f;
        public static float pieThreeQuarter = MathHelper.Pi * 0.75f;
        public static float pie = MathHelper.Pi;
        public static float pieOneQuarter = MathHelper.Pi * 1.25f;
        public static float pieOneHalf = MathHelper.Pi * 1.5f;
        public static float pieOneThreeQuarter = MathHelper.Pi * 1.75f;
        public static float pieTwo = MathHelper.Pi * 2f;

        public static void Init()
        {
            SetDirectionList();
        }

        private static void SetDirectionList()
        {
            directionPoint = new Point[9];
            directionPoint[0] = new Point(0, -1);  //up
            directionPoint[1] = new Point(1, -1);  //up right
            directionPoint[2] = new Point(1, 0); //right
            directionPoint[3] = new Point(1, 1); //downright
            directionPoint[4] = new Point(0, 1);  //down
            directionPoint[5] = new Point(-1, 1); //downleft     
            directionPoint[6] = new Point(-1, 0); //left
            directionPoint[7] = new Point(-1, -1); //upleft
            directionPoint[8] = new Point(-1, -1); //up

            pointDirection = new Dictionary<Point, int>();
            pointDirection.Add(new Point(0, -1), 0);
            pointDirection.Add(new Point(1, -1), 1);
            pointDirection.Add(new Point(1, 0), 2);
            pointDirection.Add(new Point(1, 1), 3);
            pointDirection.Add(new Point(0, 1), 4);
            pointDirection.Add(new Point(-1, 1), 5);
            pointDirection.Add(new Point(-1, 0), 6);
            pointDirection.Add(new Point(-1, -1), 7);

        }

        //TODO: Create a reverse lookup dictionary for points

        public static Point GetPoint(Direction direction)
        {
            return directionPoint[(int)direction];
        }

        public static Point GetPoint(int direction)
        {
            return directionPoint[direction];
        }

        public static int GetDirection(Point point)
        {
            if (pointDirection.ContainsKey(point))
            {
                return pointDirection[point];
            }
            return -1;
        }

        public static int GetDirectionDifference(int direction1, int direction2)
        {
            return Math.Abs(direction1 - direction2);
        }

        public static int RotateDirection(Direction direction, int itterations)
        {
            return ((int)direction + itterations + 16) % 8;
        }

        public static int RotateDirection(int direction, int itterations)
        {
            return (direction + itterations + 16) % 8;
        }

        public static Point AddPointToDirection(Point point, Direction direction)
        {
            return point + directionPoint[(int)direction];
        }

        public static Point AddPointToDirection(Point point, int direction)
        {
            return point + directionPoint[direction];
        }

        public static float DirectionToRadian(int direction)
        {
            return (float)Math.Atan2(directionPoint[direction].Y, directionPoint[direction].X);
        }

        public static float GetRadianDifference(float angleA, float angleB)
        {
            if (angleB - angleA > 3.142)
            {
                angleB -= 6.283f;
            }
            else if (angleB - angleA < -3.142)
            {
                angleB += 6.283f;
            }

            float answer = angleB - angleA;

            if (answer < 0)
            {
                return answer *= -1;
            }
            return answer;
        }


        //This only calculates 90 degree turns;  Should refine it as well
        public static Direction GetTurnDirection(int startDirection, int endDirection)
        {
            if (startDirection == 0)    //Up
            {
                if (endDirection < 4)
                {
                    return Direction.RIGHT;
                }
                else
                {
                    return Direction.LEFT;
                }
            }
            else if (startDirection == 2)
            {
                if (endDirection < 2 || endDirection > 5)
                {
                    return Direction.LEFT;
                }
                else
                {
                    return Direction.RIGHT;
                }
            }
            else if (startDirection == 4)
            {
                if (endDirection > 4)
                {
                    return Direction.RIGHT;
                }
                else
                {
                    return Direction.LEFT;
                }
            }
            else if (startDirection == 6)
            {
                if (endDirection > 6 || endDirection < 2)
                {
                    return Direction.RIGHT;
                }
                else
                {
                    return Direction.LEFT;
                }
            }
            return Direction.NONE;

        }

        public static Vector2 RadianToVector(float value)
        {
            Vector2 temp = new Vector2((float)Math.Cos(value), (float)Math.Sin(value));
            temp.Normalize();
            return temp;
        }

        public static float GetRandomRadianAngleBetween(float angle, float radianEitherSide)
        {
            float angleMin = angle - radianEitherSide;
            float angleMax = angle + radianEitherSide;
            float newAngle = RandomController.GetRandomFloat(angleMin, angleMax);
            newAngle = newAngle % AngleStuff.pieTwo;

            if (newAngle < 0)
            {
                newAngle = newAngle + AngleStuff.pieTwo;
            }
            return newAngle;
        }

        public static float GetRandomRadian()
        {
            return (float)(GameController.rnd.Next((int)(0f * 100f), (int)(pieTwo * 100f))) / 100f;
        }
    }


    enum Direction : Byte
    {
        UP = 0,
        UPRIGHT = 1,
        RIGHT = 2,
        DOWNRIGHT = 3,
        DOWN = 4,
        DOWNLEFT = 5,
        LEFT = 6,
        UPLEFT = 7,
        NONE = 8
    }
}
