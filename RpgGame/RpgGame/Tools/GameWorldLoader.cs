using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using RpgGame.GameComponents;
using RpgGame.World;

namespace RpgGame.Tools
{
    class GameWorldLoader
    {
        public GameWorld World { get; private set; }
        public int Spacing { get; private set; }
        public int Margin { get; private set; }

        private List<Texture2D> Tilesets { get; set; }
        private List<GameObject> Tiles { get; set; }

        public GameWorldLoader()
        {
            World = null;
            Tilesets = new List<Texture2D>();
            Tiles = new List<GameObject>();
        }

        public GameWorldLoader(string filename) : this()
        {
            LoadTMX(filename);
        }

        public void LoadTMX(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("Could not find " + filename);
            }

            FileStream fileStream = new FileStream(filename,FileMode.Open);

            XmlReader reader = XmlReader.Create(fileStream);
            World = new GameWorld();
            while (reader.Read())
            {
                if (reader.Name == "map" && reader.IsStartElement())
                {
                    _ParseMap(reader.ReadSubtree());
                }
                else if (reader.Name == "tileset" && reader.IsStartElement())
                {
                    _ParseTileset(reader.ReadSubtree());
                }
                else if (reader.Name == "layer" && reader.IsStartElement())
                {
                    _ParseLayer(reader.ReadSubtree());
                }
                else if (reader.Name == "objectgroup" && reader.IsStartElement())
                {
                    _ParseObjects(reader.ReadSubtree());
                }
            }
            
        }

        private void _ParseMap(XmlReader reader)
        {
            reader.Read();
            World.Width = Convert.ToInt32(reader.GetAttribute("width"));
            World.Height = Convert.ToInt32(reader.GetAttribute("height"));

            World.TileWidth = Convert.ToInt32(reader.GetAttribute("tilewidth"));
            World.TileHeight = Convert.ToInt32(reader.GetAttribute("tileheight"));

            World.PixelHeight = World.Height * World.TileHeight;
            World.PixelWidth = World.Width * World.TileWidth;            
        }

