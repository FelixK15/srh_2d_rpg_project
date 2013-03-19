using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using RpgGame.Manager;
using Microsoft.Xna.Framework.Graphics;
using RpgGame.Tools;

namespace RpgGame.GameComponents
{
    public class AnimationComponent : BaseGameComponent
    {
        private List<AbstractAnimation> AnimationList       { get; set; }
        private AbstractAnimation       CurrentAnimation    { get; set; }
        public Color                    DrawColor           { get; set; }

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

        public AnimationComponent(List<AbstractAnimation> animList)
            : this()
        {
            foreach (AbstractAnimation anim in animList){
                AnimationList.Add(anim);
            }

            CurrentAnimation = AnimationList.Last<AbstractAnimation>();
        }

        public AbstractAnimation GetAnimationPerName(string animationName)
        {
            return AnimationList.Find(o => o.Name == animationName);
        }

        public bool AddAnimation(AbstractAnimation anim)
        {
            if (AnimationList.Find(o => o.Name == anim.Name) != null){
                DeveloperConsole.AddMessage(DeveloperConsole.MessageType.ERROR,
                String.Format("Didn't add animation '{0}' because there is already an animation with that name.",anim.Name));

                return false;
            }
            AnimationList.Add(anim);
            return true;
        }

        public bool AddAnimation(AbstractAnimation anim, bool removeAnimWithSameName)
        {
            if (removeAnimWithSameName){
                AbstractAnimation prevAnim = AnimationList.Find(a => a.Name == anim.Name);
                if (prevAnim != null){
                    AnimationList.Remove(prevAnim);
                }
            }

            return AddAnimation(anim);
        }

        public bool SetCurrentAnimation(string animationName)
        {
            AbstractAnimation anim = AnimationList.Find(o => o.Name == animationName);
            if (anim == CurrentAnimation){
                return true;
            }

            if (anim != null){
                anim.CurrentAnimationIndex  = 0;
                anim.TimeSinceLastFrame     = 0;
                CurrentAnimation = anim;
                CurrentAnimation.CurrentFrame = CurrentAnimation.FrameList.First<AnimationFrame>();
                return true;
            }

            DeveloperConsole.AddMessage(DeveloperConsole.MessageType.ERROR,String.Format("Could not find animation with the name '{0}'.",animationName));
            return false;
        }

        public AbstractAnimation GetCurrentAnimation()
        {
            return CurrentAnimation;
        }
           
        public override void Update(GameTime gameTime)
        {
            if (CurrentAnimation != null){
                CurrentAnimation.Update(gameTime);
                if (CurrentAnimation.CurrentFrame != null){
                    Parent.Width    = CurrentAnimation.CurrentFrame.Sprite.Width;
                    Parent.Height   = CurrentAnimation.CurrentFrame.Sprite.Height;
                }
            }
        }

        public override void Draw(ref SpriteBatch batch)
        {
            if (CurrentAnimation.CurrentFrame != null){
                batch.Draw(CurrentAnimation.CurrentFrame.Sprite, Position, DrawColor);
            }
        }
    }
}
