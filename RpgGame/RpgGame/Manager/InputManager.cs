using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RpgGame.Input;
using Microsoft.Xna.Framework;
using RpgGame.Tools;

namespace RpgGame.Manager
{
    public class InputManager
    {
        public class InputMapper
        {
            public PlayerIndex PlayerNo;
            public InputDevice Device;
        }
        //Input States
        static public InputMapper[] InputDevices;

        public static void Initialize()
        {
            InputDevices = new InputMapper[4];

            InputDevices[0] = new InputMapper();
            InputDevices[1] = new InputMapper();
            InputDevices[2] = new InputMapper();
            InputDevices[3] = new InputMapper();

            InputDevices[0].PlayerNo = PlayerIndex.One;
            InputDevices[1].PlayerNo = PlayerIndex.Two;
            InputDevices[2].PlayerNo = PlayerIndex.Three;
            InputDevices[3].PlayerNo = PlayerIndex.Four;

            _MapInputFromFile();
        }

        public static InputDevice GetDevice(PlayerIndex index)
        {
            foreach(InputMapper Map in InputDevices){
                if(Map.PlayerNo == index){
                    return Map.Device;
                }
            }

            return null;
        }

        public static void Update()
        {
            foreach(InputMapper DeviceMapper in InputDevices){
                if(DeviceMapper.Device != null){
                    DeviceMapper.Device.Update();
                }
            }
        }

        private static void _MapInputFromFile()
        {
            FileStream InputFileStream = null;
            try{
                InputFileStream = File.OpenRead("LayoutInput.cfg");
            }catch(FileNotFoundException ex){
                DeveloperConsole.AddMessage(DeveloperConsole.MessageType.ERROR,String.Format("Could not open \"LayoutInput.cfg\".{0}",ex.Message));
                return;
            }

            StreamReader Reader      = new StreamReader(InputFileStream);
            String       CurrentLine = "";
            Int32        PlayerIndex = 0;
            while(Reader.Peek() >= 0){
                CurrentLine = Reader.ReadLine();

                if(CurrentLine != ""){
                    _EvaluateLine(CurrentLine,ref PlayerIndex);
                }
            }

            Reader.Close();
            InputFileStream.Close();
        }

        private static void _EvaluateLine(String Line,ref Int32 PlayerIndex)
        {
            if(Line.Contains("PLAYER")){
                String PlayerNo = Line.Substring(Line.LastIndexOf('_') + 1);
                if(PlayerNo == "ONE"){
                    PlayerIndex = 0;
                }else if(PlayerNo == "TWO"){
                    PlayerIndex = 1;
                }else if(PlayerNo == "THREE"){
                    PlayerIndex = 2;
                }else if(PlayerNo == "FOUR"){
                    PlayerIndex = 3;
                }else{
                    DeveloperConsole.AddMessage(DeveloperConsole.MessageType.ERROR,
                        String.Format("Unexpected token in \"LayoutInput.cfg\" within the line:\n{0}",Line));
                }
            }else if(Line.Contains("device")){
                String DeviceName = Line.Substring(Line.LastIndexOf('=') + 1);
                if(DeviceName == "keyboard"){
                    InputDevices[PlayerIndex].Device = new InputKeyboard();
                }else if(DeviceName == "controller"){
                    InputDevices[PlayerIndex].Device = new InputController();
                }else{
                    DeveloperConsole.AddMessage(DeveloperConsole.MessageType.ERROR,
                        String.Format("Unexpected token in \"LayoutInput.cfg\" within the line:\n{0}", Line));
                }
            }else{
                String MappingString = Line.Substring(Line.LastIndexOf('=') + 1);
                if(Line.Contains("up")){
                    _SetInputMapping(ref InputDevices[PlayerIndex].Device,MappingString,
                        InputDevice.Input.UP_BTN);
                }else if(Line.Contains("down")){
                    _SetInputMapping(ref InputDevices[PlayerIndex].Device,MappingString,
                        InputDevice.Input.DOWN_BTN);
                }else if(Line.Contains("left")){
                    _SetInputMapping(ref InputDevices[PlayerIndex].Device,MappingString,
                        InputDevice.Input.LEFT_BTN);
                }else if(Line.Contains("right")){
                    _SetInputMapping(ref InputDevices[PlayerIndex].Device,MappingString,
                        InputDevice.Input.RIGHT_BTN);
                }else if(Line.Contains("attack")){
                    _SetInputMapping(ref InputDevices[PlayerIndex].Device,MappingString,
                        InputDevice.Input.ATTACK_CONFIRM_BTN);
                }else if(Line.Contains("menu")){
                    _SetInputMapping(ref InputDevices[PlayerIndex].Device,MappingString,
                        InputDevice.Input.MENU_BTN);
                }else if(Line.Contains("start")){
                    _SetInputMapping(ref InputDevices[PlayerIndex].Device,MappingString,
                        InputDevice.Input.START_BTN);
                }else if(Line.Contains("select")){
                    _SetInputMapping(ref InputDevices[PlayerIndex].Device,MappingString,
                        InputDevice.Input.SELECT_BTN);
                }else{
                    DeveloperConsole.AddMessage(DeveloperConsole.MessageType.ERROR,
                        String.Format("Unexpected token in \"LayoutInput.cfg\" within the line:\n{0}", Line));
                }
            }
        }

