using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RpgGame.GameComponents
{
    class MoveableComponent : BaseGameComponent
    {
        public bool MoveLeft { get; set; }
        public bool MoveRight { get; set; }

        public bool MoveUp { get; set; }
        public bool MoveDown { get; set; }

        public float Speed { get; set; }

        public MoveableComponent() : base("MoveableComponent")
        {
            MoveDown = MoveLeft = MoveRight = MoveUp = false;
            Speed = 1.0f;
        }

        public MoveableComponent(float speed) : this()
        {
            Speed = speed;
        }

        public override void Update(GameTime gameTime)
        {
            float velocity = Speed * gameTime.ElapsedGameTime.Milliseconds;
            float x, y;
            x = Parent.Position.X;
            y = Parent.Position.Y;

            if (MoveLeft)
            {
                x-= velocity;
            }

            if(MoveRight)
            {
                x+= velocity;
            }

            if (MoveUp)
            {
                y -= velocity;
            }

            if (MoveDown)
            {
                y += velocity;
            }

            Parent.Position = new Vector2(x,y);
        }

        public override BaseGameComponent Copy()
        {
            MoveableComponent newComponent = new MoveableComponent();
            newComponent.Speed = Speed;
            return newComponent;
        }
    }
}
