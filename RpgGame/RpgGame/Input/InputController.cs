using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace RpgGame.Input
{
    public class InputController : InputDevice
    {
        public enum ControllerButton
        {
            X_BUTTON,
            Y_BUTTON,
            A_BUTTON,
            B_BUTTON,
            START_BUTTON,
            BACK_BUTTON,
            LSHOULDER_BUTTON,
            RSHOULDER_BUTTON
        }
        public class InputControllerMapping
        {
            public InputControllerMapping(Input input, ControllerButton button)
            {
                this.Input = input;
                this.Button = button;
            }

            public Input            Input   { get; set; }
            public ControllerButton Button  { get; set; }
        }

        public List<InputControllerMapping> Mapping             { get; set; }

        private const float                  StickTollerance = 0.2f;

        public InputController()
        {
            Mapping         = new List<InputControllerMapping>();
            ReleaseInput    = new List<Input>();
        }

        public override bool IsPressed(Input input)
        {
            GamePadState PadState = GamePad.GetState(PlayerNo);
            
            //check only if the game pad is connected and the button isn't in the "wait for release" state.
            if(PadState.IsConnected){
                if(!ReleaseInput.Contains(input)){
                    //Check if the input was a direction, if so check the dpad and the thumb stick
                    if (input == Input.LEFT_BTN || input == Input.RIGHT_BTN || input == Input.DOWN_BTN || input == Input.UP_BTN){
                        if (input == Input.LEFT_BTN){
                            return PadState.DPad.Left == ButtonState.Pressed || PadState.ThumbSticks.Left.X < -StickTollerance;
                        }else if (input == Input.RIGHT_BTN){
                            return PadState.DPad.Right == ButtonState.Pressed || PadState.ThumbSticks.Left.X > StickTollerance;
                        }else if (input == Input.DOWN_BTN){
                            return PadState.DPad.Down == ButtonState.Pressed || PadState.ThumbSticks.Left.Y < -StickTollerance;
                        }else{
                            return PadState.DPad.Up == ButtonState.Pressed || PadState.ThumbSticks.Left.Y > StickTollerance;
                        }
                    }else{
                        //Check the mapping of the buttons.
                        foreach (InputControllerMapping Map in Mapping){
                            if (Map.Input == input){
                                //Check every button of the controller.
                                if(Map.Button == ControllerButton.A_BUTTON){
                                    return PadState.Buttons.A == ButtonState.Pressed;
                                }else if(Map.Button == ControllerButton.B_BUTTON){
                                    return PadState.Buttons.B == ButtonState.Pressed;
                                }else if(Map.Button == ControllerButton.X_BUTTON){
                                    return PadState.Buttons.X == ButtonState.Pressed;
                                }else if(Map.Button == ControllerButton.Y_BUTTON){
                                    return PadState.Buttons.Y == ButtonState.Pressed;
                                }else if(Map.Button == ControllerButton.BACK_BUTTON){
                                    return PadState.Buttons.Back == ButtonState.Pressed;
                                }else if(Map.Button == ControllerButton.START_BUTTON){
                                    return PadState.Buttons.Start == ButtonState.Pressed;
                                }else if(Map.Button == ControllerButton.LSHOULDER_BUTTON){
                                    return PadState.Buttons.LeftShoulder == ButtonState.Pressed;
                                }else if(Map.Button == ControllerButton.RSHOULDER_BUTTON){
                                    return PadState.Buttons.RightShoulder == ButtonState.Pressed;
                                }
                            }
                        }
                    }
                }
            }
           
            return false;
        }

        public override void Update()
        {
            List<Input> TempList = new List<Input>();
            GamePadState PadState = GamePad.GetState(PlayerNo);

            //Add released buttons to a temp list.
            foreach(Input input in ReleaseInput){
                foreach (InputControllerMapping Map in Mapping){
                    if(Map.Input == input){
                        if(Map.Button == ControllerButton.A_BUTTON){
                            if(PadState.Buttons.A == ButtonState.Released){
                                TempList.Add(input);
                            }
                        }else if(Map.Button == ControllerButton.B_BUTTON){
                            if(PadState.Buttons.B == ButtonState.Released){
                                TempList.Add(input);
                            }
                        }else if(Map.Button == ControllerButton.X_BUTTON){
                            if(PadState.Buttons.X == ButtonState.Released){
                                TempList.Add(input);
                            }
                        }else if(Map.Button == ControllerButton.Y_BUTTON){
                            if(PadState.Buttons.Y == ButtonState.Released){
                                TempList.Add(input);
                            }
                        }else if(Map.Button == ControllerButton.START_BUTTON){
                            if(PadState.Buttons.Start == ButtonState.Released){
                                TempList.Add(input);
                            }
                        }else if(Map.Button == ControllerButton.BACK_BUTTON){
                            if(PadState.Buttons.Back == ButtonState.Released){
                                TempList.Add(input);
                            }
                        }else if(Map.Button == ControllerButton.LSHOULDER_BUTTON){
                            if(PadState.Buttons.LeftShoulder == ButtonState.Released){
                                TempList.Add(input);
                            }
                        }else if(Map.Button == ControllerButton.RSHOULDER_BUTTON){
                            if(PadState.Buttons.RightShoulder == ButtonState.Released){
                                TempList.Add(input);
                            }
                        }
                    }
                }
            }

            //Remove the released buttons from the release input list.
            foreach(Input input in TempList){
                ReleaseInput.Remove(input);
            }
        }
    }
}
