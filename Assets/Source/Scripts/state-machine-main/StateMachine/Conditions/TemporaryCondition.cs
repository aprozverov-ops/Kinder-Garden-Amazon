using UnityEngine;

namespace StateMachine.Conditions
{
    public class TemporaryCondition : StateCondition
    {
        private float _currentTime;
        private float _time;

        public TemporaryCondition(float time)
        {
            _time = time;
            _currentTime = time;
        }

        public override void Tick()
        {
            _currentTime -= Time.deltaTime;
        }

        public void UpdateTime(float currentTime)
        {
            _time = currentTime;
        }
        public override void InitializeCondition()
        {
            _currentTime = _time;
        }

        public override bool IsConditionSatisfied()
        {
            return _currentTime <= 0;
        }

        public override void DeInitializeCondition()
        {
            _currentTime = _time;
        }
    }
}