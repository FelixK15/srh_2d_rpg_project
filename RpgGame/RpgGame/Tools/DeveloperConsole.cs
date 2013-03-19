using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using RpgGame.Manager;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using System.Reflection;
using System.IO;

namespace RpgGame.Tools
{
    public class DeveloperConsole
    {
        public enum MessageType
        {
            NORMAL,     // White
            DEBUG,      // Green
            ERROR,      // Red
            TRACE       // Blue
        }

        //Nested class for message
        private class ConsoleMessage
        {
            public String       Message { get; set; }
            public MessageType  Type    { get; set; }

            public ConsoleMessage(String message, MessageType type)
            {
                Message = message;
                Type    = type;
            }
        }

        private static bool                 DrawFlashingLine    { get; set; }   //Boolean that determines if the flashing line should be drawn
        private static int                  FrameCounter        { get; set; }   //Frame counter for drawing the flashing line at the input
        private static SpriteFont           Font                { get; set; }   //Font to use for drawing text
        private static String               Input               { get; set; }   //String representing the input of a user
        private static List<ConsoleMessage> Messages            { get; set; }   //All messages

        private static Texture2D FlashingLine;      //Graphic for the flashing line
        private static Texture2D Background;        //Background of the console
        private static Texture2D InputBox;          //Inputbox for input
        private static Assembly  ContextAssembly;   //The assembly of the consolecontext.dll
        private static object    ContextInstance;   //An instance of the context class from the console contest.dll
        
        public static bool IsOpen { get; set; }    

        public static void Initialize()
        {
            Font        = RpgGame.ContentManager.Load<SpriteFont>("Fonts\\Arial");
            Messages    = new List<ConsoleMessage>();
            Input       = "";
            Background  = new Texture2D(GraphicSettings.GraphicDevice,
                                        GraphicSettings.ClientWidth,
                                        (int)(GraphicSettings.ClientHeight*0.5));
            InputBox    = new Texture2D(GraphicSettings.GraphicDevice,
                                        GraphicSettings.ClientWidth,
                                        Font.LineSpacing);
            FlashingLine = new Texture2D(GraphicSettings.GraphicDevice,
                                         1,Font.LineSpacing - 4);

            Texture2DFiller.Fill(ref FlashingLine,Color.Black);
            Texture2DFiller.Fill(ref Background,Color.Black);
            Texture2DFiller.Fill(ref InputBox,Color.White);

            BufferedInput.CharEnterCallbacks.Add(OnCharEnter);

            try{
                String ContextPath  = Path.GetFullPath("ConsoleContext.dll");
                ContextAssembly     = Assembly.LoadFile(ContextPath);
                ContextInstance     = ContextAssembly.CreateInstance("ConsoleContext.Context");
            }catch(Exception ex){
                AddMessage(MessageType.ERROR,ex.Message);
                AddMessage(MessageType.ERROR,"Could not load 'ConsoleContext.dll'");
            }   
        }

        public static void AddMessage(MessageType Type,String Message)
        {
            #if (DEBUG)
                //add the name of the function from which addmessage was called to the output
                //Format: ( CLASSNAME::METHODNAME - MESSSAGE)
                StackTrace Trace = new StackTrace();
                Message = "(" + Trace.GetFrame(1).GetMethod().ReflectedType.Name + "::" + Trace.GetFrame(1).GetMethod().Name + ") - " + Message;
                Messages.Add(new ConsoleMessage(Message,Type));
            #endif
        }

