using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using BushFire.Engine.UIControls.Abstract;
using BushFire.Engine.UIControls.Internal;
using BushFire.Game.Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Screens.Containers
{
    class HelpInformation : Container
    {
        float lineSpacing = 40;
        float currentSpace = 50;

        public HelpInformation() : base(new Rectangle(0, 0, 700, 800), DockType.CENTERSCREENBOTH, true)
        {
            name = "HelpInformation";
            drawSpriteBack = true;
            spriteBack = GraphicsManager.GetPreBuilt(Engine.ContentStorage.PrebuiltSprite.InGameMenuBack);
            transparency = 1f;
            AddBorder(2, Resizing.NONE, 40);

            SetSizeBounds(0, 0, 900, 2600);
            AddScrollV(17, 6, 20);

            AddHeading(40, "Help", GraphicsManager.GetSpriteFont(Font.CarterOne16), Color.White, true, false, false, false, true, GraphicsManager.GetPreBuilt(Engine.ContentStorage.PrebuiltSprite.InGameHeadingBar));
            SetMaxTransparency(0.95f);

            AddNewLine("Hold " + EngineController.keyMapList[KeyMap.OpenInfoWindow].ToString() + " and click on vehicles or intersections with");
            AddNewLine("stoplights to open their information box (Note the cursor");
            AddNewLine("change)");
            AddNewLine("Alternatively double click them");
            AddNewLineSpace("To Create Control Groups select units then hold " + EngineController.keyMapList[KeyMap.CreateControlGroup].ToString()); // + EngineController.keyMapList[KeyMap.CreateControlGroup].ToString() + " and press);
            AddNewLine("and press a control group Key");
            AddNewLineSpace("To retrieve a control group press the control group key");
            AddNewLineSpace("By the way this is all sample text");

            for (int i = 0; i < 20; i++)
            {
                AddNewLineSpace("And this is a fine looking scrollable panel.");
            }
        }

        private void AddNewLine(string value)
        {
            AddUiControl(new Label("Help", Font.OpenSans16Bold, Color.White, new Vector2(30, currentSpace), false, value));
            currentSpace += lineSpacing;
        }

        private void AddNewLineSpace(string value)
        {
            currentSpace += lineSpacing;
            AddUiControl(new Label("Help", Font.OpenSans16Bold, Color.White, new Vector2(30, currentSpace), false, value));
            currentSpace += lineSpacing;
        }


        private string GetHelpText()
        {
            return
            "Hold " + EngineController.keyMapList[KeyMap.OpenInfoWindow].ToString() + " click on vehicles \r\n" +
            " or intersections with stoplights to open \r\n" +
            "there information box.  (Note the cursor change) \r\n" +
            "or alternatively double click them";
        }
    }
}