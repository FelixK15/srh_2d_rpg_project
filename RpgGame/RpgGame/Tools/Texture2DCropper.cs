using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using RpgGame.Manager;

namespace RpgGame.Tools
{
    class Texture2DCropper
    {
        public static Texture2D Crop(Texture2D source, Rectangle rect)
        {
            Texture2D destination = null;

            if((rect.Width + rect.X) > source.Width || (rect.Height + rect.Y) > source.Height){
                int newWidth    = source.Width - rect.X;
                int newHeight   = source.Height - rect.Y;

                rect = new Rectangle(rect.X,rect.Y,newWidth,newHeight);
            }
 
            if(rect.Width != 0 && rect.Height != 0){
                int counter = 0;
                destination = new Texture2D(GraphicSettings.GraphicDevice, rect.Width, rect.Height);
                Color[] destinationPixels = new Color[rect.Width * rect.Height];
                Color[] sourcePixels = new Color[source.Width * source.Height];
                source.GetData<Color>(sourcePixels);
                for (int y = rect.Y; y < rect.Height + rect.Y; ++y)
                {
                    for (int x = rect.X; x < rect.Width + rect.X; ++x)
                    {
                        destinationPixels[counter] = sourcePixels[y * source.Width + x];
                        ++counter;
                    }
                }

                destination.SetData<Color>(destinationPixels);
            }
            
            return destination;
        }
    }
}
