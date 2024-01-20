using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

namespace Services.EventProviderComponents
{
    public class EventProvider : IService
    {
        private readonly Dictionary<Type, ArrayList> _globalEvents;

        public EventProvider() => 
            _globalEvents = new Dictionary<Type, ArrayList>();

        public void Invoke<T>(T arg) where T : IEvent
        {
            if (_globalEvents.ContainsKey(typeof(T)))
            {
                ArrayList tmp = new ArrayList(_globalEvents[typeof(T)]);
                foreach (Action<T> action in tmp)
                    action.Invoke(arg);
            }
        }

        public void Subscribe<T>(Action<T> action) where T : IEvent
        {
            if (!_globalEvents.ContainsKey(typeof(T)))
                _globalEvents.Add(typeof(T), new ArrayList());

            _globalEvents[typeof(T)].Add(action);
        }

        public void UnSubscribe<T>(Action<T> action) where T : IEvent
        {
            if (_globalEvents.ContainsKey(typeof(T)))
                _globalEvents[typeof(T)].Remove(action);
            else
                Debug.LogWarning("EventClass not presented in dictionary");
        }
    }
}