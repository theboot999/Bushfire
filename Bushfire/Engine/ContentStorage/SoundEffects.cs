using BushFire.Engine.Controllers;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine
{
    class SoundEffects
    {
        //i think the soundeffect instance and the audio type should be all stored in the audio controller


        ContentManager content;
      

        private Dictionary<SoundType, List<SoundEffectInstance>> effectPool;
        private Dictionary<SoundType, VolumeType> volumeTypeList;

        public SoundEffects(ContentManager content)
        {
            this.content = content;
            effectPool = new Dictionary<SoundType, List<SoundEffectInstance>>();
            volumeTypeList = new Dictionary<SoundType, VolumeType>();
            Load();
        }

        private void Load()
        {
            //Dont use VolumeType Mastervolume for individual effects
            //Possible Concurrent is important.  Having to many concurrent will make sounds sound bad
            LoadSound(SoundType.Effect1, VolumeType.Game, @"SoundsEffects\Effect1", 10);
            LoadSound(SoundType.Effect2, VolumeType.Interface, @"SoundsEffects\Effect2", 1);
            LoadSound(SoundType.Effect3, VolumeType.Interface, @"SoundsEffects\Effect3", 2);
            LoadSound(SoundType.Shot1, VolumeType.Game, @"SoundsEffects\Shot1", 20);
            LoadSound(SoundType.Siren1, VolumeType.Sirens, @"SoundsEffects\Siren1", 3);
        }

        private void LoadSound(SoundType soundType, VolumeType audioType, string address, int possibleConcurrent)
        {
            SoundEffect soundEffect = content.Load<SoundEffect>(address);
            volumeTypeList.Add(soundType, audioType);

            List<SoundEffectInstance> myList = new List<SoundEffectInstance>();

            for (int i = 0; i < possibleConcurrent; i++)
            {
                myList.Add(soundEffect.CreateInstance());
            }
            effectPool.Add(soundType, myList);
        }

        public SoundEffectInstance GetSound(SoundType soundType)
        {
            List<SoundEffectInstance> myList = effectPool[soundType];

            foreach (SoundEffectInstance instance in myList)
            {
                if (instance.State == SoundState.Stopped)
                {
                    return instance;
                }
            }
            return null;
        }

        public VolumeType GetVolumeType(SoundType soundType)
        {
            return volumeTypeList[soundType];
        }

        //Use this on menu change.  Will adjust all the volumes to suit
        public void VolumeSettingsChange(Dictionary<VolumeType, float> volumeList)
        {
            float master = volumeList[VolumeType.Master];

            foreach (KeyValuePair<SoundType, List<SoundEffectInstance>> instanceList in effectPool)
            {
                foreach (SoundEffectInstance instance in instanceList.Value)
                {
                    instance.Volume = volumeList[volumeTypeList[instanceList.Key]] * master;
                }
            }
        }

        public void Unload()
        {
            foreach (List<SoundEffectInstance> myList in effectPool.Values)
            {
                foreach (SoundEffectInstance soundEffectInstance in myList)
                {
                    soundEffectInstance.Dispose();
                }
            }
        }
    }

    enum SoundType
    {
        Effect1,
        Effect2,
        Effect3,
        Shot1,
        Siren1
    }

}
