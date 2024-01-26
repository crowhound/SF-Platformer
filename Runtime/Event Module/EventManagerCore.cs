using System;
using System.Collections.Generic;
using UnityEngine;

namespace SF.Events
{
    /// <summary>
    /// The categories of events in the project.
    /// </summary>
    public enum GameEventTypes
    {
        Score,
        Audio,
    }

    /// <summary>
    /// A utility class that helps classes to start listening and stop listening to events using simple function calls. 
    /// </summary>
    public static class EventRegister
    {
        public delegate void Delegate<T>(T eventType);

        /// <summary>
        /// Registers a class instance to listen for any type of event that is passed in. EventType is a generic variable type that accepts any struct.
        /// </summary>
        /// <typeparam name="EventType"></typeparam>
        /// <param name="caller"></param>
        public static void EventStartListening<EventType> (this EventListener<EventType> caller) where EventType : struct
        {
            EventManager.AddListener<EventType>(caller);
        }
        public static void EventStopListening<EventType> (this EventListener<EventType> caller) where EventType : struct
        {
            EventManager.RemoveListener<EventType>(caller);
        }
    }
    public interface EventListenerBase { }
    public interface EventListener<T> : EventListenerBase 
    {
        void OnEvent(T eventType);
    }
    public static class EventManager
    {
        private static Dictionary<Type, List<EventListenerBase>> _subscribersList;

        static EventManager()
        {
            _subscribersList = new Dictionary<Type, List<EventListenerBase>>();
        }

        public static void AddListener<EventType>(EventListener<EventType> listener) where EventType : struct
        {
            Type eventType = typeof(EventType);
            
            if(!_subscribersList.ContainsKey(eventType))
            {
                _subscribersList[eventType] = new List<EventListenerBase>();
            }

            if(!DoesListenerExists(eventType, listener))
            {
                _subscribersList[eventType].Add(listener);
            } 
        }

        public static void RemoveListener<EventType>(EventListener<EventType> listener) where EventType : struct
        {
            Type eventType = typeof(EventType);

            if(!_subscribersList.ContainsKey(eventType))
            {
                return;
            }

            List<EventListenerBase> subscriberList = _subscribersList[eventType];
            // Add a check to see if the subscriber already exists

            for (int i = 0; i < subscriberList.Count; i++)
            {
               if(subscriberList[i] == listener)
               {
                    subscriberList.Remove(subscriberList[i]);

                    if(subscriberList.Count == 0)
                    {
                        _subscribersList.Remove(eventType);
                    }

                    return;
               }
            }
        }

        public static bool DoesListenerExists(Type type, EventListenerBase listener)
        {
            List<EventListenerBase> listeners;
            if (!_subscribersList.TryGetValue(type, out listeners))
                return false;

            bool exists = false;

            for (int i = 0; i < listeners.Count; i++)
            {
                if(listeners[i] == listener)
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }
        public static void TriggerEvent<EventType> (EventType newEvent) where EventType : struct
        {
            List<EventListenerBase> eventList;
            // Tries to find values in the subscribed list of the type event and if not found return.
            if(!_subscribersList.TryGetValue(typeof(EventType), out eventList))
            {
                return;
            }
            for (int i = 0; i < eventList.Count; i++)
            {
                (eventList[i] as EventListener<EventType>).OnEvent(newEvent);
            }
        }
    }
}