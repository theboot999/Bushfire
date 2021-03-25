using BushFire.Game.Storage;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Controllers
{
    //different node types for differnt landtypes
    //cos the node will tell us about pathfinding information
    //rather than on the tile
    //so
    //we need a multi layer dictionary perhaps iwth the landtypes in it


    static class TileLogisticsController
    {
        //  private static Dictionary<int, TileLogistics> travellingNodeList;
        private static Dictionary<LandType, Dictionary<int, TileLogistic>> landTypeLogisticsList;

        //Tile logistics are used for pathfinding and movement
        //Adding streetlights etc we do not change the road ones
        //When adding buildings or trees or blockable items we change them

        public static void Init()
        {
            

            int borderPf = -1;
            int waterPf = -1;
            int treePf = -1;
            int openPf = 4;
            int normalRoadPF = 4;
            int countryRoadPF = 2;

            Vector2 center = new Vector2(GroundLayerController.halfTileSize, GroundLayerController.halfTileSize);

            Vector2 straightRoadCenter = new Vector2(64, 86);
            Vector2 internalRoadCorner = new Vector2(86, 86);
            Vector2 externalRoadCorner = new Vector2(93, 39);

            //These overrides are offsets FROM the center of the tile
            //They are calculated to a direction from that dependant on if the vehicle is going left up, right down etc
            Vector2 normalTurnOverRide = new Vector2(30, 30);
            Vector2 straightRoadTurnOverride = new Vector2(50, 50);
            Vector2 internalRoadTurnOverride = new Vector2(20, 20);
            Vector2 externalRoadTurnOverride = new Vector2(35, 35);

            landTypeLogisticsList = new Dictionary<LandType, Dictionary<int, TileLogistic>>();

            AddLogisticType(LandType.WATER, center, normalTurnOverRide, false, -1, -1, waterPf, 0);
            AddLogisticType(LandType.OPEN, center, normalTurnOverRide, false, -1, -1, openPf, 0);
            AddLogisticType(LandType.TREE, center, normalTurnOverRide, false, -1, -1, treePf, 0);
            AddLogisticType(LandType.BORDER, center, normalTurnOverRide, false, -1, -1, borderPf, 0);


            AddLogisticType(LandType.CITYROAD, straightRoadCenter, straightRoadTurnOverride, true, 6, -1, normalRoadPF, 2);
            AddLogisticType(LandType.CITYROAD, internalRoadCorner, internalRoadTurnOverride, true, 4, 6, normalRoadPF, 1);
            AddLogisticType(LandType.CITYROAD, externalRoadCorner, externalRoadTurnOverride, true, 4, -1, normalRoadPF, 7);

            //Intersections City.  We need a different road id for these so we will stop and turn in pathfinding
            AddLogisticType(LandType.CITYROAD, straightRoadCenter, straightRoadTurnOverride, true, 6, -1, normalRoadPF, 2);  //These are our straight intersection roads.  will give a different id
            AddLogisticType(LandType.CITYROAD, internalRoadCorner, internalRoadTurnOverride, true, 4, 6, normalRoadPF, 1);
            AddLogisticType(LandType.CITYROAD, externalRoadCorner, externalRoadTurnOverride, true, 4, -1, normalRoadPF, 7);


            //Bugged ones
            AddLogisticType(LandType.CITYROAD, new Vector2(64, 64), normalTurnOverRide, false, -1, -1, borderPf, 0); //FOR THE BUGGED ONES Index 20 is bugged
            AddLogisticType(LandType.CITYROAD, new Vector2(64, 64), normalTurnOverRide, false, -1, -1, borderPf, 0);

            AddLogisticType(LandType.COUNTRYROAD, straightRoadCenter, straightRoadTurnOverride, true, 6, -1, countryRoadPF, 2);
            AddLogisticType(LandType.COUNTRYROAD, internalRoadCorner, internalRoadTurnOverride, true, 4, 6, countryRoadPF, 1);
            AddLogisticType(LandType.COUNTRYROAD, externalRoadCorner, externalRoadTurnOverride, true, 4, -1, countryRoadPF, 7);

            //Intersections Country. We need a different road id for these so we will stop and turn in pathfinding
            AddLogisticType(LandType.COUNTRYROAD, straightRoadCenter, straightRoadTurnOverride, true, 6, -1, countryRoadPF, 2);  //These are our straight intersection roads.  will give a different id
            AddLogisticType(LandType.COUNTRYROAD, internalRoadCorner, internalRoadTurnOverride, true, 4, 6, countryRoadPF, 1);
            AddLogisticType(LandType.COUNTRYROAD, externalRoadCorner, externalRoadTurnOverride, true, 4, -1, countryRoadPF, 7);


            //Bugged ones
            AddLogisticType(LandType.COUNTRYROAD, new Vector2(64, 64), normalTurnOverRide, false, -1, -1, borderPf, 0); //FOR THE BUGGED ONES
            AddLogisticType(LandType.COUNTRYROAD, new Vector2(64, 64), normalTurnOverRide, false, -1, -1, borderPf, 0);

            AddLogisticType(LandType.TREE, Vector2.Zero, Vector2.Zero, false, -1, -1, 0, 0);
            AddLogisticType(LandType.BUILDING, Vector2.Zero, Vector2.Zero, false, -1, -1, 0, 0);
        }



        private static void AddLogisticType(LandType landType, Vector2 center, Vector2 byPassTurn, bool isRoad, int directionBlockOne, int directionBlockTwo, int pathfindingScore, int defaultDirection)
        {
            Dictionary<int, TileLogistic> listUsing = new Dictionary<int, TileLogistic>();

            if (landTypeLogisticsList.ContainsKey(landType))
            {
                listUsing = landTypeLogisticsList[landType];
            }
            else
            {
                landTypeLogisticsList.Add(landType, listUsing);
            }
            int index = listUsing.Count();

            for (int i = index; i < index + 4; i++)
            {
                listUsing.Add(i, new TileLogistic(landType, center, byPassTurn, isRoad, i,  directionBlockOne, directionBlockTwo, pathfindingScore, defaultDirection));
                center = RotateVectorRight(center);
                //byPassTurn = RotateVectorRight(byPassTurn);
                directionBlockOne = RotateDirectionBlock(directionBlockOne);
                directionBlockTwo = RotateDirectionBlock(directionBlockTwo);
                defaultDirection = AngleStuff.RotateDirection(defaultDirection, 2);
            }
        }

        private static Vector2 RotateVectorRight(Vector2 vector2)
        {
            float oldX = vector2.X + GroundLayerController.halfTileSize;
            float oldY = vector2.Y - GroundLayerController.halfTileSize;
            vector2.X = (oldY * -1) + GroundLayerController.halfTileSize - 1;
            vector2.Y = (oldX) - GroundLayerController.halfTileSize;
            return vector2;
        }

        private static int RotateDirectionBlock(int directionBlock)
        {
            if (directionBlock > -1)
            {
                directionBlock += 2;
                if (directionBlock > 7)
                {
                    directionBlock -= 8;
                }
            }
            return directionBlock;
        }

        public static TileLogistic GetTileLogistic(LandType landType, int index)
        {
             return landTypeLogisticsList[landType][index];
        }

    }
}
