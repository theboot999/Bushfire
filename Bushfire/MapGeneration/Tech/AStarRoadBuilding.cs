using BushFire.Game;
using BushFire.MapGeneration.Generation.RoadStuff;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Tech
{
    class AStarRoadBuilding
    {
        private Boolean[,] IsSearched;          //Whether this node has been searched
        private Boolean found = false;          //Have we found the end point
        private int[,] score;                   //Temporary array of all found total scores for each node
        private int[,] hScore;                  //Temporary array of the calculated heuristics for each node
        private int htileCost = 5;             //Heuristic tile cost for straight
        private int directionChangeCost = 24;   //Extra cost for doing a direction change
        private int newHscore = 0;              //New tile Heuristic score
        private int newPointScore = 0;          //Mew tile total score
        private int maximumToCheck = 10000000;     //Maximum amount of tiles we will check
        private Point[,] comeFrom;              //Temporary array of where that node come from
        private Point startPoint;               //Start point of our journey
        private Point currentPoint;             //Current working point
        private Point endPoint;                 //Endpoint of our journey
        private Point newPoint;                 //New point to check
        private CheckNode checkNode;            //Current Node we are checking from our Node list
        private CheckNode newNode;              //New Node we are checking from our node list
        private List<CheckNode> listToCheck;    //List of Nodes
        private Byte[,] directionFrom;          //Temporary array with the direction we come from
        private int listToCheckIndex;
        private int lowestNumber;

        public AStarRoadBuilding()
        {
            listToCheck = new List<CheckNode>();
            IsSearched = new bool[ShrunkWorldBuilder.shrunkWorldWidth, ShrunkWorldBuilder.shrunkWorldHeight];
            score = new int[ShrunkWorldBuilder.shrunkWorldWidth, ShrunkWorldBuilder.shrunkWorldHeight];
            comeFrom = new Point[ShrunkWorldBuilder.shrunkWorldWidth, ShrunkWorldBuilder.shrunkWorldHeight];
            hScore = new int[ShrunkWorldBuilder.shrunkWorldWidth, ShrunkWorldBuilder.shrunkWorldHeight];
            directionFrom = new byte[ShrunkWorldBuilder.shrunkWorldWidth, ShrunkWorldBuilder.shrunkWorldHeight];

        }

        public List<Point> GetTravelList(Point startPoint, Point endPoint, ShrunkNode[,] shrunkMap, int findTownId)
        {
            List<Point> routeList = new List<Point>();

            if (startPoint == endPoint)  //return an empty list
            {
                return routeList;
            }

            this.startPoint = startPoint;
            this.endPoint = endPoint;
            checkNode = new CheckNode();
            newPoint = this.startPoint;
            checkNode.Update(startPoint, startPoint, newPointScore);
            listToCheck.Add(checkNode);


            for (int i = 0; i < maximumToCheck; i++)
            {
                if (listToCheck.Count < 1)  //list is empty
                {
                    break;
                }

                lowestNumber = listToCheck[0].score;
                listToCheckIndex = 0;

                for (int p = 1; p < listToCheck.Count; p++)
                {
                    if (listToCheck[p].score < lowestNumber)
                    {
                        lowestNumber = listToCheck[p].score;
                        listToCheckIndex = p;
                    }
                }

                checkNode = listToCheck[listToCheckIndex]; //getting the next node to check
                currentPoint = checkNode.currentPoint;  //setting the next node to check to be the current point.

                if (currentPoint == this.endPoint) //checking to see if we reached the end;
                {
                    found = true;
                    break;
                }

                listToCheck.RemoveAt(listToCheckIndex);  //remove the point from the list

                UpdateNewPoints(shrunkMap, findTownId);  //looking for new points
            }

           
            if (found)
            {
                UpdateTravelList(routeList);
            }
    
            return routeList;  //we are returning an empty list
        }


        private void UpdateNewPoints(ShrunkNode[,] shrunkMap, int findTownId)
        {
            for (byte i = 0; i < 8; i += 2)
            {
                newPoint = AngleStuff.AddPointToDirection(currentPoint, i);
                CheckingPoints(i, shrunkMap);

                if (ShrunkWorldBuilder.xLegitShrunkMap(newPoint.X, 0) && ShrunkWorldBuilder.yLegitShrunkMap(newPoint.Y, 0))
                {
                    if (shrunkMap[newPoint.X, newPoint.Y].townId == findTownId)
                    {
                        found = true;
                        currentPoint = newPoint;
                        break;
                    }
                }
            }
        }

        private void CheckingPoints(byte direction, ShrunkNode[,] shrunkMap)
        {
            if (ShrunkWorldBuilder.xLegitShrunkMap(newPoint.X, 0) && ShrunkWorldBuilder.yLegitShrunkMap(newPoint.Y, 0))
            {
                if (!(IsSearched[newPoint.X, newPoint.Y]))  //if it hasnt been searched add it to the list
                {
                    if (direction == directionFrom[currentPoint.X, currentPoint.Y])
                    {
                        CalculateNewScores(false, shrunkMap);
                    }
                    else
                    {
                        CalculateNewScores(true, shrunkMap);

                    }
                    AddToList(direction);
                }
                else// it has been searched.  lets remove the old one from the list
                {
                    CalculateNewScores(false, shrunkMap);
                    if (newPointScore < score[newPoint.X, newPoint.Y])  //this one is better
                    {

                        AddToList(direction);
                        RemoveFromList();
                    }
                }
            }
        }

        private void RemoveFromList()
        {
            for (int i = listToCheck.Count - 1; i > -1; i--)
            {
                if (listToCheck[i].currentPoint == newPoint)
                {
                    listToCheck.RemoveAt(i);
                    break;
                }
            }
        }

        private void AddToList(byte direction)
        {

            newNode = new CheckNode();
            newNode.Update(newPoint, currentPoint, newPointScore);
            listToCheck.Add(newNode);
            score[newPoint.X, newPoint.Y] = newPointScore;
            IsSearched[newPoint.X, newPoint.Y] = true;
            comeFrom[newPoint.X, newPoint.Y] = currentPoint;
            hScore[newPoint.X, newPoint.Y] = newHscore;
            directionFrom[newPoint.X, newPoint.Y] = direction; //the direction we come from
        }

        private void CalculateNewScores(bool directionCost, ShrunkNode[,] shrunkMap)
        {
            int scoreX = Math.Abs(endPoint.X - newPoint.X);
            int scoreY = Math.Abs(endPoint.Y - newPoint.Y);

            newHscore = ((scoreX + scoreY * 3)) * htileCost;
            newPointScore = (score[currentPoint.X, currentPoint.Y]) - hScore[currentPoint.X, currentPoint.Y] + shrunkMap[newPoint.X, newPoint.Y].pathScore + newHscore;

            if (directionCost) { newPointScore += directionChangeCost; }
        }

        private List<Point> UpdateTravelList(List<Point> routeList)
        {
            routeList.Add(currentPoint);
            do
            {
                newPoint = comeFrom[currentPoint.X, currentPoint.Y];
                routeList.Add(newPoint);
                currentPoint = newPoint;
            }
            while (!(newPoint == startPoint));
            return routeList;
        }    

        private bool IsOdd(int value)
        {
            return value % 2 != 0;
        }
    }
}
