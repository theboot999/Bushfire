using BushFire.Editor.Tech;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Game;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.Map.MapObjects;
using BushFire.Game.Storage;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.MapObjects
{
    class Building : MapObjectPropertiesMultiple
    {

        public int directionFacing;
        public List<Color> availableColorsList;

        public Building(Piece[,] pieceMap, List<Shadow> shadowListLeft, List<Shadow> shadowListRight, int directionFacing, int width, int height, int elevation, List<Color>availableColorsList, int possibleInTileShift) : base(pieceMap, shadowListLeft, shadowListRight, width, height, elevation, Map.MapObjectType.BUILDING, false, possibleInTileShift)
        {
            this.availableColorsList = availableColorsList;
            this.directionFacing = directionFacing;
        }

        public Color GetColorFromAvailable(Random rnd)
        {
            if (availableColorsList.Count > 0)
            {
                return availableColorsList[rnd.Next(0, availableColorsList.Count)];
            }
            else
            {
                return Color.Black;
            }

        }

    
    }
}
