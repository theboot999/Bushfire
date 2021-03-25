using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Game.Controllers;
using BushFire.Game.Tech;
using BushFire.Game.Vehicles;
using BushFire.Game.Vehicles.Attachments;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoundingBox = BushFire.Game.Tech.BoundingBox;

namespace BushFire.Game.Map
{
    //Todo: test performance of for each vs element at for hashet
    //TODO:  when we set colours for vehicles.
    //: We add a range of possible colours
    //And progamatically change htem like the house

        //So eventaully
        //we use the active vehicle list for all active vehicles (ai and ones we use)
        //idle vehicles are parked at houses etc
        //then we slowly pop idle vehicles off the idlevehilce list and add them to active vehicles
        //until we reach aspecific count
        //will save heaps on updates etc
        //only activevehicles get updated and draw updates

    class WorldVehicles
    {
        //Lets try a hashset for performance as we are looking for duplicates

        private List<Vehicle> activeVehicleList = new List<Vehicle>();
        private List<Vehicle> idleVehicleList = new List<Vehicle>();
        private HashSet<Vehicle> selectedVehicleList = new HashSet<Vehicle>();
        private List<Vehicle> draggingVehicleList = new List<Vehicle>();
        private HashSet<Vehicle>[] controlGroupList;

        public List<Vehicle> miniMapVehicleList { get; private set; } = new List<Vehicle>();


        private Vehicle snapCameraToVehicle;


        public WorldVehicles()
        {
            controlGroupList = new HashSet<Vehicle>[5];

            for (int i = 0; i < 5; i++)
            {
                controlGroupList[i] = new HashSet<Vehicle>();
            }
        }

