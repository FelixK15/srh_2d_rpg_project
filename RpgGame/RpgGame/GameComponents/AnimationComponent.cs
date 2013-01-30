using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using RpgGame.Manager;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.GameComponents
{
    class AnimationComponent : BaseGameComponent
    {
        private List<AbstractAnimation> AnimationList { get; set; }
        private AbstractAnimation CurrentAnimation { get; set; }
        public Color DrawColor { get; set; }

        public AnimationComponent() : base("AnimationComponent")
        {
            AnimationList = new List<AbstractAnimation>();
            DrawColor = Color.White;
        }

        public AnimationComponent(AbstractAnimation anim) : this()
        {
            AnimationList.Add(anim);
            CurrentAnimation = anim;
        }

        public AbstractAnimation getAnimationPerName(string animationName)
        {
            return AnimationList.Find(o => o.Name == animationName);
        }

        public bool addAnimation(AbstractAnimation anim)
        {
            if (AnimationList.Find(o => o.Name == anim.Name) != null) return false;
            AnimationList.Add(anim);
            return true;
        }

        public bool setCurrentAnimation(string animationName)
        {
            AbstractAnimation anim = AnimationList.Find(o => o.Name == animationName);
            if (anim == CurrentAnimation)
            {
                return true;
            }

            if (anim != null)
            {
                CurrentAnimation = anim;
                CurrentAnimation.CurrentFrame = CurrentAnimation.FrameList.First<AnimationFrame>();
                return true;
            }
            return false;
        }

        public AbstractAnimation getCurrentAnimation()
        {
            return CurrentAnimation;
        }
           
        public override void Update(GameTime gameTime)
        {
            CurrentAnimation.Update(gameTime);
            Parent.Width = CurrentAnimation.CurrentFrame.texture.Width;
            Parent.Height = CurrentAnimation.CurrentFrame.texture.Height;
        }

        public override void Draw(ref SpriteBatch batch)
        {
            if (CurrentAnimation.CurrentFrame != null)
            {
                batch.Draw(CurrentAnimation.CurrentFrame.texture, Position, DrawColor);
            }
        }
    }
}
