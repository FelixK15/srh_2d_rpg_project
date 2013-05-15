using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace RpgGame.Input
{
    public class InputKeyboard : InputDevice
    {
        public class InputKeyboardMapping
        {
            public InputKeyboardMapping(Input input, Keys key){
                this.Input = input;
                this.Key   = key;
            }

            public Input Input { get; set; }
            public Keys  Key   { get; set; }
        }

        public List<InputKeyboardMapping> Mapping { get; set; }

        public InputKeyboard()
        {
            Mapping = new List<InputKeyboardMapping>();
        }

        public override bool IsPressed(Input input)
        {
            KeyboardState KeyState = Keyboard.GetState();
            
            foreach(InputKeyboardMapping Map in Mapping){
                if(Map.Input == input){
                    return KeyState.IsKeyDown(Map.Key);
                }
            }

            return false;
        }

        public override void Update()
        {

        }
    }
}
