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

            // Add a check to see if the subscriber already exists

            _subscribersList[eventType].Add(listener);
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

            _subscribersList[eventType].Add(listener);

            // TODO I need to finish the removal of the listener to the event subscriber.
        }
    
        public static void TriggerEvent<EventType> (EventType newEvent) where EventType : struct
        {

        }
    }
}