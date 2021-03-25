using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine
{
    class Textures
    {
        ContentManager content;
        private Dictionary<TextureSheet, Texture2D> spriteContentList;

        public Textures(ContentManager content)
        {
            this.content = content;
            spriteContentList = new Dictionary<TextureSheet, Texture2D>();
            Load();
        }

        private void Load()
        {
            spriteContentList.Add(TextureSheet.TextureColours, content.Load<Texture2D>(@"Textures/UI/TextureColours"));
            spriteContentList.Add(TextureSheet.Buttons, content.Load<Texture2D>(@"Textures/UI/Buttons"));
            spriteContentList.Add(TextureSheet.BackGround, content.Load<Texture2D>(@"Textures/UI/BackGround"));
            spriteContentList.Add(TextureSheet.Loading, content.Load<Texture2D>(@"Textures/UI/Loading"));
            spriteContentList.Add(TextureSheet.Editor, content.Load<Texture2D>(@"Textures/UI/Editor"));
            spriteContentList.Add(TextureSheet.Ground, content.Load<Texture2D>(@"Textures/Game/Ground"));
            spriteContentList.Add(TextureSheet.MapObjects, content.Load<Texture2D>(@"Textures/Game/MapObjects"));
            spriteContentList.Add(TextureSheet.Shadows, content.Load<Texture2D>(@"Textures/Game/Shadows"));
            spriteContentList.Add(TextureSheet.Lights, content.Load<Texture2D>(@"Textures/Game/Lights"));
            spriteContentList.Add(TextureSheet.Vehicles, content.Load<Texture2D>(@"Textures/Game/Vehicles"));
            spriteContentList.Add(TextureSheet.Particles, content.Load<Texture2D>(@"Textures/Game/Particles"));
            spriteContentList.Add(TextureSheet.WorldUI, content.Load<Texture2D>(@"Textures/Game/WorldUI"));
        }

        public Texture2D GetTexture(TextureSheet textureSheet)
        {
            return spriteContentList[textureSheet];
        }

        public Rectangle GetTextureSheetSize(TextureSheet textureSheet)
        {
            if (spriteContentList.ContainsKey(textureSheet))
            {
                return new Rectangle(0, 0, spriteContentList[textureSheet].Width, spriteContentList[textureSheet].Height);
            }
            else
            {
                return Rectangle.Empty;
            }
        }
    }

    enum TextureSheet
    {
        TextureColours,
        Buttons,
        BackGround,
        Loading,
        Ground,
        Editor,
        MapObjects,
        Shadows,
        Lights,
        Vehicles,
        Particles,
        WorldUI
    }
}
