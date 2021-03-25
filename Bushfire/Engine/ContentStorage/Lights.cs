using BushFire.Engine.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.ContentStorage
{
    class Lights
    {
        ContentManager content;
        private Dictionary<LightType, Light> lightContentList;
        

        public Lights(ContentManager content)
        {
            this.content = content;
            lightContentList = new Dictionary<LightType, Light>();
            Load();
        }

        private void Load()
        {
            lightContentList.Add(LightType.STOPLIGHTRED, new Light(new Sprite(new Rectangle(0, 0, 300, 300), TextureSheet.Lights, new Vector2(150, 90)), new Sprite(new Rectangle(80,1430,92,32), TextureSheet.MapObjects, new Vector2(48,12)), 2f, false));
            lightContentList.Add(LightType.STOPLIGHTAMBER, new Light(new Sprite(new Rectangle(300, 0, 300, 300), TextureSheet.Lights, new Vector2(150, 90)), new Sprite(new Rectangle(172, 1430, 92, 32), TextureSheet.MapObjects, new Vector2(48, 12)), 2f, false));
            lightContentList.Add(LightType.STOPLIGHTGREEN, new Light(new Sprite(new Rectangle(600, 0, 300, 300), TextureSheet.Lights, new Vector2(150, 90)) , new Sprite(new Rectangle(264, 1430, 92, 32), TextureSheet.MapObjects, new Vector2(48, 12)), 2f, false));
            lightContentList.Add(LightType.STREETLIGHTWHITE, new Light(new Sprite(new Rectangle(0, 300, 300, 300), TextureSheet.Lights, new Vector2(150, 150)) , new Sprite(new Rectangle(0, 1430, 38, 38), TextureSheet.MapObjects), 2.5f, false));
            lightContentList.Add(LightType.STREETLIGHTYELLOW, new Light(new Sprite(new Rectangle(300, 300, 300, 300), TextureSheet.Lights, new Vector2(150, 150)), new Sprite(new Rectangle(38, 1430, 38, 38), TextureSheet.MapObjects), 4f, false));         
            lightContentList.Add(LightType.HEADLIGHT, new Light(new Sprite(new Rectangle(0, 600, 300, 300), TextureSheet.Lights, new Vector2(20, 150)), new Sprite(new Rectangle(0, 1480, 15, 30), TextureSheet.MapObjects), 2f, false));
            lightContentList.Add(LightType.EMERGENCYROOFRED, new Light(new Sprite(new Rectangle(0, 900, 300, 300), TextureSheet.Lights, new Vector2(150, 150)), new Sprite(new Rectangle(20, 1480, 32, 32), TextureSheet.MapObjects), 1.5f, false));
            lightContentList.Add(LightType.EMERGENCYROOFBLUE, new Light(new Sprite(new Rectangle(300, 900, 300, 300), TextureSheet.Lights, new Vector2(150, 150)), new Sprite(new Rectangle(60, 1480, 32, 32), TextureSheet.MapObjects), 1.5f, false));
            lightContentList.Add(LightType.EMERGENCYSIDERED, new Light(new Sprite(new Rectangle(150, 1200, 150, 150), TextureSheet.Lights, new Vector2(75, 75)), new Sprite(new Rectangle(100, 1480, 32, 32), TextureSheet.MapObjects), 1f, false));
            lightContentList.Add(LightType.EMERGENCYSIDEBLUE, new Light(new Sprite(new Rectangle(0, 1200, 150, 150), TextureSheet.Lights, new Vector2(75, 75)), new Sprite(new Rectangle(140, 1480, 32, 32), TextureSheet.MapObjects), 1f, false));

            lightContentList.Add(LightType.TAILLIGHT, new Light(new Sprite(new Rectangle(0, 1350, 150, 150), TextureSheet.Lights, new Vector2(75, 75)), new Sprite(new Rectangle(180, 1480, 32, 32), TextureSheet.MapObjects), 1f, false));
            lightContentList.Add(LightType.BRAKELIGHT, new Light(new Sprite(new Rectangle(150, 1350, 150, 150), TextureSheet.Lights, new Vector2(75, 75)), new Sprite(new Rectangle(220, 1480, 32, 32), TextureSheet.MapObjects), 1f, false));

            lightContentList.Add(LightType.INDICATORSIDELEFT, new Light(new Sprite(new Rectangle(300, 1200, 150, 150), TextureSheet.Lights, new Vector2(75, 75)), new Sprite(new Rectangle(260, 1480, 32, 32), TextureSheet.MapObjects), 1f, false));
            lightContentList.Add(LightType.INDICATORSIDERIGHT, new Light(new Sprite(new Rectangle(450, 1200, 150, 150), TextureSheet.Lights, new Vector2(75, 75)), new Sprite(new Rectangle(300, 1480, 32, 32), TextureSheet.MapObjects), 1f, false));

            lightContentList.Add(LightType.INDICATORBACKLEFT, new Light(new Sprite(new Rectangle(300, 1200, 150, 150), TextureSheet.Lights, new Vector2(75, 75)), new Sprite(new Rectangle(340, 1480, 32, 32), TextureSheet.MapObjects), 1f, false));
            lightContentList.Add(LightType.INDICATORBACKRIGHT, new Light(new Sprite(new Rectangle(450, 1200, 150, 150), TextureSheet.Lights, new Vector2(75, 75)), new Sprite(new Rectangle(340, 1480, 32, 32), TextureSheet.MapObjects), 1f, false));
        }

        public Light GetLight(LightType lightType)
        {
            return lightContentList[lightType];
        }
    }
  
    enum LightType
    {
        STOPLIGHTRED,
        STOPLIGHTAMBER,
        STOPLIGHTGREEN,
        STREETLIGHTWHITE,
        STREETLIGHTYELLOW,
        HEADLIGHT,
        TAILLIGHT,
        BRAKELIGHT,
        EMERGENCYROOFRED,
        EMERGENCYROOFBLUE,
        EMERGENCYSIDERED,
        EMERGENCYSIDEBLUE,
        INDICATORSIDELEFT,
        INDICATORSIDERIGHT,
        INDICATORBACKLEFT,
        INDICATORBACKRIGHT

    }
}
