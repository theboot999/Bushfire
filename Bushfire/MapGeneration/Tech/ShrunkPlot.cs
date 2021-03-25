using BushFire.Engine.Controllers;
using BushFire.Game;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Tech
{
    class ShrunkPlot
    {
        //WE ARE STUPID WHEN EXPANDING OUR POLOTS NEED TO BE TWISE THE SIZE
        public Point pointOne { get; private set; }
        public Point pointTwo { get; private set; }
        public int plotNum { get; set; }
        public int roadFaceDirection { get; private set; }
        public int maxDepth { get; set; }
        public int currentLength { get; set; }
        public int maxLength;
        public bool isFirst;
        public int townId { get; private set; }

        public bool kill { get; private set; }

        public ShrunkPlot(int plotNum, int roadFaceDirection, int townId)
        {
            //Remember these max's are times 2 when we get there
            isFirst = true;
            this.plotNum = plotNum;
            maxDepth = 3;
            maxLength = 4;
            this.roadFaceDirection = roadFaceDirection;
            this.townId = townId;
        }


        public void AddPointOne(Point point)
        {
            pointOne = point;
        }

        public void AddPointTwo(Point point)
        {
            pointTwo = point;
        }

        public void Smooth(ShrunkNode[,] shrunkMap)
        {
            CheckSizeError();
            SortCornerPoints();           
            CheckLegitOnMap(shrunkMap);
            CheckSize();
        }

        private void CheckLegitOnMap(ShrunkNode[,] shrunkMap)
        {
            if (!kill)
            {
                for (int x = pointOne.X; x <= pointTwo.X; x++)
                {
                    for (int y = pointOne.Y; y <= pointTwo.Y; y++)
                    {
                        if (shrunkMap[x, y].landType != LandType.PLOT)
                        {
                            kill = true;
                            break;
                        }
                    }
                }
            }
        }

        private void CheckSize()
        {

        }

        public void AddDebugToMap(ShrunkNode[,] shrunkMap)
        {
            if (!kill)
            {
                for (int x = pointOne.X; x <= pointTwo.X; x++)
                {
                    for (int y = pointOne.Y; y <= pointTwo.Y; y++)
                    {
                        shrunkMap[x, y].plotId = plotNum;
                    }
                }
            }
        }    

        private void CheckSizeError()
        {
            if (pointOne.X == 0 && pointOne.Y == 0 || pointTwo.X == 0 && pointTwo.Y == 0)
            {
                kill = true;
            }
        }

        public void SortCornerPoints()
        {
            if (!kill)
            {
                int topLeftX;
                int topLeftY;
                int bottomRightX;
                int bottomRightY;

                if (pointOne.X <= pointTwo.X)
                {
                    topLeftX = pointOne.X;
                    bottomRightX = pointTwo.X;
                }
                else
                {
                    topLeftX = pointTwo.X;
                    bottomRightX = pointOne.X;
                }
                if (pointOne.Y <= pointTwo.Y)
                {
                    topLeftY = pointOne.Y;
                    bottomRightY = pointTwo.Y;
                }
                else
                {
                    topLeftY = pointTwo.Y;
                    bottomRightY = pointOne.Y;
                }
                pointOne = new Point(topLeftX, topLeftY);
                pointTwo = new Point(bottomRightX, bottomRightY);
            }
        }     
    }
}
