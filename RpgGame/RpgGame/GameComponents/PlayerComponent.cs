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
        private PlayerIndex Player              { get; set; }
        private int         AttackCounter       { get; set; }
        private int         ColorCounter        { get; set; }
        private int         IgnoreInputSpan     { get; set; }
        private bool        InteractionSwitch   { get; set; }
        private bool        AttackSwitch        { get; set; }
        private Rectangle   InteractionRect     { get; set; }

        public  int         Experience          { get; set; }

        public  string[]    WalkAnimations      { get; set; }
        public  string[]    IdleAnimations      { get; set; }

        public const int LEFT   = 0;
        public const int RIGHT  = 1;
        public const int UP     = 2;
        public const int DOWN   = 3;

        public const int MAX_DIRECTIONS = 4;

        public PlayerComponent(PlayerIndex player)
            : base("PlayerComponent")
        {
            IgnoreInputSpan = 0;
            Player          = player;
            AttackCounter   = 100;
            AttackSwitch    = true;

            WalkAnimations  = new string[MAX_DIRECTIONS];
            IdleAnimations  = new string[MAX_DIRECTIONS];
        }

        public override void Update(GameTime gameTime)
        {
            InteractionRect = _CreateInteractionRect();
            Parent.Velocity = Vector2.Zero;

            AnimationComponent animComponent = Parent.GetComponent<AnimationComponent>();
            CollisionComponent collComponent = Parent.GetComponent<CollisionComponent>();
            WeaponComponent    weapComponent = Parent.GetComponent<WeaponComponent>();
            
            //Check if we currently accepting input.
            if(IgnoreInputSpan > 0){
                IgnoreInputSpan -= gameTime.ElapsedGameTime.Milliseconds;
            }else{
                //Process input (movement, attack, etc)
                _ProcessInput();

                if(AttackCounter < 100){
                    ++AttackCounter;
                }

                //AttackSwitch determines if the attack button is still being pressed after an attack.
                //if attack switch is true, then the user holds the attack button after having attacked
                //normally. We start to charge the attack meter if the weapon has been leveled up so far.
                if (AttackSwitch){
                    if(weapComponent != null){
                        if(weapComponent.CurrentWeapon != null){
                            if(AttackCounter >= 100){
                                int maxAttackCounterMultiplier = weapComponent.CurrentWeapon.WeaponLevel;
                                maxAttackCounterMultiplier = maxAttackCounterMultiplier > 3 ? 3 : maxAttackCounterMultiplier;
                                int maxAttackCounter = 100 * (maxAttackCounterMultiplier == 0 ? 1 : maxAttackCounterMultiplier);
                                if (AttackCounter < maxAttackCounter)
                                {
                                    ++AttackCounter;
                                }
                            }
                        }
                    }
                }
            }

            if(IgnoreInputSpan <= 0){
                if (Parent.Orientation.X == 1){
                    if (Parent.Velocity != Vector2.Zero){
                        animComponent.SetCurrentAnimation(WalkAnimations[RIGHT]);
                    }else{
                        animComponent.SetCurrentAnimation(IdleAnimations[RIGHT]);
                    }
                }else if (Parent.Orientation.X == -1){
                    if (Parent.Velocity != Vector2.Zero){
                        animComponent.SetCurrentAnimation(WalkAnimations[LEFT]);
                    }else{
                        animComponent.SetCurrentAnimation(IdleAnimations[LEFT]);
                    }
                }else if (Parent.Orientation.Y == 1){
                    if (Parent.Velocity != Vector2.Zero){
                        animComponent.SetCurrentAnimation(WalkAnimations[DOWN]);
                    }else{
                        animComponent.SetCurrentAnimation(IdleAnimations[DOWN]);
                    }
                }else if (Parent.Orientation.Y == -1){
                    if (Parent.Velocity != Vector2.Zero){
                        animComponent.SetCurrentAnimation(WalkAnimations[UP]);
                    }else{
                        animComponent.SetCurrentAnimation(IdleAnimations[UP]);
                    }
                }
            }
        }

        public override void Draw(ref SpriteBatch batch)
        {
            if (InteractionRect.Width > 0 && InteractionRect.Height > 0)
            {
                SpriteFont font = RpgGame.ContentManager.Load<SpriteFont>("Fonts\\Arial");
                Texture2D test = new Texture2D(GraphicSettings.GraphicDevice, InteractionRect.Width, InteractionRect.Height);
                Color[] data = new Color[test.Width * test.Height];
                for (int i = 0; i < data.Length; ++i)
                {
                    data[i] = new Color(0.5f, 1.0f, 0.5f, 1f);
                }
                test.SetData<Color>(data);
                batch.Draw(test, InteractionRect, Color.White);
                batch.DrawString(font,String.Format("AttackCounter: {0}",AttackCounter),Vector2.Zero,Color.Red);
                test.Dispose();
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

        public void Attack()
        {
            AnimationComponent AnimComponent = Parent.GetComponent<AnimationComponent>();
            WeaponComponent    WeapComponent = Parent.GetComponent<WeaponComponent>();

            //Set the associated weapon animations
            if(WeapComponent != null){
                WeapComponent.StartAttack(AttackCounter / 100);
                AttackCounter = 0;
            }

            //Ignore user input until the animation is finished
            IgnoreInputSpan = AnimComponent.GetCurrentAnimation().AnimationLength;
        }

        private void _ProcessInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up)){
                Parent.Velocity = new Vector2(Parent.Velocity.X, -2);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down)){
                Parent.Velocity = new Vector2(Parent.Velocity.X, 2);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left)){
                Parent.Velocity = new Vector2(-2, Parent.Velocity.Y);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right)){
                Parent.Velocity = new Vector2(2, Parent.Velocity.Y);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space)){
                CollisionComponent component = Parent.GetComponent<CollisionComponent>();
                if (component != null && !InteractionSwitch){
                    InteractionSwitch = true;
                    EventManager.AddEventToQuery(new InteractionEvent(_CreateInteractionRect(), Parent));
                }
            }
            else{
                InteractionSwitch = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.X)){
                if(!AttackSwitch){
                    AttackSwitch = true;
                    Attack();
                }
            }else if (Keyboard.GetState().IsKeyUp(Keys.X)){
                if(AttackSwitch){
                    AttackSwitch = false;
                    if(AttackCounter > 100){
                         Attack();
                    }
                }
            }
        }

    }
}
