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
using GameWorld_Importer.XML_Objects;
using RpgGame.Dialogs;

namespace RpgGame
{
    class RpgGame : Microsoft.Xna.Framework.Game
    {
        private         SpriteBatch             Sprites;
        private         GraphicsDeviceManager   DeviceManager       { get; set; }

        private         InputManager            InputManager        { get; set; }

        public static   ContentManager          ContentManager      { get; private set; }
        public static   GameWorld               CurrentGameWorld    { get; set; }
      
        public RpgGame()
        {
            DeviceManager = new GraphicsDeviceManager(this);
            InputManager = new InputManager();

            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            //managers get updated. After that the current game state
            //gets updated
            UpdateManager(gameTime);
            
            //Only update the current state if the developer console is closed.
            #if (DEBUG)
            if (!DeveloperConsole.IsOpen){
                if (GameStateMachine.CurrentState != null){
                    GameStateMachine.CurrentState.Update(gameTime);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.F1)){
                    DeveloperConsole.IsOpen = true;
                }
            }
            #else
            if (GameStateMachine.CurrentState != null){
                GameStateMachine.CurrentState.Update(gameTime);
            }
            #endif

            base.Update(gameTime);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
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
            Sprites.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp,
                          DepthStencilState.Default, RasterizerState.CullNone, null, GraphicSettings.Camera.ViewMatrix);
        
            foreach (IGameState gameState in GameStateMachine.GameStates)
            {
                gameState.Draw(ref Sprites);
            }

            GraphicSettings.Camera.Draw(ref Sprites);

            Sprites.End();
            
            #if (DEBUG)
                Sprites.Begin();
                if (DeveloperConsole.IsOpen){
                    DeveloperConsole.Draw(ref Sprites);
                }
                Sprites.End();
            #endif
            
