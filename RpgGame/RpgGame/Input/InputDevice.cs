using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace RpgGame.Input
{
    public abstract class InputDevice
    {
        public enum Input {
            ATTACK_CONFIRM_BTN,
            MENU_BTN,
            START_BTN,
            SELECT_BTN,
            LEFT_BTN,
            RIGHT_BTN,
            UP_BTN,
            DOWN_BTN
        }

        public PlayerIndex PlayerNo     { get; set; }
        public List<Input> ReleaseInput { get; set; }

        public void WaitForRelease(InputDevice.Input input)
        {
            if(!ReleaseInput.Contains(input)){
                ReleaseInput.Add(input);
            }
        }
        public abstract bool IsPressed(Input input);
        public abstract void Update();
    }
}
