using System;
using UnityEngine.Events;
using FrameWork.Events.Input;
using Core.Components.Input;

namespace Framework.Events.Input.Events
{
        /// <summary>
    /// Listens to a UnityEvent with a parameter and translates it into a ControlEvent.
    /// </summary>
    internal class UnityEventListener<T, TPayload> : IEventListener<ControlEvent<TPayload>>, IDisposable
    {
        private readonly UnityEvent<T> _unityEvent;
        private readonly EventContainer<ControlEvent<TPayload>> _container;
        private readonly Func<T, TPayload> _selector;
        private UnityAction<T> _action;

        public IStateRetainer<ControlEvent<TPayload>> state => _container;

        public UnityEventListener(UnityEvent<T> unityEvent, EventContainer<ControlEvent<TPayload>> container, Func<T, TPayload> selector)
        {
            _unityEvent = unityEvent;
            _container = container;
            _selector = selector;
        }

        public IDisposable Subscribe(EventHandler<ControlEvent<TPayload>> handler)
        {
            _container.publisher += handler;
            _action = (value) =>
            {
                var payload = _selector(value);
                var evt = new ControlEvent<TPayload>
                {
                    value = payload,
                    state = InputState.Performed
                };
                _container.publisher?.Invoke(_unityEvent, evt);
            };
            _unityEvent.AddListener(_action);

            return new ActionDisposable(() =>
            {
                _container.publisher -= handler;
                if (_action != null)
                {
                    _unityEvent.RemoveListener(_action);
                }
            });
        }

        public void Unsubscribe(EventHandler<ControlEvent<TPayload>> handler)
        {
            _container.publisher -= handler;
        }

        public void Dispose()
        {
            if (_action != null)
            {
                _unityEvent.RemoveListener(_action);
            }
        }
    }

    /// <summary>
    /// Listens to a parameter-less UnityEvent and translates it into a ControlEvent.
    /// </summary>
    internal class UnityEventListener<TPayload> : IEventListener<ControlEvent<TPayload>>, IDisposable
    {
        private readonly UnityEvent _unityEvent;
        private readonly EventContainer<ControlEvent<TPayload>> _container;
        private readonly Func<TPayload> _selector;
        private UnityAction _action;

        public IStateRetainer<ControlEvent<TPayload>> state => _container;

        public UnityEventListener(UnityEvent unityEvent, EventContainer<ControlEvent<TPayload>> container, Func<TPayload> selector)
        {
            _unityEvent = unityEvent;
            _container = container;
            _selector = selector;
        }

        public IDisposable Subscribe(EventHandler<ControlEvent<TPayload>> handler)
        {
            _container.publisher += handler;
            _action = () =>
            {
                var payload = _selector();
                var evt = new ControlEvent<TPayload>
                {
                    value = payload,
                    state = InputState.Performed
                };
                _container.publisher?.Invoke(_unityEvent, evt);
            };
            _unityEvent.AddListener(_action);

            return new ActionDisposable(() =>
            {
                _container.publisher -= handler;
                if (_action != null)
                {
                    _unityEvent.RemoveListener(_action);
                }
            });
        }

        public void Unsubscribe(EventHandler<ControlEvent<TPayload>> handler)
        {
            _container.publisher -= handler;
        }

        public void Dispose()
        {
            if (_action != null)
            {
                _unityEvent.RemoveListener(_action);
            }
        }
    }
}