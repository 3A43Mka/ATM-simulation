using System;
using System.Collections.Generic;
using System.Text;

namespace CourseWork
{
    // Observer
    public enum EVENT_TYPE
    {
        CHOOSESUM,
        ADDTOWITHDRAW,
        REMOVEFROMWITHDRAW,
        WITHDRAW,
        CHANGEPIN,
    }
    public interface IEventListener
    {
        void update(EVENT_TYPE eventType, string info);
    }
    public class EventManager
    {
        Dictionary<EVENT_TYPE, IEventListener> listeners = new Dictionary<EVENT_TYPE, IEventListener>();
        public EventManager(params EVENT_TYPE[] events)
        {
            foreach(EVENT_TYPE ev in events)
            {
                listeners.Add(ev, null);
            }
        }
        public void subscribe(EVENT_TYPE ev, IEventListener listener)
        {
            if (listeners.ContainsKey(ev))
            {
                listeners[ev] = listener;
            }
        }
        public void unsubscribe(EVENT_TYPE ev, IEventListener listener)
        {
            if (listeners.ContainsKey(ev))
            {
                listeners[ev] = null;
            }
        }
        public void notify(EVENT_TYPE ev, string info)
        {
            if (listeners.ContainsKey(ev))
            {
                listeners[ev].update(ev, info);
            }
        }
    }
}
