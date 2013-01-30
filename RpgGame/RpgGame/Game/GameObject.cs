using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using RpgGame.GameComponents;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame
{
    public class GameObject
    {
        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Orientation { get; set; }

        public Vector2 Position_NextFrame
        {
            get
            {
                return Position + Velocity;
            }
        }
        
        public static List<GameObject> AllObjects { get; private set; }
        public List<IGameObjectComponent> Components { get; private set; }

        public string Name { get; set; }
        public string Type { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        //Copy Constructor
        public GameObject(ref GameObject originalObject)
            : this(originalObject.Name + "_copy")
        {
            foreach (BaseGameComponent component in originalObject.Components)
            {
                AddComponent(component.Copy());
            }
        }

        public GameObject(string name)
        {
            Components = new List<IGameObjectComponent>();
            Name = name;
            Velocity = Vector2.Zero;

            if (AllObjects == null)
            {
                AllObjects = new List<GameObject>();
            }

            AllObjects.Add(this);
        }

        public GameObject(string name, IGameObjectComponent[] components)
            : this(name)
        {
            Components = new List<IGameObjectComponent>();
            foreach (IGameObjectComponent component in components)
            {
                AddComponent(component);
            }
        }

        public GameObject()
            : this("")
        {

        }

        ~GameObject()
        {
            AllObjects.Remove(this);
        }

        public void Update(GameTime gameTime)
        {
            //Update the position of the player.
            Position += Velocity;

            //All components are getting updated first.
            foreach (IGameObjectComponent c in Components)
            {
                c.Position = Position;

                if (c.Active)
                {
                    c.Update(gameTime);
                }
            }
        }

        public void AddComponent(IGameObjectComponent component)
        {
            Components.Add(component);
            component.Parent = this;
            component.Position = Position;
            component.Init();
        }

        public void Draw(ref SpriteBatch batch)
        {
            foreach (IGameObjectComponent c in Components)
            {
                if(c.Active)
                {
                    c.Draw(ref batch);
                }
            }
        }

        public T GetComponent<T>()
        {
            foreach (IGameObjectComponent c in Components)
            {
                if (c is T)
                {
                    return (T)c;
                }
            }

            return default(T);
        }

        public List<IGameObjectComponent> GetComponents<T>()
        {
            List<IGameObjectComponent> results = new List<IGameObjectComponent>();

            foreach (IGameObjectComponent c in Components)
            {
                if (c is T)
                {
                    results.Add(c);
                }
            }

            return results;
        }
    }
}
