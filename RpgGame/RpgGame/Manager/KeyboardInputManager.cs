using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RpgGame.Events;

namespace RpgGame.Manager
{
    class KeyboardInputManager
    {
        static private KeyboardState KeysLastFrame { get; set; }
        static private KeyboardState KeysCurrentFrame { get; set; }

        public KeyboardInputManager()
        {
            KeysLastFrame = Keyboard.GetState();
            KeysCurrentFrame = Keyboard.GetState();
        }

        public void ProcessInput()
        {
            //Alle Werte des Keys arrays werden in ein array gespeichert.
            KeysCurrentFrame = Keyboard.GetState();

            Array AllKeys = System.Enum.GetValues(typeof(Keys));

            //Das Array wird durch laufen und es wird geguckt ob sich tasten seit dem letztem check verändert haben.
            foreach (Keys k in AllKeys)
            {
                if (KeysLastFrame[k] != KeysCurrentFrame[k])
                {

                    //Wenn sich Zustaende der Tasten geaendert haben, wird geguckt ob die Taste nun gedrueckt oder los gelassen wurde.
                    //Danach wird das entsprechende Event der EventManagerliste hinzugefügt
                    if (KeysCurrentFrame[k] == KeyState.Down)
                    {
                        EventManager.AddEventToQuery(new KeyPressedEvent(k));
                    }
                    else
                    {
                        EventManager.AddEventToQuery(new KeyReleasedEvent(k));
                    }
                }
            }

            KeysLastFrame = KeysCurrentFrame;
        }
    }
}