        private void _ParseTileset(XmlReader reader)
        {
            reader.Read();

            int FirstGID = Convert.ToInt32(reader.GetAttribute("firstgid"));
            Spacing = Convert.ToInt32(reader.GetAttribute("spacing"));
            Margin = Convert.ToInt32(reader.GetAttribute("margin"));

            reader.Read();
            reader.Read();

            String imagename = reader.GetAttribute("source");
            String transparency = reader.GetAttribute("trans");

            //Cut the file extension. XNA doesnt need that.
            imagename = Path.GetFileNameWithoutExtension(imagename);

            imagename = "Tilesets\\" + imagename;

            Texture2D image = RpgGame.ContentManager.Load<Texture2D>(imagename);

            //Check if there's a color in the image that we need to make tranparent
            if (transparency != null && transparency != "")
            {
                int r = Convert.ToInt32(transparency.Substring(0, 2),16);
                int g = Convert.ToInt32(transparency.Substring(2, 2),16);
                int b = Convert.ToInt32(transparency.Substring(4, 2),16);

                Color TransparentColor = new Color(r, g, b);
                Color[] Pixels = new Color[image.Width * image.Height];
                image.GetData<Color>(Pixels);
                for (int i = 0; i < Pixels.Length; ++i)
                {
                    if (Pixels[i] == TransparentColor)
                    {
                        Pixels[i] = Color.Transparent;
                    }
                }
                image.SetData<Color>(Pixels);
            }

            Rectangle cropRect = new Rectangle(Spacing, Spacing, World.TileWidth, World.TileWidth);
            int tileID = FirstGID;

            //Crop the tileset
            for (int y = 0; y < image.Height; y += World.TileHeight + Spacing)
            {
                for (int x = 0; x < image.Width; x += World.TileWidth + Spacing)
                {
                    cropRect.X = x + Spacing;
                    cropRect.Y = y + Spacing;

                    ++tileID;

                    Texture2D tileImage = Texture2DCropper.Crop(image, cropRect);

                    GameObject tile = new GameObject(tileID.ToString());
                    tile.AddComponent(new RenderableComponent(tileImage));
                    Tiles.Add(tile);
                }
            }

            //Read tile properties - if there are any.
            while (reader.Read())
            {
                
                if (reader.Name == "tile" && reader.IsStartElement())
                {
                    //Get the tileID of the tile with properties
                    tileID = Convert.ToInt32(reader.GetAttribute("id")) + FirstGID;
                    
                    if (reader.ReadToDescendant("properties"))
                    {
                        GameObject Tile = null;

                        //Find the associated GameObject.
                        if (Tiles.Count > tileID)
                        {
                            Tile = Tiles.ElementAt<GameObject>(tileID - 1);
                        }

                        //Check if the Tile has been found
                        if (Tile == null)
                        {
                            break;
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                if (reader.Name == "property" && reader.IsStartElement())
                                {
                                    String PropertyName = reader.GetAttribute("name");
                                    String PropertyValue = reader.GetAttribute("value");

                                    if (PropertyName == "invisible")
                                    {
                                        Tile.GetComponent<RenderableComponent>().Active = false;
                                    }
                                }
                            }

                            Tiles.RemoveAt(tileID - 1);
                            Tiles.Insert(tileID - 1, Tile);
                        }
                    }
                }
            }
        }

        private void _ParseLayer(XmlReader reader)
        {
            reader.Read();
            
            String LayerName = reader.GetAttribute("name");
            Layer TileLayer = World.Layers.Find(l => l.Name == LayerName);

            if (TileLayer == null)
            {
                TileLayer = new Layer(World);
                TileLayer.Name = LayerName;
            }
            else
            {
                World.Layers.Remove(TileLayer);
            }
            
            int tileID = 0;
            int x = 0;
            int y = 0;

            while (reader.Read())
            {
                
                if (reader.Name == "tile" && reader.IsStartElement())
                {
                    //Check for collision
                    tileID = Convert.ToInt32(reader.GetAttribute("gid")) - 1;
                    if (tileID >= 0)
                    {
                        GameObject tile = Tiles.ElementAt<GameObject>(tileID);

                        //DeepCopy tile
                        GameObject layerTile = new GameObject(ref tile);

                        layerTile.Width = World.TileWidth;
                        layerTile.Height = World.TileHeight;

                        layerTile.Position = new Vector2(x, y);
                        TileLayer.Tiles.Add(layerTile);

                        if (reader.ReadToDescendant("properties"))
                        {
                            while (reader.Read())
                            {
                                //Layer Properties
                            }
                        }
                    }

                    //update the position
                    x += World.TileWidth;
                    if (x >= World.PixelWidth)
                    {
                        x = 0;
                        y += World.TileHeight;
                    }
                }
            }

            //Add new layer to world.
            World.Layers.Add(TileLayer);
        }

        private void _ParseObjects(XmlReader reader)
        {
            reader.Read();

            //Try to find a layer with the same name.
            String LayerName = reader.GetAttribute("name");
            Layer ObjectLayer = World.Layers.Find(l => l.Name == LayerName);

            if (ObjectLayer == null)
            {
                ObjectLayer = new Layer(World);
                ObjectLayer.Name = LayerName;
            }
            else
            {
                World.Layers.Remove(ObjectLayer);
            }
            
            string objType = "";
            string objName = "";
            int objID = 0;
            int objX = 0;
            int objY = 0;
            int objWidth = 0;
            int objHeight = 0;

            while (reader.Read())
            {
                if (reader.Name == "object" && reader.IsStartElement())
                {
                    objName = reader.GetAttribute("name");
                    GameObject newObject = null;
                    objX = Convert.ToInt32(reader.GetAttribute("x"));
                    objY = Convert.ToInt32(reader.GetAttribute("y"));
                    objWidth = Convert.ToInt32(reader.GetAttribute("width"));
                    objHeight = Convert.ToInt32(reader.GetAttribute("height"));

                    if (reader.GetAttribute("gid") == null)
                    {
                        objType = reader.GetAttribute("type");
                        newObject = new GameObject(objName);
                    }
                    else
                    {
                        //Kopiere Tile
                        objID = Convert.ToInt32(reader.GetAttribute("gid"));
                        GameObject tile = Tiles.ElementAt<GameObject>(objID - 1);
                        newObject = new GameObject(ref tile);
                        newObject.Name = objName;

                        //Due to a STUPID bug in our leveleditor TILED we have to 
                        //subtract 1 tileheight from the y position.
                        objY -= World.TileHeight;
                    }

                    newObject.Position = new Vector2(objX, objY);
                    newObject.Width = objWidth;
                    newObject.Height = objHeight;

                    if (reader.ReadToDescendant("properties"))
                    {
                        while (reader.Read())
                        {
                            if (reader.Name == "property" && reader.IsStartElement())
                            {
                                String PropertyName = reader.GetAttribute("name");
                                String PropertyValue = reader.GetAttribute("value");
                                if (PropertyName == "script")
                                {
                                    newObject.AddComponent(new ScriptComponent(PropertyValue));
                                }

                                if (PropertyName == "invisible")
                                {
                                    newObject.GetComponent<RenderableComponent>().Active = false;
                                }

                                if (PropertyName == "npc")
                                {
                                    String folder = "Characters\\" + PropertyValue + "\\";
                                    newObject.AddComponent(new AnimationComponent(new List<AbstractAnimation>()
                                    {
                                        new SpritesheetAnimation("WalkDown",folder+"walk_down",16,36,200,4,AbstractAnimation.RepeatBehaviour.LoopAnimation),
                                        new SpritesheetAnimation("WalkUp",folder+"walk_up",16,35,200,4,AbstractAnimation.RepeatBehaviour.LoopAnimation),
                                        new SpritesheetAnimation("WalkLeft",folder+"walk_left",16,35,200,4,AbstractAnimation.RepeatBehaviour.LoopAnimation),
                                        new SpritesheetAnimation("WalkRight",folder+"walk_right",16,35,200,4,AbstractAnimation.RepeatBehaviour.LoopAnimation),
                                        new CustomAnimation("LookDown",new string[]{folder+"look_down"},new double[]{0},AbstractAnimation.RepeatBehaviour.SingleAnimation),
                                        new CustomAnimation("LookUp",new string[]{folder+"look_up"},new double[]{0},AbstractAnimation.RepeatBehaviour.SingleAnimation),
                                        new CustomAnimation("LookLeft",new string[]{folder+"look_left"},new double[]{0},AbstractAnimation.RepeatBehaviour.SingleAnimation),
                                        new CustomAnimation("LookRight",new string[]{folder+"look_right"},new double[]{0},AbstractAnimation.RepeatBehaviour.SingleAnimation)
                                    }));
                                }
                            }
                        }
                    }
                    ObjectLayer.Objects.Add(newObject);
                }
            }
            
            //Add new object layer to world
            World.Layers.Add(ObjectLayer);
        }
    }
}
