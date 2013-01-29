using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

using RpgGame.Tools;

namespace RpgGame.Manager
{
    class GraphicSettings
    {
        public static GraphicsDevice GraphicDevice { get; set; }
        
        public static Camera Camera { get; set; }
        
        public static int ClientWidth { get; set; }
        public static int ClientHeight { get; set; }
    }
}
