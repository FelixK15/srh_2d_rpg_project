using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame.Events
{
    interface IEventListener
    {
        void HandleEvent(Event GameEvent);
    }
}
