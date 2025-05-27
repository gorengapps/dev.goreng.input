using System;
using Core.Components.Input;
using UnityEngine.UI;

namespace Framework.Events.Input
{
    public class ButtonControlEventProducer<TPayload> 
        : IDisposableEventProducer<ControlEvent<TPayload>>
        where TPayload : struct
    {
        private readonly EventContainer<ControlEvent<TPayload>> _container;
        private readonly ButtonClickListener<TPayload>          _listener;

        public IStateRetainer<ControlEvent<TPayload>> state    => _container;
        public IEventListener<ControlEvent<TPayload>> listener => _listener;

        public ButtonControlEventProducer(Button button, Func<Button, TPayload> selector)
        {
            if (button == null)
            {
                throw new ArgumentNullException(nameof(button));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            _container = new EventContainer<ControlEvent<TPayload>>();
            _listener  = new ButtonClickListener<TPayload>(button, _container, selector, false);
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