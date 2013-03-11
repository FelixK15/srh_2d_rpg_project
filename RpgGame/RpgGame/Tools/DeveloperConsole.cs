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

        private static SpriteFont           Font        { get; set; }   //Font to use for drawing text
        private static String               Input       { get; set; }   //String representing the input of a user
        private static Keys                 LastKey     { get; set; }
        private static List<ConsoleMessage> Messages    { get; set; }   //All messages

        private static Texture2D Background;  //Background of the console
        private static Texture2D InputBox;    //Inputbox for input
        private static Assembly  ContextAssembly;
        private static object    ContextInstance;
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

            Texture2DFiller.Fill(ref Background,Color.Black);
            Texture2DFiller.Fill(ref InputBox,Color.White);

            BufferedInput.CharEnterCallbacks.Add(OnCharEnter);

            //TODO
//             try{
//                 String ContextPath = Path.GetFullPath("ConsoleContext.dll");
//                 ContextAssembly = Assembly.LoadFile(ContextPath);
//                 ContextInstance = ContextAssembly.CreateInstance("ConsoleContext.Context");
//             }catch(Exception ex){
//                 AddMessage(MessageType.ERROR,ex.Message);
//             }
            
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
            const int Padding     = 5;
            const int MaxMessages = 20;
            int StartMessageIndex = 0;

            if(Messages.Count > MaxMessages){
                StartMessageIndex = Messages.Count - MaxMessages;
            }

            //draw last 20 messages
            float x = 0.0f;
            float y = 0.0f;

            Batch.Draw(Background,Vector2.Zero,Color.White);
            for(int i = StartMessageIndex;i < StartMessageIndex + Messages.Count;++i){
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

                Batch.DrawString(Font,CurrentMessage.Message,new Vector2(x,y),DrawColor);
                Vector2 TextSize = Font.MeasureString(CurrentMessage.Message);
                y += TextSize.Y + Padding;
            }

            Batch.Draw(InputBox,new Vector2(0,GraphicSettings.ClientHeight*0.5f),Color.White);
            Batch.DrawString(Font,Input,new Vector2(0,GraphicSettings.ClientHeight*0.5f),Color.Black);
        }

        private static void OnCharEnter(char Character,int Params)
        {
            if(IsOpen){
                if(Character >= 32 && Character <= 126){
                    Input += Character;
                }else if(Character == 13){
                    AddMessage(MessageType.TRACE,String.Format("Processing input '{0}'",Input));
                    //TODO
//                     MethodInfo Info = ContextInstance.GetType().GetMethod(Input);
//                     Info.Invoke(ContextInstance,new object[] {"Player"});
                    Input = "";
                }else if(Character == 8){
                    if(Input.Length > 0){
                        Input = Input.Substring(0,Input.Length - 1);
                    }
                }else if(Character == 27){
                    IsOpen = false;
                    
                }
            }
        }
    }
}
