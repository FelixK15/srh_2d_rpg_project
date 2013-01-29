using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RpgGame.Events;

namespace RpgGame.Manager
{
    class MouseInputManager
    {
        static private MouseState MouseLastFrame { get; set; }
        static private MouseState MouseCurrentFrame { get; set; }

        public MouseInputManager()
        {
            MouseLastFrame = Mouse.GetState();
            MouseCurrentFrame = Mouse.GetState();
        }

        public void ProcessInput()
        {
            MouseCurrentFrame = Mouse.GetState();

            if (MouseCurrentFrame.X != MouseLastFrame.X
                || MouseCurrentFrame.Y != MouseLastFrame.Y)
            {
                EventManager.AddEventToQuery(new MouseMoveEvent(MouseCurrentFrame.X, MouseCurrentFrame.Y));
            }
            
            if (MouseCurrentFrame.LeftButton != MouseLastFrame.LeftButton)
            {
                EventManager.AddEventToQuery(new MouseButtonEvent(MouseButtonEvent.MouseButton.LEFT_BUTTON, MouseCurrentFrame.LeftButton));
            }

            if(MouseCurrentFrame.RightButton != MouseLastFrame.RightButton)
            {
                EventManager.AddEventToQuery(new MouseButtonEvent(MouseButtonEvent.MouseButton.RIGHT_BUTTON, MouseCurrentFrame.RightButton));
            }

            if(MouseCurrentFrame.MiddleButton != MouseLastFrame.MiddleButton)
            {
                EventManager.AddEventToQuery(new MouseButtonEvent(MouseButtonEvent.MouseButton.MIDDLE_BUTTON, MouseCurrentFrame.MiddleButton));
            }
        }
    }
}
