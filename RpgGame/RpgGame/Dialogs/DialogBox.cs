using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgGame.Manager;
using RpgGame.Tools;

namespace RpgGame.Dialogs
{
    static class DialogBox
    {
        static Rectangle boxSize, txtSize, actorSize;
        static Texture2D background, borderImage;
        static SpriteFont spriteFont;
        static Color borderColor;
        static int borderWidth, nameBoxWidth, messagesIndex, currentDialogNodeIndex;
        static bool show;
        static String message, actor;
        static List<String> messages;
        static Dialog currentDialog;
        static DialogNode currentDialogNode;
        private static Vector2 _textPosition, _namePosition, offset;
        static Vector2 TextPosition { get { return _textPosition; } set { _textPosition = value; } }
        static Vector2 NamePosition { get { return _namePosition; } set { _namePosition = value; } }



        public static void startDialog(int DialogID)
        {
            
        }

        public static void ShowBox(){
            if (currentDialog != null)
            {        
                messages = TextBoxCropper.cropText(currentDialogNode.Text, txtSize, ref spriteFont);
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
            TextPosition = new Vector2(GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Width/18, GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Height/5);
            background = RpgGame.ContentManager.Load<Texture2D>("Overlays/Dialog/dialog_background");
            borderImage = RpgGame.ContentManager.Load<Texture2D>("Overlays/Dialog/border");
            borderColor = Color.Black;
            borderWidth = 2;
            offset = new Vector2(4, 1);
            spriteFont = RpgGame.ContentManager.Load<SpriteFont>("Fonts/test");
            boxSize = new Rectangle((int)TextPosition.X, (int)TextPosition.Y, GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Width / 10, GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Height / 14);
            txtSize = new Rectangle(boxSize.X + 10, boxSize.Y + 10, boxSize.Width - 20, boxSize.Height - 20);
            actorSize = new Rectangle((int)TextPosition.X, (int)TextPosition.Y+boxSize.Y, GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Width / 25, GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Height / 50);
            //actorSize = new Rectangle((int)TextPosition.X, (int)TextPosition.Y + boxSize.Y, nameBoxWidth, GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Height / 50);
            NamePosition = new Vector2((int)TextPosition.X, (int)TextPosition.Y+boxSize.Y);
            show = false;
        }

        public static void Update(GameTime gameTime)
        {
            if (show)
            {

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

                if (currentDialogNode != null)
                {

                }
            }
        }

        public static void Draw(ref SpriteBatch batch)
        {
            //String testor = "Hallo, dies ist ein langer Text, um die Cropfunktion des Croppers zu testen. Aber machen wir es lieber ein Stueck laenger, um zu sehen, ob auch wirklich 2 elemente hiznugefuegt werden. War anscheinend dopch zu kurz fuer eine Box machen wir es naheuzu unendlich lang damit es auf keinen fall ein fehler sein kann haha";
            //List<String> Testlist = TextBoxCropper.cropText(testor, txtSize, ref spriteFont);
            if (show)
            {
                batch.Draw(borderImage, new Rectangle(boxSize.X - borderWidth, boxSize.Y - borderWidth, boxSize.Width + 2 * borderWidth, boxSize.Height + 2 * borderWidth), borderColor);
                batch.Draw(background, boxSize, null, Color.Blue * 0.9f);
                batch.Draw(borderImage, new Rectangle(actorSize.X - borderWidth, actorSize.Y - borderWidth, actorSize.Width + 2 * borderWidth, actorSize.Height + 2 * borderWidth), borderColor);
                batch.Draw(background, actorSize, null, Color.Blue * 0.9f);
                batch.DrawString(spriteFont, message, TextPosition+ offset, Color.White);
                batch.DrawString(spriteFont, actor, NamePosition+ offset, Color.White);
            }
        }



        public static void NextText()
        {
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
