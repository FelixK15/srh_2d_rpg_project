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
                if (spriteFont.MeasureString(line + word).X >= destinationBox.Width - 5)
                {
                    textHeight += spriteFont.MeasureString(word).Y- 2;
                    if (textHeight < destinationBox.Height)
                    {
                        box += line + "\n";
                    }
                    else
                    {                       
                        returnMessages.Add(box);
                        box = line + "\n";
                        textHeight = spriteFont.MeasureString(word).Y;
                    }
                    line = "";
                }
                line += word + " ";                 
            }
            if (line != "")
            {
                if(textHeight + spriteFont.MeasureString(line).Y < destinationBox.Height){
                    box+=line;
                } else{
                    returnMessages.Add(box);
                    box = line;
                }
            }
            if (box != "") returnMessages.Add(box);
            return returnMessages;
        }
    }
}