        public static void Draw(ref SpriteBatch Batch)
        {
            float x = 0.0f;
            float y = Background.Height;

            //Draw all messages reversed, starting from the last message
            Batch.Draw(Background,Vector2.Zero,Color.White);
            for(int i = Messages.Count - 1;i >= 0;--i){
                ConsoleMessage CurrentMessage = Messages.ElementAt<ConsoleMessage>(i);
                Color          DrawColor      = Color.White;

                //Determine string color
                if(CurrentMessage.Type == MessageType.DEBUG){
                    DrawColor = Color.LightGreen;
                }else if(CurrentMessage.Type == MessageType.ERROR){
                    DrawColor = Color.Red;
                }else if(CurrentMessage.Type == MessageType.TRACE){
                    DrawColor = Color.LightBlue;
                }
                
                //calculate new position
                Vector2 TextSize = Font.MeasureString(CurrentMessage.Message);
                y -= TextSize.Y;
                
                //Draw message
                Batch.DrawString(Font,CurrentMessage.Message,new Vector2(x,y),DrawColor);
               
            }

            //Draw inputbox and input
            Batch.Draw(InputBox,new Vector2(0,GraphicSettings.ClientHeight*0.5f),Color.White);
            Batch.DrawString(Font,Input,new Vector2(0,GraphicSettings.ClientHeight*0.5f),Color.Black);
        
            //Every 60 frames we draw the flashing line for 60 frames
            if(FrameCounter % 60 == 0){
                DrawFlashingLine = !DrawFlashingLine;
            }

            if(DrawFlashingLine){
                Batch.Draw(FlashingLine,new Vector2(Font.MeasureString(Input).X + 2,GraphicSettings.ClientHeight*0.5f + 2),Color.White);
            }

            ++FrameCounter;
        }

        private static void OnCharEnter(char Character,int Params)
        {
            if(IsOpen){
                if(Character >= 32 && Character <= 126){
                    Input += Character;
                }else if(Character == 13){
                    if(Input != ""){
                        AddMessage(MessageType.TRACE, String.Format("Processing input '{0}'", Input));
                        _ProcessInput();
                        Input = "";
                    }
                }else if(Character == 8){
                    if(Input.Length > 0){
                        Input = Input.Substring(0,Input.Length - 1);
                    }
                }else if(Character == 27){
                    IsOpen = false;
                    
                }
            }
        }

        private static void _ProcessInput()
        {
            if(ContextAssembly == null){
                return;
            }

            Input = Input.Trim();

            List<String> Parameters = new List<String>();
            String ProcessedInput   = "";
            bool FoundWhiteSpace    = false;
            bool FoundComma         = false;

            //Remove multiple white spaces and commas
            for(int i = 0;i < Input.Length;++i){
                if(Input[i] == ' '){
                    if(!FoundWhiteSpace){
                        ProcessedInput += Input[i];
                    }
                    FoundWhiteSpace = true;
                    FoundComma      = false;
                }else if(Input[i] == ','){
                   if(!FoundComma){
                       ProcessedInput += Input[i];
                   }
                    FoundComma      = true;
                    FoundWhiteSpace = false;
                }else{
                    FoundWhiteSpace = false;
                    FoundComma      = false;
                    ProcessedInput += Input[i];
                }
            }
            
            //Get Function Name
            int EndFunction = ProcessedInput.IndexOf(' ');
            if(EndFunction == -1){
                EndFunction = ProcessedInput.Length;
            }

            String FunctionName = ProcessedInput.Substring(0,EndFunction);

            //Remove the function name from the processed input
            ProcessedInput = ProcessedInput.Substring(FunctionName.Length);
            ProcessedInput = ProcessedInput.Trim();

            //Check if a function with the function name exists
            MethodInfo Info = ContextInstance.GetType().GetMethod(FunctionName);
            if(Info == null){
                AddMessage(MessageType.ERROR,String.Format("Function '{0}' not found.",FunctionName));
                return; //Get out if no function has been found.
            }
            
            //Get Parameters
            int ParameterPosition     = -1;
            do 
            {
                ParameterPosition = ProcessedInput.IndexOf(',');
                
                //Last parameter
                if(ParameterPosition == -1 && ProcessedInput.Length > 0){
                    ParameterPosition = ProcessedInput.Length;
                }

                if(ParameterPosition != -1){
                    Parameters.Add(ProcessedInput.Substring(0,ParameterPosition));

                    //Increment position to substring parameter AND comma
                    if(ProcessedInput.Length > 1 && ParameterPosition < ProcessedInput.Length){
                        ++ParameterPosition;
                    }
                    ProcessedInput = ProcessedInput.Substring(ParameterPosition);
                }
            } while (ParameterPosition != -1);

            //Try to call the function
            try{
                Info.Invoke(ContextInstance, (object[])Parameters.ToArray());
            }catch(Exception ex){
                AddMessage(MessageType.ERROR,ex.Message);
            }

            return;
        }
    }
}
