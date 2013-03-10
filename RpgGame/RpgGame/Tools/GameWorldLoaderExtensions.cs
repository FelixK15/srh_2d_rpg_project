using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgGame.Manager;
using RpgGame.GameComponents;

namespace RpgGame.Tools
{
    public class GameWorldLoaderExtensions
    {
        public static void TileInvisibleProperty(String Value,ref Texture2D Tile)
        {
            Color[] TilePixels = new Color[Tile.Width * Tile.Height];
            for(int i = 0;i < TilePixels.Length;++i){
                TilePixels[i] = Color.Transparent;
            }
            Tile.SetData<Color>(TilePixels);
        }

        public static void ObjectScriptProperty(String Value,ref GameObject Obj)
        {
            Obj.AddComponent(new ScriptComponent(Value));
        }

        public static void ObjectInteractionProperty(String Value,ref GameObject Obj)
        {
            Obj.AddComponent(new InteractionComponent());
        }
    }
}
