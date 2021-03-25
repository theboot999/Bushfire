using BushFire.Engine;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls;
using Microsoft.Xna.Framework;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Internal;
using System;
using System.Diagnostics;
using BushFire.Engine.Files;
using System.Text;

namespace BushFire.MapGeneration.Containers
{

    
    class LoadingInfo : Container
    {
        float percentJumpPerStep;
        int numberOfStepCounts;
        LoadingTypeClass[] loadingTypeList;

        public LoadingInfo(Rectangle localLocation) : base(localLocation, DockType.CENTERSCREENBOTH, true)
        {
            AddUiControl(new Label("Loading", Font.OpenSans40Bold, Color.White, new Vector2(localLocation.Width / 2, 100), true, "Creating New Map"));
            AddUiControl(new Label("DoingLabel", Font.OpenSans24Bold, Color.White, new Vector2(localLocation.Width / 2, 800), true, ""));
            AddUiControl(new Label("PercentLabel", Font.OpenSans24Bold, Color.White, new Vector2(localLocation.Width / 2, 850), true, ""));
            AddUiControl(new Label("TotalLabelPercent", Font.OpenSans24Bold, Color.White, new Vector2(localLocation.Width / 2, 750), true, ""));
            // drawSpriteBack = true;
            // drawSpriteBack = true;
            //  spriteBack = GraphicsManager.GetSpriteColour(10);
            Set();     
        }

        private void Set()
        {
            loadingTypeList = new LoadingTypeClass[16];
            loadingTypeList[0] = new LoadingTypeClass(LoadingType.CreatingPerlinNoise, 100);
            loadingTypeList[1] = new LoadingTypeClass(LoadingType.BuildingLandMass, 20);
            loadingTypeList[2] = new LoadingTypeClass(LoadingType.SmoothingLandMass, 70);
            loadingTypeList[3] = new LoadingTypeClass(LoadingType.BuildingShrunkWorld, 2);
            loadingTypeList[4] = new LoadingTypeClass(LoadingType.PlottingTownPoints, 2);
            loadingTypeList[5] = new LoadingTypeClass(LoadingType.ExpandingTowns, 2);
            loadingTypeList[6] = new LoadingTypeClass(LoadingType.ConnectingTowns, 2);
            loadingTypeList[7] = new LoadingTypeClass(LoadingType.SmoothingTowns, 2);
            loadingTypeList[8] = new LoadingTypeClass(LoadingType.ExpandingShrunkWorld, 2);
            loadingTypeList[9] = new LoadingTypeClass(LoadingType.AddingTownBuildings, 2);
            loadingTypeList[10] = new LoadingTypeClass(LoadingType.AddingRoadTiles, 1);
            loadingTypeList[11] = new LoadingTypeClass(LoadingType.AddingIntersections, 1);
            loadingTypeList[12] = new LoadingTypeClass(LoadingType.FixingIntersections, 1);
            loadingTypeList[13] = new LoadingTypeClass(LoadingType.AddingStreetLights, 1);
            loadingTypeList[14] = new LoadingTypeClass(LoadingType.AddingTrees, 10);
            loadingTypeList[15] = new LoadingTypeClass(LoadingType.CreatingMiniMap, 100);

            numberOfStepCounts = 0;

            foreach (LoadingTypeClass loadingTypeClass in loadingTypeList)
            {
                numberOfStepCounts += loadingTypeClass.stepCount;
            }

            percentJumpPerStep = 100f / numberOfStepCounts;
        }


        public void UpdateLoading(LoadingType loadingType, float percentDone)
        {
            if (percentDone > 99.5f) { percentDone = 100; }

            int index = (int)loadingType;

            float totalPercentDone = 0;

            for (int i = 0; i < index; i++)
            {
                totalPercentDone += (loadingTypeList[i].stepCount * percentJumpPerStep);
            }

            float extra = MathHelper.Lerp(0, loadingTypeList[index].stepCount * percentJumpPerStep, percentDone * 0.01f);
            totalPercentDone += extra;

            GetUiControl("DoingLabel").SetText(GetEnumWithSpaces(loadingType));
            GetUiControl("PercentLabel").SetText(Convert.ToString((int)percentDone) + "%");
            GetUiControl("TotalLabelPercent").SetText(Convert.ToString((int)totalPercentDone) + "%");
        }


        public string GetEnumWithSpaces(LoadingType loadingType)
        {
            string text = loadingType.ToString();

            if (string.IsNullOrWhiteSpace(text))
                return "";
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        private class LoadingTypeClass
        {
            public LoadingType loadingType;
            public int stepCount;

            public LoadingTypeClass(LoadingType loadingType, int stepCount)
            {
                this.loadingType = loadingType;
                this.stepCount = stepCount;
            }
        }
    }

    enum LoadingType : int
    {
        CreatingPerlinNoise = 0,
        BuildingLandMass = 1,
        SmoothingLandMass = 2,
        BuildingShrunkWorld = 3,
        PlottingTownPoints = 4,
        ExpandingTowns = 5,
        ConnectingTowns = 6,
        SmoothingTowns = 7,
        ExpandingShrunkWorld = 8,
        AddingTownBuildings = 9,
        AddingRoadTiles = 10,
        AddingIntersections = 11,
        FixingIntersections = 12,
        AddingStreetLights = 13,
        AddingTrees = 14,
        CreatingMiniMap = 15,
    }
}
