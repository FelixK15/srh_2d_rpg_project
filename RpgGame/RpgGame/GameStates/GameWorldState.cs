using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RpgGame.World;
using RpgGame.Manager;
using RpgGame.Events;
using Microsoft.Xna.Framework.Graphics;

using RpgGame.Tools;

namespace RpgGame.GameStates
{
    class GameWorldState : IGameState, IEventListener
    {
        public GameWorld World { get; set; }

        private Rectangle FreeMoveArea { get; set; }

        public GameWorldState(String gameWorldFile)
        {
            GameWorldLoader Loader = new GameWorldLoader(gameWorldFile);
            World = Loader.World;
        }

        public GameWorldState(GameWorld world)
        {
            World = world;
        }

        public void Start()
        {
            EventManager.AddListener(Event.Types.PLAYER_MOVED, this);
        }

        public void Update(GameTime gameTime)
        {
            FreeMoveArea = new Rectangle((int)(GraphicSettings.Camera.Position.X) + 80,
                                         (int)(GraphicSettings.Camera.Position.Y) + 60,
                                         (int)(GraphicSettings.Camera.Width) - 160,
                                         (int)(GraphicSettings.Camera.Height) - 120);
            World.Update(gameTime);
        }

        public void Draw(ref SpriteBatch batch)
        {
            World.Draw(ref batch);
        }

        public void Stop()
        {
            EventManager.RemoveListener(Event.Types.PLAYER_MOVED, this);
        }

        public void HandleEvent(Event eGameEvent)
        {
            if (eGameEvent is PlayerMoveEvent)
            {
                Vector2 PlayerPosition = (eGameEvent as PlayerMoveEvent).Position;

                int X = (int)GraphicSettings.Camera.Position.X;
                int Y = (int)GraphicSettings.Camera.Position.Y;

                if (PlayerPosition.X <= FreeMoveArea.X)
                {
                    X += (int)PlayerPosition.X - FreeMoveArea.X;
                }
                else if (PlayerPosition.X >= FreeMoveArea.X + FreeMoveArea.Width)
                {
                    X -= (FreeMoveArea.X + FreeMoveArea.Width) - (int)PlayerPosition.X;
                }

                if (PlayerPosition.Y <= FreeMoveArea.Y)
                {
                    Y += (int)PlayerPosition.Y - FreeMoveArea.Y;
                }
                else if (PlayerPosition.Y >= FreeMoveArea.Y + FreeMoveArea.Height)
                {
                    Y -= (FreeMoveArea.Y + FreeMoveArea.Height) - (int)PlayerPosition.Y;
                }

                if (X < 0)
                {
                    X = 0;
                }
                else if (X + GraphicSettings.Camera.Width > World.PixelWidth)
                {
                    X = World.PixelWidth - GraphicSettings.Camera.Width;
                }

                if (Y < 0)
                {
                    Y = 0;
                }
                else if (Y + GraphicSettings.Camera.Height > World.PixelWidth)
                {
                    Y = World.PixelHeight - GraphicSettings.Camera.Height;
                }

                Vector2 NewCameraPos = new Vector2(X, Y);
                GraphicSettings.Camera.Position = NewCameraPos;
            }
        }
    }
}
