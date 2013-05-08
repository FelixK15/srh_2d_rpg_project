using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgGame.GameStates;
using RpgGame.Manager;
using RpgGame.Tools;

namespace RpgGame.Dialogs
{
    public static class DialogBox
    {
        static Rectangle boxSize, txtSize, actorSize; //, arrowSize;
        static Texture2D background, borderImage;//, arrowImage;
        static SpriteFont spriteFont;
        static Color borderColor;
        static int borderWidth, nameBoxWidth, messagesIndex, currentDialogNodeIndex, characterIndex;
        static bool show, messageDrawn;
        static String message, actor, drawMessage;
        static List<String> messages;
        static Dialog currentDialog;
        static DialogNode currentDialogNode;
        static float elapsedTime, messageTimer;
        const float messageSpeed = 0.051f;
        private static Vector2 _textPosition, _namePosition, offset, _arrowPosition;
        static Vector2 TextPosition { get { return _textPosition; } set { _textPosition = value; } }
        static Vector2 NamePosition { get { return _namePosition; } set { _namePosition = value; } }
        //static Vector2 ArrowPosition { get { return _arrowPosition; } set { _arrowPosition = value; } }
        private static List<Dialog> allDialogs;


        public static bool isMessageDrawn()
        {
            return messageDrawn;
        }

        public static void drawFullMessage()
        {
            messageDrawn = true;
            drawMessage = message;
        }

        public static Dialog getDialog(int DialogID)
        {
            if (DialogID > 0 && DialogID < allDialogs.Count)
            {
                return allDialogs.ElementAt(DialogID);
            }
            return null;
        }

        public static void start(int DialogID)
        {
            if (DialogID > 0 && DialogID < allDialogs.Count)
            {
                GameStateMachine.AddState(new DialogState(allDialogs.ElementAt(DialogID-1)));
            }           
        }

        public static void ShowBox(){
            if (currentDialog != null)
            {        
                messages = TextBoxCropper.cropText(currentDialogNode.Text, boxSize, ref spriteFont);
                messagesIndex = 0;
                ShowText(messages.ElementAt(messagesIndex), currentDialogNode.Actor);
            }
        }

        public static void ShowText(string txt, string name)
        {
            message = txt;
            actor = name;
            nameBoxWidth = (int)spriteFont.MeasureString(actor).X;          
        }

        public static void Initialize()
        {
            DialogLoader DLoader = new DialogLoader("Dialog\\Dialogs");
            allDialogs = DLoader.Dialogs;
            drawMessage = "";
            messageDrawn = false;
            characterIndex = 0;
            TextPosition = new Vector2(GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Width/18, GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Height/5);
            background = RpgGame.ContentManager.Load<Texture2D>("Overlays/Dialog/dialog_background");
            borderImage = RpgGame.ContentManager.Load<Texture2D>("Overlays/Dialog/border");
            //arrowImage = RpgGame.ContentManager.Load<Texture2D>("Overlays/Dialog/doublearrow");
            borderColor = Color.Black;
            borderWidth = 2;
            offset = new Vector2(4, 1);
            spriteFont = RpgGame.ContentManager.Load<SpriteFont>("Fonts/test");
            boxSize = new Rectangle((int)TextPosition.X, (int)TextPosition.Y, GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Width / 10, GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Height / 14);
            txtSize = new Rectangle(boxSize.X + 10, boxSize.Y + 10, boxSize.Width - 20, boxSize.Height - 20);
            actorSize = new Rectangle((int)TextPosition.X, (int)TextPosition.Y+boxSize.Y, GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Width / 25, GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Height / 50);
            //arrowSize = new Rectangle((int)TextPosition.X, (int)TextPosition.Y+boxSize.Y, GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Width / 100, GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Height / 150);
            NamePosition = new Vector2((int)TextPosition.X, (int)TextPosition.Y+boxSize.Y);
            //ArrowPosition = new Vector2((int)TextPosition.X - boxSize.X, (int)TextPosition.Y - boxSize.Y);
            show = false;
        }

        public static void Update(GameTime gameTime)
        {          
            if (show)
            {
                if (!messageDrawn)
                {
                    elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    messageTimer += elapsedTime;
                    if (messageTimer >= messageSpeed)
                    {
                        if (characterIndex < message.Length)
                        {
                            drawMessage += message[characterIndex];
                            characterIndex++;
                        }
                        else
                        {
                            messageDrawn = true;
                        }
                        messageTimer = 0;
                    }
                }
                _textPosition.X = GraphicSettings.Camera.Position.X + GraphicSettings.Camera.TargetArea.Center.X / 2;
                _textPosition.Y = GraphicSettings.Camera.Position.Y + GraphicSettings.Camera.TargetArea.Bottom;
                boxSize.X = (int)TextPosition.X;
                boxSize.Y = (int)TextPosition.Y;
                txtSize.X = boxSize.X + 10;
                txtSize.Y = boxSize.Y + 10;
                actorSize.X = (int)TextPosition.X;
                actorSize.Y = (int)(TextPosition.Y - boxSize.Height / 2.8);
                actorSize.Width = nameBoxWidth+10;
                _namePosition.X = (int)TextPosition.X;
                _namePosition.Y = (int)(TextPosition.Y - boxSize.Height / 2.8);
                //arrowSize.X = (int)boxSize.X +txtSize.Width;
                //arrowSize.Y = (int)boxSize.Y +txtSize.Height;
            }
        }

        public static void Draw(ref SpriteBatch batch)
        {
            if (show)
            {
                batch.Draw(borderImage, new Rectangle(boxSize.X - borderWidth, boxSize.Y - borderWidth, boxSize.Width + 2 * borderWidth, boxSize.Height + 2 * borderWidth), borderColor);
                batch.Draw(background, boxSize, null, Color.Blue * 0.9f);
                batch.Draw(borderImage, new Rectangle(actorSize.X - borderWidth, actorSize.Y - borderWidth, actorSize.Width + 2 * borderWidth, actorSize.Height + 2 * borderWidth), borderColor);
                batch.Draw(background, actorSize, null, Color.Blue * 0.9f);
                //batch.Draw(arrowImage, arrowSize, Color.White * 0.9f);
                batch.DrawString(spriteFont, drawMessage, TextPosition+ offset, Color.White);
                batch.DrawString(spriteFont, actor, NamePosition+ offset, Color.White);
            }
        }



        public static void NextText()
        {
            messageDrawn = false;
            drawMessage = "";
            characterIndex = 0;
            if (messagesIndex + 1 < messages.Count)
            {
                messagesIndex++;
                ShowText(messages.ElementAt(messagesIndex), currentDialogNode.Actor);
            }
            else
            {            
                if (currentDialogNodeIndex + 1 < currentDialog.DialogNodes.Count)
                {
                    NextNode();
                }
                else
                {
                    stopDialog();
                }
                
            }
        }

        private static void NextNode()
        {

            currentDialogNodeIndex++;
            currentDialogNode = currentDialog.DialogNodes.ElementAt(currentDialogNodeIndex);
            if (currentDialogNode != null) ShowBox();
        }

        public static void stopDialog()
        {
            show = false;
            GameStateMachine.RemoveTopState();
        }

        public static void startDialog(Dialog currentDialogVar)
        {
            currentDialog = currentDialogVar;        
            if (currentDialog.DialogNodes != null)
            {
                currentDialogNode = currentDialog.DialogNodes.ElementAt(0);
                currentDialogNodeIndex = 0;
                show = true;
                ShowBox();
            }  
        }
    }
}
