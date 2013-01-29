using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace RpgGame.Events
{
    class KeyReleasedEvent : Event
    {
        private Keys m_key;

        public Keys ReleasedKey
        {
            get { return m_key; }
        }

        public KeyReleasedEvent(Keys key)
            : base(Event.Types.KEYBOARD_RELEASED)
        {
            m_key = key;
        }
    }
}
