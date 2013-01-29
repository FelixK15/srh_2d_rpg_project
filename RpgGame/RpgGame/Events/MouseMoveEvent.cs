using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame.Events
{
    class MouseMoveEvent : Event
    {
        private int m_posX;
        private int m_posY;

        public int X
        {
            get { return m_posX; }
        }

        public int Y
        {
            get { return m_posY; }
        }

        public MouseMoveEvent(int posX, int posY)
            : base(Event.Types.MOUSE_MOVED)
        {
            m_posX = posX;
            m_posY = posY;
        }
    }
}
