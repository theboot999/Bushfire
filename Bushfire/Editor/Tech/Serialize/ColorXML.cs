using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Editor.Tech
{
    [Serializable]
    class ColorXML
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;
        public string name;

        public ColorXML(byte r, byte g, byte b, byte a, string name)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
            this.name = name;
        }

        public ColorXML(Color color, string name)
        {
            r = color.R;
            g = color.G;
            b = color.B;
            a = color.A;
            this.name = name;
        }



        public Color ToColor()
        {
            return new Color(r, g, b, a);
        }
    }
}
