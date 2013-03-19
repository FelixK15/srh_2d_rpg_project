using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace RpgGame.Events
{
    public class InteractionEvent : Event
    {
        public Rectangle InteractionArea { get; private set; }
        public GameObject Source { get; private set; }

        public InteractionEvent(Rectangle area,GameObject source)
            : base(Event.Types.ON_INTERACTION)
        {
            InteractionArea = area;
            Source = source;
        }
    }
}
