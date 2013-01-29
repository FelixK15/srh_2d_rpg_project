using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame.Events
{
    class GamePadButtonEvent : Event
    {
        public enum GamePadButton
        {
            X_BUTTON,
            Y_BUTTON,
            B_BUTTON,
            A_BUTTON,
            START_BUTTON,
            BACK_BUTTON,
            LEFT_STICK_BUTTON,
            RIGHT_STICK_BUTTON,
            LEFT_SHOULDER_BUTTON,
            RIGHT_SHOULDER_BUTTON,

            DPAD_LEFT_BUTTON,
            DPAD_RIGHT_BUTTON,
            DPAD_UP_BUTTON,
            DPAD_DOWN_BUTTON
        }

        public GamePadButton Button { get; private set; }
        public ButtonState State { get; private set; }

        public GamePadButtonEvent(GamePadButton gpButton, ButtonState bsState) 
            : base(Event.Types.GAMEPAD_BUTTON)
        {
            Button = gpButton;
            State = bsState;
        }
    }
}
