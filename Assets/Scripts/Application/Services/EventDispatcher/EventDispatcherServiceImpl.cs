using System;
using System.Collections.Generic;
using Domain.Services.EventDispatcher;

namespace Application.Services.EventDispatcher
{
    public class EventDispatcherServiceImpl : EventDispatcherService
    {
        private readonly Dictionary<Type, SignalDelegate> _events;

        public EventDispatcherServiceImpl()
        {
            _events = new Dictionary<Type, SignalDelegate>();
        }

        public void Subscribe<T>(SignalDelegate callback) where T : Signal
        {
            var type = typeof(T);
            if (!_events.ContainsKey(type))
            {
                _events.Add(type, null);
            }

            _events[type] += callback;
        }

        public void Unsubscribe<T>(SignalDelegate callback) where T : Signal
        {
            var type = typeof(T);
            if (_events.ContainsKey(type))
            {
                _events[type] -= callback;
            }
        }

        public void Call<T>(T signal) where T : Signal
        {
            var type = typeof(T);
            if (_events.ContainsKey(type))
            {
                _events[type](signal);
            }
        }
    }
}