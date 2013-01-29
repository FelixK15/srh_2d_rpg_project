using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RpgGame.Events;
using RpgGame.Manager;
using RpgGame.Menu;

using System.Threading;

namespace RpgGame.GameStates
{
    class RingMenuState : IGameState
    {
        public RingMenu CurrentMenu { get; set; }

        private bool ActionAllowed { get; set; }
        private Texture2D Background { get; set; }
        
        public RingMenuState(RingMenu startMenu)
        {
            CurrentMenu = startMenu;
            ActionAllowed = true;
        }

        public void Start()
        {
            Background = new Texture2D(GraphicSettings.GraphicDevice,
                                        GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Width,
                                        GraphicSettings.GraphicDevice.Adapter.CurrentDisplayMode.Height);

            Color[] Pixels = new Color[Background.Width * Background.Height];
            for (int i = 0; i < Pixels.Length; ++i)
            {
                Pixels[i] = Color.Black;
                Pixels[i].A = 120;
            }
            Background.SetData<Color>(Pixels);
        }

        public void Update(GameTime gameTime)
        {
            foreach (GameObject go in CurrentMenu.Items)
            {
                go.Update(gameTime);
            }
        }

        public void Draw(ref SpriteBatch batch)
        {
            batch.Draw(Background, Vector2.Zero, Color.White);
            //GraphicManager.Graphics.Add(new GraphicHelper(0,0,Background));
            CurrentMenu.Draw(ref batch);
        }

        public void Stop()
        {
        
        }

        public void HandleEvent(Event eGameEvent)
        {
            if (eGameEvent is KeyPressedEvent)
            {
                if (ActionAllowed)
                {
                    KeyPressedEvent keyEvent = (KeyPressedEvent)eGameEvent;

                    if (keyEvent.PressedKey == Keys.A)
                    {
                        CurrentMenu.NextItem();
                        ActionAllowed = false;
                    }
                    else if (keyEvent.PressedKey == Keys.D)
                    {
                        CurrentMenu.PreviousItem();
                        ActionAllowed = false;
                    }
                    else if (keyEvent.PressedKey == Keys.W)
                    {
                        if (CurrentMenu.Previous != null)
                        {
                            CurrentMenu.ZoomOut();
                            ActionAllowed = false;
                        }

                    }
                    else if (keyEvent.PressedKey == Keys.S)
                    {
                        if (CurrentMenu.Next != null)
                        {
                            CurrentMenu.ZoomIn();
                            ActionAllowed = false;
                        }
                    }
                }
            }
            else if (eGameEvent.Type == Event.Types.RINGMENU_ITEM_CHANGED)
            {
                ActionAllowed = true;
            }
            else if (eGameEvent.Type == Event.Types.RINGMENU_ZOOM_IN)
            {
                ActionAllowed = true;
                CurrentMenu = CurrentMenu.Next;
            }
            else if (eGameEvent.Type == Event.Types.RINGMENU_ZOOM_OUT)
            {
                ActionAllowed = true;
                CurrentMenu = CurrentMenu.Previous;
            }
        }
    }
}
