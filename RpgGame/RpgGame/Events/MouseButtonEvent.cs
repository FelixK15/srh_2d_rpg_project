using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace RpgGame.Events
{
    class MouseButtonEvent : Event
    {
        public enum MouseButton
        {
            LEFT_BUTTON,
            RIGHT_BUTTON,
            MIDDLE_BUTTON
        }

        public MouseButton Button { get; set; }
        public ButtonState State { get; set; }

        public MouseButtonEvent(MouseButton mbButton, ButtonState bsState)
            : base(Event.Types.MOUSE_BUTTON)
        {
            Button = mbButton;
            State = bsState;
        }
    }
}
