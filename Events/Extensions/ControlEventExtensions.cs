using Framework.Events.Input;
using UnityEngine.InputSystem;

namespace Core.Components.Input
{
    public static class ControlEventExtensions
    {
        public static InputState ToInputState(this InputActionPhase phase)
        {
            return phase switch
            {
                InputActionPhase.Canceled => InputState.Canceled,
                InputActionPhase.Performed => InputState.Performed,
                InputActionPhase.Disabled => InputState.Disabled,
                InputActionPhase.Started => InputState.Started,
                InputActionPhase.Waiting => InputState.Waiting,
                _ => InputState.Canceled
            };
        }
        
        public static ControlEvent<T> ToControlEvent<T>(this InputAction.CallbackContext context) where T : struct
        {
            return new ControlEvent<T>
            {
                value = context.ReadValue<T>(),
                state = context.phase.ToInputState()
            };
        }
    }
}