using System;
using UnityEngine.Events;
using Core.Components.Input;

namespace Framework.Events.Input.Events
{
   public class UnityEventProducer<T, TPayload> : IDisposableEventProducer<ControlEvent<TPayload>>
    {
        private readonly EventContainer<ControlEvent<TPayload>> _container;
        private readonly UnityEventListener<T, TPayload> _listener;

        public IStateRetainer<ControlEvent<TPayload>> state => _container;
        public IEventListener<ControlEvent<TPayload>> listener => _listener;

        public UnityEventProducer(UnityEvent<T> unityEvent, Func<T, TPayload> selector)
        {
            if (unityEvent == null) throw new ArgumentNullException(nameof(unityEvent));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            _container = new EventContainer<ControlEvent<TPayload>>();
            _listener = new UnityEventListener<T, TPayload>(unityEvent, _container, selector);
        }

        public void Publish(object sender, ControlEvent<TPayload> data)
        {
            _container.publisher?.Invoke(sender, data);
        }

        public void Dispose()
        {
            _listener.Dispose();
        }
    }

    public class UnityEventProducer<TPayload> : IDisposableEventProducer<ControlEvent<TPayload>>
    {
        private readonly EventContainer<ControlEvent<TPayload>> _container;
        private readonly UnityEventListener<TPayload> _listener;

        public IStateRetainer<ControlEvent<TPayload>> state => _container;
        public IEventListener<ControlEvent<TPayload>> listener => _listener;

        public UnityEventProducer(UnityEvent unityEvent, Func<TPayload> selector)
        {
            if (unityEvent == null) throw new ArgumentNullException(nameof(unityEvent));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            _container = new EventContainer<ControlEvent<TPayload>>();
            _listener = new UnityEventListener<TPayload>(unityEvent, _container, selector);
        }

        public void Publish(object sender, ControlEvent<TPayload> data)
        {
            _container.publisher?.Invoke(sender, data);
        }

        public void Dispose()
        {
            _listener.Dispose();
        }
    }
}