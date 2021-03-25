using BushFire.Engine.Controllers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.UIControls
{
    class Message : Label
    {
        enum MessageStage
        {
            CLEARINGSTART = 0,
            MOVING = 1,
            MOVINGANDFADE = 2,
            DESTROY = 3
        }

        float speed;
        float fadein;
        float fadeout;
        float clearTime;
        float moveTime;
        float fadeTime;
        

        private float count;
        private MessageStage messageStage;

        public Message(Color fontColor, Vector2 locationText, string text) : base ("", Font.CarterOne18, fontColor, locationText, false, text)
        {
            messageStage = MessageStage.CLEARINGSTART;          
            transparency = 0;
            InitCountDown();
        }

        private void InitCountDown()
        {
            speed = DisplayController.messageSpeed;
            clearTime = 10 / speed;
            moveTime = 40 / speed;
            fadeTime = 40 / speed;

            fadein = 0.02f;
            fadeout = 0.01f;
            count = clearTime;
        }

        public bool IsClearStart()
        {
            return messageStage != MessageStage.CLEARINGSTART;
        }

        private void AdvanceStage()
        {
            int i = (int)messageStage + 1;
            messageStage = (MessageStage)i;

            if (messageStage == MessageStage.MOVING)
            {
                count = moveTime;
            }
            else if(messageStage == MessageStage.MOVINGANDFADE)
            {
                count = fadeTime;
            }
            else if (messageStage == MessageStage.DESTROY)
            {
                destroy = true;
            }
        }

        private void UpdateStage()
        {
            if (messageStage == MessageStage.CLEARINGSTART || messageStage == MessageStage.MOVING)
            {
                transparency += fadein * EngineController.drawUpdateTime;

                if (transparency > 0.8f)
                {
                    transparency = 0.8f;
                }

                preScaleLocation.Y -= speed * EngineController.drawUpdateTime;
                Rescale();
            }
            else if(messageStage == MessageStage.MOVINGANDFADE)
            {
                transparency -= fadeout * EngineController.drawUpdateTime;

                if (transparency < 0f)
                {
                    transparency = 0f;
                }
                preScaleLocation.Y -= speed * EngineController.drawUpdateTime;
                Rescale();
            }
        }


        public override void Update(Input input)
        {
            base.Update(input);

           
            count -= 0.1f * EngineController.drawUpdateTime;

            if (count < 0)
            {
                AdvanceStage();
            }

            UpdateStage();
           
        }


        

    }
}
