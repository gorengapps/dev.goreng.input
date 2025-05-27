using System;
using Framework.Events;
using Framework.Events.Input;
using UnityEngine.UI;

namespace Core.Components.Input
{
    public static class ButtonExtensions
    {
        /// <summary>
        /// Wraps this Button into a ControlEventProducer of any payload type.
        /// </summary>
        public static IDisposableEventProducer<ControlEvent<TPayload>> ToControlEventProducer<TPayload>(
            this Button button,
            Func<Button, TPayload> selector,
            bool repeat = false
        ) where TPayload : struct
        {
            return new ButtonControlEventProducer<TPayload>(button, selector);
        }
        
        /// <summary>
        /// Wraps this Button into a ControlEventProducer of any payload type.
        /// </summary>
        public static IDisposableEventProducer<ControlEvent<bool>> ToControlEventProducer(this Button button)
        {
            return new ButtonControlEventProducer<bool>(button, (ctx) => true);
        }
    }
}