using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RpgGame.Events;

namespace RpgGame.Manager
{
    class EventManager
    {
        private static Dictionary<Event.Types, List<IEventListener>> m_dEventListener = new Dictionary<Event.Types, List<IEventListener>>();
        private static List<Event> m_lsEvents = new List<Event>();

        public static Dictionary<Event.Types, List<IEventListener>> EventListener
        {
            get { return m_dEventListener; }
        }

        public static void AddListener(Event.Types type, IEventListener listener)
        {
            List<IEventListener> lsListener;

            //Versuche die Liste für den bestimmten EventType zu holen.
            m_dEventListener.TryGetValue(type,out lsListener);

            //Wenn noch keine Liste existiert, wird eine angelegt.
            if (lsListener == null)
            {
                lsListener = new List<IEventListener>();
                m_dEventListener.Add(type, lsListener);
            }

            //Füge listener hinzu.
            lsListener.Add(listener);
        }

        public static void RemoveListener(Event.Types type, IEventListener listener)
        {
            List<IEventListener> lsListener;
            m_dEventListener.TryGetValue(type, out lsListener);

            if(lsListener == null && !lsListener.Contains(listener))
            {
                return;
            }

            lsListener.Remove(listener);
            m_dEventListener.Remove(type);
            m_dEventListener.Add(type, lsListener);
        }

        public static void Update()
        {
            foreach (Event eGameEvent in m_lsEvents)
            {
                List<IEventListener> lsListener;
                m_dEventListener.TryGetValue(eGameEvent.Type,out lsListener);
                if(lsListener != null)
                {
                    IEventListener[] tmpArray = new IEventListener[lsListener.Count];
                    lsListener.CopyTo(tmpArray);
                    foreach (IEventListener evListener in tmpArray)
                    {
                        //Rufe jeden Listener auf für diesen Event Typ
                        evListener.HandleEvent(eGameEvent);
                    }
                }
            }

            //Leere Eventliste nachdem sie abgearbeitet wurde.
            m_lsEvents.Clear();
        }

        public static void AddEventToQuery(Event gameEvent)
        {
            m_lsEvents.Add(gameEvent);
        }
    }
}
