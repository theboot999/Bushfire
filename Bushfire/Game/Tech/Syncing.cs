using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Tech
{
    class Syncing
    {
        int tileX;
        int tileY;
        SyncingAction syncingAction;

        public Syncing(int tileX, int tileY, SyncingAction syncingAction)
        {
            this.tileX = tileX;
            this.tileY = tileY;
            this.syncingAction = syncingAction;
        }
    }


    enum SyncingAction
    {
        SetTileOnFire,
        RemoveTileOnFire,
        SetTileOccupied,
        RemoveTileOccupied,
        
    }

}
