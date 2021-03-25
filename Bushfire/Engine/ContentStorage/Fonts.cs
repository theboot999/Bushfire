using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BushFire.Engine
{
    class Fonts
    {
        ContentManager content;
        private Dictionary<Font, SpriteFont> fontContentList;

        public Fonts(ContentManager content)
        {
            fontContentList = new Dictionary<Font, SpriteFont>();
            this.content = content;
            Load();
        }

        public SpriteFont GetSpriteFont(Font font)
        {
            return fontContentList[font];
        }

        private void Load()
        {
            fontContentList.Add(Font.OpenSans6, content.Load<SpriteFont>(@"Fonts/OpenSans6"));
            fontContentList.Add(Font.OpenSans8, content.Load<SpriteFont>(@"Fonts/OpenSans8"));
            fontContentList.Add(Font.OpenSans10, content.Load<SpriteFont>(@"Fonts/OpenSans10"));
            fontContentList.Add(Font.OpenSans12, content.Load<SpriteFont>(@"Fonts/OpenSans12"));
            fontContentList.Add(Font.OpenSans14, content.Load<SpriteFont>(@"Fonts/OpenSans14"));
            fontContentList.Add(Font.OpenSans16, content.Load<SpriteFont>(@"Fonts/OpenSans16"));
            fontContentList.Add(Font.OpenSans18, content.Load<SpriteFont>(@"Fonts/OpenSans18"));
            fontContentList.Add(Font.OpenSans20, content.Load<SpriteFont>(@"Fonts/OpenSans20"));
            fontContentList.Add(Font.OpenSans22, content.Load<SpriteFont>(@"Fonts/OpenSans22"));
            fontContentList.Add(Font.OpenSans24, content.Load<SpriteFont>(@"Fonts/OpenSans24"));
            fontContentList.Add(Font.OpenSans26, content.Load<SpriteFont>(@"Fonts/OpenSans26"));
            fontContentList.Add(Font.OpenSans28, content.Load<SpriteFont>(@"Fonts/OpenSans28"));
            fontContentList.Add(Font.OpenSans30, content.Load<SpriteFont>(@"Fonts/OpenSans30"));
            fontContentList.Add(Font.OpenSans38, content.Load<SpriteFont>(@"Fonts/OpenSans38"));

            fontContentList.Add(Font.OpenSans16Bold, content.Load<SpriteFont>(@"Fonts/OpenSans16Bold"));
            fontContentList.Add(Font.OpenSans18Bold, content.Load<SpriteFont>(@"Fonts/OpenSans18Bold"));
            fontContentList.Add(Font.OpenSans20Bold, content.Load<SpriteFont>(@"Fonts/OpenSans20Bold"));
            fontContentList.Add(Font.OpenSans22Bold, content.Load<SpriteFont>(@"Fonts/OpenSans22Bold"));
            fontContentList.Add(Font.OpenSans24Bold, content.Load<SpriteFont>(@"Fonts/OpenSans24Bold"));
            fontContentList.Add(Font.OpenSans40Bold, content.Load<SpriteFont>(@"Fonts/OpenSans40Bold"));

            fontContentList.Add(Font.Anita6, content.Load<SpriteFont>(@"Fonts/Anita12"));
            fontContentList.Add(Font.Anita8, content.Load<SpriteFont>(@"Fonts/Anita12"));
            fontContentList.Add(Font.Anita10, content.Load<SpriteFont>(@"Fonts/Anita12"));
            fontContentList.Add(Font.Anita12, content.Load<SpriteFont>(@"Fonts/Anita12"));
            fontContentList.Add(Font.Anita14, content.Load<SpriteFont>(@"Fonts/Anita14"));
            fontContentList.Add(Font.Anita16, content.Load<SpriteFont>(@"Fonts/Anita16"));
            fontContentList.Add(Font.Anita18, content.Load<SpriteFont>(@"Fonts/Anita18"));
            fontContentList.Add(Font.Anita20, content.Load<SpriteFont>(@"Fonts/Anita20"));
            fontContentList.Add(Font.Anita22, content.Load<SpriteFont>(@"Fonts/Anita22"));
            fontContentList.Add(Font.Anita24, content.Load<SpriteFont>(@"Fonts/Anita24"));
            fontContentList.Add(Font.Anita26, content.Load<SpriteFont>(@"Fonts/Anita24"));
            fontContentList.Add(Font.Anita28, content.Load<SpriteFont>(@"Fonts/Anita24"));
            fontContentList.Add(Font.Anita30, content.Load<SpriteFont>(@"Fonts/Anita24"));

            fontContentList.Add(Font.CarterOne6, content.Load<SpriteFont>(@"Fonts/CarterOne12"));
            fontContentList.Add(Font.CarterOne8, content.Load<SpriteFont>(@"Fonts/CarterOne12"));
            fontContentList.Add(Font.CarterOne10, content.Load<SpriteFont>(@"Fonts/CarterOne12"));
            fontContentList.Add(Font.CarterOne12, content.Load<SpriteFont>(@"Fonts/CarterOne12"));
            fontContentList.Add(Font.CarterOne13, content.Load<SpriteFont>(@"Fonts/CarterOne13"));
            fontContentList.Add(Font.CarterOne14, content.Load<SpriteFont>(@"Fonts/CarterOne14"));
            fontContentList.Add(Font.CarterOne16, content.Load<SpriteFont>(@"Fonts/CarterOne16"));
            fontContentList.Add(Font.CarterOne18, content.Load<SpriteFont>(@"Fonts/CarterOne18"));
            fontContentList.Add(Font.CarterOne20, content.Load<SpriteFont>(@"Fonts/CarterOne20"));
            fontContentList.Add(Font.CarterOne22, content.Load<SpriteFont>(@"Fonts/CarterOne22"));
            fontContentList.Add(Font.CarterOne24, content.Load<SpriteFont>(@"Fonts/CarterOne24"));
            fontContentList.Add(Font.CarterOne26, content.Load<SpriteFont>(@"Fonts/CarterOne24"));
            fontContentList.Add(Font.CarterOne28, content.Load<SpriteFont>(@"Fonts/CarterOne24"));
            fontContentList.Add(Font.CarterOne30, content.Load<SpriteFont>(@"Fonts/CarterOne24"));
        }
    }

    enum Font
    {
        OpenSans6,
        OpenSans8,
        OpenSans10,
        OpenSans12,
        OpenSans14,
        OpenSans16,
        OpenSans18,
        OpenSans20,
        OpenSans22,     
        OpenSans24,
        OpenSans26,
        OpenSans28,
        OpenSans30,
        OpenSans38,
        OpenSans16Bold,
        OpenSans18Bold,
        OpenSans20Bold,
        OpenSans22Bold,
        OpenSans24Bold,
        OpenSans40Bold,
        Anita6,
        Anita8,
        Anita10,
        Anita12,
        Anita14,
        Anita16,
        Anita18,
        Anita20,
        Anita22,
        Anita24,
        Anita26,
        Anita28,
        Anita30,
        CarterOne6,
        CarterOne8,
        CarterOne10,
        CarterOne12,
        CarterOne13,
        CarterOne14,
        CarterOne16,
        CarterOne18,
        CarterOne20,
        CarterOne22,
        CarterOne24,
        CarterOne26,
        CarterOne28,
        CarterOne30
    }
}
