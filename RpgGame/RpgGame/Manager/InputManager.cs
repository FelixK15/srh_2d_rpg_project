using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame.Manager
{
    class InputManager
    {
        //Input States
        
        static private KeyboardInputManager KeyboardManager { get; set; }
        static private MouseInputManager MouseManager { get; set; }
        static private GamePadInputManager GamePadManager { get; set; }

        public InputManager()
        {
            KeyboardManager = new KeyboardInputManager();
            MouseManager = new MouseInputManager();
            GamePadManager = new GamePadInputManager();
        }

        public void ProcessInput()
        {
            KeyboardManager.ProcessInput();
            MouseManager.ProcessInput();
            GamePadManager.ProcessInput();
        }
    }
}
