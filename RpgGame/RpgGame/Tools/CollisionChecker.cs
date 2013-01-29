using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RpgGame.Tools
{
    class CollisionChecker
    {
        public static bool Intersect(Rectangle r1, Rectangle r2)
        {
            if (r1.X < r2.X + r2.Width && r1.X + r1.Width > r2.X)
            {
                if (r1.Y < r2.Y + r2.Height && r1.Y + r1.Width > r2.Y)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
