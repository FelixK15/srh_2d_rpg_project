using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;

using RpgGame.Tools;

namespace RpgGame.GameComponents
{
    class ExternalAnimation : AbstractAnimation
    {
        private String Path { get; set; }

        public ExternalAnimation(String path)
            : this(path, "")
        {

        }

        public ExternalAnimation(String path,String name)
            : base(name,RepeatBehaviour.SingleAnimation)
        {
            Path = path;
            _ProcessAnimation();
        }

        private void _ProcessAnimation()
        {
         //Load the file and create a xmlreader
            FileStream Stream = new FileStream(Path,FileMode.Open,FileAccess.Read);
            XmlReader Reader = XmlReader.Create(Stream);

            if (Name == "")
            {
                Name = System.IO.Path.GetFileNameWithoutExtension(Path);
            }
            
            List<Texture2D> Sources = new List<Texture2D>();

            //Read the xml file to the end
            while (Reader.Read())
            {
                //Set the repeat behavior if the animation node has been reached
                if (Reader.Name == "Animation" && Reader.IsStartElement())
                {
                    Behaviour = Convert.ToBoolean(Reader.GetAttribute("Loop")) ? 
                        RepeatBehaviour.LoopAnimation : RepeatBehaviour.SingleAnimation;
                }
                else if (Reader.Name == "SpriteSources" && Reader.IsStartElement())
                {
                    //Load and crop the sprite sources if the SpriteSources node has been reached
                    _ProcessSources(Reader.ReadSubtree(),ref Sources);
                }
                else if (Reader.Name == "Frames" && Reader.IsStartElement())
                {
                    //Load and set the sprites from the previously saved sources.
                    _ProcessFrames(Reader.ReadSubtree(),Sources);
                }
            }

            Reader.Close();
            Stream.Close();
        }

        private void _ProcessSources(XmlReader Reader,ref List<Texture2D> Sources)
        {
            String Path = "";

            while (Reader.Read())
            {
                if (Reader.Name == "SingleSpriteSource" && Reader.IsStartElement())
                {
                    Path = Reader.GetAttribute("Path");
                    Sources.Add(RpgGame.ContentManager.Load<Texture2D>(Path.Substring(0, Path.Length - 4)));
                }
                else if (Reader.Name == "SpriteSheetSource" && Reader.IsStartElement())
                {
                    Path = Reader.GetAttribute("Path");
                    Texture2D SpriteSheet = RpgGame.ContentManager.Load<Texture2D>(Path.Substring(0, Path.Length - 4));
                    int SpriteWidth = Convert.ToInt32(Reader.GetAttribute("SingleSpriteWidth"));
                    int SpriteHeight = Convert.ToInt32(Reader.GetAttribute("SingleSpriteHeight"));
                    int SpriteAmount = Convert.ToInt32(Reader.GetAttribute("SpriteAmount"));

                    int X = 0;
                    int Y = 0;
                    for (int i = 0; i < SpriteAmount; ++i)
                    {
                        
                        Sources.Add(Texture2DCropper.Crop(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(X, Y, SpriteWidth, SpriteHeight)));

                        if ((X + SpriteWidth) >= SpriteSheet.Width)
                        {
                            X = 0;
                            Y += SpriteHeight;
                        }
                        else
                        {
                            X += SpriteWidth;
                        }
                    }     
                }
            }
        }

        private void _ProcessFrames(XmlReader Reader,List<Texture2D> Sources)
        {
            while (Reader.Read())
            {
                if (Reader.Name == "Frame" && Reader.IsStartElement())
                {
                    AnimationFrame Frame = new AnimationFrame();
                    Frame.duration = Convert.ToInt32(Reader.GetAttribute("Duration"));

                    Reader.Read();
                    Reader.Read();
                    if (Reader.Name == "Sprites" && Reader.IsStartElement())
                    {
                        XmlReader SpritesTree = Reader.ReadSubtree();
                        while (SpritesTree.Read())
                        {
                            if (SpritesTree.Name == "Sprite" && Reader.IsStartElement())
                            {
                                int SpriteIndex = Convert.ToInt32(Reader.GetAttribute("ID")) - 1;
                                int X = Convert.ToInt32(Reader.GetAttribute("X"));
                                int Y = Convert.ToInt32(Reader.GetAttribute("Y"));
                                Frame.texture = Sources.ElementAt<Texture2D>(SpriteIndex);
                                Frame.x = X + (Frame.texture.Width / 2);
                                Frame.y = Y + (Frame.texture.Height / 2);
                            }
                        }
                    }

                    FrameList.Add(Frame);
                }
            }
        }
    }
}
