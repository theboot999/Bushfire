using BushFire.Engine.Files;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.Controllers
{
    static class AudioManager
    {
        private static SoundEffects soundEffects;
        private static Music music;
        private static Dictionary<VolumeType, float> volumeList;

        public static void Init(ContentManager content)
        {
            soundEffects = new SoundEffects(content);
            music = new Music(content);
            volumeList = new Dictionary<VolumeType, float>();

            foreach (VolumeType volumeType in (VolumeType[])Enum.GetValues(typeof(VolumeType)))
            {
                float level = 0.5f;

                for (int i = 0; i < Data.settingsXML.volumeEnumList.Count; i++)
                {
                    if (volumeType == Data.settingsXML.volumeEnumList[i])       //if this enum was in the list
                    {
                        level = Data.settingsXML.volumeLevelList[i];    //set the volume to it
                    }
                }
                //if the enum was added after the save list.  default it to 0.5f;
                volumeList.Add(volumeType, level);
            }
        }

        public static void PlaySong(Track track, bool loop)
        {
            Song song = music.GetSong(track);

            if (song != null)
            {
                MediaPlayer.Play(song);
                MediaPlayer.IsRepeating = loop;
                MediaPlayer.Volume = volumeList[VolumeType.Music] * volumeList[VolumeType.Master];
            }
        }

        public static SoundEffectInstance GetSound(SoundType soundType)
        {
            SoundEffectInstance instance = soundEffects.GetSound(soundType);

            if (instance != null)
            {
                VolumeType volumeType = soundEffects.GetVolumeType(soundType);
                instance.Volume = volumeList[volumeType] * volumeList[VolumeType.Master];
            }
            return instance;
        }

        public static void ChangeVolume(VolumeType volumeType, float value)
        {
            volumeList[volumeType] = value;
            VolumeSettingsChange();
        }

        private static void VolumeSettingsChange()
        {
            soundEffects.VolumeSettingsChange(volumeList);
            MediaPlayer.Volume = volumeList[VolumeType.Music] * volumeList[VolumeType.Master];
        }

        public static float GetVolume(SoundType soundType)
        {
            return volumeList[soundEffects.GetVolumeType(soundType)] * volumeList[VolumeType.Master];
        }


        public static float GetVolume(VolumeType volumeType)
        {
            if (volumeList.ContainsKey(volumeType))
            {
                return volumeList[volumeType];
            }
            else
            {
                return 0.5f;
            }            
        }

        public static void TestContent()
        {
            TestTracks();
            TestSoundEffects();
        }

        private static void TestTracks()
        {
            foreach (Track track in (Track[])Enum.GetValues(typeof(Track)))
            {
                Song test = music.GetSong(track);
            }
        }

        private static void TestSoundEffects()
        {
            foreach (SoundType soundType in (SoundType[])Enum.GetValues(typeof(SoundType)))
            {
                SoundEffectInstance test = GetSound(soundType);
            }
        }

        public static void Unload()
        {
            soundEffects.Unload();
        }
    }

    public enum VolumeType
    {
        Master,
        Game,
        Interface,
        Sirens,
        Music,
        Test1,
        Test2,
        Test3,
        Test46,
        Wow,
        YES,
        WOW1,
        wow2
    }

}
