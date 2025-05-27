using System;

namespace FrameWork.Events.Input
{
    public class ActionDisposable : IDisposable
    {
        private Action disposeAction;

        public ActionDisposable(Action dispose)
        {
            disposeAction = dispose;
        }

        public void Dispose()
        {
            disposeAction?.Invoke();
            disposeAction = null;
        }
    }
}