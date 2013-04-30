using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.Tools
{
    class TextBoxCropper
    {
        public static List<String> cropText(String message, Rectangle destinationBox, ref SpriteFont spriteFont)
        {
            List<String> returnMessages;
            String line = "";
            String box = "";
            returnMessages = new List<String>();
            float textHeight = 0;
            String[] words = message.Split(' ');
            foreach(String word in words){
                if (spriteFont.MeasureString(line + word).X > destinationBox.Width)
                {
                    textHeight += spriteFont.MeasureString(word).Y;
                    if (textHeight < destinationBox.Height)
                    {
                        box += line + "\n";
                    }
                    else
                    {
                        returnMessages.Add(box);
                        box = "";
                        textHeight = 0;
                    }
                    line = "";
                }
                line += word + " ";  
            }
            box += line;
            returnMessages.Add(box);
            return returnMessages;
        }
    }
}
