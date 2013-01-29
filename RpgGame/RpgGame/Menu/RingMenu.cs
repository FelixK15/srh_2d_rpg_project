using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using RpgGame.Manager;
using RpgGame.GameComponents;

using RpgGame.Events;
using RpgGame.Processes;
using Microsoft.Xna.Framework;

namespace RpgGame.Menu
{
    class RingMenu
    {
        public List<GameObject> Items { get; set; }

        private Texture2D Picker { get; set; }

        private Vector2 Position { get; set; }

        public int Radius { get; set; }
        private int CurrentItemIndex { get; set; }

        public RingMenu Next { get; set; }
        public RingMenu Previous { get; set; }

        public RingMenu(Vector2 position)
        {
            Position = position;
            Items = new List<GameObject>();
            Picker = RpgGame.ContentManager.Load<Texture2D>("picker");
            CurrentItemIndex = 0;
            Radius = 50;
        }

        public void AddItem(GameObject item)
        {
            Items.Add(item);

            item.Position = Position;
            _AlignItems(true);
        }

        public void NextItem()
        {
            if (++CurrentItemIndex >= Items.Count)
            {
                CurrentItemIndex = 0;
            }

            _AlignItems(true);
        }

        public void PreviousItem()
        {
            if (--CurrentItemIndex < 0)
            {
                CurrentItemIndex = (Items.Count - 1);
            }

            _AlignItems(false);
        }

        public void ZoomIn()
        {
            float anglestep = (float)((2 * Math.PI) / Items.Count);
            float angle = (float)(Math.PI * 1.5 - (anglestep * CurrentItemIndex));
            SphericMoveProcess moveProcess = null;
            foreach (GameObject go in Items)
            {
                moveProcess = new SphericMoveProcess(go, Position, angle, angle + anglestep, Radius, 0, 100);
                angle += anglestep;
                moveProcess.Next = new EventProcess(new Event(Event.Types.RINGMENU_ZOOM_IN));
                ProcessManager.Processes.Add(moveProcess);
            }
        }

        public void ZoomOut()
        {
            float anglestep = (float)((2 * Math.PI) / Items.Count);
            float angle = (float)(Math.PI * 1.5 - (anglestep * CurrentItemIndex));
            SphericMoveProcess moveProcess = null;
            foreach (GameObject go in Items)
            {
                moveProcess = new SphericMoveProcess(go, Position, angle, angle + anglestep, Radius, 1000, 500);
                angle += anglestep;
                moveProcess.Next = new EventProcess(new Event(Event.Types.RINGMENU_ZOOM_OUT));
                ProcessManager.Processes.Add(moveProcess);
            }
        }

        public void Draw(ref SpriteBatch batch)
        {
            foreach (GameObject go in Items)
            {
                go.Draw(ref batch);
            }

            //GraphicManager.Graphics.Add(new GraphicHelper((int)Items.ElementAt<GameObject>(CurrentItemIndex).Position.X, (int)Items.ElementAt<GameObject>(CurrentItemIndex).Position.Y, Picker));
        }

        private void _AlignItems(bool clockwise)
        {
            double modifier = clockwise ? 1.5 : 0.5;
            double anglestep = (2 * Math.PI) / Items.Count;
            double angle = (modifier * Math.PI) - (anglestep * CurrentItemIndex);

            foreach (GameObject go in Items)
            {
                SphericMoveProcess moveProcess = null;

                if (!clockwise)
                {
                    moveProcess = new SphericMoveProcess(go, Position, (float)angle, (float)(angle + anglestep), Radius, Radius, 300);
                    angle += anglestep;
                }
                else
                {
                    moveProcess = new SphericMoveProcess(go, Position, (float)angle, (float)(angle - anglestep), Radius, Radius, 300);
                    angle -= anglestep;
                }

                moveProcess.Next = new EventProcess(new Event(Event.Types.RINGMENU_ITEM_CHANGED));
                ProcessManager.Processes.Add(moveProcess);
            }
        }
    }
}
