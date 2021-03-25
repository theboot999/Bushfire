using BushFire.Engine.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine
{
    class Sprite
    {
        public Rectangle location { get; set; }   
        public Texture2D texture2D { get; set; }
        public Color color{ get; set; } = Color.White;
        public Color altColor { get; set; } = Color.White;
        public float rotation { get; set; }
        public Vector2 rotationCenter { get; set; }
		public SpriteEffects spriteEffect {get; set; } = SpriteEffects.None;
        public float drawLayer = 1f;
        public float transparency = 1f;
        public float scale = 1f;

        public Sprite(Rectangle location, TextureSheet textureSheet)
        {
            this.location = location;
            texture2D = GraphicsManager.GetTextureSheet(textureSheet);
            rotationCenter = new Vector2(location.Width / 2, location.Height / 2);
        }

        public Sprite(Rectangle location, TextureSheet textureSheet, Color color)
        {
            this.location = location;
            texture2D = GraphicsManager.GetTextureSheet(textureSheet);
            this.color = color;
            rotationCenter = new Vector2(location.Width / 2, location.Height / 2);
        }

        public Sprite(Rectangle location, Texture2D texture2D)
        {
            this.location = location;
            this.texture2D = texture2D;
            rotationCenter = new Vector2(location.Width / 2, location.Height / 2);
        }

        public Sprite(Rectangle location, TextureSheet textureSheet, Vector2 rotationCenter)
        {
            this.location = location;
            texture2D = GraphicsManager.GetTextureSheet(textureSheet);
            this.rotationCenter = rotationCenter;
        }

        public void Dispose()
        {
            //This is used for our created minimap sprites.  Normal sprites are disposed through the contentmanager
            texture2D.Dispose();
        }
    }
}
