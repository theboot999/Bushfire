using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BushFire.Engine.Controllers;
using System.Diagnostics;

namespace BushFire.Engine
{
    class Sound
    {
        //Does this need a main volume or should that be a calculated thing
        //then an adjust volume
        //TODO: add volume control
        private SoundType effect;
        private VolumeType audioType;
        private SoundEffectInstance instance;
        private float volume;  //This is the ingame volume off what it should be (eg distance off the screen - not the master volume or the volume of the thing

        public Sound(SoundType effect)
        {
            this.effect = effect;
        }

        public void Play()
        {
            if (instance == null)
            {
                instance = AudioManager.GetSound(effect);
                volume = 1f;
            }

            if (instance != null && instance.State != SoundState.Playing)
            {
                instance.IsLooped = true;
                instance.Play();
            }
        }
           
        public void PlayOneOff(float volume)
        {
            SoundEffectInstance oneOff = AudioManager.GetSound(effect);

            if (oneOff != null)
            {
                oneOff.Volume = oneOff.Volume * volume;
                oneOff.IsLooped = false;
                oneOff.Play();
            }
        }

        public void Stop()
        {
            if (instance != null)
            {
                instance.Stop();
                instance = null;
            }
        }

        public void UpdatePlaying(bool isPlaying)
        {
            if (isPlaying)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }

        public void UpdateVolume(float volume)
        {
            this.volume = volume;
            if (instance != null)
            {
                instance.Volume = AudioManager.GetVolume(effect) * volume;  //Calculte main and specific by current volume of the sound.  Perhaps this could be in a change volume thing
            }
        }

    }

}
