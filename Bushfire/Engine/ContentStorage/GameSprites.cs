using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.ContentStorage
{
    class GameSprites
    {
        Dictionary<GameSprite, Sprite> gameSpriteList;

        public GameSprites()
        {
            gameSpriteList = new Dictionary<GameSprite, Sprite>();
            Add();
        }

        private void Add()
        {
            gameSpriteList.Add(GameSprite.Smoke1, new Sprite(new Rectangle(0, 0, 50, 50), TextureSheet.Particles));
            gameSpriteList.Add(GameSprite.Smoke2, new Sprite(new Rectangle(500, 0, 50, 50), TextureSheet.Particles));
            gameSpriteList.Add(GameSprite.Smoke3, new Sprite(new Rectangle(100, 0, 50, 50), TextureSheet.Particles));
            gameSpriteList.Add(GameSprite.Smoke4, new Sprite(new Rectangle(150, 0, 50, 50), TextureSheet.Particles));
            gameSpriteList.Add(GameSprite.Smoke5, new Sprite(new Rectangle(200, 0, 50, 50), TextureSheet.Particles));
            gameSpriteList.Add(GameSprite.Smoke6, new Sprite(new Rectangle(250, 0, 50, 50), TextureSheet.Particles));
            gameSpriteList.Add(GameSprite.Smoke7, new Sprite(new Rectangle(300, 0, 50, 50), TextureSheet.Particles));
            gameSpriteList.Add(GameSprite.Smoke8, new Sprite(new Rectangle(350, 0, 50, 50), TextureSheet.Particles));
            gameSpriteList.Add(GameSprite.RedCircle, new Sprite(new Rectangle(0, 128, 32, 32), TextureSheet.Particles));
        }

        public Sprite GetGameSprite(GameSprite prebuiltSprite)
        {
            return gameSpriteList[prebuiltSprite];
        }

    }


    enum GameSprite
    {
        Smoke1,
        Smoke2,
        Smoke3,
        Smoke4,
        Smoke5,
        Smoke6,
        Smoke7,
        Smoke8,
        RedCircle
    }
}
