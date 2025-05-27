using System;
using System.Collections.Generic;
using Core.Components.Input;
using UnityEngine.UI;
using FrameWork.Events.Input;
using UnityEngine.Events;

namespace Framework.Events.Input
{
    internal class ButtonClickListener<TPayload> : IEventListener<ControlEvent<TPayload>>, IDisposable where TPayload : struct
    {
        private readonly Button _button;
        private readonly EventContainer<ControlEvent<TPayload>> _container;
        private readonly IEventListener<ControlEvent<TPayload>> _inner;
        private readonly Func<Button, TPayload> _selector;
        private readonly bool _repeat;
        private readonly List<UnityAction> _actions = new();

        public IStateRetainer<ControlEvent<TPayload>> state => _container;

        public ButtonClickListener(
            Button button,
            EventContainer<ControlEvent<TPayload>> container,
            Func<Button, TPayload> selector,
            bool repeat
        )
        {
            _button = button;
            _container = container;
            _selector = selector;
            _repeat = repeat;
            _inner = new BaseEventListener<ControlEvent<TPayload>>(container, repeat);
        }

        public IDisposable Subscribe(EventHandler<ControlEvent<TPayload>> handler)
        {
            var subscription = _inner.Subscribe(handler);

            UnityAction action = () =>
            {
                var payload = _selector(_button);
                var evt = new ControlEvent<TPayload>
                {
                    value = payload,
                    state = InputState.Performed
                };

                _container.publisher?.Invoke(_button, evt);
            };

            _actions.Add(action);
            _button.onClick.AddListener(action);

            // teardown just this subscription
            return new ActionDisposable(() =>
            {
                subscription.Dispose();
                _button.onClick.RemoveListener(action);
                _actions.Remove(action);
            });
        }

        public void Unsubscribe(EventHandler<ControlEvent<TPayload>> handler)
        {
            _inner.Unsubscribe(handler);
        }

        public void Dispose()
        {
            // remove all listeners we added
            foreach (var a in _actions)
            {
                _button.onClick.RemoveListener(a);
            }

            _actions.Clear();
        }
    }
}