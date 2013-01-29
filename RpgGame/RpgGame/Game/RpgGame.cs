using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Threading;

using RpgGame.Manager;
using RpgGame.World;
using RpgGame.Events;
using RpgGame.GameStates;
using RpgGame.Tools;
using RpgGame.GameComponents;
using RpgGame.Menu;

namespace RpgGame
{
    class RpgGame : Microsoft.Xna.Framework.Game
    {
        private SpriteBatch Sprites;

        private List<IGameState> GameStates { get; set; }
        private GraphicsDeviceManager DeviceManager { get; set; }

        private InputManager InputManager { get; set; }

        public static ContentManager ContentManager { get; private set; }

        private GameObject test { get; set; }
        private GameWorld TestWorld { get; set; }

        public IGameState CurrentGameState
        {
            get
            {
                //The current game state will always be returned.
                if (GameStates.Count > 0)
                {
                    return GameStates.Last();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                //the currently active game states gets paused and the newly added
                //game states gets started.
                if (CurrentGameState != null)
                {
                    CurrentGameState.Stop();
                }

                GameStates.Add(value);
                value.Start();
            }
        }

        public RpgGame()
        {
            GameStates = new List<IGameState>();
            DeviceManager = new GraphicsDeviceManager(this);
            InputManager = new InputManager();

            ProcessManager.Initialize();
            ScriptManager.Initialize();

            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            ContentManager = Content;
            Sprites = new SpriteBatch(GraphicsDevice);
            base.LoadContent();

            GameWorldLoader Loader = new GameWorldLoader("TestCity.tmx");
            TestWorld = Loader.World;

            test = new GameObject("Player");
            test.Type = "Player";
            test.Position = new Vector2(50, 200);

            // test.AddComponent(new ScriptComponent("test.lua"));
            test.AddComponent(new MoveableComponent(0.15f));
            test.AddComponent(new PlayerComponent(PlayerIndex.One));
            test.AddComponent(new CollisionComponent(CollisionComponent.CollisionType.DYNAMIC, new Vector2(0, 20), 16, 16,null));
            test.AddComponent(new AnimationComponent());
            test.AddComponent(new AudioComponent());
            //test.GetComponent<AudioComponent>().addAudio(new AudioObject("walk", "footsteps-6", AudioObject.RepeatBehaviour.SinglePlay));
            test.GetComponent<AudioComponent>().setCurrentAudio("walk");
            test.GetComponent<AnimationComponent>().addAnimation(new SpritesheetAnimation("WalkDown", "Characters//Hero//walk_down",
                                                                 18, 36, 200, 5, AbstractAnimation.RepeatBehaviour.LoopAnimation));
            test.GetComponent<AnimationComponent>().addAnimation(new SpritesheetAnimation("WalkLeft", "Characters//Hero//walk_left",
                                                     30, 32, 200, 6, AbstractAnimation.RepeatBehaviour.LoopAnimation));
            test.GetComponent<AnimationComponent>().addAnimation(new SpritesheetAnimation("WalkRight", "Characters//Hero//walk_right",
                                                     30, 32, 200, 6, AbstractAnimation.RepeatBehaviour.LoopAnimation));
            test.GetComponent<AnimationComponent>().addAnimation(new SpritesheetAnimation("WalkUp", "Characters//Hero//walk_up",
                                                     17, 32, 200, 5, AbstractAnimation.RepeatBehaviour.LoopAnimation));

            test.GetComponent<AnimationComponent>().addAnimation(new CustomAnimation("LookDown", new string[] { "Characters//Hero//stand_down" }, new double[] { 0 },
                                                                    AbstractAnimation.RepeatBehaviour.SingleAnimation));
            test.GetComponent<AnimationComponent>().addAnimation(new CustomAnimation("LookUp", new string[] { "Characters//Hero//stand_up" }, new double[] { 0 },
                                                        AbstractAnimation.RepeatBehaviour.SingleAnimation));
            test.GetComponent<AnimationComponent>().addAnimation(new CustomAnimation("LookLeft", new string[] { "Characters//Hero//stand_left" }, new double[] { 0 },
                                                        AbstractAnimation.RepeatBehaviour.SingleAnimation));
            test.GetComponent<AnimationComponent>().addAnimation(new CustomAnimation("LookRight", new string[] { "Characters//Hero//stand_right" }, new double[] { 0 },
                                                        AbstractAnimation.RepeatBehaviour.SingleAnimation));
            test.GetComponent<AnimationComponent>().setCurrentAnimation("LookDown");
            
            test.AddComponent(new EventComponent(Event.Types.KEYBOARD_PRESSED, delegate(GameObject Object, Event GameEvent)
                {
                    KeyPressedEvent e = (KeyPressedEvent)GameEvent;
                    if (e.PressedKey == Keys.Down)
                    {
                        Object.Velocity = new Vector2(Object.Velocity.X, 1f);
                        Object.GetComponent<AnimationComponent>().setCurrentAnimation("WalkDown");
                        Object.Orientation = new Vector2(0, 1);
                    }

                    if (e.PressedKey == Keys.Up)
                    {
                        Object.Velocity = new Vector2(Object.Velocity.X, -1f);
                        Object.GetComponent<AnimationComponent>().setCurrentAnimation("WalkUp");
                        Object.Orientation = new Vector2(0, -1);
                    }

                    if (e.PressedKey == Keys.Left)
                    {
                        Object.Velocity = new Vector2(-1f, Object.Velocity.Y);
                        Object.GetComponent<AnimationComponent>().setCurrentAnimation("WalkLeft");
                        Object.Orientation = new Vector2(-1, 0);
                    }

                    if (e.PressedKey == Keys.Right)
                    {
                        Object.Velocity = new Vector2(1f, Object.Velocity.Y);
                        Object.GetComponent<AnimationComponent>().setCurrentAnimation("WalkRight");
                        Object.Orientation = new Vector2(1, 0);
                    }
                }));

            test.AddComponent(new EventComponent(Event.Types.KEYBOARD_RELEASED, delegate(GameObject Object, Event GameEvent)
            {
                KeyReleasedEvent e = (KeyReleasedEvent)GameEvent;
                if (e.ReleasedKey == Keys.Down || e.ReleasedKey == Keys.Up)
                {
                    Object.Velocity = new Vector2(Object.Velocity.X, 0);
                }

                if (e.ReleasedKey == Keys.Left || e.ReleasedKey == Keys.Right)
                {
                    Object.Velocity = new Vector2(0, Object.Velocity.Y);
                }

                if(Object.Velocity == Vector2.Zero)
                {
                    if (Object.Orientation.X == -1)
                    {
                        Object.GetComponent<AnimationComponent>().setCurrentAnimation("LookLeft");
                    }
                    else if (Object.Orientation.X == 1)
                    {
                        Object.GetComponent<AnimationComponent>().setCurrentAnimation("LookRight");
                    }
                    else if (Object.Orientation.Y == -1)
                    {
                        Object.GetComponent<AnimationComponent>().setCurrentAnimation("LookUp");
                    }
                    else if (Object.Orientation.Y == 1)
                    {
                        Object.GetComponent<AnimationComponent>().setCurrentAnimation("LookDown");
                    }
                }

            }));

            Layer MainLayer = TestWorld.Layers.Find(l => l.Name == "main");
            MainLayer.Objects.Add(test);

            //GraphicsDevice of the GraphicManager needs to be set before calling the constructor.
            GraphicSettings.GraphicDevice = GraphicsDevice;
            GraphicSettings.ClientWidth = Window.ClientBounds.Width;
            GraphicSettings.ClientHeight = Window.ClientBounds.Height;
   
            //Create the camera.
            GraphicSettings.Camera = new Camera(Window.ClientBounds.Height, Window.ClientBounds.Width);
            GraphicSettings.Camera.Zoom = 2.0f;

            GraphicSettings.Camera.Target = test;
            GraphicSettings.Camera.TargetArea = new Rectangle(60, 80, GraphicSettings.Camera.Width - 120, GraphicSettings.Camera.Height - 160);

            CurrentGameState = new GameWorldState(TestWorld);
        }

        protected override void Update(GameTime gameTime)
        {
            //managers get updated. After that the current game state
            //gets updated
            UpdateManager(gameTime);
            if (CurrentGameState != null)
            {
                CurrentGameState.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public void RemoveCurrentGameState()
        {
            GameStates.RemoveAt(GameStates.Count);
        }

        public void RemoveAllGameStates()
        {
            GameStates.Clear();
        }

        private void UpdateManager(GameTime gameTime)
        {
            //all managers are getting updated.
            InputManager.ProcessInput();
            EventManager.Update();
            ProcessManager.Update(gameTime);
            GraphicSettings.Camera.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //all gamestates are getting rendered


            Sprites.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp,
                          DepthStencilState.Default, RasterizerState.CullNone, null, GraphicSettings.Camera.ViewMatrix);
            
           // TestWorld.Draw();
            foreach (IGameState gameState in GameStates)
            {
                gameState.Draw(ref Sprites);
            }

            GraphicSettings.Camera.Draw(ref Sprites);

            Sprites.End();
            //draw everything to the screen.
            //GraphicManager.Draw();

            base.Draw(gameTime);
        }
    }
}
