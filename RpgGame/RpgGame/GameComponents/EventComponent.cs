using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using RpgGame.Manager;
using RpgGame.Events;

namespace RpgGame.GameComponents
{
    delegate void HandleEvent(GameObject gameObject,Event gameEvent);

    class EventComponent : BaseGameComponent, IEventListener, IDisposable
    {
        private Event.Types Type { get; set; }
        private HandleEvent Function { get; set; }

        public EventComponent(Event.Types eventType,HandleEvent function) : base("EventComponent")
        {
            Function = function;
            EventManager.AddListener(eventType,this);
            Type = eventType;
        }

        public void HandleEvent( Event eGameEvent )
        {
            Function(Parent, eGameEvent);
        }

        public void Dispose()
        {
            EventManager.RemoveListener(Type, this);
        }
    }
}
