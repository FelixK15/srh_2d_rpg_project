using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RpgGame.Processes
{
    abstract public class Process
    {
        public abstract void Start();
        public abstract void Update(GameTime gameTime);
        public abstract void End();

        public bool Started
        {
            get;
            protected set;
        }

        public bool Finished
        {
            get;
            protected set;
        }

        public Process Next
        {
            get;
            set;
        }

        public Process()
        {
            Next = null;
            Started = false;
            Finished = false;
        }
    }
}
