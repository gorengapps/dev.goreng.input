using System;
using Framework.Events;
using Framework.Events.Extensions;

namespace Core.Components.Input
{
    public class CompositeEventListener<T> : IEventListener<T> where T : struct
    {
        readonly IEventProducer<T>[] sources;
        private readonly DisposeBag _disposeBag = new DisposeBag();

        public CompositeEventListener(params IEventProducer<T>[] sources)
        {
            this.sources = sources;
        }

        IDisposable IEventListener<T>.Subscribe(EventHandler<T> handler)
        {
            foreach (var producer in sources)
            {
                producer.listener.Subscribe(handler)
                    .AddToDisposables(_disposeBag);
            }

            return _disposeBag;
        }

        public void Unsubscribe(EventHandler<T> handler)
        {
            foreach (var producer in sources)
            {
                producer.listener.Unsubscribe(handler);
            }
        }
    }
}