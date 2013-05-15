using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using RpgGame.Dialogs;
using RpgGame.Events;
using RpgGame.Manager;
using RpgGame.Input;
using Microsoft.Xna.Framework;


namespace RpgGame.GameStates
{
    public class DialogState : IGameState
    {
        public  Dialog currentDialog { get; set; }

        public DialogState(Dialog dialog)
        {
            currentDialog   = dialog;
        }

        public void Start()
        {
            DialogBox.startDialog(currentDialog);
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            DialogBox.Update(gameTime);

            InputDevice Device = InputManager.GetDevice(PlayerIndex.One);
            if(Device.IsPressed(InputDevice.Input.ATTACK_CONFIRM_BTN)){
                Device.WaitForRelease(InputDevice.Input.ATTACK_CONFIRM_BTN);
                if (!DialogBox.isMessageDrawn()){
                    DialogBox.drawFullMessage();
                }else{
                    DialogBox.NextText();
                }
            }
        }

        public void Draw(ref Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            DialogBox.Draw(ref batch);
        }

        public void Stop()
        {

        }
    }
}
