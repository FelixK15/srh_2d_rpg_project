using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace RpgGame.Events
{
    class KeyPressedEvent : Event
    {
        private Keys m_key;

        public Keys PressedKey
        {
            get { return m_key; }
        }

        public KeyPressedEvent(Keys key) : base(Event.Types.KEYBOARD_PRESSED)
        {
            m_key = key;
        }
    }
}
