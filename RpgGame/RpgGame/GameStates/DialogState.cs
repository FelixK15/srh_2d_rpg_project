using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using RpgGame.Dialogs;
using RpgGame.Events;
using RpgGame.Manager;


namespace RpgGame.GameStates
{
    class DialogState : IGameState, IEventListener
    {
        public Dialog currentDialog { get; set; }

        public DialogState(Dialog dialog)
        {
            currentDialog = dialog;
            
        }

        public void Start()
        {
            EventManager.AddListener(Event.Types.KEYBOARD_PRESSED, this);
            DialogBox.startDialog(currentDialog);
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            DialogBox.Update(gameTime);
        }

        public void Draw(ref Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            DialogBox.Draw(ref batch);
        }

        public void Stop()
        {
            EventManager.RemoveListener(Event.Types.KEYBOARD_PRESSED, this);
            DialogBox.stopDialog();
        }

        public void HandleEvent(Event eGameEvent)
        {
            if (eGameEvent is KeyPressedEvent)
            {
                KeyPressedEvent keyEvent = (KeyPressedEvent)eGameEvent;

                if (keyEvent.PressedKey == Keys.A)
                {
                    DialogBox.NextText();
                }
            }
        }
    }
}
