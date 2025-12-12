using System;

namespace StateMachine.Conditions
{
    public class FuncCondition : StateCondition
    {
        private readonly Func<bool> _func;

        public FuncCondition(Func<bool> func)
        {
            _func = func;
        }

        public override bool IsConditionSatisfied()
        {
            return _func.Invoke();
        }
    }
}