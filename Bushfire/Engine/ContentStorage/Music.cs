using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine
{
    class Music
    {
        ContentManager content;
        private Dictionary<Track, Song> songContentList;

        public Music(ContentManager content)
        {
            this.content = content;
            songContentList = new Dictionary<Track, Song>();
            Load();
        }

        private void Load()
        {

        }

        public Song GetSong(Track track)
        {
            return songContentList[track];
        }
    }

    public enum Track
    {

    }
}