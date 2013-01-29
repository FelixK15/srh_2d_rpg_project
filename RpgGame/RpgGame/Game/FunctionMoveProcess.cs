using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame.Game
{
    delegate void FunctionDelegate(out int x, out int y,int duration, GameTime gameTime);

    class FunctionMoveProcess : Process
    {
        private GameObject Object
        {
            get;
            set;
        }

        private FunctionDelegate Function
        {
            get;
            set;
        }

        private int Duration
        {
            get;
            set;
        }

        public FunctionMoveProcess(GameObject go, FunctionDelegate function,int duration)
        {
            Object = go;
            Function = function;
            Duration = duration;
        }

        public override void Start()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            int x = Object.X;
            int y = Object.Y;

            Function(out x, out y,Duration,gameTime);

            Object.X = x;
            Object.Y = y;
        }

        public override void End()
        {
            
        }
    }
}