        public void AddVehicle()
        {
            Dictionary<AttachmentType, Attachment> attachmentList = new Dictionary<AttachmentType, Attachment>();
            attachmentList.Add(AttachmentType.Headlight, new NormalBeams(111, 0.40f, LightType.HEADLIGHT));
            attachmentList.Add(AttachmentType.TailLight, new NormalBeams(119, 5.90f, LightType.TAILLIGHT));
            attachmentList.Add(AttachmentType.BrakeLight, new BrakeLights(119, 5.90f, LightType.BRAKELIGHT));

            attachmentList.Add(AttachmentType.Indicator1, new IndicatorLights(100, 0.98f, LightType.INDICATORSIDELEFT, LightType.INDICATORSIDERIGHT));

            attachmentList.Add(AttachmentType.Indicator2, new IndicatorLights(117, 5.97f, LightType.INDICATORBACKLEFT, LightType.INDICATORBACKRIGHT));

            attachmentList.Add(AttachmentType.EmergencyLight1, new EmergencyLights(52, 0.45f, LightType.EMERGENCYROOFRED, LightType.EMERGENCYROOFBLUE, 0));
            attachmentList.Add(AttachmentType.EmergencyLight2, new EmergencyLights(92, 5.35f, LightType.EMERGENCYSIDERED, LightType.EMERGENCYSIDEBLUE, 1));
            attachmentList.Add(AttachmentType.EmergencyLight3, new EmergencyLights(43, 3.70f, LightType.EMERGENCYSIDERED, LightType.EMERGENCYSIDEBLUE, 2));
            float scale = 0.75f;

            Sprite selectedSprite = new Sprite(new Rectangle(100, 0, 256, 128), TextureSheet.WorldUI);
            string name = "Standard Truck";

            Sprite sprite = new Sprite(new Rectangle(0, 0, 273, 103), TextureSheet.Vehicles, Color.White);

            VehicleSpecific vehicleSpecific = new VehicleSpecific(attachmentList, scale, sprite, selectedSprite, name, 1.3f, 0.005f);




            List<Point> straightRoadList = WorldController.GetDebugListStraightRoadTiles();

            //  activeVehicleList.Add(new Vehicle(vehicleSpecific, 174, 182, true, 100));  //500 map offroad

            //   int i = 0;
            int i = 0;

             while (i < 300)
             {
                 int p = GameController.rnd.Next(0, straightRoadList.Count);


                 Tile tile = WorldController.world.tileGrid[straightRoadList[p].X, straightRoadList[p].Y];

                  if (tile.vehicle == null)
                   {
             //   int x = GameController.rnd.Next(1, 999);
              //  int y = GameController.rnd.Next(1, 999);
              //  Vehicle vehicle = new Vehicle(vehicleSpecific, x, y, true, i);
                Vehicle vehicle = new Vehicle(vehicleSpecific, straightRoadList[p].X, straightRoadList[p].Y, true, i);
                    idleVehicleList.Add(vehicle);
                    AddVehicle(vehicle, true);
                     i++;
                 }

             }

            //   i = 0;




               for (int c = 0; c < 1; c++)
               {
                   foreach (Vehicle vehicle in activeVehicleList)
                   {
                       int p = GameController.rnd.Next(0, straightRoadList.Count);
   

                    Point tile = new Point(straightRoadList[p].X, straightRoadList[p].Y);
                       vehicle.NewMoveAction(tile, true);

                   }
               }
               

               // Sprite sprite1 = new Sprite(new Rectangle(0, 0, 273, 103), TextureSheet.Vehicles, Color.Yellow);
               //  VehicleSpecific vehicleSpecific1 = new VehicleSpecific(attachmentList, scale, sprite1, selectedSprite, name, 1.7f, 0.01f);

              /*    while (i < 2)
                   {
                       int p = GameController.rnd.Next(0, straightRoadList.Count);
                       Tile tile = WorldController.world.tileGrid[straightRoadList[p].X, straightRoadList[p].Y];

                       if (tile.vehicle == null)
                       {

                           Vehicle vehicle = new Vehicle(vehicleSpecific, straightRoadList[p].X, straightRoadList[p].Y, true, i);
                           AddVehicle(vehicle, true);
                           i++;
                       }

                   }*/




            //    Sprite sprite1 = new Sprite(new Rectangle(0, 0, 273, 103), TextureSheet.Vehicles, Color.Yellow);
            //   VehicleSpecific vehicleSpecific1 = new VehicleSpecific(attachmentList, scale, sprite1, selectedSprite, name, 1.7f, 0.01f);
            /*   int c = 0;

               for (int y = 190; y < 210; y += 2)
               {
                   if (GameController.rnd.Next(0, 2) == 0)
                   {
                       activeVehicleList.Add(new Vehicle(vehicleSpecific, 177, y, true, c));  //500 map offroad
                       c++;
                   }
                   else
                   {
                       activeVehicleList.Add(new Vehicle(vehicleSpecific1, 177, y, true, c));  //500 map offroad
                       c++;
                   }
               }
               */






           /*     activeVehicleList.Add(new Vehicle(vehicleSpecific, 170, 175, true, 100));  //500 map offroad
                activeVehicleList.Add(new Vehicle(vehicleSpecific, 171, 175, true, 0));  //500 map offroad
               activeVehicleList.Add(new Vehicle(vehicleSpecific, 172, 175, true, 0));  //500 map offroad
               activeVehicleList.Add(new Vehicle(vehicleSpecific, 173, 175, true, 0));  //500 map offroad
               activeVehicleList.Add(new Vehicle(vehicleSpecific, 174, 175, true, 0));  //500 map offroad
               activeVehicleList.Add(new Vehicle(vehicleSpecific, 175, 175, true, 0));  //500 map offroad
               activeVehicleList.Add(new Vehicle(vehicleSpecific, 176, 175, true, 0));  //500 map offroad
               activeVehicleList.Add(new Vehicle(vehicleSpecific, 177, 175, true, 0));  //500 map offroad
               activeVehicleList.Add(new Vehicle(vehicleSpecific, 178, 175, true, 0));  //500 map offroad
               activeVehicleList.Add(new Vehicle(vehicleSpecific, 179, 175, true, 0));  //500 map offroad
               */
        }

        public Vehicle GetFirstSelectedVehicle()
        {
            if (selectedVehicleList.Count > 0)
            {
                return selectedVehicleList.First();
            }
            return null;
        }

