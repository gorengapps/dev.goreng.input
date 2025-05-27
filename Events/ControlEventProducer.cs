using System;
using Core.Components.Input;
using Framework.Events;
using UnityEngine.InputSystem;

namespace Framework.Events.Input
{
    /// <summary>
    /// A producer that uses a ControlEventListener under the hood.
    /// You no longer need to Dispose() the producer—disposing the subscription
    /// will unhook the InputAction events automatically.
    /// </summary>
    public class ControlEventProducer<T> : IDisposableEventProducer<ControlEvent<T>> where T : struct
    {
        private readonly ControlEventListener<ControlEvent<T>> _listener;
        private readonly EventContainer<ControlEvent<T>> _container;

        public IStateRetainer<ControlEvent<T>> state => _listener.state;
        public IEventListener<ControlEvent<T>> listener => _listener;

        public ControlEventProducer(InputAction action, Func<InputAction.CallbackContext, ControlEvent<T>> selector)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            _container = new EventContainer<ControlEvent<T>>();
            _listener = new ControlEventListener<ControlEvent<T>>(action, _container, selector);
        }

        // You can still manually publish if you really need to:
        public void Publish(object sender, ControlEvent<T> data)
        {
            _container.publisher?.Invoke(sender, data);
        }

        public void Dispose()
        {
            _listener?.Dispose();
        }
    }
}