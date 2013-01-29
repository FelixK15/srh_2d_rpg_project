using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RpgGame.Processes
{
    class MoveProcess : Process
    {
        private GameObject Object { get; set; }

        private Vector2 Destination { get; set; }
        private Vector2 Step { get; set; }

        private double Duration { get; set; }

        public MoveProcess(GameObject gameObject,Vector2 destination,double duration)
        {
            Object = gameObject;

            Destination = destination;
            Step = (destination - gameObject.Position) / (float)(duration / 16);
            Duration = duration;
        }

        public override void Start()
        {
            Started = true;
        }

        public override void Update(GameTime gameTime)
        {
            Duration -= gameTime.ElapsedGameTime.Milliseconds;

            Object.Position = Object.Position + Step;

            if(Duration < 0.0)
            {
                Finished = true;
                Object.Position = Destination;
            }
        }

        public override void End()
        {
            
        }
    }
}
