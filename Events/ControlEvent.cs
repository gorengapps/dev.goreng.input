using Framework.Events.Input;

namespace Core.Components.Input
{
    public struct ControlEvent<T>
    {
        public InputState state;
        public T value;
    }
}