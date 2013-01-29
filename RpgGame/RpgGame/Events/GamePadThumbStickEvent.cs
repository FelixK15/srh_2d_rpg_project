using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame.Events
{
    class GamePadThumbStickEvent : Event
    {
        public enum ThumbStick
        {
            LEFT_THUMBSTICK,
            RIGHT_THUMBSTICK
        }

        public ThumbStick Stick { get; private set; }
        public Vector2 Position { get; private set; }

        public GamePadThumbStickEvent(ThumbStick tsStick, Vector2 vPosition)
            : base(Event.Types.GAMEPAD_STICK)
        {
            Stick = tsStick;
            Position = vPosition;
        }
    }
}
