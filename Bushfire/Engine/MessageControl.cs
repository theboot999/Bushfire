using BushFire.Engine.Controllers;
using BushFire.Engine.UIControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine
{
    class MessageControl
    {
        Vector2 startLocation = new Vector2(50, 1000);
        List<Message> activeList = new List<Message>();
        List<Message> waitingList = new List<Message>();
        bool nextMessage;

        public MessageControl()
        {

        }

        public void AddMessage(string text, Color color)
        {
            waitingList.Add(new Message(color, startLocation, text));
        }


        private void UpdateNextMessageCounter()
        {
            if (!nextMessage)
            {
               if (activeList.Count > 0)
               {
                    nextMessage = activeList.Last().IsClearStart();
               }
               else
               {
                   nextMessage = true;
               }
            }
        }

        private void UpdateMessages(Input input)
        {
        //    foreach (Message message in activeList)
          //  {
         //       message.Update(input);
         //   }

            for (int i = activeList.Count - 1; i >= 0; i--)
            {
                activeList[i].Update(input);
                if (activeList[i].destroy)
                {                 
                    activeList.RemoveAt(i);
                }
            }         
        }

        private void UpdateDespatch()
        {
            if (nextMessage && waitingList.Count > 0)
            {
                Message message = waitingList.First();
                waitingList.Remove(message);
                activeList.Add(message);
                nextMessage = false;
            }
        }

        public void Update(Input input)
        {
            UpdateNextMessageCounter();
            UpdateMessages(input);
            UpdateDespatch();

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Message message in activeList)
            {
                message.Draw(spriteBatch, 1);
            }
        } 
    }
}