        private static void _SetInputMapping(ref InputDevice Device,String MappingString,InputDevice.Input Input)
        {
            //If the device is a controller, we don't need to set the up, right, left and down buttons.
            if(Device is InputController && (Input == InputDevice.Input.UP_BTN || Input == InputDevice.Input.LEFT_BTN ||
                                             Input == InputDevice.Input.DOWN_BTN || Input == InputDevice.Input.RIGHT_BTN)){
                return;
            }

            if(Device is InputController){
                InputController ControllerDevice = (InputController) Device;
                _MapController(ref ControllerDevice,MappingString,Input);
            }else if(Device is InputKeyboard){
                InputKeyboard KeyboardDevice = (InputKeyboard) Device;
                _MapKeyBoard(ref KeyboardDevice,MappingString,Input);
            }
        }

        private static void _MapController(ref InputController ControllerDevice,String MappingString, InputDevice.Input Input)
        {
            if(MappingString == "x"){
                    ControllerDevice.Mapping.Add(new InputController.InputControllerMapping(Input,InputController.ControllerButton.X_BUTTON));
                }else if(MappingString == "y"){
                    ControllerDevice.Mapping.Add(new InputController.InputControllerMapping(Input,InputController.ControllerButton.Y_BUTTON));
                }else if(MappingString == "a"){
                    ControllerDevice.Mapping.Add(new InputController.InputControllerMapping(Input,InputController.ControllerButton.A_BUTTON));
                }else if(MappingString == "b"){
                    ControllerDevice.Mapping.Add(new InputController.InputControllerMapping(Input,InputController.ControllerButton.B_BUTTON));
                }else if(MappingString == "start"){
                    ControllerDevice.Mapping.Add(new InputController.InputControllerMapping(Input,InputController.ControllerButton.START_BUTTON));
                }else if(MappingString == "back"){
                    ControllerDevice.Mapping.Add(new InputController.InputControllerMapping(Input,InputController.ControllerButton.BACK_BUTTON));
                }else if(MappingString == "lshoulder"){
                    ControllerDevice.Mapping.Add(new InputController.InputControllerMapping(Input,InputController.ControllerButton.LSHOULDER_BUTTON));
                }else if(MappingString == "rshoulder"){
                    ControllerDevice.Mapping.Add(new InputController.InputControllerMapping(Input,InputController.ControllerButton.RSHOULDER_BUTTON));
                }else{
                    DeveloperConsole.AddMessage(DeveloperConsole.MessageType.ERROR,
                    String.Format("Unexpected token in \"LayoutInput.cfg\" |{0}|", MappingString));
                }
        }

        private static void _MapKeyBoard(ref InputKeyboard KeyboardDevice,String MappingString,InputDevice.Input Input)
        {
            if(MappingString == "q"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.Q));
            }else if(MappingString == "w"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.W));
            }else if(MappingString == "e"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.E));
            }else if(MappingString == "r"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.R));
            }else if(MappingString == "t"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.T));
            }else if(MappingString == "y"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.Y));
            }else if(MappingString == "u"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.U));
            }else if(MappingString == "i"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.I));
            }else if(MappingString == "o"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.O));
            }else if(MappingString == "p"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.P));
            }else if(MappingString == "a"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.A));
            }else if(MappingString == "s"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.S));
            }else if(MappingString == "d"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.D));
            }else if(MappingString == "f"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.F));
            }else if(MappingString == "g"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.G));
            }else if(MappingString == "h"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.H));
            }else if(MappingString == "j"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.J));
            }else if(MappingString == "k"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.K));
            }else if(MappingString == "l"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.L));
            }else if(MappingString == "z"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.Z));
            }else if(MappingString == "x"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.X));
            }else if(MappingString == "c"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.C));
            }else if(MappingString == "v"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.V));
            }else if(MappingString == "b"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.B));
            }else if(MappingString == "n"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.N));
            }else if(MappingString == "m"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.M));
            }else if(MappingString == "space"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.Space));
            }else if(MappingString == "lalt"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.LeftAlt));
            }else if(MappingString == "ralt"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.RightAlt));
            }else if(MappingString == "lshift"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.LeftShift));
            }else if(MappingString == "rshift"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.RightShift));
            }else if(MappingString == "return"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.Enter));
            }else if(MappingString == "esc"){
                KeyboardDevice.Mapping.Add(new InputKeyboard.InputKeyboardMapping(Input,Keys.Escape));
            }else{
                    DeveloperConsole.AddMessage(DeveloperConsole.MessageType.ERROR,
                    String.Format("Unexpected token in \"LayoutInput.cfg\" |{0}|", MappingString));
            }
        }
    }
}