        private void AddVehicle(Vehicle vehicle, bool displayOnMiniMap)
        {
            activeVehicleList.Add(vehicle);

            if (displayOnMiniMap)
            {
                miniMapVehicleList.Add(vehicle);
            }
        }
      

        private Point GetTilePoint(Vector2 location, int offset)
        {
            int x = (int)(location.X / 128) + offset;
            int y = (int)(location.Y / 128) + offset;

            if (x < 0)
            {
                x = 0;
            }
            if (x > WorldController.world.worldWidth)
            {
                x = WorldController.world.worldWidth;
            }
            if (y < 0)
            {
                y = 0;
            }
            if (y > WorldController.world.worldHeight)
            {
                y = WorldController.world.worldHeight;
            }

            return new Point(x, y);
        }

        private int GetControlGroupPressed(Input input)
        {
            if (input.IsKeyMapPressed(KeyMap.SelectControlGroupOne)) { return 0; }
            if (input.IsKeyMapPressed(KeyMap.SelectControlGroupTwo)) { return 1; }
            if (input.IsKeyMapPressed(KeyMap.SelectControlGroupThree)) { return 2; }
            if (input.IsKeyMapPressed(KeyMap.SelectControlGroupFour)) { return 3; }
            if (input.IsKeyMapPressed(KeyMap.SelectControlGroupFive)) { return 4; }
            return -1;
        }

        //eventualy change this to add just an action type etc
        public void AddActionToSelectedVehicles(Input input, Point tile)
        {
            foreach (Vehicle vehicle in selectedVehicleList)
            {
                vehicle.NewMoveAction(input, tile);
            }
        }

        public void ModifyDraggingList(Input input, Vector2 topLeftV, Vector2 bottomRightV)
        {
            draggingVehicleList.Clear();
            Point topLeft = GetTilePoint(topLeftV, -2);
            Point bottomRight = GetTilePoint(bottomRightV, 2);

            for(int x = topLeft.X; x < bottomRight.X; x++)
            {
                for (int y = topLeft.Y; y < bottomRight.Y; y++)
                {
                    Vehicle vehicle = WorldController.world.tileGrid[x, y].vehicle;
                    if (vehicle != null)
                    {
                        if (IntersectsWithSelection(vehicle, topLeftV, bottomRightV))
                        {
                            draggingVehicleList.Add(WorldController.world.tileGrid[x, y].vehicle);
                        }
                    }
                }
            }
        }

