using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using RpgGame.Manager;
using RpgGame.Events;

namespace RpgGame.Processes
{
    class EventProcess : Process
    {
        public Event GameEvent
        {
            get;
            set;
        }

        public EventProcess(Event GameEvent)
        {
            this.GameEvent = GameEvent;
        }

        public override void Start()
        {
        
        }

        public override void Update(GameTime gameTime)
        {
            EventManager.AddEventToQuery(GameEvent);
            Finished = true;
        }

        public override void End()
        {
            
        }
    }
}
