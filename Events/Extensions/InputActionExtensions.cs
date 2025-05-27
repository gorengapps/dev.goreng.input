using System;
using Framework.Events.Input;
using UnityEngine.InputSystem;

namespace Core.Components.Input
{
    public static class InputActionExtensions
    {
        /// <summary>
        /// Fully‐featured publisher: started / performed / canceled
        /// </summary>
        public static ControlEventProducer<T> ToControlPublisher<T>(
            this InputAction action, 
            Func<InputAction.CallbackContext, T> selector
        ) where T : struct
            => new ControlEventProducer<T>(action,  (ctx) => new ControlEvent<T> {
                value = selector.Invoke(ctx),
                state = ctx.phase.ToInputState()
            });

        public static ControlEventProducer<T> ToControlPublisher<T>(this InputAction action) where T : struct
            => new ControlEventProducer<T>(action, (ctx) => new ControlEvent<T> {
                value = ctx.ReadValue<T>(),
                state = ctx.phase.ToInputState()
            });

        /// <summary>
        /// Only taps into the Performed phase, emits true on performed.
        /// </summary>
        public static ControlEventProducer<bool> ToControlTrigger(this InputAction action)
            => new ControlEventProducer<bool>(action, (ctx) => new ControlEvent<bool> {
                value = true,
                state = ctx.phase.ToInputState()
            });
    }
}
