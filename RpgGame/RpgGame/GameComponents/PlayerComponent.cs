using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgGame.Events;
using RpgGame.Manager;
using RpgGame.Tools;

namespace RpgGame.GameComponents
{
    class PlayerComponent : BaseGameComponent
    {
        private PlayerIndex Player { get; set; }
        private int ActionCounter { get; set; }
        private int ColorCounter {get;set;}

        public PlayerComponent(PlayerIndex player)
            : base("PlayerComponent")
        {
            Player = player;
            ActionCounter = 100;
        }

        public override void Update(GameTime gameTime)
        {
            Parent.Velocity = Vector2.Zero;
            AnimationComponent animComponent = Parent.GetComponent<AnimationComponent>();

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Parent.Velocity = new Vector2(Parent.Velocity.X, -1);
                Parent.Orientation = new Vector2(0, -1);
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                Parent.Velocity = new Vector2(Parent.Velocity.X, 1);
                Parent.Orientation = new Vector2(0, 1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Parent.Velocity = new Vector2(-1, Parent.Velocity.Y);
                Parent.Orientation = new Vector2(-1, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Parent.Velocity = new Vector2(1, Parent.Velocity.Y);
                Parent.Orientation = new Vector2(1, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                ActionCounter = 0;
            }

            if (Parent.Orientation.X == 1)
            {
                if (Parent.Velocity != Vector2.Zero)
                {
                    animComponent.setCurrentAnimation("WalkRight");
                }
                else
                {
                    animComponent.setCurrentAnimation("LookRight");
                }
            }
            else if (Parent.Orientation.X == -1)
            {
                if (Parent.Velocity != Vector2.Zero)
                {
                    animComponent.setCurrentAnimation("WalkLeft");
                }
                else
                {
                    animComponent.setCurrentAnimation("LookLeft");
                }
            }
            else if (Parent.Orientation.Y == 1)
            {
                if (Parent.Velocity != Vector2.Zero)
                {
                    animComponent.setCurrentAnimation("WalkDown");
                }else{
                    animComponent.setCurrentAnimation("LookDown");
                }
            }
            else if (Parent.Orientation.Y == -1)
            {
                if (Parent.Velocity != Vector2.Zero)
                {
                    animComponent.setCurrentAnimation("WalkUp");
                }
                else
                {
                    animComponent.setCurrentAnimation("LookUp");
                }
            }

            if (ActionCounter < 100)
            {
                ++ActionCounter;
            }
        }

        public override void Draw(ref SpriteBatch batch)
        {
    
        }

        public override void Init()
        {
            
        }

        public override BaseGameComponent Copy()
        {
            return null;
        }
    }
}
