using BushFire.Engine.Controllers;
using BushFire.Game;
using BushFire.Game.Controllers;
using BushFire.Game.Map;
using BushFire.MapGeneration.Containers;
using BushFire.MapGeneration.Tech;
using BushFire.Menu.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Generation.RoadStuff
{
    class TownSitesBuilder
    {
        private int distanceBetweenTowns;
        private List<string> townNameList;
        private int offSetFromEdge = 20;

        public TownSitesBuilder(ShrunkNode[,] shrunkMap, List<Town> townList, ref List<Point> connectionList, LoadingInfo loadingInfo)
        {
            float percentDone = 0;
            float percentJump = 100f / CreatingWorld.numberOfTowns;

            Random rnd = GameController.GetRandomWithSeed();
            InitTownNames();
            int numberOfChecks = 5000;
            bool safeDistance;
            distanceBetweenTowns = GetDistanceBetweenTowns();
            int townId = 0;

            for (int i = 0; i < numberOfChecks; i++)
            {
                //TODO: take this next line out i just like this map for debugging
                //rndExtras = GameController.rnd;
                Point checkingPoint = new Point(rnd.Next(offSetFromEdge, ShrunkWorldBuilder.shrunkWorldWidth), rnd.Next(offSetFromEdge, ShrunkWorldBuilder.shrunkWorldHeight - offSetFromEdge));
                safeDistance = false;

                if (ShrunkWorldBuilder.IsOpen(checkingPoint, shrunkMap))
                {
                    safeDistance = true;

                    foreach (Town otherTown in townList)
                    {
                        if (IsToCloseDistance(checkingPoint, otherTown.GetShrunkPoint()))
                        {
                            safeDistance = false;
                            break;
                        }
                    }
                }

                if (safeDistance)
                {
                    townList.Add(new Town(checkingPoint, townId, GetTownName(rnd)));
                    townId++;
                    percentDone += percentJump;
                    loadingInfo.UpdateLoading(LoadingType.PlottingTownPoints, percentDone);

                }

                if (townList.Count == CreatingWorld.numberOfTowns)
                {
                    break;
                }
            }
            connectionList = CreateConnectionList(townList);
        }

        private bool IsToCloseDistance(Point point1, Point point2)
        {
            return ((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y)) < distanceBetweenTowns * distanceBetweenTowns;
        }

        private List<Point> CreateConnectionList(List<Town> townList)
        {
            List<Point> outPutList = new List<Point>();
            int lowestTownId1 = 0;
            int lowestTownId2 = 0;
            int score;
            int foundBranch1 = 0;
            int foundBranch2 = 0;
            int branchCount = 0;

            Dictionary<int, Dictionary<int, int>> listOfTrees = new Dictionary<int, Dictionary<int, int>>();

            if (townList.Count > 1)
            {
                for (int i = 0; i < townList.Count; i++)
                {
                    listOfTrees.Add(i, new Dictionary<int, int>());
                    Dictionary<int, int> listOfBranch = listOfTrees[i];
                    listOfBranch.Add(i, i);
                    branchCount++;
                }

                float smallPercentDone = 0;
                float percentJump = 100f / listOfTrees.Count;

                do
                {
                    score = 0;
                    smallPercentDone += percentJump;
                 //   screenLoading.UpdatePercentage1((int)smallPercentDone);

                    foreach (KeyValuePair<int, Dictionary<int, int>> listOfBranch in listOfTrees)  //each tree branch
                    {
                        foreach (KeyValuePair<int, int> townId in listOfBranch.Value)
                        {
                            foreach (KeyValuePair<int, Dictionary<int, int>> listOfBranchChecking in listOfTrees)  //each tree branch
                            {
                                if (listOfBranch.Key != listOfBranchChecking.Key)
                                {

                                    foreach (KeyValuePair<int, int> townCheckingId in listOfBranchChecking.Value)
                                    {

                                        if (GetDistance(townList[townId.Value].GetShrunkPoint(), townList[townCheckingId.Value].GetShrunkPoint()) < score || score == 0)
                                        {
                                            score = GetDistance(townList[townId.Value].GetShrunkPoint(), townList[townCheckingId.Value].GetShrunkPoint());
                                            lowestTownId1 = townId.Value;
                                            lowestTownId2 = townCheckingId.Value;
                                            foundBranch1 = listOfBranch.Key;
                                            foundBranch2 = listOfBranchChecking.Key;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    outPutList.Add(new Point(lowestTownId1, lowestTownId2));
                    Dictionary<int, int> listOfBranch1 = listOfTrees[foundBranch1];
                    Dictionary<int, int> listOfBranch2 = listOfTrees[foundBranch2];

                    foreach (int townId2 in listOfBranch2.Values)
                    {
                        listOfBranch1.Add(branchCount, townId2);
                        branchCount++;
                    }
                    listOfTrees.Remove(foundBranch2);
                }
                while (listOfTrees.Count() > 1);
            }
            return outPutList;
        }

        private int GetDistance(Point point1, Point point2)
        {
            int a = point2.X - point1.X;
            int b = point2.Y - point1.Y;
            return (int)Math.Sqrt(a * a + b * b);
        }

        private int GetDistanceBetweenTowns()
        {
                 double tempCalc = ((CreatingWorld.worldWidth * CreatingWorld.worldHeight) / CreatingWorld.numberOfTowns) * 0.5;
                 return ((int)Math.Sqrt(tempCalc)) / 2;
        }

        private string GetTownName(Random rndExtras)
        {
            if (townNameList.Count > 0)
            {
                int i = rndExtras.Next(0, townNameList.Count);
                string name = townNameList[i];
                townNameList.RemoveAt(i);
                return name;
            }
            else
            {
                return "OutOfTownNames";
            }
        }

        private void InitTownNames()
        {
            townNameList = new List<string>();

            townNameList.Add("Lawkarta");
            townNameList.Add("Dodgedol");
            townNameList.Add("Daydale");
            townNameList.Add("Backgrad");
            townNameList.Add("Sagehampton");
            townNameList.Add("Greencaster");
            townNameList.Add("Strongby");
            townNameList.Add("Bridgeford Park");
            townNameList.Add("Eastdale");
            townNameList.Add("Saydale");
            townNameList.Add("West Weirview");
            townNameList.Add("Clamdol");
            townNameList.Add("Cruxcaster");
            townNameList.Add("Posthampton");
            townNameList.Add("South Farmmouth");
            townNameList.Add("Great Passworth");
            townNameList.Add("Bayworth");
            townNameList.Add("Lexingstead");
            townNameList.Add("North Seakarta");
            townNameList.Add("Cloudbury Island");
            townNameList.Add("Hallpool");
            townNameList.Add("Luntown");
            townNameList.Add("Forttown");
            townNameList.Add("Farmness");
            townNameList.Add("Riverwich");
            townNameList.Add("Buoyton");
            townNameList.Add("South Holtsville");
            townNameList.Add("Mannortown");
            townNameList.Add("Melcaster");
            townNameList.Add("Northton Beach");
            townNameList.Add("Casterborough");
            townNameList.Add("West Dayfield");
            townNameList.Add("Goldland");
            townNameList.Add("Southingford");
            townNameList.Add("Goldby");
            townNameList.Add("Gilpool");
            townNameList.Add("Buoyton");
            townNameList.Add("Rockgrad");
            townNameList.Add("Elview Park");
            townNameList.Add("Eggborough");
            townNameList.Add("New Fairfield");
            townNameList.Add("Hamgrad");
            townNameList.Add("New Readingview");
            townNameList.Add("Great Bannborough");
            townNameList.Add("Proborough Beach");
            townNameList.Add("New Richgrad");
            townNameList.Add("Manley");
            townNameList.Add("Winterville");
            townNameList.Add("Profield");
            townNameList.Add("Costscester");
            townNameList.Add("Buoyfield Hills");
            townNameList.Add("Hospool");
            townNameList.Add("Redford");
            townNameList.Add("Windale");
            townNameList.Add("Postfolk");
            townNameList.Add("West Meddale");
            townNameList.Add("Medton");
            townNameList.Add("West Foxgrad");
            townNameList.Add("Saltpool");
            townNameList.Add("Kettlefield");
            townNameList.Add("Farmby");
            townNameList.Add("Weststead");
            townNameList.Add("Tallburg");
            townNameList.Add("Bellburgh");
            townNameList.Add("South Princegrad");
            townNameList.Add("Elborough");
            townNameList.Add("Mannorton");
            townNameList.Add("Lawcester");
            townNameList.Add("Cloudworth");
            townNameList.Add("Julton");
            townNameList.Add("Pailbrough");
            townNameList.Add("Mansland Beach");
            townNameList.Add("Tallgrad");
            townNameList.Add("Riverland Beach");
            townNameList.Add("Wingpool");
            townNameList.Add("Hollowkarta");
            townNameList.Add("Millham");
            townNameList.Add("Great Manside");
            townNameList.Add("Fishburg");
            townNameList.Add("Prowich");
            townNameList.Add("Wheelland");
            townNameList.Add("Winterville");
            townNameList.Add("Mandale");
            townNameList.Add("Greendol");
            townNameList.Add("Redfield");
            townNameList.Add("Waterford");
            townNameList.Add("Bayford Beach");
            townNameList.Add("Northgrad Hills");
            townNameList.Add("Stoneland");
            townNameList.Add("North Fishburg");
            townNameList.Add("Sugarpool");
            townNameList.Add("Summergrad");
            townNameList.Add("East Wheelcaster");
            townNameList.Add("West Passpool");
            townNameList.Add("Frostview");
            townNameList.Add("Wingby");
            townNameList.Add("Griffinley");
            townNameList.Add("Cloudfield");
            townNameList.Add("Wheelfield");
            townNameList.Add("Sandkarta");
        }
    }
}
