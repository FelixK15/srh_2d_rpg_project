using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RpgGame.Manager;
using Microsoft.Xna.Framework.Graphics;

using RpgGame.Tools;

namespace RpgGame.GameComponents
{
    public class CollisionComponent : BaseGameComponent
    {
        public enum CollisionType
        {
            STATIC,
            DYNAMIC
        }

        public Vector2 Offset { get; set; }
        public CollisionType Type { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public List<String> CollisionTypes { get; private set; }
        private List<GameObject> CollisionObjects { get; set; }

        public CollisionComponent(CollisionType type)
            : this(type,Vector2.Zero,0,0,null)
        {

        }

        public CollisionComponent(CollisionType type,Vector2 offset,int width,int height,List<String> collisionTypes)
            : base("CollisionComponent")
        {
            Width = width;
            Height = height;
            Offset = offset;
            Type = type;
            CollisionTypes = collisionTypes == null ? new List<String>() : collisionTypes;
            CollisionObjects = new List<GameObject>();
        }

        public override void Update(GameTime gameTime)
        {
            bool remove = false;
            ScriptComponent scriptComponent = null;
            CollisionComponent collisionComponent = null;
            List<GameObject> objectsToRemove = new List<GameObject>();
            
            foreach (GameObject go in CollisionObjects)
            {
                scriptComponent = Parent.GetComponent<ScriptComponent>();
                collisionComponent = go.GetComponent<CollisionComponent>();

                if (collisionComponent == null)
                {
                    remove = true;
                }
                else
                {
                    if (!CollisionChecker.Intersect(new Rectangle((int)(Position.X + Offset.X),(int)(Position.Y + Offset.Y),
                                                            Width,Height), new Rectangle((int)(collisionComponent.Position.X + collisionComponent.Offset.X),
                                                            (int)(collisionComponent.Position.Y + collisionComponent.Offset.Y), collisionComponent.Width, collisionComponent.Height)))
                    {
                        remove = true;
                    }
                }

                if (scriptComponent != null)
                {
                    if (remove)
                    {
                        scriptComponent.CallFunction("OnTriggerLeave", new object[] { go });
                        objectsToRemove.Add(go);
                        remove = false;
                    }
                    else
                    {
                        scriptComponent.CallFunction("OnCollision", new object[] { go });
                    }
                }          
            }

            foreach (GameObject go in objectsToRemove)
            {
                CollisionObjects.Remove(go);
            }
        }

        public override void Draw(ref SpriteBatch batch)
        {
//             Texture2D test = new Texture2D(GraphicManager.GraphicDevice,Width,Height);
//             Color[] tetc = new Color[test.Width * test.Height];
//             for (int i = 0; i < (test.Width * test.Height); ++i)
//             {
//                 tetc[i] = Color.Blue;
//             }
//             test.SetData<Color>(tetc);
//             GraphicManager.Graphics.Add(new GraphicHelper((int)(Position.X + Offset.X), (int)(Position.Y + Offset.Y), test));
        }

        public void AddCollisionObject(GameObject gameObject)
        {
            if (CollisionObjects.Find(go => go == gameObject) == null)
            {
                ScriptComponent component = Parent.GetComponent<ScriptComponent>();

                if (component != null)
                {
                    component.CallFunction("OnTriggerEnter", new object[] { gameObject });
                }

                CollisionObjects.Add(gameObject);
            }
        }

        public override BaseGameComponent Copy()
        {
            CollisionComponent copyFromThis = new CollisionComponent(Type,Offset,Width,Height,CollisionTypes);
            return copyFromThis;
        }
    }
}
