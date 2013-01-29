using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame.Events
{
    class GamePadShoulderTriggerEvent : Event
    {
        public enum ShoulderTrigger
        {
            LEFT_SHOULDER_TRIGGER,
            RIGHT_SHOULDER_TRIGGER
        }

        public ShoulderTrigger Trigger { get; private set; }
        public float Value { get; private set; }

        public GamePadShoulderTriggerEvent(ShoulderTrigger stTrigger,float fValue)
            : base(Event.Types.GAMEPAD_SHOULDER)
        {
            Trigger = stTrigger;
            Value = fValue;
        }
    }
}
