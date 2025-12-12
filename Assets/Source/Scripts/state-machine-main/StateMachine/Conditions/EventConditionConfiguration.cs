using System;

namespace StateMachine.Conditions
{
    public class EventConditionConfiguration
    {
        public event Action action;

        public void Invoke()
        {
            action?.Invoke();
        }
    }
}