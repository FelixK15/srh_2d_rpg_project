using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RpgGame.Events;

namespace RpgGame.Manager
{
    class GamePadInputManager
    {
        static private GamePadState GamePadLastFrame { get; set; }
        static private GamePadState GamePadCurrentFrame { get; set; }

        static public float TriggerDeadZone { get; set; }
        static public float ThumbStickDeadZone { get; set; }

        public GamePadInputManager()
        {
            GamePadLastFrame = GamePad.GetState(PlayerIndex.One);
            GamePadCurrentFrame = GamePad.GetState(PlayerIndex.One);
        }

        public void ProcessInput()
        {
            GamePadCurrentFrame = GamePad.GetState(PlayerIndex.One);

            CheckConnectivity();
            ProcessButtons();
            ProcessDPad();
            ProcessThumbSticks();
            ProcessTriggers();

            GamePadLastFrame = GamePadCurrentFrame;
        }

        private void ProcessButtons()
        {
            if (GamePadLastFrame.Buttons.X != GamePadCurrentFrame.Buttons.X)
            {
                EventManager.AddEventToQuery(new GamePadButtonEvent(GamePadButtonEvent.GamePadButton.X_BUTTON, GamePadCurrentFrame.Buttons.X));
            }

            if (GamePadLastFrame.Buttons.Y != GamePadCurrentFrame.Buttons.Y)
            {
                EventManager.AddEventToQuery(new GamePadButtonEvent(GamePadButtonEvent.GamePadButton.Y_BUTTON, GamePadCurrentFrame.Buttons.Y));
            }

            if (GamePadLastFrame.Buttons.B != GamePadCurrentFrame.Buttons.B)
            {
                EventManager.AddEventToQuery(new GamePadButtonEvent(GamePadButtonEvent.GamePadButton.B_BUTTON, GamePadCurrentFrame.Buttons.B));
            }

            if (GamePadLastFrame.Buttons.A != GamePadCurrentFrame.Buttons.A)
            {
                EventManager.AddEventToQuery(new GamePadButtonEvent(GamePadButtonEvent.GamePadButton.A_BUTTON, GamePadCurrentFrame.Buttons.A));
            }

            if (GamePadLastFrame.Buttons.Start != GamePadCurrentFrame.Buttons.Start)
            {
                EventManager.AddEventToQuery(new GamePadButtonEvent(GamePadButtonEvent.GamePadButton.START_BUTTON, GamePadCurrentFrame.Buttons.Start));
            }

            if (GamePadLastFrame.Buttons.Back != GamePadCurrentFrame.Buttons.Back)
            {
                EventManager.AddEventToQuery(new GamePadButtonEvent(GamePadButtonEvent.GamePadButton.BACK_BUTTON, GamePadCurrentFrame.Buttons.Back));
            }

            if (GamePadLastFrame.Buttons.LeftShoulder != GamePadCurrentFrame.Buttons.LeftShoulder)
            {
                EventManager.AddEventToQuery(new GamePadButtonEvent(GamePadButtonEvent.GamePadButton.LEFT_SHOULDER_BUTTON, GamePadCurrentFrame.Buttons.LeftShoulder));
            }

            if (GamePadLastFrame.Buttons.RightShoulder != GamePadCurrentFrame.Buttons.RightShoulder)
            {
                EventManager.AddEventToQuery(new GamePadButtonEvent(GamePadButtonEvent.GamePadButton.RIGHT_SHOULDER_BUTTON, GamePadCurrentFrame.Buttons.RightShoulder));
            }

            if (GamePadLastFrame.Buttons.RightStick != GamePadCurrentFrame.Buttons.RightStick)
            {
                EventManager.AddEventToQuery(new GamePadButtonEvent(GamePadButtonEvent.GamePadButton.RIGHT_STICK_BUTTON, GamePadCurrentFrame.Buttons.RightStick));
            }

            if (GamePadLastFrame.Buttons.LeftStick != GamePadCurrentFrame.Buttons.LeftStick)
            {
                EventManager.AddEventToQuery(new GamePadButtonEvent(GamePadButtonEvent.GamePadButton.LEFT_STICK_BUTTON, GamePadCurrentFrame.Buttons.LeftStick));
            }
        }

