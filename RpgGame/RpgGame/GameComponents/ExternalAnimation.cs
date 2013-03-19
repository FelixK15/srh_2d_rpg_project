using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;

using RpgGame.Tools;
using Microsoft.Xna.Framework;
using Animation_Importer.XML_Objects;

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
            XMLAnimation animationDescription = null;

            try{
                animationDescription = RpgGame.ContentManager.Load<XMLAnimation>(Path);
            }catch(Exception ex){
                DeveloperConsole.AddMessage(DeveloperConsole.MessageType.ERROR,String.Format("Could not load animation file '{0}' ({1})",Path,ex.Message));
                return;
            }
            
            List<Texture2D> Sources = new List<Texture2D>();

            Behaviour = Convert.ToBoolean(animationDescription.Loop) ? RepeatBehaviour.LoopAnimation : RepeatBehaviour.SingleAnimation;

            foreach(XMLSpriteSource Source in animationDescription.SpriteSources){
                _ProcessSources(Source,ref Sources);
            }

            foreach(XMLFrame Frame in animationDescription.Frames){
                _ProcessFrames(Frame,Sources);
            }
        }

        private void _ProcessSources(XMLSpriteSource sourceDesc,ref List<Texture2D> Sources)
        {
            const int ValidLength = 4;

            String      Path         = sourceDesc.Path;
            Texture2D   SourceImages = null;

            //we cut the file extension off of the imagepath
            if(Path.Length > ValidLength){
                Path = Path.Substring(0, Path.Length - 4);

                //and convert it to our folder structure
                string ImagePath = _FindImagePath(Path);
                if(System.IO.Path.GetExtension(ImagePath) == ".xnb"){
                    ImagePath = ImagePath.Replace(Environment.CurrentDirectory,"");
                    ImagePath = ImagePath.Replace("\\Content\\","");
                    ImagePath = ImagePath.Replace(".xnb","");

                    Path = ImagePath;
                }
            }else{
                DeveloperConsole.AddMessage(DeveloperConsole.MessageType.ERROR,String.Format("Could not process sprite source '{0}'\n because it is not a valid file name.",Path));
                return;
            }

            //Try to load the texture from the path in the xml
            try{
                SourceImages = RpgGame.ContentManager.Load<Texture2D>(Path);
            }catch(Exception ex){
                DeveloperConsole.AddMessage(DeveloperConsole.MessageType.ERROR,String.Format("Could not load image file '{0}'\n ({1})",Path,ex.Message));
                return;
            }
            

            if(sourceDesc is XMLSpriteSheetSource){
                int X,Y;
                X = Y = 0;

                XMLSpriteSheetSource sheetSourceDesc = (XMLSpriteSheetSource)sourceDesc;
                for(int i = 0;i < sheetSourceDesc.SpriteAmount;++i){
                    Texture2D croppedImages = Texture2DCropper.Crop(SourceImages,new Rectangle(X,Y,sheetSourceDesc.SingleSpriteWith,sheetSourceDesc.SingleSpriteHeight));
                    if(X + sheetSourceDesc.SingleSpriteWith >= SourceImages.Width){
                        X = 0;
                        Y += sheetSourceDesc.SingleSpriteHeight;
                    }else{
                        X += sheetSourceDesc.SingleSpriteWith;
                    }
                    
                    Sources.Add(croppedImages);
                }
            }else{
                Sources.Add(SourceImages);
            }
        }

        private void _ProcessFrames(XMLFrame frameDesc,List<Texture2D> sources)
        {
            AnimationFrame Frame = new AnimationFrame();
            Frame.Duration = frameDesc.Duration;

            foreach (XMLSprite Sprite in frameDesc.Sprites){
                if (Sprite.ID - 1 < sources.Count){
                    Frame.Sprite = sources.ElementAt<Texture2D>(Sprite.ID - 1);

                    if (Frame.Sprite != null){
                        int offset_X = (int)(Sprite.X - Frame.Sprite.Width * 0.5f);
                        int offset_y = (int)(Sprite.Y - Frame.Sprite.Height * 0.5f);

                        Frame.Offset = new Vector2(offset_X, offset_y);
                    }else{
                        DeveloperConsole.AddMessage(DeveloperConsole.MessageType.ERROR, String.Format("Sprite at ID '{0}' is null.", Sprite.ID - 1));
                    }
                }
            }

            FrameList.Add(Frame);
            
        }

        private String _FindImagePath(String ImageFilename)
        {
            String RealPath = System.IO.Path.GetFullPath(ImageFilename);
            RealPath = RealPath.Substring(0,RealPath.LastIndexOf('\\'));
            return _FindImagePathREC(RealPath,ImageFilename,0);
        }

        private String _FindImagePathREC(String RealPath,String ImageFilename,int RevCounter)
        {
            if(RevCounter == 5){
                return RealPath;
            }

            string[] AllFiles = Directory.GetFiles(RealPath);

            foreach(string FilePath in AllFiles){
                if(System.IO.Path.GetFileNameWithoutExtension(FilePath) == ImageFilename){
                    return FilePath;
                }
            }

            string[] AllDirectories = Directory.GetDirectories(RealPath);
            foreach(string DirectoryPath in AllDirectories){
                string PossiblePath = _FindImagePathREC(DirectoryPath,ImageFilename,RevCounter + 1);

                if(System.IO.Path.GetFileNameWithoutExtension(PossiblePath) == ImageFilename){
                    return PossiblePath;
                }
            }

            return _FindImagePathREC(RealPath + "..\\",ImageFilename,RevCounter + 1);
        }
    }
}
