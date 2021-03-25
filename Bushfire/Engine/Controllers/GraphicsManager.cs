using BushFire.Engine.ContentStorage;
using BushFire.Game.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.Controllers
{
    static class GraphicsManager
    {
        private static Textures texures;
        private static Fonts fonts;
        private static Cursors cursors;
        private static Effects effects;
        private static Lights lights;
        private static PrebuiltSprites prebuiltSprites;
        private static GameSprites gameSprites;
        

        public static void Init(ContentManager content)
        {
            texures = new Textures(content);
            fonts = new Fonts(content);
            cursors = new Cursors(content);
            effects = new Effects(content);
            lights = new Lights(content);
            prebuiltSprites = new PrebuiltSprites();
            gameSprites = new GameSprites();
        }
  
        public static Texture2D GetTextureSheet(TextureSheet textureSheet)
        {
            return texures.GetTexture(textureSheet);
        }

        public static Sprite GetSpriteColour(int id)
        {
            int column = (id % 10);
            int row = ((id - column) / 10);
            Rectangle location = new Rectangle(1 + (column * 3), 1 + (row * 3), 1, 1);
            return new Sprite(location, TextureSheet.TextureColours);
        }

        public static SpriteFont GetSpriteFont(Font font)
        {
            return fonts.GetSpriteFont(font);
        }

        public static Rectangle GetTextureSheetSize(TextureSheet textureSheet)
        {
            return texures.GetTextureSheetSize(textureSheet);
        }

        public static Sprite GetPreBuilt(PrebuiltSprite prebuiltSprite)
        {
            return prebuiltSprites.GetPreBuilt(prebuiltSprite);
        }

        public static Sprite GetGameSprite(GameSprite prebuiltSprite)
        {
            return gameSprites.GetGameSprite(prebuiltSprite);
        }
   

        public static MouseCursor GetMouseCursor(CursorType cursorType)
        {
            return cursors.GetMouseCursor(cursorType);
        }

        public static Effect GetEffect(EffectType effectType)
        {
            return effects.GetEffect(effectType);
        }

        public static Light GetLight(LightType lightType)
        {
            return lights.GetLight(lightType);
        }

        public static void TestContent()
        {
            TestCursors();
            TestFonts();
            TestSprites();
            TestEffects();
            TestLights();
        }

        private static void TestCursors()
        {
            foreach (CursorType cursorType in (CursorType[])Enum.GetValues(typeof(CursorType)))
            {
                MouseCursor test = GetMouseCursor(cursorType);
            }
        }

        private static void TestFonts()
        {
            foreach (Font font in (Font[])Enum.GetValues(typeof(Font)))
            {
                SpriteFont test = GetSpriteFont(font);
            }
        } 

        private static void TestSprites()
        {
            foreach (TextureSheet textureSheet in (TextureSheet[])Enum.GetValues(typeof(TextureSheet)))
            {
                Texture2D test = GetTextureSheet(textureSheet);
            }
        }

        private static void TestEffects()
        {
            foreach (EffectType effectType in (EffectType[])Enum.GetValues(typeof(EffectType)))
            {
                Effect test = GetEffect(effectType);
            }
        }


        private static void TestLights()
        {
            foreach (LightType lightType in (LightType[])Enum.GetValues(typeof(LightType)))
            {
                Light test = GetLight(lightType);
            }
        }

    }
}
