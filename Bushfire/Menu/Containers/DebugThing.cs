using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Menu.Containers
{
    class DebugThing : Container
    {
      //  private float updateTimer;

            //this is really dumb we need to get rid of the updates in the timer class
            //and basically do the updates here for calculating output

        public DebugThing() : base(new Rectangle(10, 10, 350, 450), DockType.TOPLEFTFIXEDBOTH, true)
        {
      //      updateTimer = 1f;
            name = "Debug";
            alwaysOnTop = true;
            canChangeFocusOrder = false;

            AddUiControl(new Label("UPDATEFPS", Font.Anita14, Color.SkyBlue, new Vector2(10, 15), false, ""));
            AddUiControl(new Label("DRAWFPS", Font.Anita14, Color.SkyBlue, new Vector2(10, 35), false, ""));
            AddUiControl(new Label("AVERAGEFPS", Font.Anita14, Color.SkyBlue, new Vector2(10, 55), false, ""));
            AddUiControl(new Label("SCALE", Font.Anita14, Color.SkyBlue, new Vector2(10, 75), false, ""));
            AddUiControl(new Label("SKIPPEDFRAMES", Font.Anita14, Color.SkyBlue, new Vector2(10, 95), false, ""));
            AddUiControl(new Label("MEMORYUSE", Font.Anita14, Color.SkyBlue, new Vector2(10, 115), false, ""));
            AddUiControl(new Label("GCCOUNT", Font.Anita14, Color.SkyBlue, new Vector2(10, 135), false, ""));
            AddUiControl(new Label("MAINTHREADTIME", Font.Anita14, Color.SkyBlue, new Vector2(10, 155), false, ""));
            AddUiControl(new Label("SECONDTHREADTIME", Font.Anita14, Color.SkyBlue, new Vector2(10, 175), false, ""));
            AddUiControl(new Label("DRAWTIME", Font.Anita14, Color.SkyBlue, new Vector2(10, 195), false, ""));
            AddUiControl(new Label("TILESONSCREEN", Font.Anita14, Color.SkyBlue, new Vector2(10, 215), false, ""));
            AddUiControl(new Label("UPDATEFIRETILES", Font.Anita14, Color.SkyBlue, new Vector2(10, 235), false, ""));
            AddUiControl(new Label("UPDATEMINIFIRETILES", Font.Anita14, Color.SkyBlue, new Vector2(10, 255), false, ""));
        }

        private float counter;


        public override void Update(Input input)
        {
            // if (DisplayController.showDebugWindow)
            //  {
            SetControlText("UPDATEFPS", "Update Fps: " + Convert.ToString((int)EngineController.updateFrameRate));
            SetControlText("DRAWFPS", "Draw Fps: " + Convert.ToString((int)EngineController.drawFrameRate));
            SetControlText("AVERAGEFPS", "Av Update Fps: " + Convert.ToString((int)EngineController.averageFrameRate));
            SetControlText("SCALE", "Scale: " + DisplayController.uiScale.ToString());
            SetControlText("SKIPPEDFRAMES", "Skipped Frames: " + EngineController.debugSkippedFrames.ToString());
            SetControlText("MEMORYUSE", "Memory Use: " + GC.GetTotalMemory(false) / 1024 / 1024);
            SetControlText("GCCOUNT", "GC Count: " + GC.CollectionCount(0));
            SetControlText("UPDATEFIRETILES", "FIRE TILES: " + Convert.ToString(EngineController.debugFires));
            SetControlText("UPDATEMINIFIRETILES", "MINI FIRE TILES: " + Convert.ToString(EngineController.debugMiniFires));

            counter -= EngineController.drawUpdateTime * 0.02f;

            if (counter < 0)
            {
                counter = 1;
                SetControlText("MAINTHREADTIME", "Thread 1 Ticks : " + Convert.ToString(EngineController.mainThreadTime / 10000f) + "ms");
                SetControlText("SECONDTHREADTIME", "Thread 2 Ticks: " + Convert.ToString(EngineController.secondThreadTime / 10000f) + "ms");
                SetControlText("DRAWTIME", "DS Ticks: " + Convert.ToString(EngineController.drawTime / 10000f) + "ms");
                SetControlText("TILESONSCREEN", "TilesOnScreen: " + Convert.ToString(EngineController.debugTilesOnScreen));
            }


            if (input.IsKeyMapPressed(KeyMap.ResetFramesCounter))
            {
                EngineController.resetTimer = true;
            }
            // }
            base.Update(input);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
         //   if (DisplayController.showDebugWindow)
         //   {
                base.Draw(spriteBatch);
          //  }
        }
    }
}
