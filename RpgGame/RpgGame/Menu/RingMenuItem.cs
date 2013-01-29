using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.Menu
{
    class RingMenuItem : IComparable
    {
        public Texture2D Graphic
        {
            get;
            set;
        }

        public int Priority
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        public RingMenuItem(string name, string graphicpath)
        {
            Priority = 0;
            Name = name;
            Graphic = RpgGame.ContentManager.Load<Texture2D>(graphicpath);
        }

        public RingMenuItem(string name, Texture2D Graphic)
        {
            Name = name;
            this.Graphic = Graphic;
        }

        public Int32 CompareTo(RingMenuItem other)
        {
            if (other.Priority > Priority)
            {
                return 1;
            }
            else if (other.Priority < Priority)
            {
                return -1;
            }

            return 0;
        }

        public Int32 CompareTo(System.Object obj)
        {
            if (obj is RingMenuItem)
            {
                return CompareTo(obj as RingMenuItem);
            }

            return 0;
        }
    }
}
