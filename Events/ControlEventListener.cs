using System;
using FrameWork.Events.Input;
using UnityEngine.InputSystem;

namespace Framework.Events.Input
{
    /// <summary>
    /// Listens to a Unity InputAction, publishes through an EventContainer<T>,
    /// and automatically unhooks the action when the returned IDisposable is disposed.
    /// </summary>
    internal class ControlEventListener<T> : IEventListener<T>, IDisposable where T : struct
    {
        private readonly IEventContainer<T> _container; 
        private readonly InputAction _action;
        private readonly Func<InputAction.CallbackContext, T> _selector;
        
        public IStateRetainer<T> state => _container;
        
        public ControlEventListener(
            InputAction action,
            IEventContainer<T> container,
            Func<InputAction.CallbackContext, T> selector
        )
        {
            _action = action;
            _selector = selector;
            _container = container;
            
            _action.started += OnInput;
            _action.performed += OnInput;
            _action.canceled += OnInput;
        }
        
        public void Dispose()
        {
            _action.started -= OnInput;
            _action.performed -= OnInput;
            _action.canceled -= OnInput;
        }
        
        public IDisposable Subscribe(EventHandler<T> handler)
        {
            _container.publisher += handler;
            
            // 3) return a composite that cleans up both
            return new ActionDisposable(() => {
                
                // unsubscribe handler
                _container.publisher -= handler;
            });
        }

        public void Unsubscribe(EventHandler<T> handler)
        {
            _container.publisher -= handler;
        }
        
        private void OnInput(InputAction.CallbackContext ctx)
        {
            var data = _selector(ctx);
            _container.publisher?.Invoke(ctx.action, data);
        }
    }
}