        private void ProcessDPad()
        {
            if (GamePadLastFrame.DPad.Up != GamePadCurrentFrame.DPad.Up)
            {
                EventManager.AddEventToQuery(new GamePadButtonEvent(GamePadButtonEvent.GamePadButton.DPAD_UP_BUTTON, GamePadCurrentFrame.DPad.Up));
            }

            if (GamePadLastFrame.DPad.Down != GamePadCurrentFrame.DPad.Down)
            {
                EventManager.AddEventToQuery(new GamePadButtonEvent(GamePadButtonEvent.GamePadButton.DPAD_DOWN_BUTTON, GamePadCurrentFrame.DPad.Down));
            }

            if (GamePadLastFrame.DPad.Left != GamePadCurrentFrame.DPad.Left)
            {
                EventManager.AddEventToQuery(new GamePadButtonEvent(GamePadButtonEvent.GamePadButton.DPAD_LEFT_BUTTON, GamePadCurrentFrame.DPad.Left));
            }

            if (GamePadLastFrame.DPad.Right != GamePadCurrentFrame.DPad.Right)
            {
                EventManager.AddEventToQuery(new GamePadButtonEvent(GamePadButtonEvent.GamePadButton.DPAD_RIGHT_BUTTON, GamePadCurrentFrame.DPad.Right));
            }
        }

        private void ProcessTriggers()
        {
            if (GamePadCurrentFrame.Triggers.Left > TriggerDeadZone)
            {
                EventManager.AddEventToQuery(new GamePadShoulderTriggerEvent(GamePadShoulderTriggerEvent.ShoulderTrigger.LEFT_SHOULDER_TRIGGER, GamePadCurrentFrame.Triggers.Left));
            }

            if (GamePadCurrentFrame.Triggers.Right > TriggerDeadZone)
            {
                EventManager.AddEventToQuery(new GamePadShoulderTriggerEvent(GamePadShoulderTriggerEvent.ShoulderTrigger.RIGHT_SHOULDER_TRIGGER, GamePadCurrentFrame.Triggers.Right));
            }
        }

        private void ProcessThumbSticks()
        {
            if (!ThumbStickInDeadZone(GamePadCurrentFrame.ThumbSticks.Left))
            {
                EventManager.AddEventToQuery(new GamePadThumbStickEvent(GamePadThumbStickEvent.ThumbStick.LEFT_THUMBSTICK, GamePadCurrentFrame.ThumbSticks.Left));
            }

            if (!ThumbStickInDeadZone(GamePadCurrentFrame.ThumbSticks.Right))
            {
                EventManager.AddEventToQuery(new GamePadThumbStickEvent(GamePadThumbStickEvent.ThumbStick.RIGHT_THUMBSTICK, GamePadCurrentFrame.ThumbSticks.Right));
            }
        }

        private bool ThumbStickInDeadZone(Vector2 Position)
        {
            float[] ThumbStickDeadZoneEdges = { -ThumbStickDeadZone, ThumbStickDeadZone * 2,};

            if (GamePadCurrentFrame.ThumbSticks.Left != Vector2.Zero)
            {
                if (Position.X < ThumbStickDeadZoneEdges[0] || Position.X > ThumbStickDeadZoneEdges[1])
                {
                    return false;
                }

                if(Position.Y < ThumbStickDeadZoneEdges[0] || Position.Y > ThumbStickDeadZoneEdges[1])
                {
                    return false;
                }
            }

            return true;
        }

        private void CheckConnectivity()
        {
            if (GamePadLastFrame.IsConnected != GamePadCurrentFrame.IsConnected)
            {
                if (GamePadCurrentFrame.IsConnected)
                {
                    EventManager.AddEventToQuery(new Event(Event.Types.GAMEPAD_CONNECT));
                }
                else
                {
                    EventManager.AddEventToQuery(new Event(Event.Types.GAMEPAD_DISCONNECT));
                }
            }
        }
    }
}
