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
        private PlayerIndex Player          { get; set; }
        private int ActionCounter           { get; set; }
        private int ColorCounter            { get; set; }
        private int InteractionCooldown     { get; set; }

        private Rectangle InteractionRect   { get; set; }

        public PlayerComponent(PlayerIndex player)
            : base("PlayerComponent")
        {
            Player = player;
            ActionCounter = 100;
        }

        public override void Update(GameTime gameTime)
        {
            InteractionRect = _CreateInteractionRect();
            Parent.Velocity = Vector2.Zero;
            AnimationComponent animComponent = Parent.GetComponent<AnimationComponent>();
            CollisionComponent collComponent = Parent.GetComponent<CollisionComponent>();

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Parent.Velocity = new Vector2(Parent.Velocity.X, -2);
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                Parent.Velocity = new Vector2(Parent.Velocity.X, 2);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Parent.Velocity = new Vector2(-2, Parent.Velocity.Y);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Parent.Velocity = new Vector2(2, Parent.Velocity.Y);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                CollisionComponent component = Parent.GetComponent<CollisionComponent>();
                if(component != null)
                {
                    if (InteractionCooldown <= 0)
                    {
                        EventManager.AddEventToQuery(new InteractionEvent(_CreateInteractionRect(), Parent));
                    }
                }
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
        }

        public override void Draw(ref SpriteBatch batch)
        {
            if (InteractionRect.Width > 0 && InteractionRect.Height > 0)
            {
                Texture2D test = new Texture2D(GraphicSettings.GraphicDevice, InteractionRect.Width, InteractionRect.Height);
                Color[] data = new Color[test.Width * test.Height];
                for (int i = 0; i < data.Length; ++i)
                {
                    data[i] = new Color(0.5f, 1.0f, 0.5f, 1f);
                }
                test.SetData<Color>(data);
                batch.Draw(test, InteractionRect, Color.White);
            }
        }

        public override void Init()
        {
            
        }

        public override BaseGameComponent Copy()
        {
            return null;
        }

        private Rectangle _CreateInteractionRect()
        {
            CollisionComponent component = Parent.GetComponent<CollisionComponent>();

            int width = 10;
            int height = 10;

            int x = (int)(component.Position.X + component.Offset.X) + (int)(Parent.Orientation.X * width);
            int y = (int)(component.Position.Y + component.Offset.Y) + (int)(Parent.Orientation.Y * height);

            //If the player is facing down or right, we have to add the
            //dimension of the collisioncomponent as well
            if(Parent.Orientation.X > 0){
                x += component.Width;
            }else if (Parent.Orientation.Y > 0){
                y += component.Height;
            }
            
            return new Rectangle(x, y, width, height);
        }
    }
}
