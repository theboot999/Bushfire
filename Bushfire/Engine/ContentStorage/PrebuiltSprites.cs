using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.ContentStorage
{
    class PrebuiltSprites
    {
        Dictionary<PrebuiltSprite, Sprite> prebuiltSpriteList;
        
        public PrebuiltSprites()
        {
            prebuiltSpriteList = new Dictionary<PrebuiltSprite, Sprite>();
            Add();
        }

        private void Add()
        {
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonMenuLargeBlueBack, new Sprite(new Rectangle(0, 100, 402, 92), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonMenuLargeBlueFront, new Sprite(new Rectangle(0, 194, 402, 92), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonMenuLargeGreyBack, new Sprite(new Rectangle(440, 100, 402, 92), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonMenuLargeGreyFront, new Sprite(new Rectangle(440, 194, 402, 92), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonGreyLargeBack, new Sprite(new Rectangle(0, 0, 241, 48), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonGreyLargeFront, new Sprite(new Rectangle(0, 50, 241, 48), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonGreyMediumBack, new Sprite(new Rectangle(243, 0, 149, 48), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonGreyMediumFront, new Sprite(new Rectangle(243, 50, 149, 48), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonGreySmallBack, new Sprite(new Rectangle(394, 0, 48, 48), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonGreySmallFront, new Sprite(new Rectangle(394, 50, 48, 48), TextureSheet.Buttons));

            prebuiltSpriteList.Add(PrebuiltSprite.ButtonGreyMicroBack, new Sprite(new Rectangle(1116, 0, 38, 38), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonGreyMicroFront, new Sprite(new Rectangle(1116, 40, 38, 38), TextureSheet.Buttons));

            prebuiltSpriteList.Add(PrebuiltSprite.ButtonBlueLargeBack, new Sprite(new Rectangle(444, 0, 241, 48), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonBlueLargeFront, new Sprite(new Rectangle(444, 50, 241, 48), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonBlueMediumBack, new Sprite(new Rectangle(687, 0, 149, 48), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonBlueMediumFront, new Sprite(new Rectangle(687, 50, 149, 48), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonBlueSmallBack, new Sprite(new Rectangle(838, 0, 48, 48), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonBlueSmallFront, new Sprite(new Rectangle(838, 50, 48, 48), TextureSheet.Buttons));


            prebuiltSpriteList.Add(PrebuiltSprite.ButtonPinBack, new Sprite(new Rectangle(888, 0, 36, 36), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonPinFront, new Sprite(new Rectangle(888, 38, 36, 36), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonCloseBack, new Sprite(new Rectangle(926, 0, 36, 36), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonCloseFront, new Sprite(new Rectangle(926, 38, 36, 36), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonZoomBack, new Sprite(new Rectangle(964, 0, 36, 36), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonZoomFront, new Sprite(new Rectangle(964, 38, 36, 36), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonTextBack, new Sprite(new Rectangle(1002, 0, 36, 36), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonTextFront, new Sprite(new Rectangle(1002, 38, 36, 36), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonMinimapToggleBack, new Sprite(new Rectangle(0, 500, 110, 110), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonMiniMapToggleFront, new Sprite(new Rectangle(120, 500, 110, 110), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonTownToggleBack, new Sprite(new Rectangle(240, 500, 110, 110), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.ButtonTownToggleFront, new Sprite(new Rectangle(360, 500, 110, 110), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.TopBar, new Sprite(new Rectangle(0, 290, 1100, 54), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.InGameHeadingBar, new Sprite(new Rectangle(0, 346, 1040, 20), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.InGameMenuBack, new Sprite(new Rectangle(0, 369, 1100, 8), TextureSheet.Buttons));
            prebuiltSpriteList.Add(PrebuiltSprite.EditorPanelBackGrey, new Sprite(new Rectangle(0, 380, 1040, 18), TextureSheet.Buttons));
        }

        public Sprite GetPreBuilt(PrebuiltSprite prebuiltSprite)
        {
            return prebuiltSpriteList[prebuiltSprite];
        }

    }


    enum PrebuiltSprite
    {
        ButtonMenuLargeBlueBack,
        ButtonMenuLargeBlueFront,
        ButtonMenuLargeGreyBack,
        ButtonMenuLargeGreyFront,
        ButtonGreyLargeBack,
        ButtonGreyLargeFront,
        ButtonGreyMediumBack,
        ButtonGreyMediumFront,
        ButtonGreySmallBack,
        ButtonGreySmallFront,
        ButtonGreyMicroBack,
        ButtonGreyMicroFront,
        ButtonBlueLargeBack,
        ButtonBlueLargeFront,
        ButtonBlueMediumBack,
        ButtonBlueMediumFront,
        ButtonBlueSmallBack,
        ButtonBlueSmallFront,
        ButtonPinBack,
        ButtonPinFront,
        ButtonCloseBack,
        ButtonCloseFront,
        ButtonZoomBack,
        ButtonZoomFront,
        ButtonTextBack,
        ButtonTextFront,
        ButtonMinimapToggleBack,
        ButtonMiniMapToggleFront,
        ButtonTownToggleBack,
        ButtonTownToggleFront,
        TopBar,
        InGameHeadingBar,
        InGameMenuBack,
        EditorPanelBackGrey,
    }
}
