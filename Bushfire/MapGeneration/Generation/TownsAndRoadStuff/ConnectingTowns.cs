using BushFire.Engine.Controllers;
using BushFire.Game;
using BushFire.Game.Map;
using BushFire.MapGeneration.Containers;
using BushFire.MapGeneration.Tech;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Generation.RoadStuff
{
    class ConnectingTowns
    {


        public ConnectingTowns(ref List<Town> townList, ref List<Point> connectionList, ShrunkNode[,] shrunkMap, LoadingInfo loadingInfo)
        {
            float percentDone = 0;
            float percentJump = 100f / connectionList.Count;

            int townStartId;
            int targetid;

            Dictionary<int, Boolean> connectedList = new Dictionary<int, bool>();

            for (int i = 0; i < townList.Count; i++)
            {
                if (i == 0)
                {
                    connectedList.Add(i, false);
                }
                else
                {
                    connectedList.Add(i, false);
                }
            }

            int numberofTowns = townList.Count;


            for (int i = 0; i < connectionList.Count; i++)
            {
                percentDone += percentJump;
                loadingInfo.UpdateLoading(LoadingType.ConnectingTowns, percentDone);
                townStartId = connectionList[i].X;
                targetid = connectionList[i].Y;
                Point startPoint = townList[townStartId].GetShrunkPoint();
                Point endPoint = townList[targetid].GetShrunkPoint();
                List<Point> connectingPoints = new List<Point>();


                AStarRoadBuildingNew aStar = new AStarRoadBuildingNew();
                
                connectingPoints = aStar.GetTravelList(startPoint, endPoint, shrunkMap, targetid);
                UpdateShrunkMap(connectingPoints, townStartId, shrunkMap);

            }

        }

        private void UpdateShrunkMap(List<Point> connectingPoints, int townId, ShrunkNode[,] shrunkMap)
        {
            for (int i = 0; i < connectingPoints.Count; i++)
            {
                if (shrunkMap[connectingPoints[i].X, connectingPoints[i].Y].landType != LandType.CITYROAD)
                {
                    shrunkMap[connectingPoints[i].X, connectingPoints[i].Y].SetLandType(LandType.COUNTRYROAD);
                    shrunkMap[connectingPoints[i].X, connectingPoints[i].Y].SetTownId(townId);
                }
            }
        }   
    }
}
