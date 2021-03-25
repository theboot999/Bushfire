using BushFire.Editor.Tech;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.Storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map.MapObjects
{
    class Tree : MapObjectPropertiesSingle
    {

        public Tree(Piece piece, Shadow shadowLeft, Shadow shadowRight, Point shadowPoint, int elevation, int possibleInTileShift) : base(elevation, MapObjectType.TREE, true, possibleInTileShift)
        {
            this.piece = piece;
            this.shadowLeft = shadowLeft;
            this.shadowRight = shadowRight;
         //   this.shadowPoint = shadowPoint;
            this.elevation = elevation;
        }




    }
}
