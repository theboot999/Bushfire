using BushFire.Editor.Containers;
using BushFire.Engine;
using BushFire.Game;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.Storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Editor.Tech
{
    class BorderMap
    {
        Border[,] normalBorderMap;
        List<GroundLayer> roadList;
        Border highlight;
        EditorParams editorParams;

        CompressedBuilding compressedBuilding;

        public BorderMap(CompressedBuilding compressedBuilding, EditorParams editorParams)
        {
            this.editorParams = editorParams;
            this.compressedBuilding = compressedBuilding;
            normalBorderMap = new Border[editorParams.overallSize, editorParams.overallSize];
            roadList = new List<GroundLayer>();

            highlight = new Border(11, 0, 0, 5);
            highlight.visible = false;

            for (int x = 0; x < editorParams.overallSize; x++)
            {
                for (int y = 0; y < editorParams.overallSize; y++)
                {
                    normalBorderMap[x, y] = new Border(20, x, y, 2);
                }
            }

            for (int x = 0; x < editorParams.overallSize; x++)
            {
                if (compressedBuilding.baseDirectionFacing == 0)
                {

                    roadList.Add(GroundLayerController.GetLayerByIndex(LayerType.CITYROAD, 2)); //North
                }
                else
                {
                    roadList.Add(GroundLayerController.GetLayerByIndex(LayerType.CITYROAD, 1));  //West
                }
            }
        }


        public void UpdateHighlight(Spot spot)
        {
            highlight.visible = false;

            if (spot != null)
            {
                if (editorParams.TileBuildingLegit(spot))
                {
                    highlight.SetNewSpot(spot);
                    highlight.visible = true;
                }

                if (editorParams.TileShadowLegit(spot))
                {
                    highlight.SetNewSpot(spot);
                    highlight.visible = true;
                }

            }
        }

        public void Update(Input input)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawNormalBorder(spriteBatch);
            DrawRoad(spriteBatch);
            highlight.Draw(spriteBatch);
        }


        private void DrawNormalBorder(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < compressedBuilding.usingWidth; x++)
            {
                for (int y = 0; y < compressedBuilding.usingHeight; y++)
                {

                    normalBorderMap[x, y].Draw(spriteBatch);
                }
            }
        }
     
        private void DrawRoad(SpriteBatch spriteBatch)
        {
            

            if (compressedBuilding.baseDirectionFacing == 0)
            {
                int x = 0;
                foreach (GroundLayer layer in roadList)
                {
                    layer.DrawMiniMapTile(spriteBatch, new Rectangle(x * GroundLayerController.tileSize + GroundLayerController.halfTileSize, -GroundLayerController.halfTileSize, GroundLayerController.tileSize, GroundLayerController.tileSize));
                    x++;
                }
            }
            else
            {
                int y = 0;
                foreach (GroundLayer layer in roadList)
                {
                    layer.DrawMiniMapTile(spriteBatch, new Rectangle(-GroundLayerController.halfTileSize, y * GroundLayerController.tileSize + GroundLayerController.halfTileSize, GroundLayerController.tileSize, GroundLayerController.tileSize));
                    y++;
                }
            }
        }

    }
}
