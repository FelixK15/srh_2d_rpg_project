using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RpgGame.Manager;

namespace RpgGame.Tools
{
    class Camera
    {
        private Rectangle _targetArea;
        private int _height;
        private int _width;

        public int Height 
        {
            get
            {
                return (int)(_height / Zoom);
            } 
        }
        public int Width 
        {
            get
            {
                return (int)(_width / Zoom);
            }
        }

        public GameObject Target { get; set; }
        public Rectangle TargetArea { get; set; }

        public Vector2 Position { get; set; }
        public float Zoom { get; set; }
        public Matrix ViewMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Position, 0)) * Matrix.CreateScale(Zoom, Zoom, 0);
            }
        }
      
        public Camera()
            : this(0, 0)
        {
            
        }

        public Camera(int Height,int Width)
        {
            _width = Width;
            _height = Height;
            Zoom = 1.0f;
        }

        public void Update(GameTime gameTime)
        {
            if (Target != null)
            {
                _targetArea = new Rectangle(TargetArea.X + (int)Position.X,
                                            TargetArea.Y + (int)Position.Y,
                                            TargetArea.Width,TargetArea.Height);


                Vector2 TargetPosition = Target.Position;

                int X = (int)Position.X;
                int Y = (int)Position.Y;

                if (TargetPosition.X <= _targetArea.X)
                {
                    X += (int)TargetPosition.X - _targetArea.X;
                }
                else if (TargetPosition.X + Target.Width >= _targetArea.X + _targetArea.Width)
                {
                    X -= (_targetArea.X + _targetArea.Width) - (int)(TargetPosition.X + Target.Width);
                }

                if (TargetPosition.Y <= _targetArea.Y)
                {
                    Y += (int)TargetPosition.Y - _targetArea.Y;
                }
                else if (TargetPosition.Y + Target.Height >= _targetArea.Y + _targetArea.Height)
                {
                    Y -= (_targetArea.Y + _targetArea.Height) - (int)(TargetPosition.Y + Target.Height);
                }

                if (X < 0){
                    X = 0;
                }else{
                    if(RpgGame.CurrentGameWorld != null){
                        if(X + Width > RpgGame.CurrentGameWorld.PixelWidth){
                            X = (RpgGame.CurrentGameWorld.PixelWidth - Width);
                        }
                    }
                }

                
                

                if (Y < 0){
                    Y = 0;
                }else{
                    if(RpgGame.CurrentGameWorld != null){
                        if(Y + Height > RpgGame.CurrentGameWorld.PixelHeight){
                            Y = (RpgGame.CurrentGameWorld.PixelHeight - Height);
                        }
                    }
                }

                Position = new Vector2(X, Y);
            }
        }

        public void Draw(ref SpriteBatch batch)
        {
            Texture2D test = new Texture2D(GraphicSettings.GraphicDevice, TargetArea.Width, TargetArea.Height);
            Color[] clr = new Color[test.Width * test.Height];

            for (int i = 0; i < clr.Length; ++i)
            {
                clr[i] = Color.Red;
                clr[i].A = 80;
            }
            test.SetData<Color>(clr);
            batch.Draw(test,new Vector2(_targetArea.X,_targetArea.Y),Color.White);
        }
    }
}
