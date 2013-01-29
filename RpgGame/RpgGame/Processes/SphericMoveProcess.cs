using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RpgGame.Processes
{
    class SphericMoveProcess : Process
    {
        private float Angle { get; set; }
        private float EndAngle { get; set; }
        private float StepAngle { get; set; }
        private int Duration { get; set; }
        private float Radius { get; set; }
        private int EndRadius { get; set; }
        private float StepRadius { get; set; }
        private Vector2 Position { get; set; }
        private GameObject Object { get; set; }

        public SphericMoveProcess(GameObject gameObject,Vector2 position,float startAngle, float endAngle, int startRadius, int endRadius,int duration)
        {
            Position = position;

            Object = gameObject;
            Angle = startAngle;
            Radius = (float)startRadius;
            EndAngle = endAngle;
            EndRadius = endRadius;
            Duration = duration;

            StepAngle = (endAngle - startAngle) / duration;
            StepRadius = ((float)endRadius - (float)startRadius) / duration;
        }

        public override void Start()
        {
            float x, y;
            x = Position.X + (Radius * (float)Math.Cos(Angle));
            y = Position.Y + (Radius * (float)Math.Sin(Angle));

            Object.Position = new Vector2(x, y);
        }

        public override void Update(GameTime gameTime)
        {
            if (Duration <= 0)
            {
                Finished = true;
            }
            else
            {
                Duration -= gameTime.ElapsedGameTime.Milliseconds;
                Angle += StepAngle * gameTime.ElapsedGameTime.Milliseconds;
                Radius += StepRadius * gameTime.ElapsedGameTime.Milliseconds;

                float x = (float)(Radius * Math.Cos(Angle));
                float y = (float)(Radius * Math.Sin(Angle));

                Object.Position = new Vector2(Position.X + x, Position.Y + y);
            }
        }

        public override void End()
        {
            float x, y;
            x = (float)(EndRadius * Math.Cos(EndAngle));
            y = (float)(EndRadius * Math.Sin(EndAngle));

            Object.Position = new Vector2(Position.X + x, Position.Y + y);
        }
    }
}
