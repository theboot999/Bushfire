using BushFire.Engine.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Editor.Tech
{
    [Serializable]
    class BuildingsBinary
    {
        public List<CompressedBuilding> buildingList;

        public BuildingsBinary()
        {
            buildingList = new List<CompressedBuilding>();

        }

        public int GetNextId()
        {
            if (buildingList != null)
            {
                return buildingList.Count;
            }
            return 0;
        }

        public void AddBuilding(CompressedBuilding building)
        {
            buildingList.Add(building);

            int p = -1;

            for (int i = buildingList.Count -1; i >= 0; i--)
            {
                if (buildingList[i].id == building.id)
                {
                    buildingList.RemoveAt(i);
                    p = i;
                }
            }

            if (p > -1)
            {
                buildingList.Insert(p, building);
            }
            else
            {
                buildingList.Add(building);
            }

            Data.SaveBuildingsBinary(this);
        }

        public CompressedBuilding GetCompressedBuilding(int id)
        {
            foreach (CompressedBuilding building in buildingList)
            {
                if (building.id == id)
                {
                    return building;
                }
            }
            return null;
        }
    }
}
