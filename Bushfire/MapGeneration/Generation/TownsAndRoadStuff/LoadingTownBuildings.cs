using BushFire.Editor.Controllers;
using BushFire.Editor.Tech;
using BushFire.Engine.Files;
using BushFire.Game.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Generation.TownsAndRoadStuff
{
    class LoadingTownBuildings
    {
        public LoadingTownBuildings(List<Building> buildingList)
        {
            BuildingsBinary buildingsBinary = Data.LoadBuildingsBinary();

            foreach (CompressedBuilding building in buildingsBinary.buildingList)
            {
                if (building.isActive)
                {
                    buildingList.AddRange(building.GetBuildingList());
                }
            }

        }
    }
}
