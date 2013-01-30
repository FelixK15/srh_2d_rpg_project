using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame.Events
{
    public class Event
    {
        public enum Types
        {
            MOUSE_MOVED,
            MOUSE_BUTTON,

            KEYBOARD_PRESSED,
            KEYBOARD_RELEASED,

            GAMEPAD_BUTTON,
            GAMEPAD_STICK,
            GAMEPAD_SHOULDER,
            GAMEPAD_DISCONNECT,
            GAMEPAD_CONNECT,

            RINGMENU_ITEM_CHANGED,
            RINGMENU_ZOOM_OUT,
            RINGMENU_ZOOM_IN,

            PLAYER_MOVED,
            INTERACTION
        }

        private Types m_tEventType;

        public Types Type
        {
            get { return m_tEventType; }
            set { m_tEventType = value; }
        }

        public Event(Types tEventType)
        {
            m_tEventType = tEventType;
        }
    }
}
