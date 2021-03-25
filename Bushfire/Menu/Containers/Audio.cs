using BushFire.Engine;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls;
using Microsoft.Xna.Framework;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls.Internal;
using System;
using System.Diagnostics;
using BushFire.Engine.Files;

namespace BushFire.Menu.Containers
{
    class Audio : Container
    {
        public Audio(Rectangle localLocation, DockType dockType) : base(localLocation, dockType, true)
        {
            name = "AudioSettings";

            alwaysOnTop = true;
            spriteBack = GraphicsManager.GetSpriteColour(3);
            drawSpriteBack = true;
            canChangeFocusOrder = false;

            AddBorder(2, Resizing.NONE, 3);
            AddHeading(50, "Audio Settings", GraphicsManager.GetSpriteFont(Font.OpenSans20Bold), Color.White, false, false, false, false, false, GraphicsManager.GetSpriteColour(6));
            


            SetSizeBounds(0, 0, 900, 2000);
            AddScrollV(15, 6, 20);
            AddButtons();
        }

        private void AddButtons()
        {
            int count = 0;
            ComboMenuCycle audio;

            foreach (VolumeType volumeType in (VolumeType[])Enum.GetValues(typeof(VolumeType)))
            {
                string name = volumeType.ToString();

                audio = new ComboMenuCycle(name, name, new Point(50, 60 + (count * 100)), false, true, true);

                for (int i = 0; i < 11; i++)
                {
                    float value = (float)i / 10f;
                    audio.AddCycleObject(new CycleObject(i.ToString(), value));

                    float currentVolume = AudioManager.GetVolume(volumeType);
                    if (currentVolume == value)
                    {
                        audio.SetLastIndex();
                    }
                }
                AddUiControl(audio);
                count++;
            }
        }

        private void UpdateButtons()
        {
            foreach (VolumeType volumeType in (VolumeType[])Enum.GetValues(typeof(VolumeType)))
            {
                string name = volumeType.ToString();

                ComboMenuCycle cycle = (ComboMenuCycle)GetUiControl(name);

                if (cycle != null && cycle.IsEitherPress())
                {
                    float value = (float)cycle.GetSelectedCycleObject();
                    AudioManager.ChangeVolume(volumeType, value);
                    Data.SaveSettings();
                }
            }
        }

        public override void Update(Input input)
        {
            base.Update(input);
            UpdateButtons();
        }
    }
}