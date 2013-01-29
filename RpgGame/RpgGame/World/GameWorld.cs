using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.World
{
    class GameWorld
    {
        public List<Layer> Layers { get; private set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int PixelWidth { get; set; }
        public int PixelHeight { get; set; }

        public GameWorld()
        {
            Layers = new List<Layer>();
        }

        public void Update(GameTime time)
        {
            foreach (Layer l in Layers)
            {
                l.Update(time);
            }
        }

        public void Draw(ref SpriteBatch batch)
        {
            foreach (Layer l in Layers)
            {
                l.Draw(ref batch);
            }
        }
    }
}