        public bool CheckForCursorHover(Vector2 topLeftV, Vector2 bottomRightV)
        {
            Point topLeft = GetTilePoint(topLeftV, -2);
            Point bottomRight = GetTilePoint(bottomRightV, 2);

            for (int x = topLeft.X; x < bottomRight.X; x++)
            {
                for (int y = topLeft.Y; y < bottomRight.Y; y++)
                {
                    Vehicle vehicle = WorldController.world.tileGrid[x, y].vehicle;
                    if (vehicle != null)
                    {
                        if (IntersectsWithSelection(vehicle, topLeftV, bottomRightV))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        private bool IntersectsWithSelection(Vehicle vehicle, Vector2 topLeftV, Vector2 bottomRightV)
        {
            //Buffer selection
            topLeftV.X -= 20f;
            topLeftV.Y -= 20f;
            bottomRightV.X += 20f;
            bottomRightV.Y += 20f;

            //Cheap calculation if our selection box has intersected with the center of a vehicle
            Vector2 vehicleLocation = vehicle.GetPosition();
            if (vehicleLocation.X > topLeftV.X && vehicleLocation.X < bottomRightV.X && vehicleLocation.Y > topLeftV.Y && vehicleLocation.Y < bottomRightV.Y)
            {
                return true;
            }

            //Else we will do a rotated bounding box detection
            BoundingBox boundingBoxVehicle = new BoundingBox(vehicle.GetSize(), vehicleLocation, vehicle.GetDirectionRadian());
            BoundingBox boundingBoxSelection = new BoundingBox(topLeftV, bottomRightV);
            return Intersects.IsBoundingBoxCollision(boundingBoxVehicle, boundingBoxSelection);
        }

        public MouseDragUpResult ModifySelectedList(Input input, Vector2 topLeftV, Vector2 bottomRightV)
        {
            draggingVehicleList.Clear();
            if (input.IsKeyMapDown(KeyMap.AddUnitToSelection) || input.IsKeyMapDown(KeyMap.RemoveUnitFromSelection)) { }
            else
            {
                selectedVehicleList.Clear();
            }

            Point topLeft = GetTilePoint(topLeftV, -2);
            Point bottomRight = GetTilePoint(bottomRightV, 2);

            for (int x = topLeft.X; x < bottomRight.X; x++)
            {
                for (int y = topLeft.Y; y < bottomRight.Y; y++)
                {


                    Vehicle vehicle = WorldController.world.tileGrid[x, y].vehicle;
                    if (vehicle != null && IntersectsWithSelection(vehicle, topLeftV, bottomRightV))
                    {
                        if(input.IsKeyMapDown(KeyMap.RemoveUnitFromSelection))
                        {
                            if (selectedVehicleList.Contains(vehicle))
                            selectedVehicleList.Remove(vehicle);
                        }
                        else if (!selectedVehicleList.Contains(vehicle))
                        {
                            selectedVehicleList.Add(vehicle);
                        }                     
                    }
                }
            }

            if (selectedVehicleList.Count == 0)
            {
                return MouseDragUpResult.NONE;
            }
            else if (selectedVehicleList.Count == 1 && input.LeftButtonDoubleClick() || selectedVehicleList.Count == 1 && input.IsKeyMapDown(KeyMap.OpenInfoWindow))
            {
                return MouseDragUpResult.CLICKEDONEVEHICLE;
            }
            else
            {
                return MouseDragUpResult.COLLECTEDVEHICLES;
            }
        }

        private void UpdateSnapCameraToVehicle(Camera mainCamera)
        {
            if (snapCameraToVehicle != null)
            {
                mainCamera.CenterOn(snapCameraToVehicle.GetPosition());
            }
        }

        private void UpdateControlGroups(Input input)
        {
            if (input.IsKeyMapDown(KeyMap.CreateControlGroup))
            {
                int index = GetControlGroupPressed(input);
                if (index > -1)
                {
                    //Create a group
                    controlGroupList[index].Clear();

                    foreach (Vehicle vehicle in selectedVehicleList)
                    {                     
                        controlGroupList[index].Add(vehicle);
                    }
                    
                    
                }
            }
            else
            {                
                int index = GetControlGroupPressed(input);
                if (index > -1)
                {
                    //Recall a group
                    selectedVehicleList.Clear();

                    foreach (Vehicle vehicle in controlGroupList[index])
                    {
                        selectedVehicleList.Add(vehicle);                        
                    }
                }
            }
        }

        public void Update(Input input, Camera mainCamera)
        {

            UpdateControlGroups(input);


            if (input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.I))
            {
                selectedVehicleList.Clear();

                foreach (Vehicle vehicle in activeVehicleList)
                {
                    selectedVehicleList.Add(vehicle);
                }
            }

            if (input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.G))
            {
                if (snapCameraToVehicle != null)
                {
                    snapCameraToVehicle = null;
                }
                else if (selectedVehicleList.Count > 0)
                {
                    snapCameraToVehicle = selectedVehicleList.ElementAt(0);
                }
            }

            foreach (Vehicle vehicle in selectedVehicleList)
            {
                vehicle.UpdateIfSelected(input);
            }


            UpdateSnapCameraToVehicle(mainCamera);

            for (int i = 0; i < activeVehicleList.Count; i++)
            {
                activeVehicleList[i].Update();
            }
        }

        public void DrawSelectedVehiclesUI(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < draggingVehicleList.Count; i++)
            {
                draggingVehicleList[i].DrawDragging(spriteBatch);
            }

            foreach (Vehicle vehicle in selectedVehicleList)
            {
                vehicle.DrawSelected(spriteBatch);
            }
        }
    }
}
