using BushFire.Editor.Containers;
using BushFire.Game.Map.MapObjectComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Editor.Tech
{
    class EditorParams
    {
        public EditingMode editingMode { get; private set; }
        public ShadowSide shadowSide { get; set; }
        public int overallSize { get; set; }
        public int borderOverSize { get; set; }
        public Shadow hoverShadow { get; set; }
        public Piece samplePiece { get; set; }
        public float snap;
        public bool editingModeChanged { get; set; }

        public EditorParams()
        {
            editingMode = EditingMode.Building;
            shadowSide = ShadowSide.LEFT;
            overallSize = 30;
            borderOverSize = 5;
            snap = 1f;
        }

        public void ChangeEditingMode(EditingMode editingMode)
        {
            this.editingMode = editingMode;
            editingModeChanged = true;
        }

        public bool TileBuildingLegit(Spot spot)
        {
            if (spot.x >= 0 && spot.x < overallSize)
            {
                return spot.y >= 0 && spot.y < overallSize;
            }
            return false;
        }


        public bool TileShadowLegit(Spot spot)
        {
            if (spot.x >= 0 - borderOverSize && spot.x < overallSize + borderOverSize)
            {
                return spot.y >= 0 - borderOverSize && spot.y < overallSize + borderOverSize;
            }
            return false;
        }
    }



    enum EditingMode
    {
        Building,
        Shadows,
        Objects,
        DrivingPicker,
    }
}
