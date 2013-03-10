using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RpgGame.Manager;
using RpgGame.Events;

namespace RpgGame.GameComponents
{
    class InteractionComponent : BaseGameComponent, IEventListener
    {
        public InteractionComponent() : base("InteractionComponent")
        {
            
        }

        public override void Init()
        {
            EventManager.AddListener(Event.Types.INTERACTION,this);
        }

        ~InteractionComponent()
        {
            EventManager.RemoveListener(Event.Types.INTERACTION,this);
        }

        public void HandleEvent( Event GameEvent )
        {
            //The interaction component does nothing else than to listen for interaction events
            //and calling the corresponding script method via the script component
            InteractionEvent InteractionEvent   = (InteractionEvent)GameEvent;
            ScriptComponent ScriptComponent     = Parent.GetComponent<ScriptComponent>();

            //Only check for collision with the interaction area if the game object has a script component
            if(ScriptComponent != null && ScriptComponent.HasFunction("OnInteraction")){
                //check for intersection with the interaction area
                if (Parent.Position.X + Parent.Width > InteractionEvent.InteractionArea.X &&
                    Parent.Position.X < InteractionEvent.InteractionArea.X + InteractionEvent.InteractionArea.Width)
                {
                    if (Parent.Position.Y + Parent.Height > InteractionEvent.InteractionArea.Y &&
                        Parent.Position.Y < InteractionEvent.InteractionArea.Y + InteractionEvent.InteractionArea.Height)
                    {
                        //Call the interaction function in the script.
                        ScriptComponent.CallFunction("OnInteraction",new object[] {InteractionEvent.Source});
                    }
                }
            }
        }
    }
}