            base.Draw(gameTime);
        }

        protected override void Initialize()
        {
            //Intialize the manager
            ProcessManager.Initialize();
            ScriptManager.Initialize();
            BufferedInput.Initialize(Window);
            GameStateMachine.Initialize();

            //GraphicsDevice of the GraphicManager needs to be set before calling the constructor.
            GraphicSettings.GraphicDevice   = GraphicsDevice;
            GraphicSettings.ClientWidth     = Window.ClientBounds.Width;
            GraphicSettings.ClientHeight    = Window.ClientBounds.Height;

            //Set static content variable and create SpriteBatch
            ContentManager = Content;
            Sprites = new SpriteBatch(GraphicsDevice);

            #if (DEBUG)
                DeveloperConsole.Initialize();
            #endif

            //Create the camera.
            GraphicSettings.Camera              = new Camera(Window.ClientBounds.Height, Window.ClientBounds.Width);
            GraphicSettings.Camera.Zoom         = 2.0f;
            GraphicSettings.Camera.TargetArea   = new Rectangle(60, 80, GraphicSettings.Camera.Width - 120, GraphicSettings.Camera.Height - 160);

            _InitializeWorldLoader();
        }

        protected override void BeginRun()
        {
            GameWorldLoader Loader = new GameWorldLoader("Maps\\TestCity");
            

            CurrentGameWorld = Loader.World;

            DialogBox.Initialize(); 

            GameObject test = new GameObject("Player");
            test.Type = "Player";
            test.Position = new Vector2(50, 200);

            GraphicSettings.Camera.Target = test;

            Weapon weapon = new Weapon("Scripts\\Weapons\\TestSword.cs");
            weapon.Name = "Sword";
            weapon.WeaponLevel = 3;
            weapon.WalkAnimations[PlayerComponent.LEFT]     = "WalkLeft_Sword";
            weapon.WalkAnimations[PlayerComponent.RIGHT]    = "WalkRight_Sword";
            weapon.WalkAnimations[PlayerComponent.UP]       = "WalkUp_Sword";
            weapon.WalkAnimations[PlayerComponent.DOWN]     = "WalkDown_Sword";
            weapon.IdleAnimations[PlayerComponent.LEFT]     = "LookLeft";
            weapon.IdleAnimations[PlayerComponent.RIGHT]    = "LookRight";
            weapon.IdleAnimations[PlayerComponent.UP]       = "LookUp";
            weapon.IdleAnimations[PlayerComponent.DOWN]     = "LookDown";
            weapon.AttackDamagePerLevel[0] = 20;
            weapon.AttackDamagePerLevel[1] = 350;
            weapon.AttackDamagePerLevel[2] = 100;
            weapon.AttackDamagePerLevel[3] = 200;
            weapon.AttackDuration[0] = 20;
            weapon.AttackDuration[1] = 20;
            weapon.AttackDuration[2] = 20;
            weapon.AttackDuration[3] = 20;

            test.AddComponent(new PlayerComponent(PlayerIndex.One));
            test.AddComponent(new CollisionComponent(CollisionComponent.CollisionType.DYNAMIC, new Vector2(0, 16), 22, 16, null));
            test.AddComponent(new AnimationComponent());
            test.AddComponent(new WeaponComponent(weapon));

            test.GetComponent<AnimationComponent>().AddAnimation(new SpritesheetAnimation("WalkDown_Sword", "Characters//Hero//walk_down",
                                                                 18, 36, 200, 5, AbstractAnimation.RepeatBehaviour.LoopAnimation));
            test.GetComponent<AnimationComponent>().AddAnimation(new SpritesheetAnimation("WalkLeft_Sword", "Characters//Hero//walk_left",
                                                     30, 32, 200, 6, AbstractAnimation.RepeatBehaviour.LoopAnimation));
            test.GetComponent<AnimationComponent>().AddAnimation(new SpritesheetAnimation("WalkRight_Sword", "Characters//Hero//walk_right",
                                                     30, 32, 200, 6, AbstractAnimation.RepeatBehaviour.LoopAnimation));
            test.GetComponent<AnimationComponent>().AddAnimation(new SpritesheetAnimation("WalkUp_Sword", "Characters//Hero//walk_up",
                                                     17, 32, 200, 5, AbstractAnimation.RepeatBehaviour.LoopAnimation));

            test.GetComponent<AnimationComponent>().AddAnimation(new ExternalAnimation("Animations//Hero//Attack_Down","AttackDown_Sword"));

            test.GetComponent<AnimationComponent>().AddAnimation(new CustomAnimation("LookDown", new string[] { "Characters//Hero//stand_down" }, new int[] { 0 },
                                                                    AbstractAnimation.RepeatBehaviour.SingleAnimation));
            test.GetComponent<AnimationComponent>().AddAnimation(new CustomAnimation("LookUp", new string[] { "Characters//Hero//stand_up" }, new int[] { 0 },
                                                        AbstractAnimation.RepeatBehaviour.SingleAnimation));
            test.GetComponent<AnimationComponent>().AddAnimation(new CustomAnimation("LookLeft", new string[] { "Characters//Hero//stand_left" }, new int[] { 0 },
                                                        AbstractAnimation.RepeatBehaviour.SingleAnimation));
            test.GetComponent<AnimationComponent>().AddAnimation(new CustomAnimation("LookRight", new string[] { "Characters//Hero//stand_right" }, new int[] { 0 },
                                                        AbstractAnimation.RepeatBehaviour.SingleAnimation));
            test.GetComponent<AnimationComponent>().SetCurrentAnimation("LookDown");

            Layer MainLayer = CurrentGameWorld.Layers.Find(l => l.Name == "main");
            MainLayer.Objects.Add(test);

            GameObject enemy = new GameObject("Enemy");
            enemy.AddComponent(new EnemyComponent(100,100,10,10));
            enemy.AddComponent(new RenderableComponent("Characters\\NPC1\\look_down"));
            enemy.AddComponent(new CollisionComponent(CollisionComponent.CollisionType.STATIC,new Vector2(0,10),10,10,null));

            enemy.Position = new Vector2(300,300);

            MainLayer.Objects.Add(enemy);

            GameStateMachine.AddState(new GameWorldState(CurrentGameWorld));
            //GameStateMachine.AddState(new DialogState(DLoader.Dialogs.ElementAt(0)));
        }

        private void _InitializeWorldLoader()
        {
            GameWorldLoader.Initialize();

            GameWorldLoader.ObjectPropertyFunctions.Add("interaction",GameWorldLoaderExtensions.ObjectInteractionProperty);
            GameWorldLoader.ObjectPropertyFunctions.Add("script",GameWorldLoaderExtensions.ObjectScriptProperty);
            GameWorldLoader.TilePropertyFunctions.Add("invisible",GameWorldLoaderExtensions.TileInvisibleProperty);
        }

    }
}
