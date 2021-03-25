using BushFire.Engine.Controllers;
using BushFire.Game.Controllers;
using BushFire.Game.Tech;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Vehicles
{
    class VehicleParameters
    {
        //
        public bool lightEmergencyOn { get; set; }
        public bool lightGeneralOn { get; set; }
        public bool lightBrakingOn { get; set; }

        public Direction indicatorDirection { get; set; }
        public bool isIndicatorFlash { get; private set; }
        private float indicatorCounter { get; set; }

        public bool[] isRedLightEmergency { get; private set; }
        public bool[] isBlueLightEmergency { get; private set; }

        private float[] emergencyRedCounter { get; set; }
        private float[] emergencyBlueCounter { get; set; }
        private float[] lightTiming { get; set; }

        public bool isBraking { get; set; }
        private float brakeCounter { get; set; }

        public readonly Vehicle vehicle;

        public float currentSpeedPercentage;

        public int directionTravelling;

        public Vehicle followingVehicle;

        public int debugNum;


        public VehicleParameters(bool hasEmergencyLights, Vehicle vehicle)
        {
            this.vehicle = vehicle;

            if (hasEmergencyLights)
            {
                int numberOfLights = 3;
                isRedLightEmergency = new bool[numberOfLights];
                isBlueLightEmergency = new bool[numberOfLights];
                emergencyRedCounter = new float[numberOfLights];
                emergencyBlueCounter = new float[numberOfLights];
                lightTiming = new float[numberOfLights];


                for (int i = 0; i < numberOfLights; i++)
                {
                    isRedLightEmergency[i] = GameController.rnd.Next(0, 2) == 1;

                    lightTiming[i] = GameController.rnd.Next(700, 900) / 1000f;

                    if (isRedLightEmergency[i])
                    {
                        emergencyBlueCounter[i] = 1;
                        emergencyRedCounter[i] = (float)GameController.rnd.Next(300, 900) / 1000f;
                    }
                    else
                    {
                        emergencyRedCounter[i] = 1;
                        emergencyBlueCounter[i] = (float)GameController.rnd.Next(300, 900) / 1000f;
                        isBlueLightEmergency[i] = true;
                    }
                }
            }
        }

        //Draw updates only get called during drawing and only updates vehicles that are on the screen
        public void UpdateDraw()
        {
            //Emergency Lights
            if (lightEmergencyOn)
            {
                for (int i = 0; i < 3; i++)
                {
                    emergencyRedCounter[i] -= EngineController.drawUpdateTime * 0.05f;

                    if (emergencyRedCounter[i] < 0)
                    {
                        emergencyRedCounter[i] = lightTiming[i];
                        isRedLightEmergency[i] = !isRedLightEmergency[i];
                    }

                    emergencyBlueCounter[i] -= EngineController.drawUpdateTime * 0.05f;

                    if (emergencyBlueCounter[i] < 0)
                    {
                        emergencyBlueCounter[i] = lightTiming[i];
                        isBlueLightEmergency[i] = !isBlueLightEmergency[i];
                    }
                }
            }

            //Indicators
            if (indicatorDirection == Direction.LEFT || indicatorDirection == Direction.RIGHT)
            {
                indicatorCounter -= EngineController.drawUpdateTime * 0.03f;
                if (indicatorCounter < 0)
                {
                    indicatorCounter = 1;
                    isIndicatorFlash = !isIndicatorFlash;
                }
            }

            //Brakes
            if (isBraking)
            {
                brakeCounter += EngineController.drawUpdateTime * 0.3f;
            }
            else if (brakeCounter > 0)
            {
                brakeCounter -= EngineController.drawUpdateTime * 0.05f;
            }

            if (brakeCounter > 1)
            {
                lightBrakingOn = true;
                if (brakeCounter > 3)
                {
                    brakeCounter = 3;
                }
            }
            else
            {
                lightBrakingOn = false;
            }
         }
    }
}
