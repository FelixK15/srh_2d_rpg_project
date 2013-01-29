using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.GameStates
{
    interface IGameState
    {
        void Start();
        void Update(GameTime gameTime);
        void Draw(ref SpriteBatch batch);
        void Stop();
    }
}
