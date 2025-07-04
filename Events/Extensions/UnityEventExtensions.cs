using System;
using Framework.Events;
using Framework.Events.Input.Events;
using UnityEngine.Events;

namespace Core.Components.Input
{
    public static class UnityEventExtensions
    {
        public static IDisposableEventProducer<ControlEvent<TPayload>> ToControlEventProducer<T, TPayload>(
            this UnityEvent<T> unityEvent,
            Func<T, TPayload> selector
        )
        {
            return new UnityEventProducer<T, TPayload>(unityEvent, selector);
        }
        
        public static IDisposableEventProducer<ControlEvent<T>> ToControlEventProducer<T>(
            this UnityEvent<T> unityEvent
        )
        {
            return new UnityEventProducer<T, T>(unityEvent, value => value);
        }

        public static IDisposableEventProducer<ControlEvent<TPayload>> ToControlEventProducer<TPayload>(
            this UnityEvent unityEvent,
            Func<TPayload> selector
        )
        {
            return new UnityEventProducer<TPayload>(unityEvent, selector);
        }

        public static IDisposableEventProducer<ControlEvent<bool>> ToControlEventProducer(this UnityEvent unityEvent)
        {
            return new UnityEventProducer<bool>(unityEvent, () => true);
        }
    }
}